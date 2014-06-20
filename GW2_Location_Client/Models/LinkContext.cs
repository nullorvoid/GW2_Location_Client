using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GW2_Location_Client.Models
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LinkContext
    {
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 28)]
        public byte[] ServerAddress;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 MapId;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 MapType;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 ShardId;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 Instance;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 BuildId;
    }
}
