using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObsGw2Plugin.MumbleLink
{
    public interface IMumbleLinkConnector : IDisposable
    {
        void OpenMemoryMappedFile();

        void CloseMemoryMappedFile();

        LinkedMem ReadMemoryMappedFile();
    }
}
