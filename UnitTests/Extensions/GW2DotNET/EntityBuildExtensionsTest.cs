using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GW2DotNET.Entities.Builds;
using NUnit.Framework;
using ObsGw2Plugin.Extensions.GW2DotNET;

namespace ObsGw2Plugin.UnitTests.Extensions.GW2DotNET
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class EntityBuildExtensionsTest
    {
        [Test]
        public void EntityBuildToDictionary()
        {
            Build build = new Build() { BuildId = 12345 };
            var expected = new Dictionary<string, object>()
            {
                {"build_id", build.BuildId},
            };
            var actual = build.ToDictionary();

            CollectionAssert.AreEquivalent(expected, actual);
        }
    }
}
