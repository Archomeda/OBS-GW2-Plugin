using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using CLROBS;
using ObsGw2Plugin.Extensions;
using ObsGw2Plugin.Imaging;
using ObsGw2Plugin.Imaging.Animations;
using ObsGw2Plugin.Scripting;

namespace ObsGw2Plugin
{
    class Gw2InfoSource : AbstractImageSource, IDisposable
    {
        private object textureLock = new object();
        private Texture texture = null;
        private XElement config;
        private string oldText = "";
        private bool hideWhenGw2IsInactive = true;

        private TextImage textImage = null;

        public Gw2InfoSource(XElement config)
        {
            this.config = config;
            UpdateSettings();
        }

        public override void UpdateSettings()
        {
            try
            {
                string text = Gw2Plugin.Instance.ScriptsManager.FormatString(this.config.GetString("textFormat"));
                this.oldText = text;

                this.hideWhenGw2IsInactive = this.config.GetBoolean("hideWhenGw2IsInactive");

                FontFamily fontFamily = new FontFamily(this.config.GetString("textFont"));
                int fontSize = this.config.GetInt("textFontSize");
                Color textColor = this.config.GetColor2("textColor");
                Color backColor = this.config.GetColor2("backColor");
                bool effectBold = this.config.GetBoolean("textBold");
                bool effectItalic = this.config.GetBoolean("textItalic");
                bool effectUnderline = this.config.GetBoolean("textUnderline");

                bool outline = this.config.GetBoolean("outline");
                Color outlineColor = outline ? this.config.GetColor2("outlineColor") : Colors.Transparent;
                double outlineThickness = outline ? this.config.GetFloat("outlineThickness") : 0;

                bool scrolling = this.config.GetBoolean("scrolling");
                int scrollingSpeed = this.config.GetInt("scrollingSpeed");
                string scrollingDelimiter = this.config.GetString("scrollingDelimiter");
                int scrollingMaxWidth = this.config.GetInt("scrollingMaxWidth");
                AlignmentX scrollingAlign = AlignmentX.Left;
                switch (this.config.GetString("scrollingAlign"))
                {
                    case "Left": scrollingAlign = AlignmentX.Left; break;
                    case "Center": scrollingAlign = AlignmentX.Center; break;
                    case "Right": scrollingAlign = AlignmentX.Right; break;
                }
                bool scrollingLargeOnly = this.config.GetBoolean("scrollingLargeOnly");

                this.textImage = new TextImage()
                {
                    Text = text,
                    FontFamily = fontFamily,
                    FontSize = fontSize,
                    TextColor = textColor,
                    BackColor = backColor,
                    EffectBold = effectBold,
                    EffectItalic = effectItalic,
                    EffectUnderline = effectUnderline,
                    OutlineColor = outlineColor,
                    OutlineThickness = outlineThickness
                };
                if (scrolling)
                {
                    TextImage scrollingDelimiterImage = new TextImage()
                    {
                        Text = scrollingDelimiter,
                        FontFamily = fontFamily,
                        FontSize = fontSize,
                        TextColor = textColor,
                        BackColor = backColor,
                        EffectBold = effectBold,
                        EffectItalic = effectItalic,
                        EffectUnderline = effectUnderline,
                        OutlineColor = outlineColor,
                        OutlineThickness = outlineThickness
                    };
                    ScrollingAnimator scrollingAnimator = new ScrollingAnimator()
                    {
                        DelimiterImage = scrollingDelimiterImage,
                        PixelsPerSecond = scrollingSpeed,
                        MaxWidth = scrollingMaxWidth,
                        TextAlign = scrollingAlign,
                        ScrollMode = scrollingLargeOnly ? ScrollMode.TooWideOnly : ScrollMode.ForcedContinuous
                    };

                    this.textImage.CustomWidth = scrollingMaxWidth;
                    this.textImage.Animators.Add(scrollingAnimator);
                }

                this.UpdateTexture();

                this.config.Parent.SetInt("cx", this.textImage.CustomWidth ?? this.textImage.Bitmap.PixelWidth);
                this.config.Parent.SetInt("cy", this.textImage.Bitmap.PixelHeight);
                this.Size.X = this.textImage.CustomWidth ?? this.textImage.Bitmap.PixelWidth;
                this.Size.Y = this.textImage.Bitmap.PixelHeight;
            }
            catch (Exception ex)
            {
                API.Instance.Log("Gw2Plugin: {0}", ex.ToString());
                Debug.BreakDebugger();
            }
        }

        public override void Render(float x, float y, float width, float height)
        {
            try
            {
                if (this.textImage != null)
                {
                    this.UpdateTexture();
                    lock (textureLock)
                    {
                        if (this.texture != null)
                            GS.DrawSprite(texture, 0xFFFFFFFF, x, y, x + width, y + height);
                    }
                }
            }
            catch (Exception ex)
            {
                API.Instance.Log("Gw2Plugin: {0}", ex.ToString());
                Debug.BreakDebugger();
            }
        }

        public bool UpdateTexture()
        {
            string text = Gw2Plugin.Instance.ScriptsManager.FormatString(this.config.GetString("textFormat"));
            if (text != this.oldText)
            {
                this.oldText = text;
                this.textImage.Text = text;
            }

            if (!this.textImage.AnimationActive)
                this.textImage.StartAnimation();

            if (this.textImage.AnimateFrame() || this.texture == null)
            {
                int pixelWidth = this.textImage.Bitmap.PixelWidth;
                int pixelHeight = this.textImage.Bitmap.PixelHeight;
                byte[] pixels = this.textImage.GetPixels();

                Texture newTexture = GS.CreateTexture((uint)pixelWidth, (uint)pixelHeight, GSColorFormat.GS_BGRA, null, false, false);
                newTexture.SetImage(pixels, GSImageFormat.GS_IMAGEFORMAT_BGRA, (uint)this.textImage.GetStride());

                this.config.Parent.SetInt("cx", pixelWidth);
                this.config.Parent.SetInt("cy", pixelHeight);
                this.Size.X = pixelWidth;
                this.Size.Y = pixelHeight;

                lock (textureLock)
                    this.texture = newTexture;

                return true;
            }

            return false;
        }


        #region IDisposable members

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.texture != null)
                    this.texture.Dispose();
            }
        }

        #endregion

    }
}
