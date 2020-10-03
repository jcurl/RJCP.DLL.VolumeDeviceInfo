namespace RJCP.Native.Win32
{
    using System.Runtime.InteropServices;

    internal partial class WinIoCtl
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct STORAGE_DEVICE_NUMBER_EX
        {
            public int Version;
            public int Size;
            public StorageDeviceFlags Flags;
            public DeviceType DeviceType;
            public int DeviceNumber;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] DeviceGuid;
            public int PartitionNumber;
        }
    }
}
