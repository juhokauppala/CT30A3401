using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WikiCrawler.DataFetch
{
    class PageParser
    {
        public PageParser()
        {

        }

        public List<Page> FindPagesFrom(Page page, string goalHost)
        {
            return FindLinksFrom(page, goalHost).Select(str => new Page(new Uri(str), page, str)).ToList();
        }

        public List<string> FindLinksFrom(Page page, string goalHost)
        {
            return FindLinksFrom(page.Body, goalHost);
        }

        public List<string> FindLinksFrom(string html, string goalHost)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            HtmlNode document = htmlDocument.DocumentNode;
            IEnumerable<HtmlNode> links = document.QuerySelectorAll("a[href]");
            return links.Select(link => link.GetAttributeValue("href", "")).Where(link => AddressIsWikipediaOrRelative(link, goalHost)).ToList();
        }

        private bool AddressIsWikipediaOrRelative(string address, string goalHost)
        {
            Uri uri;
            bool isUri = Uri.TryCreate(address, UriKind.RelativeOrAbsolute, out uri);
            
            if (!isUri)
                return false;

            if (!uri.IsAbsoluteUri && Regex.IsMatch(uri.OriginalString, @"^\/wiki\/[^:]*$"))
                return true;

            return Regex.IsMatch(uri.OriginalString, $@"{goalHost}\/wiki\/");
        }
    }
}
