using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using NSubstitute;
using NUnit.Framework;
using ObsGw2Plugin.MumbleLink;
using ObsGw2Plugin.UnitTests.Utils;

namespace ObsGw2Plugin.UnitTests.MumbleLink
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class MumbleLinkManagerTest
    {
        [Test]
        public void IsListening()
        {
            MumbleLinkManager manager = new MumbleLinkManager();
            Assert.IsFalse(manager.IsListening, "Not listening before");

            manager.StartListener();
            Assert.IsTrue(manager.IsListening, "Listening");

            manager.StopListener();
            // It may take a while before it has stopped
            Assert.That(() => manager.IsListening, Is.False.After(2000, 10), "Not listening after");
        }

        [Test]
        public void UseMumbleLinkFile()
        {
            MumbleLinkManager manager = new MumbleLinkManager();
            manager.UseMumbleLinkFile<MumbleLinkFile>();
            Assert.IsInstanceOf<MumbleLinkFile>(manager.MumbleLinkFile);

            manager = new MumbleLinkManager();
            manager.UseMumbleLinkFile(new MumbleLinkFile());
            Assert.IsInstanceOf<MumbleLinkFile>(manager.MumbleLinkFile);
        }

        [Test]
        public void UseMumbleLinkConnector()
        {
            MumbleLinkManager manager = new MumbleLinkManager();
            manager.UseMumbleLinkConnector<MumbleLinkConnector>();
            Assert.IsInstanceOf<MumbleLinkConnector>(manager.MumbleLinkConnector);

            manager = new MumbleLinkManager();
            manager.UseMumbleLinkConnector(new MumbleLinkConnector());
            Assert.IsInstanceOf<MumbleLinkConnector>(manager.MumbleLinkConnector);
        }

        [Test]
        public void IsActive()
        {
            MumbleLinkManager manager = new MumbleLinkManager();

            IMumbleLinkConnector connector = Substitute.For<IMumbleLinkConnector>();
            LinkedMem linkedMem = new LinkedMem() { uiTick = 1 };
            connector.ReadMemoryMappedFile().Returns(linkedMem);
            manager.UseMumbleLinkConnector(connector);

            Assert.IsFalse(manager.IsActive, "Not active before");
            manager.Check();
            Assert.IsTrue(manager.IsActive, "Active after");
        }

        [Test]
        public void TimeoutRate()
        {
            MumbleLinkManager manager = new MumbleLinkManager();
            manager.TimeoutRate = 0.1;

            IMumbleLinkConnector connector = Substitute.For<IMumbleLinkConnector>();
            LinkedMem linkedMem = new LinkedMem() { uiTick = 1 };
            connector.ReadMemoryMappedFile().Returns(linkedMem);
            manager.UseMumbleLinkConnector(connector);

            Assert.IsFalse(manager.IsActive, "Not active before");
            manager.Check();
            Assert.IsTrue(manager.IsActive, "Active");
            Thread.Sleep((int)(manager.TimeoutRate * 1000));
            manager.Check();
            Assert.IsFalse(manager.IsActive, "Not active after");
        }

        [Test]
        public void UpdateMumbleLinkFile()
        {
            MumbleLinkManager manager = new MumbleLinkManager();
            IMumbleLinkConnector connector = Substitute.For<IMumbleLinkConnector>();
            IMumbleLinkFile file = Substitute.For<IMumbleLinkFile>();

            LinkedMem linkedMem = new LinkedMem() { uiTick = 1 };
            connector.ReadMemoryMappedFile().Returns(linkedMem);
            manager.UseMumbleLinkConnector(connector);
            manager.UseMumbleLinkFile(file);

            manager.Check();
            file.Received(1).SetDataFromLinkedMem(linkedMem);
        }

        [Test]
        public void MumbleLinkStateChanged()
        {
            MumbleLinkManager manager = new MumbleLinkManager();
            manager.TimeoutRate = 0.1;
            MumbleLinkState? actualState = null;
            string actualName = null;
            string expectedName = "Super Adventure Wars FTW";
            manager.MumbleLinkStateChanged += (s, e) =>
            {
                actualState = e.State;
                actualName = e.Name;
            };

            IMumbleLinkConnector connector = Substitute.For<IMumbleLinkConnector>();
            LinkedMem linkedMem = new LinkedMem() { uiTick = 1 };
            unsafe { PointerUtils.CopyArrayToPointer(expectedName.ToCharArray(), linkedMem.name); }
            connector.ReadMemoryMappedFile().Returns(linkedMem);
            manager.UseMumbleLinkConnector(connector);

            manager.Check();
            Assert.IsTrue(manager.IsActive, "Active");
            Assert.AreEqual(expectedName, actualName, "Name");
            actualState = null;
            actualName = null;
            Thread.Sleep((int)(manager.TimeoutRate * 1000));
            manager.Check();
            Assert.IsFalse(manager.IsActive, "Not active after");
        }
    }
}
