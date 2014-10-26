using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GW2DotNET.Entities.Maps;

namespace ObsGw2Plugin.Extensions.GW2DotNET
{
    public static class EntityContinentExtensions
    {
        public static IDictionary<string, object> ToDictionary(this Continent continent)
        {
            return new Dictionary<string, object>()
            {
                { "continent_id", continent.ContinentId },
                { "name", continent.Name },
                { "continent_dims", new Dictionary<string, double>()
                    {
                        { "width", continent.ContinentDimensions.Width },
                        { "height", continent.ContinentDimensions.Height }
                    }
                },
                { "min_zoom", continent.MinimumZoom },
                { "max_zoom", continent.MaximumZoom },
                { "floors", continent.FloorIds },
            };
        }
    }
}
