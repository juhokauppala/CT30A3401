using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikiCrawler.DataFetch;

namespace WikiCrawler
{
    class DataHandler
    {
        public delegate void ReportParseResultDelegate(Page parsedPage);
        public int PagesVisited { get => pagesVisited.Count; }

        private List<Page> pagesVisited;
        private HashSet<Page> pagesFound;

        private object pagesVisitedLock = new object();
        private object pagesFoundLock = new object();

        public DataHandler()
        {
            pagesVisited = new List<Page>();
            pagesFound = new HashSet<Page>();
        }

        public void ReportParseResult(Page parsedPage)
        {
            lock (pagesFoundLock)
            {
                parsedPage.Links.ForEach(link => pagesFound.Add(link));
            }
        }

        public void ReportFetchResult(Page visitedPage)
        {
            lock (pagesVisitedLock)
            {
                pagesVisited.Add(visitedPage);
            }
        }

        public bool HasFoundPage(string page)
        {
            lock (pagesFoundLock)
            {
                return pagesFound.Any(p => p.AbsoluteUri == page);
            }
        }

        public bool HasVisitedPage(string page)
        {
            lock (pagesVisitedLock)
            {
                return pagesVisited.Any(p => p.AbsoluteUri == page);
            }
            
        }

        public Page GetFinalResult(string goalPage)
        {
            lock (pagesVisitedLock)
            {
                return pagesVisited.Find(p => p.Links.Any(p => p.AbsoluteUri == goalPage));
            }
        }
    }
}
