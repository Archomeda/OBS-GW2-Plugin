using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ObsGw2Plugin.MumbleLink
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct LinkedMem
    {
        public uint uiVersion;
        public uint uiTick;
        public fixed float fAvatarPosition[3];
        public fixed float fAvatarFront[3];
        public fixed float fAvatarTop[3];
        public fixed char name[256];
        public fixed float fCameraPosition[3];
        public fixed float fCameraFront[3];
        public fixed float fCameraTop[3];
        public fixed char identity[256];
        public uint context_len;
        public fixed byte context[256];
        public fixed char description[2048];
    }
}
