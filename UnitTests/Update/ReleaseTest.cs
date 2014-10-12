using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using NUnit.Framework;
using ObsGw2Plugin.Update;

// Disable warning about comparison to same variable (CS1718)
#pragma warning disable 1718

namespace ObsGw2Plugin.UnitTests.Update
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ReleaseTest
    {
        [Test]
        public void Constructor()
        {
            Version version = new Version(1, 0);
            string url = "http://url.to/some?download#location";
            Release release = new Release(version, url);

            Assert.AreEqual(version, release.Version, "Version");
            Assert.AreEqual(url, release.Url, "Url");
        }

        [Test]
        public void EqualitySelf()
        {
            Release release = new Release(new Version(1, 0), "http://url.to/some?download#location");

            Assert.IsTrue(release.Equals(release), "Equals");
            Assert.IsTrue(release == release, "==");
            Assert.IsFalse(release != release, "!=");
            Assert.AreEqual(release.GetHashCode(), release.GetHashCode(), "GetHashCode");
        }

        [Test]
        public void EqualityVersion()
        {
            Release releaseA = new Release(new Version(1, 0), "http://url.to/some?download#location");
            Release releaseB = new Release(new Version(1, 0), "http://url.to/some?download#location");

            Assert.IsTrue(releaseA.Equals(releaseB), "Equals");
            Assert.IsTrue(releaseA == releaseB, "==");
            Assert.IsFalse(releaseA != releaseB, "!=");
            Assert.AreEqual(releaseA.GetHashCode(), releaseB.GetHashCode(), "GetHashCode");
        }

        [Test]
        public void EqualityOther()
        {
            object releaseA = new Release(new Version(1, 0), "http://url.to/some?download#location");
            object releaseB = new Release(new Version(1, 0), "http://url.to/some?download#location");

            Assert.IsTrue(releaseA.Equals(releaseB), "Equals");
            Assert.AreEqual(releaseA.GetHashCode(), releaseB.GetHashCode(), "GetHashCode");
        }

        [Test]
        public void InequalityVector3()
        {
            Release releaseA = new Release(new Version(1, 0), "http://url.to/some?download#location");
            Release releaseB = new Release(new Version(2, 0), "http://url.to/another?download#location");

            Assert.IsFalse(releaseA.Equals(releaseB), "Equals");
            Assert.IsFalse(releaseA == releaseB, "==");
            Assert.IsTrue(releaseA != releaseB, "!=");
        }

        [Test]
        public void InequalityOther()
        {
            object release = new Release(new Version(1, 0), "http://url.to/some?download#location");
            object obj = new object();

            Assert.IsFalse(release.Equals(obj));
        }

        [Test]
        public void InequalityNull()
        {
            object release = new Release(new Version(1, 0), "http://url.to/some?download#location");

            Assert.IsFalse(release.Equals(null), "Equals");
            Assert.IsFalse(release == null, "==");
            Assert.IsTrue(release != null, "!=");
        }

        [Test]
        public void InequalityNull2()
        {
            Release release = null;
            Assert.IsTrue(release == null, "==");
            Assert.IsFalse(release != null, "!=");
        }
    }
}
