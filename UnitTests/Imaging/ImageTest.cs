using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NUnit.Framework;
using ObsGw2Plugin.Imaging;

namespace ObsGw2Plugin.UnitTests.Imaging
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public abstract class ImageTest
    {
        protected Image Image;


        [SetUp]
        public abstract void SetUp();


        [Test]
        public void GetPixels()
        {
            BitmapSource bitmapSource = new RenderTargetBitmap(4, 1, 96, 96, PixelFormats.Pbgra32);
            this.Image.GetType().GetProperty("Bitmap").SetValue(this.Image, bitmapSource);

            Assert.IsNotNull(this.Image.GetPixels());
        }

        [Test]
        public void GetPixelsNull()
        {
            Assert.IsNull(this.Image.GetPixels());
        }

        public static IEnumerable<object[]> EnumerateProperties()
        {
            return new List<object[]>() {
                new object[] { "Bitmap", new RenderTargetBitmap(4, 1, 96, 96, PixelFormats.Pbgra32) },
                new object[] { "CustomWidth", 4 },
                new object[] { "CustomHeight", 8 },
            };
        }

        [Test, TestCaseSource(typeof(ImageTest), "EnumerateProperties")]
        public void NotifyPropertyChangedImageBase(string propertyName, object newValue)
        {
            List<string> actualProperties = new List<string>();
            this.Image.PropertyChanged += (s, e) => actualProperties.Add(e.PropertyName);

            // Test for change
            this.Image.GetType().GetProperty(propertyName).SetValue(this.Image, newValue);
            CollectionAssert.Contains(actualProperties, propertyName, "Event call");
            Assert.AreEqual(newValue, this.Image.GetType().GetProperty(propertyName).GetValue(this.Image), "Property change");

            // Test for no change
            actualProperties.Clear();
            this.Image.GetType().GetProperty(propertyName).SetValue(this.Image, newValue);
            CollectionAssert.DoesNotContain(actualProperties, propertyName, "No event call");
            Assert.AreEqual(newValue, this.Image.GetType().GetProperty(propertyName).GetValue(this.Image), "No property change");
        }

    }
}
