using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikiCrawler.DataFetch;

namespace WikiCrawler.Workers
{
    class Parser : Worker
    {
        private DataHandler.ReportParseResultDelegate callback;
        private string goalHost;

        public Parser(DataHandler.ReportParseResultDelegate callback, string goalHost) : base()
        {
            this.callback = callback;
            this.goalHost = goalHost;
        }

        protected override void Work(object argObject)
        {
            ParserArgs args = (ParserArgs)argObject;
            Page page = args.Page;
            PageParser pageParser = new PageParser();
            if (Program.Verbose)
                Console.WriteLine($"Parsing: {page.Name}");

            Uri test;
            page.Links = pageParser.FindLinksFrom(page, goalHost).Where(link => Uri.TryCreate(link, UriKind.RelativeOrAbsolute, out test)).Select(l => new Page(new Uri(l, UriKind.RelativeOrAbsolute), page, l)).ToList();

            callback(page);
            base.Work(argObject);
        }
    }
}
