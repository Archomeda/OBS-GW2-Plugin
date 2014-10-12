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
            if (object.ReferenceEquals(releaseA, releaseB))
                return true;

            if ((object)releaseA == null || (object)releaseB == null)
                return object.Equals(releaseA, releaseB);

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
            if (other == null)
                return false;

            bool equals = true;
            if (this.Version != null)
                equals = this.Version.Equals(other.Version);
            if (this.Url != null)
                equals = equals && this.Url.Equals(other.Url);
            return equals;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 13;
                if (this.Version != null)
                    hash = hash * 7 + this.Version.GetHashCode();
                if (this.Url != null)
                    hash = hash * 7 + this.Url.GetHashCode();
                return hash;
            }
        }
    }
}
