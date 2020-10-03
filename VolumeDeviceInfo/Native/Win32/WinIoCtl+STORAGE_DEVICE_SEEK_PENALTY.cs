namespace RJCP.Native.Win32
{
    using System.Runtime.InteropServices;

    internal partial class WinIoCtl
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct STORAGE_DEVICE_SEEK_PENALTY
        {
            public uint Version;
            public uint Size;
            [MarshalAs(UnmanagedType.U1)]
            public bool IncursSeekPenalty;
        }
    }
}
