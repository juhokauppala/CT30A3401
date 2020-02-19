using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WikiCrawler.DataFetch
{
    class Page
    {
        public Uri Uri { get; private set; }
        public List<Page> Path { get; private set; }
        
        public string Name { get; private set; }
        public string Body { get; set; }
        public List<Page> Links { get; set; }

        public string AbsoluteUri
        {
            get
            {
                string absolute;
                if (Uri.IsAbsoluteUri)
                {
                    absolute = Uri.OriginalString;
                } else if (Regex.IsMatch(Uri.OriginalString, @"^//"))
                {
                    absolute = "https:" + Uri.OriginalString;
                } else
                {
                    absolute = "https://" + FindHostNameInAncestors() + Uri.OriginalString;
                }
                return absolute;
            }
        }

        public Page(Uri uri, Page parent, string name)
        {
            Uri = uri;
            Path = parent != null ? parent.GetCopyOfPath() : new List<Page>();
            Path.Add(this);
            Name = name;
            Links = new List<Page>();
        }

        public override string ToString()
        {
            return string.Join("\n", Path.Select(p => p.Name));
        }

        public string FindHostNameInAncestors()
        {
            Page currentPage = this;
            while (!currentPage.Uri.IsAbsoluteUri)
            {
                currentPage = currentPage.Path[currentPage.Path.Count - 2];
            }
            return currentPage.Uri.Host;
        }

        public List<Page> GetCopyOfPath()
        {
            return new List<Page>(Path);
        }
    }
}
