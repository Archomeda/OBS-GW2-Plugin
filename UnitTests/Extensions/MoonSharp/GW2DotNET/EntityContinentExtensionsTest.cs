using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GW2DotNET.Entities.Maps;
using NUnit.Framework;
using ObsGw2Plugin.Extensions.MoonSharp.GW2DotNET;

namespace ObsGw2Plugin.UnitTests.Extensions.MoonSharp.GW2DotNET
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class EntityContinentExtensionsTest
    {
        [Test]
        public void EntityContinentToDictionary()
        {
            int continentId = 1;
            string name = "Tyria";
            Size2D continentDims = new Size2D(500, 1000);
            int minZoom = 2;
            int maxZoom = 8;
            List<int> floors = new List<int>() { 3, 5 };

            var expected = new Dictionary<string, object>()
            {
                { "continent_id", continentId },
                { "name", name },
                { "continent_dims", new Dictionary<string, double>()
                    {
                        { "width", continentDims.Width },
                        { "height", continentDims.Height }
                    }
                },
                { "min_zoom", minZoom },
                { "max_zoom", maxZoom },
                { "floors", floors },
            };

            Continent continent = new Continent()
            {
                ContinentId = continentId,
                Name = name,
                ContinentDimensions = continentDims,
                MinimumZoom = minZoom,
                MaximumZoom = maxZoom,
                FloorIds = floors
            };

            var actual = continent.ToDictionary();

            Assert.AreEqual(expected, actual, "Continent");
            CollectionAssert.AreEquivalent((IDictionary<string, double>)expected["continent_dims"],
                (IDictionary<string, double>)actual["continent_dims"], "Continent Dims");
            CollectionAssert.AreEqual((IList<int>)expected["floors"], (IList<int>)actual["floors"], "Floors");
        }
    }
}
