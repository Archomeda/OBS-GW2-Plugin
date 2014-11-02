using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoonSharp.Interpreter;
using ObsGw2Plugin.MumbleLink;

namespace ObsGw2Plugin.Extensions.MoonSharp
{
    public static class Vector3Extensions
    {
        public static IDictionary<string, double> ToDictionary(this Vector3 vector3)
        {
            return new Dictionary<string, double>()
            {
                {"x", vector3.X},
                {"y", vector3.Y},
                {"z", vector3.Z}
            };
        }
    }
}
