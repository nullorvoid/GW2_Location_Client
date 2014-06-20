using GW2_Location_Client.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GW2_Location_Client.Service
{
    /// <summary>
    /// Using a combination of code found via the Guild Wars 2 Forums, this class was constructed.  The main two pieces of code can be found:
    /// https://wuvoverlay.codeplex.com/
    /// 
    /// </summary>
    public class PlayerPosition
    {
        private const float METER_TO_INCH = 39.3701f;
        private const string DATA_FILE = "MumbleLink";

        private MemoryMappedFile MappedFile;
        private MemoryMappedViewStream ViewStream;
        private int LinkDataSize;

        public PlayerPosition()
        {
            InitializeMemoryMap();
        }

        ~PlayerPosition()
        {
            if (ViewStream != null)
            {
                ViewStream.Dispose();
            }
            if (MappedFile != null)
            {
                MappedFile.Dispose();
            }
        }

        private void InitializeMemoryMap()
        {
            var mmf = MemoryMappedFile.CreateOrOpen(DATA_FILE, 1024 * 10);
            var view = mmf.CreateViewStream();

            MappedFile = mmf;
            ViewStream = view;
            LinkDataSize = Marshal.SizeOf(typeof(LinkData));
        }

        public Location GetCoordinates()
        {
            LinkData l = Read();
            return ParsePlayerCoords(l);
        }

        public PlayerDetails GetPlayerInfo()
        {
            LinkData l = Read();
            return ParsePlayerInfo(l);
        }

        public Player GetPlayerData()
        {
            Player player = new Player();

            LinkData l = Read();
            player.Info = ParsePlayerInfo(l);
            player.Location = ParsePlayerCoords(l);

            return player;
        }

        private LinkData Read()
        {
            LinkData data;

            byte[] byteData = new byte[LinkDataSize];
            ViewStream.Seek(0, System.IO.SeekOrigin.Begin);
            ViewStream.Read(byteData, 0, LinkDataSize);

            IntPtr intPtr = Marshal.AllocHGlobal(byteData.Length);
            Marshal.Copy(byteData, 0, intPtr, byteData.Length);
            data = (LinkData)Marshal.PtrToStructure(intPtr, typeof(LinkData));
            Marshal.FreeHGlobal(intPtr);

            return data;
        }

        private PlayerDetails ParsePlayerInfo(LinkData l)
        {
            PlayerDetails player = new PlayerDetails();
            if (!string.IsNullOrEmpty(l.Identity))
                player = JsonConvert.DeserializeObject<PlayerDetails>(l.Identity);
            return player;
        }

        private Location ParsePlayerCoords(LinkData l)
        {
            Location loc = new Location();
            loc.X = l.AvatarPosition[0] * METER_TO_INCH; //west to east
            loc.Y = l.AvatarPosition[2] * METER_TO_INCH; //north to south
            loc.Z = -l.AvatarPosition[1] * METER_TO_INCH; //altitude
            if (l.Context.ShardId < 10000)
            {
                loc.World_id = Convert.ToInt32(l.Context.ShardId);
            }
            else { loc.World_id = 0; }
            loc.Map_id = Convert.ToInt32(l.Context.MapId);
            return loc;
        }
    }
}
