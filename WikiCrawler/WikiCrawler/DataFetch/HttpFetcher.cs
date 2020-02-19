using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WikiCrawler.DataFetch
{
    class HttpFetcher
    {
        private HttpClient httpClient;

        public HttpFetcher()
        {
            httpClient = new HttpClient();
        }

        public async Task<string> FetchBody(string address)
        {
            return await httpClient.GetStringAsync(address);
        }
    }
}
