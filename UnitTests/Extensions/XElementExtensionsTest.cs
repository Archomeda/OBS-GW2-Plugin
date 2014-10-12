using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using NUnit.Framework;
using ObsGw2Plugin.Extensions;

namespace ObsGw2Plugin.UnitTests.Extensions
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class XElementExtensionsTest
    {
        [Test]
        public void BooleanToIntFalse()
        {
            bool boolean = false;
            int expected = 0;
            int actual = XElementExtensions.ToInt(boolean);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void BooleanToIntTrue()
        {
            bool boolean = true;
            int expected = 1;
            int actual = XElementExtensions.ToInt(boolean);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IntToBooleanFalse()
        {
            int integer = 0;
            bool expected = false;
            bool actual = XElementExtensions.ToBoolean(integer);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IntToBooleanTrue()
        {
            int integer = 1;
            bool expected = true;
            bool actual = XElementExtensions.ToBoolean(integer);

            Assert.AreEqual(expected, actual);
        }
    }
}
