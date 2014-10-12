using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ObsGw2Plugin
{
    public class TextImage : INotifyPropertyChanged
    {
        #region Properties

        private BitmapSource image;
        private BitmapSource baseImage;
        private BitmapSource scrollingImage;
        private string text = "";
        private FontFamily fontFamily = new FontFamily("Arial");
        private int fontSize = 48;
        private Color textColor = Colors.White;
        private Color backColor = Colors.Transparent;
        private bool effectBold = false;
        private bool effectItalic = false;
        private bool effectUnderline = false;
        private Color outlineColor = Colors.Black;
        private double outlineThickness = 1;
        private bool enableScrolling = false;
        private int scrollingSpeed = 5;
        private string scrollingDelimiter = " - ";
        private int scrollingMaxWidth = 250;
        private ScrollingAligns scrollingAlign = ScrollingAligns.Left;
        private bool scrollingLargeOnly = true;

        public BitmapSource Image
        {
            get { return this.image; }
            protected set
            {
                this.image = value;
                this.OnNotifyPropertyChanged("Image");
            }
        }

        public BitmapSource BaseImage
        {
            get { return this.baseImage; }
            protected set
            {
                this.baseImage = value;
                this.OnNotifyPropertyChanged("BaseImage");
            }
        }

        public BitmapSource ScrollingImage
        {
            get { return this.scrollingImage; }
            protected set
            {
                this.scrollingImage = value;
                this.OnNotifyPropertyChanged("ScrollingImage");
            }
        }

        public string Text
        {
            get { return this.text; }
            set
            {
                this.text = value;
                this.OnNotifyPropertyChanged("Text");
                this.UpdateBaseImage();
                this.UpdateScrollingImage();
            }
        }

        public FontFamily FontFamily
        {
            get { return this.fontFamily; }
            set
            {
                this.fontFamily = value;
                this.OnNotifyPropertyChanged("FontFamily");
                this.UpdateBaseImage();
                this.UpdateScrollingImage();
            }
        }

        public int FontSize
        {
            get { return this.fontSize; }
            set
            {
                this.fontSize = value;
                this.OnNotifyPropertyChanged("FontSize");
                this.UpdateBaseImage();
                this.UpdateScrollingImage();
            }
        }

        public Color TextColor
        {
            get { return this.textColor; }
            set
            {
                this.textColor = value;
                this.OnNotifyPropertyChanged("TextColor");
                this.UpdateBaseImage();
                this.UpdateScrollingImage();
            }
        }

        public Color BackColor
        {
            get { return this.backColor; }
            set
            {
                this.backColor = value;
                this.OnNotifyPropertyChanged("BackColor");
                this.UpdateBaseImage();
                this.UpdateScrollingImage();
            }
        }

        public bool EffectBold
        {
            get { return this.effectBold; }
            set
            {
                this.effectBold = value;
                this.OnNotifyPropertyChanged("EffectBold");
                this.UpdateBaseImage();
                this.UpdateScrollingImage();
            }
        }

        public bool EffectItalic
        {
            get { return this.effectItalic; }
            set
            {
                this.effectItalic = value;
                this.OnNotifyPropertyChanged("EffectItalic");
                this.UpdateBaseImage();
                this.UpdateScrollingImage();
            }
        }

        public bool EffectUnderline
        {
            get { return this.effectUnderline; }
            set
            {
                this.effectUnderline = value;
                this.OnNotifyPropertyChanged("EffectUnderline");
                this.UpdateBaseImage();
                this.UpdateScrollingImage();
            }
        }

        public Color OutlineColor
        {
            get { return this.outlineColor; }
            set
            {
                this.outlineColor = value;
                this.OnNotifyPropertyChanged("OutlineColor");
                this.UpdateBaseImage();
                this.UpdateScrollingImage();
            }
        }

        public double OutlineThickness
        {
            get { return this.outlineThickness; }
            set
            {
                this.outlineThickness = value;
                this.OnNotifyPropertyChanged("OutlineThickness");
                this.UpdateBaseImage();
                this.UpdateScrollingImage();
            }
        }

        public bool EnableScrolling
        {
            get { return this.enableScrolling; }
            set
            {
                this.enableScrolling = value;
                this.OnNotifyPropertyChanged("EnableScrolling");
                this.UpdateBaseImage();
                this.UpdateScrollingImage();
            }
        }

        public int ScrollingSpeed
        {
            get { return this.scrollingSpeed; }
            set
            {
                this.scrollingSpeed = value;
                this.OnNotifyPropertyChanged("ScrollingSpeed");
                this.UpdateScrollingImage();
            }
        }

        public string ScrollingDelimiter
        {
            get { return this.scrollingDelimiter; }
            set
            {
                this.scrollingDelimiter = value;
                this.OnNotifyPropertyChanged("ScrollingDelimiter");
                this.UpdateScrollingImage();
            }
        }

        public int ScrollingMaxWidth
        {
            get { return this.scrollingMaxWidth; }
            set
            {
                this.scrollingMaxWidth = value;
                this.OnNotifyPropertyChanged("ScrollingMaxWidth");
                this.UpdateBaseImage();
                this.UpdateScrollingImage();
            }
        }

        public ScrollingAligns ScrollingAlign
        {
            get { return this.scrollingAlign; }
            set
            {
                this.scrollingAlign = value;
                this.OnNotifyPropertyChanged("ScrollingAlign");
                this.UpdateBaseImage();
                this.UpdateScrollingImage();
            }
        }

        public bool ScrollingLargeOnly
        {
            get { return this.scrollingLargeOnly; }
            set
            {
                this.scrollingLargeOnly = value;
                this.OnNotifyPropertyChanged("ScrollingLargeOnly");
                this.UpdateBaseImage();
                this.UpdateScrollingImage();
            }
        }


        protected virtual void OnNotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler h = this.PropertyChanged;
            if (h != null)
                h(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion


        public virtual void UpdateBaseImage()
        {
            string text = this.Text;
            BitmapSource image = this.CreateTextImage(text);
            if (this.EnableScrolling && this.ScrollingLargeOnly && image.PixelWidth < this.ScrollingMaxWidth)
            {
                // Image is smaller than desired width when scrolling, align it according to user settings
                Rect rect = new Rect(0, 0, this.ScrollingMaxWidth, image.PixelHeight);
                AlignmentX alignX = AlignmentX.Center;
                switch (this.ScrollingAlign)
                {
                    case ScrollingAligns.Left: alignX = AlignmentX.Left; break;
                    case ScrollingAligns.Center: alignX = AlignmentX.Center; break;
                    case ScrollingAligns.Right: alignX = AlignmentX.Right; break;
                }
                ImageBrush imageBrush = new ImageBrush(image)
                {
                    AlignmentX = alignX,
                    Stretch = Stretch.None,
                    TileMode = TileMode.None
                };
                DrawingVisual visual = new DrawingVisual();
                using (DrawingContext context = visual.RenderOpen())
                {
                    context.DrawRectangle(imageBrush, null, rect);
                }
                RenderTargetBitmap render = new RenderTargetBitmap((int)rect.Width, (int)rect.Height, 96, 96, PixelFormats.Pbgra32);
                render.Render(visual);
                render.Freeze();
                this.BaseImage = render;
            }
            else
            {
                this.BaseImage = image;
            }
        }

        public virtual void UpdateScrollingImage()
        {
            string text = this.Text + this.ScrollingDelimiter;
            BitmapSource image = this.CreateTextImage(text);
            image.Freeze();
            this.ScrollingImage = image;
        }

        protected virtual BitmapSource CreateTextImage(string text)
        {
            BitmapSource image = null;
            FontWeight fontWeight = this.EffectBold ? FontWeights.Bold : FontWeights.Normal;
            FontStyle fontStyle = this.EffectItalic ? FontStyles.Italic : FontStyles.Normal;

            FormattedText formattedText = new FormattedText(text, CultureInfo.InvariantCulture, FlowDirection.LeftToRight,
                new Typeface(this.FontFamily, fontStyle, fontWeight, FontStretches.Normal), this.FontSize, Brushes.Black);

            if (this.EffectUnderline)
                formattedText.SetTextDecorations(new TextDecorationCollection() { TextDecorations.Underline });

            Rect rect = new Rect(0, 0, Math.Ceiling(formattedText.WidthIncludingTrailingWhitespace) + this.OutlineThickness,
                Math.Ceiling(formattedText.Height) + this.OutlineThickness);

            if (rect.Width > 0 && rect.Height > 0)
            {
                Geometry textGeometry = formattedText.BuildGeometry(new Point(this.OutlineThickness / 2, this.OutlineThickness / 2));

                DrawingVisual visual = new DrawingVisual();
                using (DrawingContext context = visual.RenderOpen())
                {
                    context.DrawRectangle(new SolidColorBrush(this.BackColor), null, rect);
                    if (this.OutlineThickness > 0)
                        context.DrawGeometry(new SolidColorBrush(this.TextColor),
                            new Pen(new SolidColorBrush(this.OutlineColor), this.OutlineThickness), textGeometry);
                    else
                        context.DrawGeometry(new SolidColorBrush(this.TextColor), null, textGeometry);
                }
                RenderTargetBitmap render = new RenderTargetBitmap((int)rect.Width, (int)rect.Height, 96, 96, PixelFormats.Pbgra32);
                render.Render(visual);
                render.Freeze();
                image = render;
            }
            return image;
        }

        public virtual byte[] GetPixels(int stride)
        {
            if (this.Image != null)
            {
                byte[] pixels = new byte[this.Image.PixelWidth * stride];
                this.Image.CopyPixels(pixels, stride, 0);
                return pixels;
            }
            return null;
        }


        private DateTime lastFrameUpdate = DateTime.Now;
        private double lastOffsetX = 0;

        public virtual bool RenderNextFrame()
        {
            if (this.BaseImage != null && this.ScrollingImage != null)
            {
                if (!this.EnableScrolling || this.ScrollingLargeOnly && this.BaseImage.PixelWidth == this.ScrollingMaxWidth)
                {
                    // Don't scroll when not activated or text fits in the desired width
                    this.lastFrameUpdate = DateTime.Now;
                    if (this.Image != this.BaseImage)
                    {
                        this.Image = this.BaseImage;
                        return true;
                    }
                    else
                        return false;
                }
                else
                {
                    // Scroll always
                    DateTime updateTime = DateTime.Now;
                    TimeSpan timeDiff = updateTime - this.lastFrameUpdate;
                    double offsetX = this.lastOffsetX + (timeDiff.TotalSeconds * this.ScrollingSpeed);
                    offsetX %= this.ScrollingImage.PixelWidth;

                    Rect viewportRect = new Rect(-offsetX, 0, this.ScrollingImage.PixelWidth, this.ScrollingImage.PixelHeight);
                    ImageBrush imageBrush = new ImageBrush(this.ScrollingImage)
                    {
                        AlignmentX = AlignmentX.Left,
                        Stretch = Stretch.None,
                        TileMode = TileMode.Tile,
                        Viewport = viewportRect,
                        ViewportUnits = BrushMappingMode.Absolute
                    };
                    DrawingVisual visual = new DrawingVisual();
                    using (DrawingContext context = visual.RenderOpen())
                    {
                        context.DrawRectangle(imageBrush, null, new Rect(0, 0, this.ScrollingMaxWidth, this.ScrollingImage.PixelHeight));
                    }

                    RenderTargetBitmap render = new RenderTargetBitmap(this.ScrollingMaxWidth, this.ScrollingImage.PixelHeight, 96, 96, PixelFormats.Pbgra32);
                    render.Render(visual);
                    render.Freeze();
                    this.Image = render;

                    this.lastFrameUpdate = updateTime;
                    if (this.lastOffsetX != offsetX)
                    {
                        this.lastOffsetX = offsetX;
                        return true;
                    }
                }
            }

            return false;
        }


        public enum ScrollingAligns
        {
            Left,
            Center,
            Right
        }

    }
}
