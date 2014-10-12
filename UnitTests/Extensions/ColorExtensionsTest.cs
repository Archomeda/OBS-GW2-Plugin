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
    public class ColorExtensionsTest
    {
        [Test]
        public void GetColorUInt()
        {
            Color color = Color.FromArgb(0xFE, 0xDC, 0xBA, 0x98);
            uint expected = 0xFEDCBA98;
            uint actual = color.GetColorUInt();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetColorInt()
        {
            unchecked
            {
                Color color = Color.FromArgb(0xFE, 0xDC, 0xBA, 0x98);
                int expected = (int)0xFEDCBA98;
                int actual = color.GetColorInt();

                Assert.AreEqual(expected, actual);
            }
        }

        [Test]
        public void GetColorFromUInt()
        {
            uint color = 0xFEDCBA98;
            Color expected = Color.FromArgb(0xFE, 0xDC, 0xBA, 0x98);
            Color actual = color.GetColor();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetColorFromInt()
        {
            unchecked
            {
                int color = (int)0xFEDCBA98;
                Color expected = Color.FromArgb(0xFE, 0xDC, 0xBA, 0x98);
                Color actual = color.GetColor();

                Assert.AreEqual(expected, actual);
            }
        }
    }
}
