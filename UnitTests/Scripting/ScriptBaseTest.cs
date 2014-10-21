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

namespace ObsGw2Plugin.UnitTests.Scripting
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public abstract class ScriptBaseTest
    {
        protected ScriptBase script;

        [TestFixtureSetUp]
        public virtual void Init()
        {
            Script.WarmUp();
        }

        [SetUp]
        public abstract void SetUp();


        protected virtual string GetScriptFilename(string file)
        {
            return Path.Combine("Scripting", "ScriptBase" + file + ".lua");
        }


        [Test]
        public void InitScript()
        {
            string filename = this.GetScriptFilename("TestDummy");

            this.script.InitScript(filename);
            Assert.IsNotNull(this.script.LuaScript, "LuaScript");
            Assert.AreEqual(0, this.script.Locals.Count, "Locals");
        }

        [Test]
        public virtual void InitScriptAllGlobalsPresent()
        {
            string filename = this.GetScriptFilename("Test");
            string id = "Test";
            ISet<string> hooks = new HashSet<string>() { "Hook1", "Hook2" };

            this.script.InitScript(filename);
            Assert.AreEqual(filename, this.script.LuaScriptFilename, "LuaScriptFilename");
            Assert.AreEqual(id, this.script.Id, "Id");
            CollectionAssert.AreEquivalent(hooks, this.script.Hooks, "Hooks");
            Assert.DoesNotThrow(() => this.script.UpdateCachedVariable(), "Update");
        }

        [Test]
        public void InitScriptMissingRequiredGlobalId()
        {
            string filename = this.GetScriptFilename("TestMissingId");

            Assert.Throws<MissingGlobalException>(() => this.script.InitScript(filename));
        }

        [Test]
        public void InitScriptMissingRequiredGlobalUpdate()
        {
            string filename = this.GetScriptFilename("TestMissingUpdate");

            this.script.InitScript(filename);
            Assert.Throws<MissingGlobalException>(() => this.script.UpdateCachedVariable());
        }

        [Test]
        public void InitScriptOptionalGlobalHook()
        {
            string filename = this.GetScriptFilename("TestOptional");

            this.script.InitScript(filename);
            Assert.AreEqual(null, this.script.Hooks);
        }

        [Test]
        public void HasCachedVariable()
        {
            string filename = this.GetScriptFilename("Test");
            DynValue variable = DynValue.NewNumber(42);

            this.script.InitScript(filename);
            Assert.IsFalse(this.script.HasCachedVariable, "No cached variable");
            this.script.UpdateCachedVariable();
            Assert.IsTrue(this.script.HasCachedVariable, "Has cached variable");
        }

        [Test]
        public void UpdateCachedVariable()
        {
            string filename = this.GetScriptFilename("Test");
            DynValue variable = DynValue.NewNumber(42);

            this.script.InitScript(filename);
            Assert.IsTrue(this.script.UpdateCachedVariable(), "Update");
            Assert.AreEqual(variable, this.script.GetLiveVariable(), "Equality live");
            Assert.AreEqual(variable, this.script.CachedVariable, "Equality cache");
            Assert.IsFalse(this.script.UpdateCachedVariable(), "No update");
        }

        [Test]
        public void CachedVariableChangedEvent()
        {
            string filename = this.GetScriptFilename("Test");
            DynValue variable = DynValue.NewNumber(42);

            bool eventFired = false;
            object oldVariable = null;
            object newVariable = null;

            this.script.InitScript(filename);
            this.script.CachedVariableChanged += (s_, e_) =>
            {
                eventFired = true;
                oldVariable = e_.OldVariable;
                newVariable = e_.NewVariable;
            };
            this.script.UpdateCachedVariable();
            Assert.IsTrue(eventFired, "Event fired with update");
            Assert.AreEqual(null, oldVariable, "Update oldVariable");
            Assert.AreEqual(variable, newVariable, "Update newVariable");
            eventFired = false;
            this.script.UpdateCachedVariable();
            Assert.IsFalse(eventFired, "Event fired with no update");
        }

        [Test]
        public void GlobalCSharpLocalVar()
        {
            string filename = this.GetScriptFilename("TestDummy");
            DynValue dynValueNewItem = DynValue.NewString("NewItem");
            DynValue dynValueNewValue = DynValue.NewString("NewValue");


            this.script.InitScript(filename);
            var func = (CallbackFunction)this.script.LuaScript.Globals["localvar"];
            Assert.AreEqual(DynValue.NewNil(), func.Invoke(null, new List<DynValue>() { dynValueNewItem }));
            Assert.AreEqual(DynValue.NewNil(), func.Invoke(null, new List<DynValue>() { dynValueNewItem, dynValueNewValue }));
            Assert.AreEqual(dynValueNewValue, func.Invoke(null, new List<DynValue>() { dynValueNewItem }));
        }

        [Test]
        public void GlobalCSharpGetCurrent()
        {
            string filename = this.GetScriptFilename("Test");
            DynValue variable = DynValue.NewNumber(42);

            this.script.InitScript(filename);
            this.script.UpdateCachedVariable();
            var func = (CallbackFunction)this.script.LuaScript.Globals["getcurrent"];
            Assert.AreEqual(variable, func.Invoke(null, null));
        }

        [Test]
        public void GlobalCSharpGetVar()
        {
            string filename = this.GetScriptFilename("Test");
            string variableId = "aVariable";
            DynValue result = DynValue.NewString("2 times 42");
            IScriptsManager scriptsManager = Substitute.For<IScriptsManager>();
            scriptsManager.GetCachedResult(variableId).ReturnsForAnyArgs(result);

            this.script.InitScript(filename);
            this.script.ScriptsManager = scriptsManager;
            var func = (CallbackFunction)this.script.LuaScript.Globals["getvar"];
            Assert.AreEqual(result, func.Invoke(null, new List<DynValue>() { DynValue.NewString(variableId) }));
        }

        [Test]
        public void GlobalCSharpTimestamp()
        {
            string filename = this.GetScriptFilename("TestDummy");
            DateTime unixStart = new DateTime(1970, 1, 1);

            this.script.InitScript(filename);
            var func = (CallbackFunction)this.script.LuaScript.Globals["timestamp"];
            double secondsStart = (DateTime.Now - unixStart).TotalSeconds;
            DynValue secondsReal = func.Invoke(null, null);
            double secondsEnd = (DateTime.Now - unixStart).TotalSeconds;
            Assert.IsTrue(secondsStart <= secondsReal.Number && secondsReal.Number <= secondsEnd);
        }
    }
}
