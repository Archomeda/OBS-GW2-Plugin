using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ObsGw2Plugin.MumbleLink
{
    public class Gw2MumbleLinkFile : MumbleLinkFile
    {
        private string characterName = "";
        private uint professionId = 0;
        private uint mapId = 0;
        private uint worldId = 0;
        private uint teamColorId = 0;
        private bool isCommander = false;

        private byte[] serverAddress = null;
        private uint mapType = 0;
        private uint shardId = 0;
        private uint instance = 0;
        private uint buildId = 0;


        public string CharacterName
        {
            get { return this.characterName; }
            set
            {
                if (this.characterName != value)
                {
                    this.characterName = value;
                    this.OnNotifyPropertyChanged("CharacterName");
                }
            }
        }

        public uint ProfessionId
        {
            get { return this.professionId; }
            set
            {
                if (this.professionId != value)
                {
                    this.professionId = value;
                    this.OnNotifyPropertyChanged("ProfessionId");
                }
            }
        }

        public uint MapId
        {
            get { return this.mapId; }
            set
            {
                if (this.mapId != value)
                {
                    this.mapId = value;
                    this.OnNotifyPropertyChanged("MapId");
                }
            }
        }

        public uint WorldId
        {
            get { return this.worldId; }
            set
            {
                if (this.worldId != value)
                {
                    this.worldId = value;
                    this.OnNotifyPropertyChanged("WorldId");
                }
            }
        }

        public uint TeamColorId
        {
            get { return this.teamColorId; }
            set
            {
                if (this.teamColorId != value)
                {
                    this.teamColorId = value;
                    this.OnNotifyPropertyChanged("TeamColorId");
                }
            }
        }

        public bool IsCommander
        {
            get { return this.isCommander; }
            set
            {
                if (this.isCommander != value)
                {
                    this.isCommander = value;
                    this.OnNotifyPropertyChanged("IsCommander");
                }
            }
        }


        public byte[] ServerAddress
        {
            get { return this.serverAddress; }
            set
            {
                if (!object.Equals(this.serverAddress, value))
                {
                    this.serverAddress = value;
                    this.OnNotifyPropertyChanged("ServerAddress");
                }
            }
        }

        public uint MapType
        {
            get { return this.mapType; }
            set
            {
                if (this.mapType != value)
                {
                    this.mapType = value;
                    this.OnNotifyPropertyChanged("MapType");
                }
            }
        }

        public uint ShardId
        {
            get { return this.shardId; }
            set
            {
                if (this.shardId != value)
                {
                    this.shardId = value;
                    this.OnNotifyPropertyChanged("ShardId");
                }
            }
        }

        public uint Instance
        {
            get { return this.instance; }
            set
            {
                if (this.instance != value)
                {
                    this.instance = value;
                    this.OnNotifyPropertyChanged("Instance");
                }
            }
        }

        public uint BuildId
        {
            get { return this.buildId; }
            set
            {
                if (this.buildId != value)
                {
                    this.buildId = value;
                    this.OnNotifyPropertyChanged("BuildId");
                }
            }
        }


        unsafe public override void SetDataFromLinkedMem(LinkedMem data)
        {
            if (new string(data.name) != "Guild Wars 2")
            {
                this.IsValid = false;
                return;
            }

            base.SetDataFromLinkedMem(data);

            dynamic identityJson = JObject.Parse(new string(data.identity));
            this.CharacterName = identityJson.name;
            this.ProfessionId = identityJson.profession;
            this.MapId = identityJson.map_id;
            this.WorldId = identityJson.world_id;
            this.TeamColorId = identityJson.team_color_id;
            this.IsCommander = identityJson.commander;

            GW2Context gw2Context = (GW2Context)Marshal.PtrToStructure((IntPtr)data.context, typeof(GW2Context));
            this.ServerAddress = new byte[] { gw2Context.serverAddress.sin_addr.s_b1, gw2Context.serverAddress.sin_addr.s_b2,
                gw2Context.serverAddress.sin_addr.s_b3, gw2Context.serverAddress.sin_addr.s_b4 };
            this.MapType = gw2Context.mapType;
            this.ShardId = gw2Context.shardId;
            this.Instance = gw2Context.instance;
            this.BuildId = gw2Context.buildId;

            this.IsValid = true;
        }

    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct GW2Context
    {
        public SockAddr_In serverAddress;
        public uint mapId;
        public uint mapType;
        public uint shardId;
        public uint instance;
        public uint buildId;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct SockAddr_In
    {
        private ushort _sin_family;
        public ushort sin_port;
        public In_Addr sin_addr;
        public fixed byte sin_zero[8];

        public AddressFamily sin_family
        {
            get { return (AddressFamily)this._sin_family; }
            set { this._sin_family = (ushort)value; }
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct In_Addr
    {
        public byte s_b1, s_b2, s_b3, s_b4;
        public ushort s_w1, s_w2;
        public ulong s_addr;
    }

}
