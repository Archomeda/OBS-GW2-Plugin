using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ObsGw2Plugin.Update
{
    public interface IAsyncDownloader
    {
        Task<string> DownloadAsync(string url, int timeout);
    }
}
