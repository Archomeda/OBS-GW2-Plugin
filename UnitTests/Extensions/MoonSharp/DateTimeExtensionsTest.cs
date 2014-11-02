using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ObsGw2Plugin.Extensions.MoonSharp;

namespace ObsGw2Plugin.UnitTests.Extensions.MoonSharp
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class DateTimeExtensionsTest
    {
        [Test]
        public void DateTimeToDictionary()
        {
            DateTime dateTime = DateTime.Now;
            var expected = new Dictionary<string, int>()
            {
                { "year", dateTime.Year },
                { "month", dateTime.Month },
                { "day", dateTime.Day },
                { "day_of_week", (int)dateTime.DayOfWeek },
                { "day_of_year", dateTime.DayOfYear },
                { "hour", dateTime.Hour },
                { "minute", dateTime.Minute },
                { "second", dateTime.Second },
                { "millisecond", dateTime.Millisecond }
            };
            var actual = dateTime.ToDictionary();

            CollectionAssert.AreEquivalent(expected, actual);
        }

    }
}
