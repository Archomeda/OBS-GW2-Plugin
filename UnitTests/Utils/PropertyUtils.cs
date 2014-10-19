using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace ObsGw2Plugin.UnitTests.Utils
{
    [ExcludeFromCodeCoverage]
    public static class PropertyUtils
    {
        public static void TestNotifyPropertyChanged(INotifyPropertyChanged obj, string propertyName, object newValue)
        {
            List<string> actualProperties = new List<string>();
            obj.PropertyChanged += (s, e) => actualProperties.Add(e.PropertyName);

            // Test for change
            obj.GetType().GetProperty(propertyName).SetValue(obj, newValue);
            CollectionAssert.Contains(actualProperties, propertyName, "Event call");
            Assert.AreEqual(newValue, obj.GetType().GetProperty(propertyName).GetValue(obj), "Property change");

            // Test for no change
            actualProperties.Clear();
            obj.GetType().GetProperty(propertyName).SetValue(obj, newValue);
            CollectionAssert.DoesNotContain(actualProperties, propertyName, "No event call");
            Assert.AreEqual(newValue, obj.GetType().GetProperty(propertyName).GetValue(obj), "No property change");
        }
    }
}
