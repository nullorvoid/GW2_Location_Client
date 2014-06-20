using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GW2_Location_Client.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct LinkData
    {
        [MarshalAs(UnmanagedType.U4)]
        public UInt32 Version;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 Tick;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
        public float[] AvatarPosition;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
        public float[] AvatarFront;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
        public float[] AvatarTop;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string Name;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
        public float[] CameraPosition;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
        public float[] CameraFront;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)]
        public float[] CameraTop;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string Identity;

        [MarshalAs(UnmanagedType.U4)]
        public UInt32 ContextLen;

        // Context
        [MarshalAs(UnmanagedType.Struct)]
        public LinkContext Context;
    }
}
