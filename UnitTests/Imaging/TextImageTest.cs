using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NSubstitute;
using NUnit.Framework;
using ObsGw2Plugin.Imaging;
using ObsGw2Plugin.Imaging.Animations;

namespace ObsGw2Plugin.UnitTests.Imaging
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class TextImageTest : ImageTest
    {
        protected TextImage TextImage
        {
            get { return (TextImage)this.Image; }
            set { this.Image = value; }
        }

        public override void SetUp()
        {
            this.TextImage = new TextImage();
        }


        [Test]
        public void Constructor()
        {
            Assert.NotNull(this.TextImage.Animators);
        }

        [Test]
        public void CustomSize()
        {
            int customWidth = 300;
            int customHeight = 200;
            this.TextImage.CustomWidth = customWidth;
            this.TextImage.CustomHeight = customHeight;
            Assert.AreEqual(customWidth, this.TextImage.Bitmap.PixelWidth, "Width");
            Assert.AreEqual(customHeight, this.TextImage.Bitmap.PixelHeight, "Height");
        }

        [Test]
        public void StartStopAnimation()
        {
            Assert.IsFalse(this.TextImage.AnimationActive, "Not active at start");
            this.TextImage.StartAnimation();
            Assert.IsTrue(this.TextImage.AnimationActive, "Active after start");
            this.TextImage.StopAnimation();
            Assert.IsFalse(this.TextImage.AnimationActive, "Not active after stop");
        }

        [Test]
        public void ResetAnimation()
        {
            IAnimator animator = Substitute.For<IAnimator>();
            this.TextImage.Animators.Add(animator);
            this.TextImage.ResetAnimation();
            animator.Received(1).ResetState();
        }

        [Test]
        public void AnimateFrame()
        {
            IAnimator animator = Substitute.For<IAnimator>();
            BitmapSource result;
            animator.RenderNextFrame(null, new DateTime(), out result).ReturnsForAnyArgs(
                p =>
                {
                    p[2] = new RenderTargetBitmap(1, 1, 96, 96, PixelFormats.Pbgra32);
                    return AnimationState.InProgress;
                },
                p =>
                {
                    p[2] = new RenderTargetBitmap(1, 1, 96, 96, PixelFormats.Pbgra32);
                    return AnimationState.NoChange;
                }
            );
            this.TextImage.Animators.Add(animator);
            this.TextImage.StartAnimation();
            Assert.IsTrue(this.TextImage.AnimateFrame(), "In progress");
            Assert.IsFalse(this.TextImage.AnimateFrame(), "No change");
        }

        [Test]
        public void NoAnimateFrameWhenNotAnimated()
        {
            Assert.IsFalse(this.TextImage.AnimateFrame());
        }


        public new static IEnumerable<object[]> EnumerateProperties()
        {
            return new List<object[]>() {
                new object[] { "Text", "TextHere" },
                new object[] { "FontFamily", new FontFamily("Courier New") },
                new object[] { "FontSize", 96 },
                new object[] { "TextColor", Colors.Azure },
                new object[] { "BackColor", Colors.ForestGreen },
                new object[] { "EffectBold", true },
                new object[] { "EffectItalic", true },
                new object[] { "EffectUnderline", true },
                new object[] { "OutlineColor", Colors.Violet },
                new object[] { "OutlineThickness", 4 },
            };
        }

        [Test, TestCaseSource(typeof(TextImageTest), "EnumerateProperties")]
        public void NotifyPropertyChangedTextImage(string propertyName, object newValue)
        {
            List<string> actualProperties = new List<string>();
            this.TextImage.PropertyChanged += (s, e) => actualProperties.Add(e.PropertyName);

            // Test for change
            this.TextImage.GetType().GetProperty(propertyName).SetValue(this.TextImage, newValue);
            CollectionAssert.Contains(actualProperties, propertyName, "Event call");
            Assert.AreEqual(newValue, this.TextImage.GetType().GetProperty(propertyName).GetValue(this.TextImage), "Property change");

            // Test for no change
            actualProperties.Clear();
            this.TextImage.GetType().GetProperty(propertyName).SetValue(this.TextImage, newValue);
            CollectionAssert.DoesNotContain(actualProperties, propertyName, "No event call");
            Assert.AreEqual(newValue, this.TextImage.GetType().GetProperty(propertyName).GetValue(this.TextImage), "No property change");
        }

    }
}
