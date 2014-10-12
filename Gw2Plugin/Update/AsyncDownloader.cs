using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace ObsGw2Plugin.Update
{
    public class AsyncDownloader : IAsyncDownloader
    {
        public async Task<string> DownloadAsync(string url)
        {
            return await this.DownloadAsync(url, 0);
        }

        public async Task<string> DownloadAsync(string url, int timeout)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                if (timeout > 0)
                    httpClient.Timeout = TimeSpan.FromMilliseconds(timeout);

                using (HttpResponseMessage response = await httpClient.GetAsync(url))
                {
                    return await response.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
                }
            }
        }
    }
}
