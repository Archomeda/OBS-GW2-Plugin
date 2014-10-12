using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObsGw2Plugin.MumbleLink
{
    public interface IMumbleLinkFile : INotifyPropertyChanged
    {
        bool IsValid { get; set; }


        uint UIVersion { get; set; }

        uint UITick { get; set; }

        string Name { get; set; }


        Vector3 AvatarPosition { get; set; }

        Vector3 AvatarFront { get; set; }

        Vector3 AvatarTop { get; set; }

        Vector3 CameraPosition { get; set; }

        Vector3 CameraFront { get; set; }

        Vector3 CameraTop { get; set; }


        string Identity { get; set; }

        unsafe byte* Context { get; set; }

        uint ContextLength { get; set; }

        string Description { get; set; }


        void SetDataFromLinkedMem(LinkedMem data);
    }
}
