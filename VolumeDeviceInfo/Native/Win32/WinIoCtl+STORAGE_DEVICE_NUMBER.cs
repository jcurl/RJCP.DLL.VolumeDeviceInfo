namespace RJCP.Native.Win32
{
    using System.Runtime.InteropServices;

    internal partial class WinIoCtl
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct STORAGE_DEVICE_NUMBER
        {
            public DeviceType DeviceType;
            public int DeviceNumber;
            public int PartitionNumber;
        }
    }
}
