namespace VolumeTest.Native.Win32
{
    using System;
    using System.Runtime.InteropServices;
    using static Kernel32;

    internal static partial class WinIoCtl
    {
        public static int CtlCode(DeviceType type, int function, DeviceControlMethod method, DeviceControlAccess access)
        {
            return ((int)type << 16) |
                ((int)access << 14) |
                (function << 2) |
                (int)method;
        }

        public static readonly int IOCTL_STORAGE_QUERY_PROPERTY = CtlCode(DeviceType.MassStorage, 0x500, DeviceControlMethod.Buffered, DeviceControlAccess.Any);

        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern bool DeviceIoControl(SafeHandle hDevice, int dwIoControlCode,
            SafeAllocHandle lpInBuffer, int nInBufferSize, SafeAllocHandle lpOutBuffer, int nOutBufferSize,
            out uint lpBytesReturned, IntPtr lpOverlapped);
    }
}
