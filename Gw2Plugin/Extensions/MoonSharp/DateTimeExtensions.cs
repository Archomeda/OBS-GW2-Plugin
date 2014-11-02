using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObsGw2Plugin.Extensions.MoonSharp
{
    public static class DateTimeExtensions
    {
        public static IDictionary<string, int> ToDictionary(this DateTime dateTime)
        {
            return new Dictionary<string, int>()
            {
                { "year", dateTime.Year },
                { "month", dateTime.Month },
                { "day", dateTime.Day },
                { "day_of_week", (int)dateTime.DayOfWeek }, // 0 = Sunday, 6 = Saturday
                { "day_of_year", dateTime.DayOfYear },
                { "hour", dateTime.Hour },
                { "minute", dateTime.Minute },
                { "second", dateTime.Second },
                { "millisecond", dateTime.Millisecond }
            };
        }
    }
}
