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
using ObsGw2Plugin.Scripting;

namespace ObsGw2Plugin
{
    class Gw2InfoSource : AbstractImageSource, IDisposable
    {
        private object textureLock = new object();
        private TextImage textImage = null;
        private Texture texture = null;
        private XElement config;
        private string oldText = "";
        private bool hideWhenGw2IsInactive = true;

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
                TextImage.ScrollingAligns scrollingAlign = TextImage.ScrollingAligns.Center;
                switch (this.config.GetString("scrollingAlign"))
                {
                    case "Left": scrollingAlign = TextImage.ScrollingAligns.Left; break;
                    case "Center": scrollingAlign = TextImage.ScrollingAligns.Center; break;
                    case "Right": scrollingAlign = TextImage.ScrollingAligns.Right; break;
                }
                bool scrollingLargeOnly = this.config.GetBoolean("scrollingLargeOnly");

                TextImage newTextImage = new TextImage();
                newTextImage.Text = text;
                newTextImage.FontFamily = fontFamily;
                newTextImage.FontSize = fontSize;
                newTextImage.TextColor = textColor;
                newTextImage.BackColor = backColor;
                newTextImage.EffectBold = effectBold;
                newTextImage.EffectItalic = effectItalic;
                newTextImage.EffectUnderline = effectUnderline;
                newTextImage.OutlineColor = outlineColor;
                newTextImage.OutlineThickness = outlineThickness;
                newTextImage.EnableScrolling = scrolling;
                newTextImage.ScrollingSpeed = scrollingSpeed;
                newTextImage.ScrollingDelimiter = scrollingDelimiter;
                newTextImage.ScrollingMaxWidth = scrollingMaxWidth;
                newTextImage.ScrollingAlign = scrollingAlign;
                newTextImage.ScrollingLargeOnly = scrollingLargeOnly;

                this.textImage = newTextImage;
                this.UpdateTexture();

                this.config.Parent.SetInt("cx", this.textImage.Image.PixelWidth);
                this.config.Parent.SetInt("cy", this.textImage.Image.PixelHeight);
                this.Size.X = this.textImage.Image.PixelWidth;
                this.Size.Y = this.textImage.Image.PixelHeight;
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
                    if (!this.hideWhenGw2IsInactive || Gw2Plugin.Instance.MumbleLinkManager.IsActive)
                    {
                        this.UpdateTexture();
                        lock (textureLock)
                        {
                            if (this.texture != null)
                                GS.DrawSprite(texture, 0xFFFFFFFF, x, y, x + width, y + height);
                        }
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

            if (this.textImage.RenderNextFrame() || this.texture == null)
            {
                int pixelWidth = this.textImage.Image.PixelWidth;
                int pixelHeight = this.textImage.Image.PixelHeight;
                int stride = pixelWidth * 4;
                byte[] pixels = this.textImage.GetPixels(stride);

                Texture newTexture = GS.CreateTexture((uint)pixelWidth, (uint)pixelHeight, GSColorFormat.GS_BGRA, null, false, false);
                newTexture.SetImage(pixels, GSImageFormat.GS_IMAGEFORMAT_BGRA, (uint)stride);

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
