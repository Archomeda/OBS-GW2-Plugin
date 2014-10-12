using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using ObsGw2Plugin.Update;

namespace ObsGw2Plugin.UnitTests.Update
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ReleaseCheckerTest
    {
        [Test]
        public void UseAsyncDownloader()
        {
            ReleaseChecker releaseChecker = new ReleaseChecker();
            releaseChecker.UseAsyncDownloader<AsyncDownloader>();
            Assert.IsInstanceOf<AsyncDownloader>(releaseChecker.AsyncDownloader);

            IAsyncDownloader downloader = Substitute.For<IAsyncDownloader>();
            releaseChecker.UseAsyncDownloader(downloader);
            Assert.AreSame(downloader, releaseChecker.AsyncDownloader);
        }

        [Test]
        public void Check()
        {
            string versionString = "v1.0";
            Version version = new Version(1, 0);
            string url = "http://url.to/some?download#location";
            string returnJson = string.Format(@"[ {{ ""html_url"": ""{0}"", ""tag_name"": ""{1}"" }} ]", url, versionString);

            ReleaseChecker releaseChecker = new ReleaseChecker();
            IAsyncDownloader downloader = Substitute.For<IAsyncDownloader>();
            downloader.DownloadAsync(null, 0).ReturnsForAnyArgs(Task.FromResult(returnJson));
            releaseChecker.UseAsyncDownloader(downloader);

            Release result = releaseChecker.Check().Result;
            Assert.AreEqual(version, result.Version, "Version");
            Assert.AreEqual(url, result.Url, "Url");
        }

        [Test]
        public void CheckNoMatch()
        {
            string versionString = "1.0";
            string url = "http://url.to/some?download#location";
            string returnJson = string.Format(@"[ {{ ""html_url"": ""{0}"", ""tag_name"": ""{1}"" }} ]", url, versionString);

            ReleaseChecker releaseChecker = new ReleaseChecker();
            IAsyncDownloader downloader = Substitute.For<IAsyncDownloader>();
            downloader.DownloadAsync(null, 0).ReturnsForAnyArgs(Task.FromResult(returnJson));
            releaseChecker.UseAsyncDownloader(downloader);

            Release result = releaseChecker.Check().Result;
            Assert.AreEqual(null, result);
        }
        
        [Test]
        public void CheckInvalidVersion()
        {
            string versionString = "v1.3.5.2150000000";
            string url = "http://url.to/some?download#location";
            string returnJson = string.Format(@"[ {{ ""html_url"": ""{0}"", ""tag_name"": ""{1}"" }} ]", url, versionString);

            ReleaseChecker releaseChecker = new ReleaseChecker();
            IAsyncDownloader downloader = Substitute.For<IAsyncDownloader>();
            downloader.DownloadAsync(null, 0).ReturnsForAnyArgs(Task.FromResult(returnJson));
            releaseChecker.UseAsyncDownloader(downloader);

            Release result = releaseChecker.Check().Result;
            Assert.AreEqual(null, result);
        }

        [Test]
        public void CheckEmptyArray()
        {
            string returnJson = "[]";

            ReleaseChecker releaseChecker = new ReleaseChecker();
            IAsyncDownloader downloader = Substitute.For<IAsyncDownloader>();
            downloader.DownloadAsync(null, 0).ReturnsForAnyArgs(Task.FromResult(returnJson));
            releaseChecker.UseAsyncDownloader(downloader);

            Release result = releaseChecker.Check().Result;
            Assert.AreEqual(null, result);
        }

        [Test]
        public void CheckInvalidJson()
        {
            string returnJson = "[[[";

            ReleaseChecker releaseChecker = new ReleaseChecker();
            IAsyncDownloader downloader = Substitute.For<IAsyncDownloader>();
            downloader.DownloadAsync(null, 0).ReturnsForAnyArgs(Task.FromResult(returnJson));
            releaseChecker.UseAsyncDownloader(downloader);

            Release result = releaseChecker.Check().Result;
            Assert.AreEqual(null, result);
        }
    }
}
