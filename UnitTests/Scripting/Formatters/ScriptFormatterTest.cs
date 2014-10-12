using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;
using NSubstitute;
using NUnit.Framework;
using ObsGw2Plugin.Scripting;
using ObsGw2Plugin.Scripting.Exceptions;
using ObsGw2Plugin.Scripting.Formatters;

namespace ObsGw2Plugin.UnitTests.Scripting.Formatters
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ScriptFormatterTest : ScriptBaseTest
    {
        public override void SetUp()
        {
            this.script = new ScriptFormatter();
        }


        private ScriptFormatter scriptFormatter { get { return (ScriptFormatter)this.script; } }

        protected override string GetScriptFilename(string file)
        {
            return Path.Combine("Scripting", "Formatters", "ScriptFormatter" + file + ".lua");
        }

        [Test]
        public override void InitScriptAllGlobalsPresent()
        {
            base.InitScriptAllGlobalsPresent();

            string name = "TestName";
            IEnumerable<string> category = new List<string>() { "Category1", "Category2" };
            Assert.AreEqual(name, this.scriptFormatter.Name, "Name");
            CollectionAssert.AreEqual(category, this.scriptFormatter.Category, "Category");
        }

        [Test]
        public void InitScriptMissingRequiredGlobalName()
        {
            string filename = this.GetScriptFilename("TestMissingName");

            Assert.Throws<MissingGlobalException>(() => this.script.InitScript(filename));
        }

        [Test]
        public void InitScriptOptionalGlobalCategory()
        {
            string filename = this.GetScriptFilename("TestOptional");

            this.script.InitScript(filename);
            Assert.AreEqual(null, this.scriptFormatter.Category);
        }

        [Test]
        public void GlobalCSharpGetText()
        {
            string filename = this.GetScriptFilename("Test");
            string formatterId = "%aFormatter%";
            DynValue result = DynValue.NewString("2 times 42");
            IScriptsManager scriptsManager = Substitute.For<IScriptsManager>();
            scriptsManager.GetCachedResult(formatterId).ReturnsForAnyArgs(result);

            this.script.InitScript(filename);
            this.script.ScriptsManager = scriptsManager;
            var func = (CallbackFunction)this.script.LuaScript.Globals["gettext"];
            Assert.AreEqual(result, func.Invoke(null, new List<DynValue>() { DynValue.NewString(formatterId) }));
        }

    }
}
