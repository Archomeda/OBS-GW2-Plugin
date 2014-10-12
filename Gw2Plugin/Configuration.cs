using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ObsGw2Plugin
{
    [JsonObject]
    public class Configuration
    {
        public Configuration()
        {
            this.LastVersionCheck = new DateTime(0);
            this.LastVersionRelease = new Version();
            this.LastVersionReleaseUrl = "";
        }

        [JsonProperty]
        public DateTime LastVersionCheck { get; set; }

        [JsonProperty]
        public Version LastVersionRelease { get; set; }

        [JsonProperty]
        public string LastVersionReleaseUrl { get; set; }
    }
}
