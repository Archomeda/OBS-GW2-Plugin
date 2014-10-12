using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObsGw2Plugin.MumbleLink
{
    public interface IMumbleLinkManager
    {
        bool IsListening { get; }

        bool IsActive { get; }

        double TimeoutRate { get; set; }

        IMumbleLinkFile MumbleLinkFile { get; }

        IMumbleLinkConnector MumbleLinkConnector { get; }


        void UseMumbleLinkFile<T>() where T : IMumbleLinkFile, new();

        void UseMumbleLinkFile(IMumbleLinkFile mumbleLinkFile);

        void UseMumbleLinkConnector<T>() where T : IMumbleLinkConnector, new();

        void UseMumbleLinkConnector(IMumbleLinkConnector mumbleLinkConnector);


        void StartListener();

        void StopListener();

        void Check();


        event EventHandler<MumbleLinkStateEventArgs> MumbleLinkStateChanged;
    }
}
