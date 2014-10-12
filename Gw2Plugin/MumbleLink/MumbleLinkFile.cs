using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ObsGw2Plugin.MumbleLink
{
    public class MumbleLinkFile : IMumbleLinkFile
    {
        private uint uiVersion = 0;
        private uint uiTick = 0;
        private string name = "";

        private Vector3 avatarPosition = new Vector3();
        private Vector3 avatarFront = new Vector3();
        private Vector3 avatarTop = new Vector3();
        private Vector3 cameraPosition = new Vector3();
        private Vector3 cameraFront = new Vector3();
        private Vector3 cameraTop = new Vector3();

        private string identity = "";
        private string description = "";


        public bool IsValid { get; set; }


        public uint UIVersion
        {
            get { return this.uiVersion; }
            set
            {
                if (this.uiVersion != value)
                {
                    this.uiVersion = value;
                    this.OnNotifyPropertyChanged("UIVersion");
                }
            }
        }

        public uint UITick
        {
            get { return this.uiTick; }
            set
            {
                if (this.uiTick != value)
                {
                    this.uiTick = value;
                    this.OnNotifyPropertyChanged("UITick");
                }
            }
        }

        public string Name
        {
            get { return this.name; }
            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    this.OnNotifyPropertyChanged("Name");
                }
            }
        }


        public Vector3 AvatarPosition
        {
            get { return this.avatarPosition; }
            set
            {
                if (this.avatarPosition != value)
                {
                    this.avatarPosition = value;
                    this.OnNotifyPropertyChanged("AvatarPosition");
                }
            }
        }

        public Vector3 AvatarFront
        {
            get { return this.avatarFront; }
            set
            {
                if (this.avatarFront != value)
                {
                    this.avatarFront = value;
                    this.OnNotifyPropertyChanged("AvatarFront");
                }
            }
        }

        public Vector3 AvatarTop
        {
            get { return this.avatarTop; }
            set
            {
                if (this.avatarTop != value)
                {
                    this.avatarTop = value;
                    this.OnNotifyPropertyChanged("AvatarTop");
                }
            }
        }

        public Vector3 CameraPosition
        {
            get { return this.cameraPosition; }
            set
            {
                if (this.cameraPosition != value)
                {
                    this.cameraPosition = value;
                    this.OnNotifyPropertyChanged("CameraPosition");
                }
            }
        }

        public Vector3 CameraFront
        {
            get { return this.cameraFront; }
            set
            {
                if (this.cameraFront != value)
                {
                    this.cameraFront = value;
                    this.OnNotifyPropertyChanged("CameraFront");
                }
            }
        }

        public Vector3 CameraTop
        {
            get { return this.cameraTop; }
            set
            {
                if (this.cameraTop != value)
                {
                    this.cameraTop = value;
                    this.OnNotifyPropertyChanged("CameraTop");
                }
            }
        }


        public string Identity
        {
            get { return this.identity; }
            set
            {
                if (this.identity != value)
                {
                    this.identity = value;
                    this.OnNotifyPropertyChanged("Identity");
                }
            }
        }

        public byte[] Context { get; set; }

        public string Description
        {
            get { return this.description; }
            set
            {
                if (this.description != value)
                {
                    this.description = value;
                    this.OnNotifyPropertyChanged("Description");
                }
            }
        }


        unsafe public virtual void SetDataFromLinkedMem(LinkedMem data)
        {
            this.UIVersion = data.uiVersion;
            this.UITick = data.uiTick;
            this.Name = new string(data.name);
            this.AvatarPosition = new Vector3(data.fAvatarPosition[0], data.fAvatarPosition[1], data.fAvatarPosition[2]);
            this.AvatarFront = new Vector3(data.fAvatarFront[0], data.fAvatarFront[1], data.fAvatarFront[2]);
            this.AvatarTop = new Vector3(data.fAvatarTop[0], data.fAvatarTop[1], data.fAvatarTop[2]);
            this.CameraPosition = new Vector3(data.fCameraPosition[0], data.fCameraPosition[1], data.fCameraPosition[2]);
            this.CameraFront = new Vector3(data.fCameraFront[0], data.fCameraFront[1], data.fCameraFront[2]);
            this.CameraTop = new Vector3(data.fCameraTop[0], data.fCameraTop[1], data.fCameraTop[2]);
            this.Identity = new string(data.identity);
            this.Context = new byte[data.context_len];
            Marshal.Copy((IntPtr)data.context, this.Context, 0, this.Context.Length);
            this.Description = new string(data.description);

            this.IsValid = true;
        }


        #region INotifyPropertyChanged members

        protected virtual void OnNotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler h = this.PropertyChanged;
            if (h != null)
                h(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

    }
}
