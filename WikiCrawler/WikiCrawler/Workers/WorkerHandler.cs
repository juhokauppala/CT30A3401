using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikiCrawler.DataFetch;

namespace WikiCrawler.Workers
{
    class WorkerHandler
    {
        private List<Fetcher> Fetchers;
        private List<Parser> Parsers;

        private Queue<ParserArgs> parseQueue;
        private Queue<FetcherArgs> fetchQueue;

        private DataHandler dataHandler;
        private string goalHost;
        public int FetchQueueCount { get => fetchQueue.Count; }
        public int ParseQueueCount { get => parseQueue.Count; }

        public WorkerHandler(DataHandler dataHandler, string allowedHost)
        {
            Fetchers = new List<Fetcher>();
            Parsers = new List<Parser>();
            parseQueue = new Queue<ParserArgs>();
            fetchQueue = new Queue<FetcherArgs>();

            this.dataHandler = dataHandler;
            goalHost = allowedHost;
        }

        public void AddFetcher()
        {
            Fetchers.Add(new Fetcher(AddParseJob));
        }

        public void AddFetchers(int num)
        {
            for (int i = 0; i < num; i++)
            {
                AddFetcher();
            }
        }
        public void AddParser()
        {
            Parsers.Add(new Parser(OnParseJobDone, goalHost));
        }

        public void AddParsers(int num)
        {
            for (int i = 0; i < num; i++)
            {
                AddParser();
            }
        }

        public void AddParseJob(ParserArgs args)
        {
            parseQueue.AddNew(args);
            dataHandler.ReportFetchResult(args.Page);
        }

        public void AddFetchJob(FetcherArgs args)
        {
            fetchQueue.AddNew(args);
        }

        public void StartNextFetchJob()
        {
            Fetcher fetcher = Fetchers.Where(f => !f.HasWork).FirstOrDefault();
            if (fetcher == null)
                return;

            FetcherArgs job = fetchQueue.GetNext();

            if (job != null)
            {
                if (!dataHandler.HasVisitedPage(job.Url))
                {
                    fetcher.StartWork(job);
                } else
                {
                    //Console.WriteLine($"Already visited: {job.Url}");
                }
            }
        }

        public void StartNextParseJob()
        {
            Parser parser = Parsers.Where(p => !p.HasWork).FirstOrDefault();
            ParserArgs job = parseQueue.GetNext();

            if (parser != null && job != null)
                parser.StartWork(job);
        }

        public void OnParseJobDone(Page parsedPage)
        {
            dataHandler.ReportParseResult(parsedPage);
            foreach (Page link in parsedPage.Links)
            {
                AddFetchJob(new FetcherArgs()
                {
                    Parent = parsedPage,
                    Url = link.AbsoluteUri
                });
            }
        }

        public void Stop()
        {
            Fetchers.Clear();
            Parsers.Clear();
        }
    }
}
