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
    public class EntityMapFloorExtensionsTest
    {
        [Test]
        public void EntityMapFloorToDictionary()
        {
            int floorId = 1;
            int continentId = 2;
            Size2D textureDims = new Size2D(500, 1000);

            var expected = new Dictionary<string, object>()
            {
                { "floor_id", floorId },
                { "continent_id", continentId },
                { "texture_dims", new Dictionary<string, double>()
                    {
                        { "width", textureDims.Width },
                        { "height", textureDims.Height }
                    }
                },
                { "regions", new Dictionary<int, object>() }
            };

            Floor floor = new Floor()
            {
                FloorId = floorId,
                ContinentId = continentId,
                TextureDimensions = textureDims
            };

            var actual = floor.ToDictionary();

            Assert.AreEqual(expected, actual, "Floor");
            CollectionAssert.AreEquivalent((IDictionary<string, double>)expected["texture_dims"],
                (IDictionary<string, double>)actual["texture_dims"], "Texture Dims");
        }

        [Test]
        public void EntityMapFloorClampedViewToDictionary()
        {
            Rectangle clampedView = new Rectangle(new Point2D(10, 20), new Point2D(50, 60));

            var expected = new Dictionary<string, double>()
            {
                { "x", clampedView.X },
                { "y", clampedView.Y },
                { "width", clampedView.Width },
                { "height", clampedView.Height }
            };

            Floor floor = new Floor()
            {
                ClampedView = clampedView
            };

            var actual = floor.ToDictionary();
            CollectionAssert.AreEquivalent(expected, (IDictionary<string, double>)actual["clamped_view"]);
        }

        [Test]
        public void EntityMapFloorWithRegionToDictionary()
        {
            Region region = new Region() { RegionId = 123 };
            Floor floor = new Floor() { Regions = new Dictionary<int, Region>() { { region.RegionId, region } } };

            var expected = region.ToDictionary();
            var actualRegions = (IDictionary<int, object>)floor.ToDictionary()["regions"];
            var actual = (IDictionary<string, object>)actualRegions[region.RegionId];

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void EntityRegionToDictionary()
        {
            int regionId = 5;
            string name = "Crystal Desert";
            Point2D labelCoord = new Point2D(50, 25);

            var expected = new Dictionary<string, object>()
            {
                { "region_id", regionId },
                { "name", name },
                { "label_coord", new Dictionary<string, double>()
                    {
                        { "x", labelCoord.X },
                        { "y", labelCoord.Y }
                    }
                },
                { "maps", new Dictionary<int, object>() },
            };

            Region region = new Region()
            {
                RegionId = regionId,
                Name = name,
                LabelCoordinates = labelCoord
            };

            var actual = region.ToDictionary();

            Assert.AreEqual(expected, actual, "Region");
            CollectionAssert.AreEquivalent((IDictionary<string, double>)expected["label_coord"],
                (IDictionary<string, double>)actual["label_coord"], "Label Coords");
        }

        [Test]
        public void EntityRegionWithSubregionToDictionary()
        {
            Subregion subregion = new Subregion() { MapId = 111 };
            Region region = new Region() { Maps = new Dictionary<int, Subregion>() { { subregion.MapId, subregion } } };

            var expected = subregion.ToDictionary();
            var actualSubregions = (IDictionary<int, object>)region.ToDictionary()["maps"];
            var actual = (IDictionary<string, object>)actualSubregions[subregion.MapId];

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void EntitySubregionToDictionary()
        {
            int mapId = 1;
            string name = "Very Mists";
            int minLevel = 40;
            int maxLevel = 55;
            int defaultFloor = 5;
            Rectangle mapRectangle = new Rectangle(new Point2D(100, 200), new Point2D(200, 300));
            Rectangle continentRectangle = new Rectangle(new Point2D(10, 20), new Point2D(20, 30));

            var expected = new Dictionary<string, object>()
            {
                { "map_id", mapId },
                { "name", name },
                { "min_level", minLevel },
                { "max_level", maxLevel },
                { "default_floor", defaultFloor },
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
                },
                { "points_of_interest", new List<object>() },
                { "tasks", new List<object>() },
                { "skill_challenges", new List<object>() },
                { "sectors", new List<object>() }
            };

            Subregion subregion = new Subregion()
            {
                MapId = mapId,
                Name = name,
                MinimumLevel = minLevel,
                MaximumLevel = maxLevel,
                DefaultFloor = defaultFloor,
                MapRectangle = mapRectangle,
                ContinentRectangle = continentRectangle
            };

            var actual = subregion.ToDictionary();

            Assert.AreEqual(expected, actual, "Subregion");
            CollectionAssert.AreEquivalent((IDictionary<string, double>)expected["map_rect"],
                (IDictionary<string, double>)actual["map_rect"], "Map rectangle");
            CollectionAssert.AreEquivalent((IDictionary<string, double>)expected["continent_rect"],
                (IDictionary<string, double>)actual["continent_rect"], "Continent Rectangle");
        }

        [Test]
        public void EntitySubregionWithPointOfInterestToDictionary()
        {
            PointOfInterest pointOfInterest = new PointOfInterest() { PointOfInterestId = 54 };
            Subregion subregion = new Subregion() { PointsOfInterest = new List<PointOfInterest>() { pointOfInterest } };

            var expected = pointOfInterest.ToDictionary();
            var actualPointOfInterest = (ICollection<object>)subregion.ToDictionary()["points_of_interest"];
            var actual = (IDictionary<string, object>)actualPointOfInterest.First();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void EntitySubregionWithRenownTaskToDictionary()
        {
            RenownTask task = new RenownTask() { TaskId = 12 };
            Subregion subregion = new Subregion() { Tasks = new List<RenownTask>() { task } };

            var expected = task.ToDictionary();
            var actualPointOfInterest = (ICollection<object>)subregion.ToDictionary()["tasks"];
            var actual = (IDictionary<string, object>)actualPointOfInterest.First();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void EntitySubregionWithSkillChallengeToDictionary()
        {
            SkillChallenge skillChallenge = new SkillChallenge() { Coordinates = new Point2D(10, 20) };
            Subregion subregion = new Subregion() { SkillChallenges = new List<SkillChallenge>() { skillChallenge } };

            var expected = skillChallenge.ToDictionary();
            var actualPointOfInterest = (ICollection<object>)subregion.ToDictionary()["skill_challenges"];
            var actual = (IDictionary<string, object>)actualPointOfInterest.First();

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void EntitySubregionWithSectorToDictionary()
        {
            Sector sector = new Sector() { SectorId = 60 };
            Subregion subregion = new Subregion() { Sectors = new List<Sector>() { sector } };

            var expected = sector.ToDictionary();
            var actualPointOfInterest = (ICollection<object>)subregion.ToDictionary()["sectors"];
            var actual = (IDictionary<string, object>)actualPointOfInterest.First();

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> EnumeratePointOfInterestTypes()
        {
            return new List<object[]>() {
                new object[] { "unknown", new PointOfInterest() },
                new object[] { "landmark", new Landmark() },
                new object[] { "waypoint", new Waypoint() },
                new object[] { "vista", new Vista() }
            };
        }

        [Test, TestCaseSource(typeof(EntityMapFloorExtensionsTest), "EnumeratePointOfInterestTypes")]
        public void EntityPointOfInterestToDictionary(string typeName, PointOfInterest pointOfInterest)
        {
            int poiId = 4;
            string name = "Very interesting";
            int floor = 1;
            Point2D coord = new Point2D(500, 1000);

            var expected = new Dictionary<string, object>()
            {
                { "poi_id", poiId },
                { "name", name },
                { "type", typeName },
                { "floor", floor },
                { "coord", new Dictionary<string, double>()
                    {
                        { "x", coord.X },
                        { "y", coord.Y }
                    }
                },
            };

            pointOfInterest.PointOfInterestId = poiId;
            pointOfInterest.Name = name;
            pointOfInterest.Floor = floor;
            pointOfInterest.Coordinates = coord;

            var actual = pointOfInterest.ToDictionary();

            Assert.AreEqual(expected, actual, "Point of Interest");
            CollectionAssert.AreEquivalent((IDictionary<string, double>)expected["coord"],
                (IDictionary<string, double>)actual["coord"], "Coords");
        }

        [Test]
        public void EntityRenownTaskToDictionary()
        {
            int taskId = 101;
            string objective = "Do something tedious";
            int level = 42;
            Point2D coord = new Point2D(500, 1000);

            var expected = new Dictionary<string, object>()
            {
                { "task_id", taskId },
                { "objective", objective },
                { "level", level },
                { "coord", new Dictionary<string, double>()
                    {
                        { "x", coord.X },
                        { "y", coord.Y }
                    }
                },
            };

            RenownTask task = new RenownTask()
            {
                TaskId = taskId,
                Objective = objective,
                Level = level,
                Coordinates = coord,
            };

            var actual = task.ToDictionary();

            Assert.AreEqual(expected, actual, "Renown task");
            CollectionAssert.AreEquivalent((IDictionary<string, double>)expected["coord"],
                (IDictionary<string, double>)actual["coord"], "Coords");
        }

        [Test]
        public void EntitySkillChallengeToDictionary()
        {
            Point2D coord = new Point2D(500, 1000);

            var expected = new Dictionary<string, object>()
            {
                { "coord", new Dictionary<string, double>()
                    {
                        { "x", coord.X },
                        { "y", coord.Y }
                    }
                },
            };

            SkillChallenge skillChallenge = new SkillChallenge()
            {
                Coordinates = coord,
            };

            var actual = skillChallenge.ToDictionary();

            Assert.AreEqual(expected, actual, "Skill Challenge");
            CollectionAssert.AreEquivalent((IDictionary<string, double>)expected["coord"],
                (IDictionary<string, double>)actual["coord"], "Coords");
        }

        [Test]
        public void EntitySectorToDictionary()
        {
            int sectorId = 777;
            string name = "Your lucky sector";
            int level = 77;
            Point2D coord = new Point2D(500, 1000);

            var expected = new Dictionary<string, object>()
            {
                { "sector_id", sectorId },
                { "name", name },
                { "level", level },
                { "coord", new Dictionary<string, double>()
                    {
                        { "x", coord.X },
                        { "y", coord.Y }
                    }
                },
            };

            Sector sector = new Sector()
            {
                SectorId = sectorId,
                Name = name,
                Level = level,
                Coordinates = coord,
            };

            var actual = sector.ToDictionary();

            Assert.AreEqual(expected, actual, "Sector");
            CollectionAssert.AreEquivalent((IDictionary<string, double>)expected["coord"],
                (IDictionary<string, double>)actual["coord"], "Coords");
        }

    }
}
