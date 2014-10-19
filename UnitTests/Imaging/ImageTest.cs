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
using ObsGw2Plugin.UnitTests.Utils;

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


        #region NotifyPropertyChanged tests

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
            PropertyUtils.TestNotifyPropertyChanged(this.Image, propertyName, newValue);
        }

        #endregion

    }
}
