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
            this.ScrollMode = ScrollMode.OnlyWhenTextIsTooLarge;
        }


        private bool invalidated = false;
        private AlignmentX textAlign;


        public IImage DelimiterImage { get; set; }

        public int PixelsPerSecond { get; set; }

        public ScrollMode ScrollMode { get; set; }

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

        public double? CurrentOffsetX { get; set; }


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

        public AnimationState RenderNextFrame(BitmapSource sourceBitmap, DateTime prevUpdate, out BitmapSource outBitmap)
        {
            if (!object.Equals(this.prevSourceBitmap, sourceBitmap))
            {
                this.CacheBitmapWithDelimiter(sourceBitmap);
                this.prevSourceBitmap = sourceBitmap;
                this.invalidated = true;
            }

            BitmapSource sourceToUse = null;

            bool scroll = sourceBitmap.PixelWidth > this.MaxWidth || this.ScrollMode == ScrollMode.Always;
            if (scroll)
                sourceToUse = this.cachedBitmapWithDelimiter;
            else
                sourceToUse = sourceBitmap;

            int viewportWidth = sourceToUse.PixelWidth;
            int viewportHeight = sourceToUse.PixelHeight;
            int renderWidth = this.MaxWidth;
            int renderHeight = sourceToUse.PixelHeight;

            double? newOffsetX = null;
            if (scroll)
            {
                TimeSpan timeDiff = DateTime.Now - prevUpdate;
                newOffsetX = (this.CurrentOffsetX ?? 0) + (timeDiff.TotalSeconds * this.PixelsPerSecond);
                newOffsetX %= viewportWidth;
            }

            if (this.CurrentOffsetX != newOffsetX || this.invalidated)
            {
                ImageBrush imageBrush = new ImageBrush(sourceToUse)
                {
                    AlignmentX = this.TextAlign,
                    Stretch = Stretch.None,
                    TileMode = scroll ? TileMode.Tile : TileMode.None,
                    Viewport = new Rect(scroll ? -newOffsetX.Value : 0, 0, scroll ? viewportWidth : renderWidth, viewportHeight),
                    ViewportUnits = BrushMappingMode.Absolute
                };

                DrawingVisual visual = new DrawingVisual();
                using (DrawingContext context = visual.RenderOpen())
                {
                    context.DrawRectangle(imageBrush, null, new Rect(0, 0, renderWidth, renderHeight));
                }

                RenderTargetBitmap render = new RenderTargetBitmap(renderWidth, renderHeight, 96, 96, PixelFormats.Pbgra32);
                render.Render(visual);
                render.Freeze();

                outBitmap = render;
                this.CurrentOffsetX = newOffsetX;
                this.invalidated = false;
                return AnimationState.InProgress;
            }

            outBitmap = sourceBitmap;
            return AnimationState.NoChange;
        }

        public void ResetState()
        {
            this.CurrentOffsetX = 0;
        }

    }

    public enum ScrollMode
    {
        OnlyWhenTextIsTooLarge,
        Always
    }
}
