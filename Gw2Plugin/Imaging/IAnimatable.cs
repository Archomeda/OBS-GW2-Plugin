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
    public interface IAnimatable : INotifyPropertyChanged
    {
        bool AnimationActive { get; }

        IList<IAnimator> Animators { get; }

        void StartAnimation();

        void StopAnimation();

        void ResetAnimation();

        bool AnimateFrame();

    }
}
