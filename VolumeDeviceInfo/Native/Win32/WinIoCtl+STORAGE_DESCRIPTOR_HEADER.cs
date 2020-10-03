namespace RJCP.Native.Win32
{
    using System.Runtime.InteropServices;

    internal partial class WinIoCtl
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct STORAGE_DESCRIPTOR_HEADER
        {
            public uint Version;
            public uint Size;
        }
    }
}
