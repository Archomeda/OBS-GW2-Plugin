using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;
using NSubstitute;
using NUnit.Framework;
using ObsGw2Plugin.Extensions;
using ObsGw2Plugin.MumbleLink;
using ObsGw2Plugin.Scripting;
using ObsGw2Plugin.Scripting.Formatters;
using ObsGw2Plugin.Scripting.Variables;

namespace ObsGw2Plugin.UnitTests.Scripting
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ScriptsManagerTest
    {
        protected IScriptsManager scriptsManager;

        [SetUp]
        public virtual void SetUp()
        {
            this.scriptsManager = new ScriptsManager();
        }


        protected virtual IScriptVariable RegisterScriptVariableInManager(string id)
        {
            IScriptVariable scriptVariable = Substitute.For<IScriptVariable>();
            scriptVariable.Id.Returns(id);
            this.scriptsManager.RegisterScriptVariable(scriptVariable);
            return scriptVariable;
        }

        protected virtual IScriptFormatter RegisterScriptFormatterInManager(string id)
        {
            IScriptFormatter scriptFormatter = Substitute.For<IScriptFormatter>();
            scriptFormatter.Id.Returns(id);
            this.scriptsManager.RegisterScriptFormatter(scriptFormatter);
            return scriptFormatter;
        }

        [Test]
        public void RegisterScriptVariable()
        {
            IScriptVariable scriptVariable = this.RegisterScriptVariableInManager("scriptVariable");

            CollectionAssert.Contains(this.scriptsManager.GetListOfScriptVariables(), scriptVariable, "Contains");
            Assert.AreSame(scriptVariable, this.scriptsManager.GetScriptVariable(scriptVariable.Id), "Equality");
            Assert.AreSame(this.scriptsManager, scriptVariable.ScriptsManager, "ScriptsManager");
        }

        [Test]
        public void RegisterScriptFormatter()
        {
            IScriptFormatter scriptFormatter = this.RegisterScriptFormatterInManager("scriptFormatter");

            CollectionAssert.Contains(this.scriptsManager.GetListOfScriptFormatters(), scriptFormatter, "Contains");
            Assert.AreSame(scriptFormatter, this.scriptsManager.GetScriptFormatter(scriptFormatter.Id), "Equality");
            Assert.AreSame(this.scriptsManager, scriptFormatter.ScriptsManager, "ScriptsManager");
        }

        [Test]
        public void RegisterScriptVariableOverwrite()
        {
            this.RegisterScriptVariableInManager("scriptVariable");
            IScriptVariable scriptVariable = this.RegisterScriptVariableInManager("scriptVariable");
            Assert.AreSame(scriptVariable, this.scriptsManager.GetScriptVariable(scriptVariable.Id));
        }

        [Test]
        public void RegisterScriptFormatterOverwrite()
        {
            this.RegisterScriptFormatterInManager("scriptFormatter");
            IScriptFormatter scriptFormatter = this.RegisterScriptFormatterInManager("scriptFormatter");
            Assert.AreSame(scriptFormatter, this.scriptsManager.GetScriptFormatter(scriptFormatter.Id));
        }

        [Test]
        public void UnregisterScriptVariable()
        {
            IScriptVariable scriptVariable = this.RegisterScriptVariableInManager("scriptVariable");
            this.scriptsManager.UnregisterScriptVariable(scriptVariable.Id);
            Assert.Throws<KeyNotFoundException>(() => this.scriptsManager.GetScriptVariable(scriptVariable.Id));
        }

        [Test]
        public void UnregisterScriptFormatter()
        {
            IScriptFormatter scriptFormatter = this.RegisterScriptFormatterInManager("scriptFormatter");
            this.scriptsManager.UnregisterScriptFormatter(scriptFormatter.Id);
            Assert.Throws<KeyNotFoundException>(() => this.scriptsManager.GetScriptFormatter(scriptFormatter.Id));
        }

        [Test]
        public void UseMumbleLinkFile()
        {
            IMumbleLinkFile mumbleLinkFile = Substitute.For<IMumbleLinkFile>();
            this.scriptsManager.UseMumbleLinkFile(mumbleLinkFile);
            Assert.AreSame(mumbleLinkFile, this.scriptsManager.MumbleLinkFile);
        }

        [Test]
        public void GetCachedMumbleLinkResult()
        {
            uint uiVersion = 1;
            uint uiTick = 1234;
            string name = "Guild Wars 2";
            Vector3 avatarPosition = new Vector3(34.5f, 65.25f, -12.75f);
            Vector3 avatarFront = new Vector3(89.125f, 14.5f, 90.25f);
            Vector3 avatarTop = new Vector3(84.5f, -37.25f, 65f);
            Vector3 cameraPosition = new Vector3(95.25f, 10f, 61f);
            Vector3 cameraFront = new Vector3(-74.625f, 98.875f, 0f);
            Vector3 cameraTop = new Vector3(41.5f, 28.25f, 36.75f);
            string description = "Some awesome description is supposed to be here...";

            string characterName = "Rox";
            uint professionId = 4;
            uint mapId = 218;
            uint worldId = 2003;
            uint teamColorId = 55;
            bool isCommander = true;
            byte[] serverAddress = new byte[] { 11, 22, 33, 44 };
            uint mapType = 2;
            uint shardId = 123456789;
            uint instance = 10;
            uint buildId = 39660;

            Gw2MumbleLinkFile gw2MumbleLinkFile = new Gw2MumbleLinkFile()
            {
                UIVersion = uiVersion,
                UITick = uiTick,
                Name = name,
                AvatarPosition = avatarPosition,
                AvatarFront = avatarFront,
                AvatarTop = avatarTop,
                CameraPosition = cameraPosition,
                CameraFront = cameraFront,
                CameraTop = cameraTop,
                Description = description,
                CharacterName = characterName,
                ProfessionId = professionId,
                MapId = mapId,
                WorldId = worldId,
                TeamColorId = teamColorId,
                IsCommander = isCommander,
                ServerAddress = serverAddress,
                MapType = mapType,
                ShardId = shardId,
                Instance = instance,
                BuildId = buildId
            };
            this.scriptsManager.UseMumbleLinkFile(gw2MumbleLinkFile);

            Assert.AreEqual(uiVersion, this.scriptsManager.GetCachedResult("UIVersion"), "UIVersion");
            Assert.AreEqual(uiTick, this.scriptsManager.GetCachedResult("UITick"), "UITick");
            Assert.AreEqual(name, this.scriptsManager.GetCachedResult("Name"), "Name");
            Assert.AreEqual(avatarPosition.ToDictionary(), this.scriptsManager.GetCachedResult("AvatarPosition"), "Name");
            Assert.AreEqual(avatarFront.ToDictionary(), this.scriptsManager.GetCachedResult("AvatarFront"), "AvatarFront");
            Assert.AreEqual(avatarTop.ToDictionary(), this.scriptsManager.GetCachedResult("AvatarTop"), "AvatarTop");
            Assert.AreEqual(cameraPosition.ToDictionary(), this.scriptsManager.GetCachedResult("CameraPosition"), "CameraPosition");
            Assert.AreEqual(cameraFront.ToDictionary(), this.scriptsManager.GetCachedResult("CameraFront"), "CameraFront");
            Assert.AreEqual(cameraTop.ToDictionary(), this.scriptsManager.GetCachedResult("CameraTop"), "CameraTop");
            Assert.AreEqual(description, this.scriptsManager.GetCachedResult("Description"), "Description");

            Assert.AreEqual(characterName, this.scriptsManager.GetCachedResult("CharacterName"), "CharacterName");
            Assert.AreEqual(professionId, this.scriptsManager.GetCachedResult("ProfessionId"), "ProfessionId");
            Assert.AreEqual(mapId, this.scriptsManager.GetCachedResult("MapId"), "MapId");
            Assert.AreEqual(worldId, this.scriptsManager.GetCachedResult("WorldId"), "WorldId");
            Assert.AreEqual(teamColorId, this.scriptsManager.GetCachedResult("TeamColorId"), "TeamColorId");
            Assert.AreEqual(isCommander, this.scriptsManager.GetCachedResult("IsCommander"), "IsCommander");
            Assert.AreEqual(serverAddress, this.scriptsManager.GetCachedResult("ServerAddress"), "ServerAddress");
            Assert.AreEqual(mapType, this.scriptsManager.GetCachedResult("MapType"), "MapType");
            Assert.AreEqual(shardId, this.scriptsManager.GetCachedResult("ShardId"), "ShardId");
            Assert.AreEqual(instance, this.scriptsManager.GetCachedResult("Instance"), "Instance");
            Assert.AreEqual(buildId, this.scriptsManager.GetCachedResult("BuildId"), "BuildId");
        }

        [Test]
        public void GetCachedVariableResult()
        {
            IScriptVariable scriptVariable = this.RegisterScriptVariableInManager("scriptVariable");
            DynValue expected = DynValue.NewString("Ho ho ho!!");
            scriptVariable.CachedVariable.Returns(expected);
            this.scriptsManager.UseMumbleLinkFile(new Gw2MumbleLinkFile()); // Include more possible ids we need to skip

            Assert.AreEqual(expected, this.scriptsManager.GetCachedResult(scriptVariable.Id));
        }

        [Test]
        public void GetCachedVariableResult_RunFirstUpdateNoChange()
        {
            IScriptVariable scriptVariable = this.RegisterScriptVariableInManager("scriptVariable");
            scriptVariable.HasCachedVariable.Returns(false);
            scriptVariable.UpdateCachedVariable().Returns(false);
            IScriptVariable scriptVariableHooked = this.RegisterScriptVariableInManager("scriptVariableHooked");
            scriptVariableHooked.Hooks.Returns(new HashSet<string>() { "scriptVariable" });

            this.scriptsManager.GetCachedResult(scriptVariable.Id);
            scriptVariable.Received(1).UpdateCachedVariable();
            scriptVariableHooked.Received(0).UpdateCachedVariable();
        }

        [Test]
        public void GetCachedVariableResult_RunFirstUpdateAndNotify()
        {
            IScriptVariable scriptVariable = this.RegisterScriptVariableInManager("scriptVariable");
            scriptVariable.HasCachedVariable.Returns(false);
            scriptVariable.UpdateCachedVariable().Returns(true);
            IScriptVariable scriptVariableHooked = this.RegisterScriptVariableInManager("scriptVariableHooked");
            scriptVariableHooked.Hooks.Returns(new HashSet<string>() { "scriptVariable" });

            this.scriptsManager.GetCachedResult(scriptVariable.Id);
            scriptVariable.Received(1).UpdateCachedVariable();
            scriptVariableHooked.Received(1).UpdateCachedVariable();
        }

        [Test]
        public void GetCachedFormatterResult()
        {
            IScriptFormatter scriptFormatter = this.RegisterScriptFormatterInManager("scriptFormatter");
            DynValue expected = DynValue.NewString("Ho ho ho!!");
            scriptFormatter.CachedVariable.Returns(expected);
            Assert.AreEqual(expected, this.scriptsManager.GetCachedResult("%" + scriptFormatter.Id + "%"));
        }

        [Test]
        public void GetCachedFormatterResult_RunFirstUpdateNoChange()
        {
            IScriptFormatter scriptFormatter = this.RegisterScriptFormatterInManager("scriptFormatter");
            scriptFormatter.HasCachedVariable.Returns(false);
            scriptFormatter.UpdateCachedVariable().Returns(false);
            IScriptFormatter scriptFormatterHooked = this.RegisterScriptFormatterInManager("scriptFormatterHooked");
            scriptFormatterHooked.Hooks.Returns(new HashSet<string>() { "%scriptFormatter%" });

            this.scriptsManager.GetCachedResult("%" + scriptFormatter.Id + "%");
            scriptFormatter.Received(1).UpdateCachedVariable();
            scriptFormatterHooked.Received(0).UpdateCachedVariable();
        }

        [Test]
        public void GetCachedFormatterResult_RunFirstUpdateAndNotify()
        {
            IScriptFormatter scriptFormatter = this.RegisterScriptFormatterInManager("scriptFormatter");
            scriptFormatter.HasCachedVariable.Returns(false);
            scriptFormatter.UpdateCachedVariable().Returns(true);
            IScriptFormatter scriptFormatterHooked = this.RegisterScriptFormatterInManager("scriptFormatterHooked");
            scriptFormatterHooked.Hooks.Returns(new HashSet<string>() { "%scriptFormatter%" });

            this.scriptsManager.GetCachedResult("%" + scriptFormatter.Id + "%");
            scriptFormatter.Received(1).UpdateCachedVariable();
            scriptFormatterHooked.Received(1).UpdateCachedVariable();
        }


        [TestCase("ShouldNotExist", Result = null)]
        [TestCase("%ShouldNotExistAsWell%", Result = null)]
        public object GetNotExistingCachedResult(string inputId)
        {
            return this.scriptsManager.GetCachedResult(inputId);
        }

        [Test]
        public void NotifySubscribersToUpdate_ScriptVariable()
        {
            IScriptVariable scriptVariable = this.RegisterScriptVariableInManager("scriptVariable");
            scriptVariable.Hooks.Returns(new HashSet<string>() { "scriptVariableHook" });
            IScriptFormatter scriptFormatter = this.RegisterScriptFormatterInManager("scriptFormatter");
            scriptFormatter.Hooks.Returns(new HashSet<string>() { "scriptVariableHook" });
            IScriptVariable scriptVariableHook = Substitute.For<IScriptVariable>();
            scriptVariableHook.Id.Returns("scriptVariableHook");

            this.scriptsManager.NotifySubscribersToUpdate(scriptVariableHook);
            scriptVariable.Received(1).UpdateCachedVariable();
            scriptFormatter.Received(1).UpdateCachedVariable();
        }

        [Test]
        public void NotifySubscribersToUpdate_ScriptFormatter()
        {
            IScriptVariable scriptVariable = this.RegisterScriptVariableInManager("scriptVariable");
            scriptVariable.Hooks.Returns(new HashSet<string>() { "%scriptFormatterHook%" });
            IScriptFormatter scriptFormatter = this.RegisterScriptFormatterInManager("scriptFormatter");
            scriptFormatter.Hooks.Returns(new HashSet<string>() { "%scriptFormatterHook%" });
            IScriptFormatter scriptFormatterHook = Substitute.For<IScriptFormatter>();
            scriptFormatterHook.Id.Returns("scriptFormatterHook");

            this.scriptsManager.NotifySubscribersToUpdate(scriptFormatterHook);
            scriptVariable.Received(0).UpdateCachedVariable(); // Variable does not have access to the cached variable of a formatter
            scriptFormatter.Received(1).UpdateCachedVariable();
        }

        [Test]
        public void NotifySubscribersToUpdateFromMumbleLink()
        {
            MumbleLinkFile mumbleLinkFile = new MumbleLinkFile();
            this.scriptsManager.UseMumbleLinkFile(mumbleLinkFile);

            IScriptVariable scriptVariable = this.RegisterScriptVariableInManager("scriptVariable");
            scriptVariable.Hooks.Returns(new HashSet<string>() { "UIVersion" });

            mumbleLinkFile.UIVersion = 10;
            scriptVariable.Received(1).UpdateCachedVariable();
        }

        [Test]
        public void NotifySubscribersToUpdateRecursiveVariable()
        {
            IScriptVariable scriptVariable = this.RegisterScriptVariableInManager("scriptVariable");
            scriptVariable.Hooks.Returns(new HashSet<string>() { "scriptVariableHook" });
            scriptVariable.UpdateCachedVariable().Returns(true);
            IScriptVariable scriptVariable2 = this.RegisterScriptVariableInManager("scriptVariable2");
            scriptVariable2.Hooks.Returns(new HashSet<string>() { "scriptVariable" });

            IScriptVariable scriptVariableHook = Substitute.For<IScriptVariable>();
            scriptVariableHook.Id.Returns("scriptVariableHook");

            this.scriptsManager.NotifySubscribersToUpdate(scriptVariableHook);
            scriptVariable.Received(1).UpdateCachedVariable();
            scriptVariable2.Received(1).UpdateCachedVariable();
        }

        [Test]
        public void NotifySubscribersToUpdateRecursiveVariableToFormatter()
        {
            IScriptFormatter scriptFormatter = this.RegisterScriptFormatterInManager("scriptFormatter");
            scriptFormatter.Hooks.Returns(new HashSet<string>() { "scriptVariableHook" });
            scriptFormatter.UpdateCachedVariable().Returns(true);
            IScriptFormatter scriptFormatter2 = this.RegisterScriptFormatterInManager("scriptFormatter2");
            scriptFormatter2.Hooks.Returns(new HashSet<string>() { "%scriptFormatter%" });

            IScriptVariable scriptVariableHook = Substitute.For<IScriptVariable>();
            scriptVariableHook.Id.Returns("scriptVariableHook");

            this.scriptsManager.NotifySubscribersToUpdate(scriptVariableHook);
            scriptFormatter.Received(1).UpdateCachedVariable();
            scriptFormatter2.Received(1).UpdateCachedVariable();
        }

        [Test]
        public void NotifySubscribersToUpdateRecursiveFormatter()
        {
            IScriptFormatter scriptFormatter = this.RegisterScriptFormatterInManager("scriptFormatter");
            scriptFormatter.Hooks.Returns(new HashSet<string>() { "%scriptFormatterHook%" });
            scriptFormatter.UpdateCachedVariable().Returns(true);
            IScriptFormatter scriptFormatter2 = this.RegisterScriptFormatterInManager("scriptFormatter2");
            scriptFormatter2.Hooks.Returns(new HashSet<string>() { "%scriptFormatter%" });

            IScriptFormatter scriptFormatterHook = Substitute.For<IScriptFormatter>();
            scriptFormatterHook.Id.Returns("scriptFormatterHook");

            this.scriptsManager.NotifySubscribersToUpdate(scriptFormatterHook);
            scriptFormatter.Received(1).UpdateCachedVariable();
            scriptFormatter2.Received(1).UpdateCachedVariable();
        }

        [Test]
        public void FormatString()
        {
            IScriptFormatter scriptFormatter = this.RegisterScriptFormatterInManager("scriptFormatter");
            string toReturn = "SF";
            scriptFormatter.CachedVariable.Returns(DynValue.NewString(toReturn));
            string stringToFormat = "123%scriptFormatter%456";
            string expected = "123" + toReturn + "456";
            Assert.AreEqual(expected, this.scriptsManager.FormatString(stringToFormat));
        }

        [Test]
        public void FormatStringNot()
        {
            IScriptFormatter scriptFormatter = this.RegisterScriptFormatterInManager("scriptFormatter");
            string stringToFormat = "123%notAScriptFormatter%456";
            Assert.AreEqual(stringToFormat, this.scriptsManager.FormatString(stringToFormat));
        }

        [Test]
        public void FormatStringNull()
        {
            IScriptFormatter scriptFormatter = this.RegisterScriptFormatterInManager("scriptFormatter");
            string toReturn = null;
            scriptFormatter.CachedVariable.Returns(DynValue.NewString(toReturn));
            string stringToFormat = "123%scriptFormatter%456";
            string expected = "123456";
            Assert.AreEqual(expected, this.scriptsManager.FormatString(stringToFormat));
        }

        [Test]
        public void FormatStringNoVariables()
        {
            IScriptVariable scriptVariable = this.RegisterScriptVariableInManager("scriptVariable");
            string stringToFormat = "123%scriptVariable%456";
            Assert.AreEqual(stringToFormat, this.scriptsManager.FormatString(stringToFormat));
        }

    }
}
