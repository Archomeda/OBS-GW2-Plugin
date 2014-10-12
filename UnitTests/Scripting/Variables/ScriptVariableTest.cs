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
using ObsGw2Plugin.Scripting.Variables;

namespace ObsGw2Plugin.UnitTests.Scripting.Variables
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ScriptVariableTest : ScriptBaseTest
    {
        public override void SetUp()
        {
            this.script = new ScriptVariable();
        }


        protected override string GetScriptFilename(string file)
        {
            return Path.Combine("Scripting", "Variables", "ScriptVariable" + file + ".lua");
        }

    }
}
