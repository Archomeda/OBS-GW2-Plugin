using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ObsGw2Plugin.Imaging.Animations
{
    public class FadeAnimator : IAnimator
    {
        public FadeAnimator()
            : this(FadeMode.FadeIn)
        {
            this.OpacityDeltaPerSecond = 1;
        }

        public FadeAnimator(FadeMode fadeMode)
        {
            this.FadeMode = fadeMode;
            this.CurrentOpacity = fadeMode == FadeMode.FadeIn ? 0 : 1;
        }


        public double OpacityDeltaPerSecond { get; set; }

        public FadeMode FadeMode { get; set; }

        public double CurrentOpacity { get; set; }


        #region IAnimator members

        private bool animationFinished = false;

        public virtual AnimationState RenderNextFrame(BitmapSource sourceBitmap, DateTime prevUpdate, out BitmapSource outBitmap)
        {
            if (this.animationFinished)
            {
                outBitmap = sourceBitmap;
                return AnimationState.Finished;
            }

            TimeSpan timeDiff = DateTime.Now - prevUpdate;
            double opacityDelta = timeDiff.TotalSeconds * this.OpacityDeltaPerSecond;
            double newOpacity = this.CurrentOpacity + (this.FadeMode == FadeMode.FadeIn ? opacityDelta : -opacityDelta);

            if (newOpacity > 1)
                newOpacity = 1;
            else if (newOpacity < 0)
                newOpacity = 0;

            if (this.CurrentOpacity != newOpacity)
            {
                ImageBrush imageBrush = new ImageBrush(sourceBitmap)
                {
                    Stretch = Stretch.None,
                    TileMode = TileMode.None,
                    Opacity = newOpacity,
                };

                DrawingVisual visual = new DrawingVisual();
                using (DrawingContext context = visual.RenderOpen())
                {
                    context.DrawRectangle(imageBrush, null, new Rect(0, 0, sourceBitmap.PixelWidth, sourceBitmap.PixelHeight));
                }

                RenderTargetBitmap render = new RenderTargetBitmap(sourceBitmap.PixelWidth, sourceBitmap.PixelHeight, 96, 96, PixelFormats.Pbgra32);
                render.Render(visual);
                render.Freeze();
                outBitmap = render;

                this.CurrentOpacity = newOpacity;
                if (newOpacity == 0 || newOpacity == 1)
                {
                    this.OnAnimationFinished(this, new AnimationFinishedEventArgs());
                    this.animationFinished = true;
                }

                return AnimationState.InProgress;
            }

            outBitmap = sourceBitmap;
            return AnimationState.NoChange;
        }

        public void ResetState()
        {
            this.CurrentOpacity = this.FadeMode == FadeMode.FadeIn ? 0 : 1;
            this.animationFinished = false;
        }


        protected virtual void OnAnimationFinished(object sender, AnimationFinishedEventArgs e)
        {
            var h = this.AnimationFinished;
            if (h != null)
                h(sender, e);
        }

        public event EventHandler<AnimationFinishedEventArgs> AnimationFinished;

        #endregion

    }

    public enum FadeMode
    {
        FadeIn,
        FadeOut
    }
}
