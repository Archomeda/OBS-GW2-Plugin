using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ObsGw2Plugin.Scripting.Exceptions;

namespace ObsGw2Plugin.UnitTests.Scripting.Exceptions
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class MissingGlobalExceptionTest
    {
        [Test]
        public void Constructor()
        {
            string filename = "FileA.txt";
            string globalName = "FileA";
            MissingGlobalException exception = new MissingGlobalException(filename, globalName);

            Assert.AreEqual(filename, exception.ScriptFilename);
            Assert.AreEqual(globalName, exception.GlobalName);
        }
    }
}
