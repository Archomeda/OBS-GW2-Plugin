using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using CLROBS;

namespace ObsGw2Plugin.Extensions
{
    public static class XElementExtensions
    {
        public static int ToInt(bool boolean)
        {
            return boolean ? 1 : 0;
        }

        public static bool ToBoolean(int integer)
        {
            return integer > 0;
        }


        public static bool GetBoolean(this XElement element, string name)
        {
            return ToBoolean(element.GetInt(name));
        }

        public static bool GetBoolean(this XElement element, string name, bool defaultBoolean)
        {
            return ToBoolean(element.GetInt(name, ToInt(defaultBoolean)));
        }

        public static void SetBoolean(this XElement element, string name, bool boolean)
        {
            element.SetInt(name, ToInt(boolean));
        }

        public static List<bool> GetBooleanList(this XElement element, string name)
        {
            return element.GetIntList(name).Select(i => ToBoolean(i)).ToList();
        }

        public static void SetBooleanList(this XElement element, string name, List<bool> booleanList)
        {
            element.SetIntList(name, booleanList.Select(b => ToInt(b)).ToList());
        }


        public static Color GetColor2(this XElement element, string name)
        {
            return element.GetColor(name).GetColor();
        }
        public static Color GetColor2(this XElement element, string name, Color defaultColor)
        {
            return element.GetColor(name, defaultColor.GetColorInt()).GetColor();
        }

        public static void SetColor2(this XElement element, string name, Color color)
        {
            element.SetColor(name, color.GetColorInt());
        }



        public static List<Color> GetColorList2(this XElement element, string name)
        {
            return element.GetColorList(name).Select(i => i.GetColor()).ToList();
        }

        public static void SetColorList2(this XElement element, string name, List<Color> colorList)
        {
            element.SetColorList(name, colorList.Select(c => c.GetColorInt()).ToList());
        }

    }
}
