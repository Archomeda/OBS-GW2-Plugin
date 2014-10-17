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
    public interface IImage : INotifyPropertyChanged
    {
        BitmapSource Bitmap { get; }

        int? CustomWidth { get; set; }

        int? CustomHeight { get; set; }


        int GetStride();

        byte[] GetPixels();

    }
}
