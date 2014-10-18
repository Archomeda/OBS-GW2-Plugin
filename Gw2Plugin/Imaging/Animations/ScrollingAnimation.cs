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
    public class ScrollingAnimation : IAnimator
    {
        public ScrollingAnimation()
        {
            this.PixelsPerSecond = 5;
            this.ScrollMode = ScrollMode.TooWideOnly;
        }


        private bool invalidated = false;
        private ScrollMode scrollMode;
        private AlignmentX textAlign;


        public IImage DelimiterImage { get; set; }

        public int PixelsPerSecond { get; set; }

        public ScrollMode ScrollMode
        {
            get { return this.scrollMode; }
            set
            {
                if (this.scrollMode != value)
                {
                    this.scrollMode = value;
                    this.invalidated = true;
                }
            }
        }

        public int MaxWidth { get; set; }

        public AlignmentX TextAlign
        {
            get { return this.textAlign; }
            set
            {
                if (this.textAlign != value)
                {
                    this.textAlign = value;
                    this.invalidated = true;
                }
            }
        }

        public double CurrentOffsetX { get; set; }


        private BitmapSource prevSourceBitmap = null;
        private BitmapSource cachedBitmapWithDelimiter = null;

        private void CacheBitmapWithDelimiter(BitmapSource sourceBitmap)
        {
            if (this.DelimiterImage != null)
            {
                int renderWidth = sourceBitmap.PixelWidth + this.DelimiterImage.Bitmap.PixelWidth;
                int renderHeight = sourceBitmap.PixelHeight;

                ImageBrush imageBrushImage = new ImageBrush(sourceBitmap)
                {
                    AlignmentX = AlignmentX.Left,
                    Stretch = Stretch.None,
                    TileMode = TileMode.None,
                };

                ImageBrush imageBrushDelimiter = new ImageBrush(this.DelimiterImage.Bitmap)
                {
                    AlignmentX = AlignmentX.Left,
                    AlignmentY = AlignmentY.Center,
                    Stretch = Stretch.None,
                    TileMode = TileMode.None,
                };

                DrawingVisual visual = new DrawingVisual();
                using (DrawingContext context = visual.RenderOpen())
                {
                    context.DrawRectangle(imageBrushImage, null, new Rect(0, 0, sourceBitmap.PixelWidth, sourceBitmap.PixelHeight));

                    context.DrawRectangle(imageBrushDelimiter, null,
                        new Rect(sourceBitmap.PixelWidth, 0, this.DelimiterImage.Bitmap.PixelWidth, this.DelimiterImage.Bitmap.PixelHeight));
                }

                RenderTargetBitmap render = new RenderTargetBitmap(renderWidth, renderHeight, 96, 96, PixelFormats.Pbgra32);
                render.Render(visual);
                render.Freeze();

                this.cachedBitmapWithDelimiter = render;
            }
            else
            {
                this.cachedBitmapWithDelimiter = sourceBitmap;
            }
        }


        #region IAnimator members

        private bool animationFinished = false;

        public virtual AnimationState RenderNextFrame(BitmapSource sourceBitmap, DateTime prevUpdate, out BitmapSource outBitmap)
        {
            if (!object.Equals(this.prevSourceBitmap, sourceBitmap))
            {
                this.CacheBitmapWithDelimiter(sourceBitmap);
                this.prevSourceBitmap = sourceBitmap;
                this.invalidated = true;
            }

            switch (this.ScrollMode)
            {
                case ScrollMode.TooWideOnly:
                    return this.RenderNextFrame_ScrollTooWideOnly(sourceBitmap, prevUpdate, out outBitmap);
                case ScrollMode.ForcedOnce:
                    return this.RenderNextFrame_ScrollForcedOnce(sourceBitmap, prevUpdate, out outBitmap);
                case ScrollMode.ForcedContinuous:
                    return this.RenderNextFrame_ScrollForcedContinuous(sourceBitmap, prevUpdate, out outBitmap);
                default:
                    outBitmap = sourceBitmap;
                    return AnimationState.NoChange;
            }
        }

        private AnimationState RenderNextFrame_ScrollTooWideOnly(BitmapSource sourceBitmap, DateTime prevUpdate, out BitmapSource outBitmap)
        {
            if (sourceBitmap.PixelWidth > this.MaxWidth)
                return this.RenderNextFrame_ScrollForcedContinuous(sourceBitmap, prevUpdate, out outBitmap);

            int viewportWidth = this.MaxWidth;
            int viewportHeight = sourceBitmap.PixelHeight;
            int renderWidth = this.MaxWidth;
            int renderHeight = sourceBitmap.PixelHeight;

            if (this.invalidated)
            {
                ImageBrush imageBrush = new ImageBrush(sourceBitmap)
                {
                    AlignmentX = this.TextAlign,
                    Stretch = Stretch.None,
                    TileMode = TileMode.None,
                    Viewport = new Rect(0, 0, viewportWidth, viewportHeight),
                    ViewportUnits = BrushMappingMode.Absolute
                };

                outBitmap = this.DrawImageBrushToRenderer(imageBrush, renderWidth, renderHeight);
                this.invalidated = false;
                return AnimationState.InProgress;
            }

            outBitmap = sourceBitmap;
            return AnimationState.NoChange;
        }

        private AnimationState RenderNextFrame_ScrollForcedOnce(BitmapSource sourceBitmap, DateTime prevUpdate, out BitmapSource outBitmap)
        {
            if (animationFinished)
            {
                outBitmap = sourceBitmap;
                return AnimationState.Finished;
            }

            int viewportWidth = sourceBitmap.PixelWidth;
            int viewportHeight = sourceBitmap.PixelHeight;
            int renderWidth = this.MaxWidth;
            int renderHeight = sourceBitmap.PixelHeight;

            TimeSpan timeDiff = DateTime.Now - prevUpdate;
            double newOffsetX = this.CurrentOffsetX + (timeDiff.TotalSeconds * this.PixelsPerSecond);

            if (this.CurrentOffsetX != newOffsetX || this.invalidated)
            {
                ImageBrush imageBrush = new ImageBrush(sourceBitmap)
                {
                    AlignmentX = AlignmentX.Left,
                    Stretch = Stretch.None,
                    TileMode = TileMode.None,
                    Viewport = new Rect(-newOffsetX + renderWidth, 0, viewportWidth, viewportHeight),
                    ViewportUnits = BrushMappingMode.Absolute
                };

                outBitmap = this.DrawImageBrushToRenderer(imageBrush, renderWidth, renderHeight);
                this.CurrentOffsetX = newOffsetX;
                this.invalidated = false;

                if (newOffsetX > viewportWidth + renderWidth)
                {
                    this.OnAnimationFinished(this, new AnimationFinishedEventArgs());
                    this.animationFinished = true;
                }

                return AnimationState.InProgress;
            }

            outBitmap = sourceBitmap;
            return AnimationState.NoChange;
        }

        private AnimationState RenderNextFrame_ScrollForcedContinuous(BitmapSource sourceBitmap, DateTime prevUpdate, out BitmapSource outBitmap)
        {
            int viewportWidth = this.cachedBitmapWithDelimiter.PixelWidth;
            int viewportHeight = this.cachedBitmapWithDelimiter.PixelHeight;
            int renderWidth = this.MaxWidth;
            int renderHeight = this.cachedBitmapWithDelimiter.PixelHeight;

            TimeSpan timeDiff = DateTime.Now - prevUpdate;
            double newOffsetX = this.CurrentOffsetX + (timeDiff.TotalSeconds * this.PixelsPerSecond);
            newOffsetX %= viewportWidth;

            if (this.CurrentOffsetX != newOffsetX || this.invalidated)
            {
                ImageBrush imageBrush = new ImageBrush(this.cachedBitmapWithDelimiter)
                {
                    AlignmentX = AlignmentX.Left,
                    Stretch = Stretch.None,
                    TileMode = TileMode.Tile,
                    Viewport = new Rect(-newOffsetX, 0, viewportWidth, viewportHeight),
                    ViewportUnits = BrushMappingMode.Absolute
                };

                outBitmap = this.DrawImageBrushToRenderer(imageBrush, renderWidth, renderHeight);
                this.CurrentOffsetX = newOffsetX;
                this.invalidated = false;
                return AnimationState.InProgress;
            }

            outBitmap = sourceBitmap;
            return AnimationState.NoChange;
        }

        private BitmapSource DrawImageBrushToRenderer(ImageBrush imageBrush, int renderWidth, int renderHeight)
        {
            DrawingVisual visual = new DrawingVisual();
            using (DrawingContext context = visual.RenderOpen())
            {
                context.DrawRectangle(imageBrush, null, new Rect(0, 0, renderWidth, renderHeight));
            }

            RenderTargetBitmap render = new RenderTargetBitmap(renderWidth, renderHeight, 96, 96, PixelFormats.Pbgra32);
            render.Render(visual);
            render.Freeze();
            return render;
        }

        public void ResetState()
        {
            this.CurrentOffsetX = 0;
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

    public enum ScrollMode
    {
        TooWideOnly,
        ForcedOnce,
        ForcedContinuous
    }
}
