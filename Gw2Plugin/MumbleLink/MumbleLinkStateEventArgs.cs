using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObsGw2Plugin.MumbleLink
{
    public class MumbleLinkStateEventArgs : EventArgs
    {
        public MumbleLinkStateEventArgs(MumbleLinkState state, string name)
        {
            this.State = state;
            this.Name = name;
        }


        public MumbleLinkState State { get; set; }

        public string Name { get; set; }
    }

    public enum MumbleLinkState
    {
        Connected,
        Disconnected
    }
}
