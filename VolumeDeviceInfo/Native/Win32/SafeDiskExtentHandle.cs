namespace RJCP.Native.Win32
{
    using System;
    using System.Runtime.InteropServices;
    using static WinIoCtl;
#if NETFRAMEWORK
    using System.Runtime.ConstrainedExecution;
#endif

    internal class SafeDiskExtentHandle : SafeAllocHandle
    {
        public SafeDiskExtentHandle()
        {
            handle = Marshal.AllocHGlobal(SizeOf);
        }

        public static int SizeOf { get { return 1024; } }

        public DISK_EXTENT[] ToStructure()
        {
            bool success = false;
            DangerousAddRef(ref success);
            if (!success) throw new InvalidOperationException();

            try {
                int length = Marshal.ReadInt32(handle);
                DISK_EXTENT[] extents = new DISK_EXTENT[length];
                int arrayElem = Marshal.SizeOf(typeof(DISK_EXTENT));
                IntPtr arrayStart = handle + 8;
                for (int i = 0; i < length; i++) {
                    extents[i] = (DISK_EXTENT)Marshal.PtrToStructure(arrayStart, typeof(DISK_EXTENT));
                    arrayStart += arrayElem;
                }
                return extents;
            } finally {
                DangerousRelease();
            }
        }

        public override bool IsInvalid { get { return handle == IntPtr.Zero; } }

#if NETFRAMEWORK
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
        protected override bool ReleaseHandle()
        {
            Marshal.FreeHGlobal(handle);
            return true;
        }
    }
}
