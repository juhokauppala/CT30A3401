using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using WikiCrawler.DataFetch;

namespace WikiCrawler.Workers
{
    class Fetcher : Worker
    {
        public delegate void ReturnFetchedPage(ParserArgs parserArgs);
        private ReturnFetchedPage callback;

        public Fetcher(ReturnFetchedPage callback) : base()
        {
            this.callback = callback;
        }

        protected async override void Work(object argObject)
        {
            FetcherArgs args = (FetcherArgs)argObject;
            HttpFetcher httpFetcher = new HttpFetcher();
            Page page = new Page(new Uri(args.Url, UriKind.RelativeOrAbsolute), args.Parent, args.Url);
            if (Program.Verbose)
                Console.WriteLine($"Fetching: {page.AbsoluteUri}");
            try
            {
                string html = await httpFetcher.FetchBody(page.AbsoluteUri);
                page.Body = html;

                callback(new ParserArgs()
                {
                    Page = page
                });
            } catch (HttpRequestException e)
            {
                Console.WriteLine($"HttpRequestException on page: {page.AbsoluteUri}\n{e.Message}");
            } catch (ArgumentException e)
            {
                Console.WriteLine($"ArgumentException on page: {page.AbsoluteUri}\n{e.Message}");
            }
            base.Work(argObject);
        }
    }
}
