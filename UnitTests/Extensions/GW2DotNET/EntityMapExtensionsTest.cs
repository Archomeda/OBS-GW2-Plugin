using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GW2DotNET.Entities.Maps;
using NUnit.Framework;
using ObsGw2Plugin.Extensions.GW2DotNET;

namespace ObsGw2Plugin.UnitTests.Extensions.GW2DotNET
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class EntityMapExtensionsTest
    {
        [Test]
        public void EntityMapToDictionary()
        {
            int mapId = 1;
            string mapName = "Very Mists";
            int minLevel = 40;
            int maxLevel = 55;
            int defaultFloor = 5;
            List<int> floors = new List<int>() { 3, 5 };
            int regionId = 3;
            string regionName = "The Deep Mists";
            int continentId = 10;
            string continentName = "Mists of the Deep";
            Rectangle mapRectangle = new Rectangle(new Point2D(100, 200), new Point2D(200, 300));
            Rectangle continentRectangle = new Rectangle(new Point2D(10, 20), new Point2D(20, 30));

            var expected = new Dictionary<string, object>()
            {
                { "map_id", mapId },
                { "map_name", mapName },
                { "min_level", minLevel },
                { "max_level", maxLevel },
                { "default_floor", defaultFloor },
                { "floors", floors },
                { "region_id", regionId },
                { "region_name", regionName },
                { "continent_id", continentId },
                { "continent_name", continentName },
                { "map_rect", new Dictionary<string, double>()
                    {
                        { "x", mapRectangle.X },
                        { "y", mapRectangle.Y },
                        { "width", mapRectangle.Width },
                        { "height", mapRectangle.Height }
                    }
                },
                { "continent_rect", new Dictionary<string, double>()
                    {
                        { "x", continentRectangle.X },
                        { "y", continentRectangle.Y },
                        { "width", continentRectangle.Width },
                        { "height", continentRectangle.Height }
                    }
                }
            };

            Map map = new Map()
            {
                MapId = mapId,
                MapName = mapName,
                MinimumLevel = minLevel,
                MaximumLevel = maxLevel,
                DefaultFloor = defaultFloor,
                Floors = floors,
                RegionId = regionId,
                RegionName = regionName,
                ContinentId = continentId,
                ContinentName = continentName,
                MapRectangle = mapRectangle,
                ContinentRectangle = continentRectangle
            };

            var actual = map.ToDictionary();

            Assert.AreEqual(expected, actual, "Map");
            CollectionAssert.AreEqual((IList<int>)expected["floors"], (IList<int>)actual["floors"], "Floors");
            CollectionAssert.AreEquivalent((IDictionary<string, double>)expected["map_rect"],
                (IDictionary<string, double>)actual["map_rect"], "Map rectangle");
            CollectionAssert.AreEquivalent((IDictionary<string, double>)expected["continent_rect"],
                (IDictionary<string, double>)actual["continent_rect"], "Continent Rectangle");
        }
    }
}
