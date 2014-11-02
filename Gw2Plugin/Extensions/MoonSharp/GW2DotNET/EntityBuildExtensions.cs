using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GW2DotNET.Entities.Builds;

namespace ObsGw2Plugin.Extensions.MoonSharp.GW2DotNET
{
    public static class EntityBuildExtensions
    {
        public static IDictionary<string, object> ToDictionary(this Build build)
        {
            return new Dictionary<string, object>()
            {
                { "build_id", build.BuildId }
            };
        }
    }
}
