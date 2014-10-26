using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using NUnit.Framework;
using ObsGw2Plugin.MumbleLink;
using ObsGw2Plugin.UnitTests.Utils;

namespace ObsGw2Plugin.UnitTests.MumbleLink
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class Gw2MumbleLinkFileTest : MumbleLinkFileTest
    {
        [Test]
        unsafe public void SetDataFromLinkedGw2Mem()
        {
            // MumbleLinkFileTest contains a test that covers the basic stuff of MumbleLinkFile
            // Only the stuff that is new in Gw2MumbleLinkFile is covered here
            string name = "Guild Wars 2";
            string characterName = "Rox";
            uint professionId = 4;
            uint mapId = 218;
            uint worldId = 2003;
            uint teamColorId = 55;
            bool isCommander = true;
            byte[] serverAddressBytes = new byte[] { 11, 22, 33, 44 };
            IPAddress serverAddress = new IPAddress(serverAddressBytes);
            uint mapType = 2;
            uint shardId = 123456789;
            uint instance = 10;
            uint buildId = 39660;

            GW2Context gw2Context = new GW2Context()
            {
                serverAddress = new SockAddr_In()
                {
                    sin_family = AddressFamily.InterNetwork,
                    sin_addr = new In_Addr()
                    {
                        s_b1 = serverAddressBytes[0],
                        s_b2 = serverAddressBytes[1],
                        s_b3 = serverAddressBytes[2],
                        s_b4 = serverAddressBytes[3],
                        //s_w1 = (ushort)((serverAddress[0] << 8) | serverAddress[1]),
                        //s_w2 = (ushort)((serverAddress[2] << 8) | serverAddress[3]),
                        //s_addr = (ulong)((serverAddress[0] << 24) | (serverAddress[1] << 16) | (serverAddress[2] << 8) | serverAddress[3])
                    },
                },
                mapId = mapId,
                mapType = mapType,
                shardId = shardId,
                instance = instance,
                buildId = buildId
            };

            string identityJson = @"{""name"": ""Rox"",""profession"": 4,""map_id"": 218,""world_id"": 2003,""team_color_id"": 55,""commander"": true}";
            byte[] context = PointerUtils.GetBytes(gw2Context);

            LinkedMem linkedMem = new LinkedMem();
            PointerUtils.CopyArrayToPointer(name.ToCharArray(), linkedMem.name);
            PointerUtils.CopyArrayToPointer(identityJson.ToCharArray(), linkedMem.identity);
            PointerUtils.CopyArrayToPointer(context, linkedMem.context);
            linkedMem.context_len = (uint)context.Length;

            Gw2MumbleLinkFile gw2MumbleLinkFile = new Gw2MumbleLinkFile();
            gw2MumbleLinkFile.SetDataFromLinkedMem(linkedMem);

            Assert.IsTrue(gw2MumbleLinkFile.IsValid, "IsValid");
            Assert.AreEqual(characterName, gw2MumbleLinkFile.CharacterName, "CharacterName");
            Assert.AreEqual(professionId, gw2MumbleLinkFile.ProfessionId, "ProfessionId");
            Assert.AreEqual(mapId, gw2MumbleLinkFile.MapId, "MapId");
            Assert.AreEqual(worldId, gw2MumbleLinkFile.WorldId, "WorldId");
            Assert.AreEqual(teamColorId, gw2MumbleLinkFile.TeamColorId, "TeamColorId");
            Assert.AreEqual(isCommander, gw2MumbleLinkFile.IsCommander, "IsCommander");
            Assert.AreEqual(serverAddress, gw2MumbleLinkFile.ServerAddress, "ServerAddress");
            Assert.AreEqual(mapType, gw2MumbleLinkFile.MapType, "MapType");
            Assert.AreEqual(shardId, gw2MumbleLinkFile.ShardId, "ShardId");
            Assert.AreEqual(instance, gw2MumbleLinkFile.Instance, "Instance");
            Assert.AreEqual(buildId, gw2MumbleLinkFile.BuildId, "BuildId");
        }

        [Test]
        unsafe public void SetDataFromLinkedMemNotGW2()
        {
            string name = "Not Guild Wars 2";

            LinkedMem linkedMem = new LinkedMem();
            PointerUtils.CopyArrayToPointer(name.ToCharArray(), linkedMem.name);

            Gw2MumbleLinkFile gw2MumbleLinkFile = new Gw2MumbleLinkFile();
            gw2MumbleLinkFile.SetDataFromLinkedMem(linkedMem);

            Assert.IsFalse(gw2MumbleLinkFile.IsValid);
        }


        #region NotifyPropertyChanged tests

        public new static IEnumerable<object[]> EnumerateProperties()
        {
            return new List<object[]>() {
                new object[] { "CharacterName", "Rox" },
                new object[] { "ProfessionId", 4u },
                new object[] { "MapId", 218u },
                new object[] { "WorldId", 2003u },
                new object[] { "TeamColorId", 55u },
                new object[] { "IsCommander", true },
                new object[] { "ServerAddress", new IPAddress(new byte[] {11, 22, 33, 44}) },
                new object[] { "MapType", 2u },
                new object[] { "ShardId", 123456789u },
                new object[] { "Instance", 10u },
                new object[] { "BuildId", 39660u },
            };
        }

        [Test, TestCaseSource(typeof(Gw2MumbleLinkFileTest), "EnumerateProperties")]
        public void NotifyPropertyChangedGw2MumbleLinkFile(string propertyName, object newValue)
        {
            PropertyUtils.TestNotifyPropertyChanged(new Gw2MumbleLinkFile(), propertyName, newValue);
        }

        #endregion

    }
}
