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
using ObsGw2Plugin.Imaging.Animations;

namespace ObsGw2Plugin.Imaging
{
    public class TextImage : Image, IAnimatable
    {
        public TextImage()
        {
            this.Animators = new List<IAnimator>();
        }


        #region Image properties

        public override int? CustomWidth
        {
            get { return base.CustomWidth; }
            set
            {
                if (base.CustomWidth != value)
                {
                    base.CustomWidth = value;
                    this.PreRenderBitmap();
                }
            }
        }

        public override int? CustomHeight
        {
            get { return base.CustomHeight; }
            set
            {
                if (base.CustomHeight != value)
                {
                    base.CustomHeight = value;
                    this.PreRenderBitmap();
                }
            }
        }

        #endregion


        #region TextImage properties

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


        public string Text
        {
            get { return this.text; }
            set
            {
                if (this.text != value)
                {
                    this.text = value;
                    this.OnNotifyPropertyChanged("Text");
                    this.PreRenderBitmap();
                }
            }
        }

        public FontFamily FontFamily
        {
            get { return this.fontFamily; }
            set
            {
                if (!object.Equals(this.fontFamily, value))
                {
                    this.fontFamily = value;
                    this.OnNotifyPropertyChanged("FontFamily");
                    this.PreRenderBitmap();
                }
            }
        }

        public int FontSize
        {
            get { return this.fontSize; }
            set
            {
                if (this.fontSize != value)
                {
                    this.fontSize = value;
                    this.OnNotifyPropertyChanged("FontSize");
                    this.PreRenderBitmap();
                }
            }
        }

        public Color TextColor
        {
            get { return this.textColor; }
            set
            {
                if (this.textColor != value)
                {
                    this.textColor = value;
                    this.OnNotifyPropertyChanged("TextColor");
                    this.PreRenderBitmap();
                }
            }
        }

        public Color BackColor
        {
            get { return this.backColor; }
            set
            {
                if (this.backColor != value)
                {
                    this.backColor = value;
                    this.OnNotifyPropertyChanged("BackColor");
                    this.PreRenderBitmap();
                }
            }
        }

        public bool EffectBold
        {
            get { return this.effectBold; }
            set
            {
                if (this.effectBold != value)
                {
                    this.effectBold = value;
                    this.OnNotifyPropertyChanged("EffectBold");
                    this.PreRenderBitmap();
                }
            }
        }

        public bool EffectItalic
        {
            get { return this.effectItalic; }
            set
            {
                if (this.effectItalic != value)
                {
                    this.effectItalic = value;
                    this.OnNotifyPropertyChanged("EffectItalic");
                    this.PreRenderBitmap();
                }
            }
        }

        public bool EffectUnderline
        {
            get { return this.effectUnderline; }
            set
            {
                if (this.effectUnderline != value)
                {
                    this.effectUnderline = value;
                    this.OnNotifyPropertyChanged("EffectUnderline");
                    this.PreRenderBitmap();
                }
            }
        }

        public Color OutlineColor
        {
            get { return this.outlineColor; }
            set
            {
                if (this.outlineColor != value)
                {
                    this.outlineColor = value;
                    this.OnNotifyPropertyChanged("OutlineColor");
                    this.PreRenderBitmap();
                }
            }
        }

        public double OutlineThickness
        {
            get { return this.outlineThickness; }
            set
            {
                if (this.outlineThickness != value)
                {
                    this.outlineThickness = value;
                    this.OnNotifyPropertyChanged("OutlineThickness");
                    this.PreRenderBitmap();
                }
            }
        }

        #endregion


        private DateTime prevAnimationUpdate;
        private BitmapSource preRenderedBitmap;

        protected virtual void PreRenderBitmap()
        {
            BitmapSource bitmap = null;
            FontWeight fontWeight = this.EffectBold ? FontWeights.Bold : FontWeights.Normal;
            FontStyle fontStyle = this.EffectItalic ? FontStyles.Italic : FontStyles.Normal;

            FormattedText formattedText = new FormattedText(text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight,
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
                bitmap = render;
            }

            this.preRenderedBitmap = bitmap;
            if (!this.AnimationActive)
                this.ApplyBitmap(bitmap);
        }

        protected virtual void ApplyBitmap(BitmapSource bitmap)
        {
            if (this.CustomWidth.HasValue || this.CustomHeight.HasValue)
            {
                Rect rect = new Rect(0, 0, this.CustomWidth ?? bitmap.PixelWidth, this.CustomHeight ?? bitmap.PixelHeight);

                ImageBrush imageBrush = new ImageBrush(bitmap)
                {
                    AlignmentX = AlignmentX.Left,
                    Stretch = Stretch.None,
                    TileMode = TileMode.None,
                };

                DrawingVisual visual = new DrawingVisual();
                using (DrawingContext context = visual.RenderOpen())
                {
                    context.DrawRectangle(imageBrush, null, rect);
                }

                RenderTargetBitmap render = new RenderTargetBitmap((int)rect.Width, (int)rect.Height, 96, 96, PixelFormats.Pbgra32);
                render.Render(visual);
                render.Freeze();

                this.Bitmap = render;
            }
            else
            {
                this.Bitmap = bitmap;
            }
        }


        #region IAnimatable members

        public bool AnimationActive { get; protected set; }

        public IList<IAnimator> Animators { get; protected set; }

        public void StartAnimation()
        {
            if (!this.AnimationActive)
            {
                this.AnimationActive = true;
                this.prevAnimationUpdate = DateTime.Now;
            }
        }

        public void StopAnimation()
        {
            if (this.AnimationActive)
            {
                this.AnimationActive = false;
            }
        }

        public void ResetAnimation()
        {
            foreach (IAnimator animator in this.Animators)
                animator.ResetState();

            this.ApplyBitmap(this.preRenderedBitmap);
        }

        public virtual bool AnimateFrame()
        {
            if (this.AnimationActive)
            {
                DateTime prevUpdate = this.prevAnimationUpdate;
                this.prevAnimationUpdate = DateTime.Now;

                BitmapSource animatedBitmap = this.preRenderedBitmap;
                bool updated = false;
                foreach (IAnimator animator in this.Animators)
                {
                    AnimationState state = animator.RenderNextFrame(animatedBitmap, prevUpdate, out animatedBitmap);
                    if (state == AnimationState.InProgress)
                        updated = true;
                }

                if (updated || this.Bitmap == null)
                {
                    this.ApplyBitmap(animatedBitmap);
                    return true;
                }
            }
            return false;
        }

        #endregion

    }
}
