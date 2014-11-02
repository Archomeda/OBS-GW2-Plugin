using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GW2DotNET.Entities.Maps;

namespace ObsGw2Plugin.Extensions.MoonSharp.GW2DotNET
{
    public static class EntityMapExtensions
    {
        public static IDictionary<string, object> ToDictionary(this Map map)
        {
            return new Dictionary<string, object>()
            {
                { "map_id", map.MapId },
                { "map_name", map.MapName },
                { "min_level", map.MinimumLevel },
                { "max_level", map.MaximumLevel },
                { "default_floor", map.DefaultFloor },
                { "floors", map.Floors },
                { "region_id", map.RegionId },
                { "region_name", map.RegionName },
                { "continent_id", map.ContinentId },
                { "continent_name", map.ContinentName },
                { "map_rect", new Dictionary<string, double>()
                    {
                        { "x", map.MapRectangle.X },
                        { "y", map.MapRectangle.Y },
                        { "width", map.MapRectangle.Width },
                        { "height", map.MapRectangle.Height }
                    }
                },
                { "continent_rect", new Dictionary<string, double>()
                    {
                        { "x", map.ContinentRectangle.X },
                        { "y", map.ContinentRectangle.Y },
                        { "width", map.ContinentRectangle.Width },
                        { "height", map.ContinentRectangle.Height }
                    }
                }
            };
        }
    }
}
