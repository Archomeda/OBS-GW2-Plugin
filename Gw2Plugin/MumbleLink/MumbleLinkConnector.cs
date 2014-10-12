using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ObsGw2Plugin.MumbleLink
{
    public class MumbleLinkConnector : IMumbleLinkConnector
    {
        private MemoryMappedFile memoryMappedFile;
        private MemoryMappedViewAccessor accessor;

        public void OpenMemoryMappedFile()
        {
            this.memoryMappedFile = MemoryMappedFile.CreateOrOpen("MumbleLink", Marshal.SizeOf(typeof(LinkedMem)), MemoryMappedFileAccess.ReadWrite);
            this.accessor = this.memoryMappedFile.CreateViewAccessor();
        }

        public LinkedMem ReadMemoryMappedFile()
        {
            LinkedMem linkedMem;
            accessor.Read(0, out linkedMem);
            return linkedMem;
        }

        public void CloseMemoryMappedFile()
        {
            if (this.accessor != null)
                accessor.Dispose();
            if (this.memoryMappedFile != null)
                this.memoryMappedFile.Dispose();
        }


        #region IDisposable members

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.CloseMemoryMappedFile();
            }
        }

        #endregion

    }
}
