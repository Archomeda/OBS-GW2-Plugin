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


        event EventHandler<AnimationFinishedEventArgs> AnimationFinished;

    }

    public enum AnimationState
    {
        NoChange,
        InProgress,
        Finished
    }

    public class AnimationFinishedEventArgs : EventArgs { }

}
