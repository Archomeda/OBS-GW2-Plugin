using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using NUnit.Framework;
using ObsGw2Plugin.Imaging.Animations;

namespace ObsGw2Plugin.UnitTests.Imaging.Animations
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FadeAnimatorTest
    {
        [TestCase(FadeMode.FadeIn, Result = 0d)]
        [TestCase(FadeMode.FadeOut, Result = 1d)]
        public double OpacityValueStart(FadeMode fadeMode)
        {
            FadeAnimator animator = new FadeAnimator(fadeMode);
            return animator.CurrentOpacity;
        }

        [TestCase(FadeMode.FadeIn, Result = 1d)]
        [TestCase(FadeMode.FadeOut, Result = 0d)]
        public double OpacityValueEnd(FadeMode fadeMode)
        {
            FadeAnimator animator = new FadeAnimator(fadeMode) { OpacityDeltaPerSecond = 1 };

            BitmapSource inBitmap = new RenderTargetBitmap(1, 1, 96, 96, PixelFormats.Pbgra32);
            BitmapSource outBitmap;
            animator.RenderNextFrame(inBitmap, DateTime.Now.AddSeconds(-1), out outBitmap);
            return animator.CurrentOpacity;
        }

        [Test]
        public void AnimationEventBusy()
        {
            FadeAnimator animator = new FadeAnimator(FadeMode.FadeIn) { OpacityDeltaPerSecond = 0.0001 };

            bool eventFired = false;
            animator.AnimationFinished += (s_, e_) => eventFired = true;

            BitmapSource inBitmap = new RenderTargetBitmap(1, 1, 96, 96, PixelFormats.Pbgra32);
            BitmapSource outBitmap;
            Assert.AreEqual(AnimationState.InProgress, animator.RenderNextFrame(inBitmap, DateTime.Now.AddSeconds(-1), out outBitmap));
            Assert.IsFalse(eventFired);
        }

        [Test]
        public void AnimationEventFinish()
        {
            FadeAnimator animator = new FadeAnimator(FadeMode.FadeIn) { OpacityDeltaPerSecond = 1 };

            bool eventFired = false;
            animator.AnimationFinished += (s_, e_) => eventFired = true;

            BitmapSource inBitmap = new RenderTargetBitmap(1, 1, 96, 96, PixelFormats.Pbgra32);
            BitmapSource outBitmap;
            Assert.AreEqual(AnimationState.InProgress, animator.RenderNextFrame(inBitmap, DateTime.Now.AddSeconds(-1), out outBitmap));
            Assert.IsTrue(eventFired);
            Assert.AreEqual(AnimationState.Finished, animator.RenderNextFrame(inBitmap, DateTime.Now.AddSeconds(-1), out outBitmap));
        }
    }
}
