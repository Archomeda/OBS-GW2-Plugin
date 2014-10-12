using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ObsGw2Plugin.Update
{
    public class ReleaseChecker
    {
        public const string GitHubApiReleasesUrl = "https://api.github.com/repos/Archomeda/OBS-GW2-Plugin/releases";


        public ReleaseChecker()
        {
            this.AsyncDownloader = new AsyncDownloader();
        }


        public IAsyncDownloader AsyncDownloader { get; protected set; }

        public void UseAsyncDownloader<T>() where T : IAsyncDownloader, new()
        {
            this.UseAsyncDownloader(new T());
        }

        public void UseAsyncDownloader(IAsyncDownloader asyncDownloader)
        {
            this.AsyncDownloader = asyncDownloader;
        }


        public async Task<Release> Check()
        {
            try
            {
                string jsonString = await this.AsyncDownloader.DownloadAsync(GitHubApiReleasesUrl, 2500);
                JArray json = JArray.Parse(jsonString);
                if (json.Count > 0)
                {
                    dynamic jsonRelease = json[0];
                    Match match = Regex.Match((string)jsonRelease.tag_name, @"v((\.?\d+)*)");
                    if (match.Success)
                    {
                        Version version;
                        if (Version.TryParse(match.Groups[1].Value, out version))
                        {
                            return new Release(version, (string)jsonRelease.html_url);
                        }
                    }
                }
            }
            catch (Exception) { }

            return null;
        }

    }
}
