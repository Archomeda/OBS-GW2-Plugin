using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GW2DotNET.Entities.Maps;

namespace ObsGw2Plugin.Extensions.GW2DotNET
{
    public static class EntityMapFloorExtensions
    {
        public static IDictionary<string, object> ToDictionary(this Floor floor)
        {
            var result = new Dictionary<string, object>()
            {
                { "floor_id", floor.FloorId },
                { "continent_id", floor.ContinentId },
                { "texture_dims", new Dictionary<string, double>()
                    {
                        { "width", floor.TextureDimensions.Width },
                        { "height", floor.TextureDimensions.Height }
                    }
                },
                { "regions", new Dictionary<int, object>() }
            };

            if (floor.ClampedView.HasValue)
            {
                result["clamped_view"] = new Dictionary<string, double>()
                {
                    { "x", floor.ClampedView.Value.X },
                    { "y", floor.ClampedView.Value.Y },
                    { "width", floor.ClampedView.Value.Width },
                    { "height", floor.ClampedView.Value.Height }
                };
            }

            if (floor.Regions != null)
            {
                foreach (var region in floor.Regions)
                {
                    ((IDictionary<int, object>)result["regions"])[region.Key] = region.Value.ToDictionary();
                }
            }

            return result;
        }

        public static IDictionary<string, object> ToDictionary(this Region region)
        {
            var regionResult = new Dictionary<string, object>()
            {
                { "region_id", region.RegionId },
                { "name", region.Name },
                { "label_coord", new Dictionary<string, double>()
                    {
                        { "x", region.LabelCoordinates.X },
                        { "y", region.LabelCoordinates.Y }
                    }
                },
                { "maps", new Dictionary<int, object>() }
            };

            if (region.Maps != null)
            {
                foreach (var map in region.Maps)
                {
                    ((IDictionary<int, object>)regionResult["maps"])[map.Key] = map.Value.ToDictionary();
                }
            }

            return regionResult;
        }

        public static IDictionary<string, object> ToDictionary(this Subregion subRegion)
        {
            var subRegionResult = new Dictionary<string, object>()
            {
                { "map_id", subRegion.MapId },
                { "name", subRegion.Name },
                { "min_level", subRegion.MinimumLevel },
                { "max_level", subRegion.MaximumLevel },
                { "default_floor", subRegion.DefaultFloor },
                { "map_rect", new Dictionary<string, double>()
                    {
                        { "x", subRegion.MapRectangle.X },
                        { "y", subRegion.MapRectangle.Y },
                        { "width", subRegion.MapRectangle.Width },
                        { "height", subRegion.MapRectangle.Height }
                    }
                },
                { "continent_rect", new Dictionary<string, double>()
                    {
                        { "x", subRegion.ContinentRectangle.X },
                        { "y", subRegion.ContinentRectangle.Y },
                        { "width", subRegion.ContinentRectangle.Width },
                        { "height", subRegion.ContinentRectangle.Height }
                    }
                },
                { "points_of_interest", new List<object>() },
                { "tasks", new List<object>() },
                { "skill_challenges", new List<object>() },
                { "sectors", new List<object>() }
            };

            if (subRegion.PointsOfInterest != null)
            {
                foreach (var poi in subRegion.PointsOfInterest)
                {
                    ((IList<object>)subRegionResult["points_of_interest"]).Add(poi.ToDictionary());
                }
            }

            if (subRegion.Tasks != null)
            {
                foreach (var task in subRegion.Tasks)
                {
                    ((IList<object>)subRegionResult["tasks"]).Add(task.ToDictionary());
                }
            }

            if (subRegion.SkillChallenges != null)
            {
                foreach (var skillChallenge in subRegion.SkillChallenges)
                {
                    ((IList<object>)subRegionResult["skill_challenges"]).Add(skillChallenge.ToDictionary());
                }
            }

            if (subRegion.Sectors != null)
            {
                foreach (var sector in subRegion.Sectors)
                {
                    ((IList<object>)subRegionResult["sectors"]).Add(sector.ToDictionary());
                }
            }

            return subRegionResult;
        }

        public static IDictionary<string, object> ToDictionary(this PointOfInterest pointOfInterest)
        {
            string pointOfInterestType = "unknown";
            if (pointOfInterest.GetType() == typeof(Landmark))
                pointOfInterestType = "landmark";
            else if (pointOfInterest.GetType() == typeof(Waypoint))
                pointOfInterestType = "waypoint";
            else if (pointOfInterest.GetType() == typeof(Vista))
                pointOfInterestType = "vista";

            return new Dictionary<string, object>()
            {
                { "poi_id", pointOfInterest.PointOfInterestId },
                { "name", pointOfInterest.Name },
                { "type", pointOfInterestType },
                { "floor", pointOfInterest.Floor },
                { "coord", new Dictionary<string, double>()
                    {
                        { "x", pointOfInterest.Coordinates.X },
                        { "y", pointOfInterest.Coordinates.Y }
                    }
                }
            };
        }

        public static IDictionary<string, object> ToDictionary(this RenownTask task)
        {
            return new Dictionary<string, object>()
            {
                { "task_id", task.TaskId },
                { "objective", task.Objective },
                { "level", task.Level },
                { "coord", new Dictionary<string, double>()
                    {
                        { "x", task.Coordinates.X },
                        { "y", task.Coordinates.Y }
                    }
                }
            };
        }

        public static IDictionary<string, object> ToDictionary(this SkillChallenge skillChallenge)
        {
            return new Dictionary<string, object>()
            {
                { "coord", new Dictionary<string, double>()
                    {
                        { "x", skillChallenge.Coordinates.X },
                        { "y", skillChallenge.Coordinates.Y }
                    }
                }
            };
        }

        public static IDictionary<string, object> ToDictionary(this Sector sector)
        {
            return new Dictionary<string, object>()
            {
                { "sector_id", sector.SectorId },
                { "name", sector.Name },
                { "level", sector.Level },
                { "coord", new Dictionary<string, double>()
                    {
                        { "x", sector.Coordinates.X },
                        { "y", sector.Coordinates.Y }
                    }
                }
            };
        }

    }
}
