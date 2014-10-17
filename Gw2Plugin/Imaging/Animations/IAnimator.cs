using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ObsGw2Plugin.Imaging.Animations
{
    public interface IAnimator
    {
        AnimationState RenderNextFrame(BitmapSource sourceBitmap, DateTime prevUpdate, out BitmapSource outBitmap);

        void ResetState();

    }

    public enum AnimationState
    {
        NoChange,
        InProgress,
        Finished
    }

    //TODO Future stuff: Extend animations with support for continuous or one-time animations
    /*public enum AnimationType
    {
        Continuous,
        OneTime
    }*/
}
