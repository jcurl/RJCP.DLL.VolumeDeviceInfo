namespace VolumeInfo.Native.Win32
{
    using System;
    using System.Runtime.InteropServices;
    using System.Security;

    public abstract class SafeAllocHandle : SafeHandle
    {
        protected SafeAllocHandle() : base(IntPtr.Zero, true) { }
    }

    public class SafeAllocHandle<T> : SafeAllocHandle where T : struct
    {
        private readonly bool withObj;

        public SafeAllocHandle()
        {
            SizeOf = Marshal.SizeOf(typeof(T));
            try {
                // The finally part can't be interrupted by Thread.Abort
            } finally {
                handle = Marshal.AllocHGlobal(SizeOf);
            }
        }

        public SafeAllocHandle(object obj)
        {
            if (obj != null) {
                SizeOf = Marshal.SizeOf(obj);

                try {
                    // The finally part can't be interrupted by Thread.Abort
                } finally {
                    withObj = true;
                    handle = Marshal.AllocHGlobal(SizeOf);
                    Marshal.StructureToPtr(obj, handle, false);
                }
            }
        }

        public SafeAllocHandle(int size)
        {
            if (size <= 0) throw new ArgumentOutOfRangeException(nameof(size), "Must be a positive value");
            SizeOf = size;

            try {
                // The finally part can't be interrupted by Thread.Abort
            } finally {
                handle = Marshal.AllocHGlobal(SizeOf);
            }
        }

        public int SizeOf { get; private set; }

        public T ToStructure()
        {
            bool success = false;
            DangerousAddRef(ref success);
            if (!success) throw new InvalidOperationException();

            try {
                return (T)Marshal.PtrToStructure(handle, typeof(T));
            } finally {
                DangerousRelease();
            }
        }

        public string ToStringAnsi()
        {
            bool success = false;
            DangerousAddRef(ref success);
            if (!success) throw new InvalidOperationException();

            try {
                return Marshal.PtrToStringAnsi(handle);
            } finally {
                DangerousRelease();
            }
        }

        public string ToStringAnsi(int offset)
        {
            bool success = false;
            DangerousAddRef(ref success);
            if (!success) throw new InvalidOperationException();

            try {
                return Marshal.PtrToStringAnsi(handle + offset);
            } finally {
                DangerousRelease();
            }
        }

        public override bool IsInvalid { get { return handle == IntPtr.Zero; } }

        [SecurityCritical]
        protected override bool ReleaseHandle()
        {
            if (handle != IntPtr.Zero) {
                try {
                    // The finally part can't be interrupted by Thread.Abort
                } finally {
                    if (withObj) Marshal.DestroyStructure(handle, typeof(T));
                    Marshal.FreeHGlobal(handle);
                    handle = IntPtr.Zero;
                }

                return true;
            }

            return false;
        }
    }
}
