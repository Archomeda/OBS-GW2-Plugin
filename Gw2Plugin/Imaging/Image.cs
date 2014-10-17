using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using ObsGw2Plugin.Imaging.Animations;

namespace ObsGw2Plugin.Imaging
{
    public abstract class Image : IImage
    {
        private BitmapSource bitmap;
        private int? customWidth;
        private int? customHeight;

        public virtual BitmapSource Bitmap
        {
            get { return this.bitmap; }
            protected set
            {
                if (!object.Equals(this.bitmap, value))
                {
                    this.bitmap = value;
                    this.OnNotifyPropertyChanged("Bitmap");
                }
            }
        }

        public virtual int? CustomWidth
        {
            get { return this.customWidth; }
            set
            {
                if (this.customWidth != value)
                {
                    this.customWidth = value;
                    this.OnNotifyPropertyChanged("CustomWidth");
                }
            }
        }
        public virtual int? CustomHeight
        {
            get { return this.customHeight; }
            set
            {
                if (this.customHeight != value)
                {
                    this.customHeight = value;
                    this.OnNotifyPropertyChanged("CustomHeight");
                }
            }
        }


        public virtual int GetStride()
        {
            return this.Bitmap.PixelWidth * ((this.Bitmap.Format.BitsPerPixel + 7) / 8);
        }

        public virtual byte[] GetPixels()
        {
            if (this.Bitmap != null)
            {
                int stride = this.GetStride();
                byte[] pixels = new byte[this.Bitmap.PixelHeight * stride];
                this.Bitmap.CopyPixels(pixels, stride, 0);
                return pixels;
            }
            return null;
        }


        #region INotifyPropertyChanged members

        protected virtual void OnNotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler h = this.PropertyChanged;
            if (h != null)
                h(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

    }
}
