using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;
using NUnit.Framework;


namespace ObsGw2Plugin.UnitTests
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ScriptSyntaxValidators
    {
        [TestFixtureSetUp]
        public void Init()
        {
            Script.WarmUp();
        }

        static IEnumerable<string> EnumerateScriptFilenames()
        {
            return Directory.EnumerateFiles(Path.Combine("Gw2Plugin", "ScriptFormatters"), "*.lua")
                .Concat(Directory.EnumerateFiles(Path.Combine("Gw2Plugin", "ScriptVariables"), "*.lua"));
        }

        [Test, TestCaseSource("EnumerateScriptFilenames")]
        public void ValidateScript(string filename)
        {
            Assert.DoesNotThrow(() => Script.RunFile(filename));
        }
    }
}
