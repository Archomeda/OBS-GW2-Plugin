using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using NUnit.Framework;
using ObsGw2Plugin.Extensions;
using ObsGw2Plugin.MumbleLink;

namespace ObsGw2Plugin.UnitTests.Extensions
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class DynValueExtensionsTest
    {
        [Test]
        public void Vector3ToDictionary()
        {
            Vector3 vector = new Vector3(1, 2, 3);
            var expected = new Dictionary<string, double>()
            {
                {"x", 1},
                {"y", 2},
                {"z", 3}
            };
            var actual = vector.ToDictionary();

            CollectionAssert.AreEquivalent(expected, actual);
        }
    }
}
