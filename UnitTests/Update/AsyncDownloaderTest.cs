using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ObsGw2Plugin.Update;

namespace ObsGw2Plugin.UnitTests.Update
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class AsyncDownloaderTest
    {
        [Test]
        public void DownloadAsync()
        {
            AsyncDownloader downloader = new AsyncDownloader();
            string url = "http://localhost:51234/";
            string expected = "That's a nice string you have there";

            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();

            listener.BeginGetContext(result =>
            {
                HttpListenerContext context = listener.EndGetContext(result);
                using (StreamWriter writer = new StreamWriter(context.Response.OutputStream))
                {
                    writer.Write(expected);
                    writer.Flush();
                }
            }, null);

            string actual = downloader.DownloadAsync(url).Result;

            listener.Stop();
            listener.Close();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DownloadAsyncTimeout()
        {
            AsyncDownloader downloader = new AsyncDownloader();
            string url = "http://localhost:51234/";
            int timeout = 250;

            HttpListener listener = new HttpListener();
            listener.Prefixes.Add(url);
            listener.Start();

            Assert.That(async () => await downloader.DownloadAsync(url, timeout), Throws.InstanceOf<TaskCanceledException>());

            listener.Stop();
            listener.Close();
        }
    }
}
