using System;
using System.Collections.Generic;
using System.Text;
using WikiCrawler.DataFetch;

namespace WikiCrawler.Workers
{
    class FetcherArgs
    {
        public string Url;
        public Page Parent;

        public override string ToString()
        {
            return Url;
        }
    }
}
