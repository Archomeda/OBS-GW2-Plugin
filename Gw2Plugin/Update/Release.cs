using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObsGw2Plugin.Update
{
    public class Release : IEquatable<Release>
    {
        public Release(Version version, string url)
        {
            this.Version = version;
            this.Url = url;
        }

        public Version Version { get; set; }

        public string Url { get; set; }


        public static bool operator ==(Release releaseA, Release releaseB)
        {
            return releaseA.Equals(releaseB);
        }

        public static bool operator !=(Release releaseA, Release releaseB)
        {
            return !(releaseA == releaseB);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Release))
                return false;

            return this.Equals((Release)obj);
        }

        public bool Equals(Release other)
        {
            return this.Version.Equals(other.Version) && this.Url.Equals(other.Url);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 13;
                hash = hash * 7 + this.Version.GetHashCode();
                hash = hash * 7 + this.Url.GetHashCode();
                return hash;
            }
        }
    }
}
