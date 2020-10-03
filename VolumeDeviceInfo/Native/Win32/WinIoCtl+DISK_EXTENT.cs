namespace RJCP.Native.Win32
{
    using System.Runtime.InteropServices;

    internal partial class WinIoCtl
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct DISK_EXTENT
        {
            public int DeviceNumber;
            public long StartingOffset;
            public long ExtentLength;
        }
    }
}
