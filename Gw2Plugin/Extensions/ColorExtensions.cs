using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ObsGw2Plugin.Extensions
{
    public static class ColorExtensions
    {
        public static uint GetColorUInt(this Color color)
        {
            return (uint)color.A << 24 | (uint)color.R << 16 | (uint)color.G << 8 | (uint)color.B;
        }

        public static int GetColorInt(this Color color)
        {
            return (int)GetColorUInt(color);
        }

        public static Color GetColor(this uint colorUInt)
        {
            byte a = (byte)(colorUInt >> 24);
            byte r = (byte)(colorUInt >> 16);
            byte g = (byte)(colorUInt >> 8);
            byte b = (byte)colorUInt;
            return Color.FromArgb(a, r, g, b);
        }

        public static Color GetColor(this int colorInt)
        {
            return GetColor((uint)colorInt);
        }
    }
}
