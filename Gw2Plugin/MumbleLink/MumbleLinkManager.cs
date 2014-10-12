using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ObsGw2Plugin.MumbleLink
{
    public class MumbleLinkManager : IMumbleLinkManager
    {
        public MumbleLinkManager()
        {
            this.TimeoutRate = 2;
            this.MumbleLinkFile = new MumbleLinkFile();
            this.MumbleLinkConnector = new MumbleLinkConnector();
        }

        private bool stopRequested = false;
        private uint lastTick = 0;
        private DateTime? lastTickUpdate = null;

        public bool IsListening { get; protected set; }

        public bool IsActive { get; protected set; }

        public double TimeoutRate { get; set; }

        public IMumbleLinkFile MumbleLinkFile { get; protected set; }

        public IMumbleLinkConnector MumbleLinkConnector { get; protected set; }


        public void UseMumbleLinkFile<T>() where T : IMumbleLinkFile, new()
        {
            this.UseMumbleLinkFile(new T());
        }

        public void UseMumbleLinkFile(IMumbleLinkFile mumbleLinkFile)
        {
            this.MumbleLinkFile = mumbleLinkFile;
        }

        public void UseMumbleLinkConnector<T>() where T : IMumbleLinkConnector, new()
        {
            this.UseMumbleLinkConnector(new T());
        }

        public void UseMumbleLinkConnector(IMumbleLinkConnector mumbleLinkConnector)
        {
            this.MumbleLinkConnector = mumbleLinkConnector;
        }


        public virtual void StartListener()
        {
            if (!IsListening)
            {
                this.stopRequested = false;
                this.IsListening = true;
                new Thread(CheckLoop).Start();
            }
        }

        public virtual void StopListener()
        {
            if (IsListening)
                this.stopRequested = true;
        }

        private void CheckLoop()
        {
            this.MumbleLinkConnector.OpenMemoryMappedFile();

            while (!this.stopRequested)
            {
                this.Check();
                Thread.Sleep(1);
            }

            this.MumbleLinkConnector.CloseMemoryMappedFile();
            this.IsListening = false;
        }

        public virtual void Check()
        {
            LinkedMem linkedMem = this.MumbleLinkConnector.ReadMemoryMappedFile();
            if (linkedMem.uiTick != this.lastTick)
            {
                this.lastTick = linkedMem.uiTick;
                this.lastTickUpdate = DateTime.Now;
                this.MumbleLinkFile.SetDataFromLinkedMem(linkedMem);
                if (!this.IsActive)
                {
                    this.IsActive = true;
                    unsafe { this.OnMumbleLinkStateChanged(new MumbleLinkStateEventArgs(MumbleLinkState.Connected, new string(linkedMem.name))); }
                }
            }
            else if (this.lastTickUpdate != null && this.IsActive && (DateTime.Now - this.lastTickUpdate.Value).TotalSeconds >= this.TimeoutRate)
            {
                this.IsActive = false;
                this.MumbleLinkFile.SetDataFromLinkedMem(linkedMem);
                this.OnMumbleLinkStateChanged(new MumbleLinkStateEventArgs(MumbleLinkState.Disconnected, null));
            }
        }


        protected virtual void OnMumbleLinkStateChanged(MumbleLinkStateEventArgs args)
        {
            EventHandler<MumbleLinkStateEventArgs> h = this.MumbleLinkStateChanged;
            if (h != null)
                h(this, args);
        }

        public event EventHandler<MumbleLinkStateEventArgs> MumbleLinkStateChanged;



    }
}
