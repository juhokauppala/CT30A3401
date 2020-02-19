using System;
using System.Diagnostics;
using System.Threading;
using System.Text.RegularExpressions;

using WikiCrawler.Workers;

namespace WikiCrawler
{
    class Program
    {
        public static bool Verbose = false;

        static void Main(string[] args)
        {
            bool restart = true;
            while (restart)
            {
                restart = Run();
            }
        }

        private static bool Run()
        {
            const int numFetchers = 9;
            const int numParsers = 3;

            Console.Write("Starting page: ");
            string startPage = Console.ReadLine();
            if (startPage.Length == 0)
                startPage = @"https://fi.wikipedia.org/wiki/Hyperlinkki";
            Console.Write("Goal page: ");
            string goalPage = Console.ReadLine();
            if (goalPage.Length == 0)
                goalPage = @"https://fi.wikipedia.org/wiki/Fyysikko";

            Uri goal = new Uri(goalPage, UriKind.Absolute);

            Console.WriteLine($"Looking for a path from {startPage} to {goalPage}");

            DataHandler dataHandler = new DataHandler();

            WorkerHandler workerHandler = new WorkerHandler(dataHandler, goal.Host);
            workerHandler.AddFetchers(numFetchers);
            workerHandler.AddParsers(numParsers);

            workerHandler.AddFetchJob(new FetcherArgs()
            {
                Url = startPage
            });
            Stopwatch logged = Stopwatch.StartNew();
            while (!dataHandler.HasFoundPage(goalPage))
            {
                workerHandler.StartNextFetchJob();
                workerHandler.StartNextParseJob();
                if (!Program.Verbose && logged.Elapsed.TotalSeconds >= 5)
                {
                    Console.WriteLine($"Visited pages: {dataHandler.PagesVisited}\tTo fetch: {workerHandler.FetchQueueCount}\tTo parse: {workerHandler.ParseQueueCount}");
                    logged.Restart();
                }
                Thread.Yield();
            }
            workerHandler.Stop();
            Console.WriteLine("Goal page found!");
            Console.WriteLine(dataHandler.GetFinalResult(goalPage).ToString());
            Console.Write("Restart? ");
            return Regex.IsMatch(Console.ReadLine(), "[1yY]");
        }
    }
}
