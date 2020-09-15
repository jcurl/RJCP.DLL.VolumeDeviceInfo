namespace VolumeInfo.Native.Win32
{
    using System;

    internal partial class Kernel32
    {
        [Flags]
        public enum FileAttributes
        {
            ReadOnly = 0x00000001,
            Hidden = 0x00000002,
            System = 0x00000004,
            Directory = 0x00000010,
            Archive = 0x00000020,
            Device = 0x00000040,
            Normal = 0x00000080,
            Temporary = 0x00000100,
            SparseFile = 0x00000200,
            ReparsePoint = 0x00000400,
            Compressed = 0x00000800,
            Offline = 0x00001000,
            NotContentIndexed = 0x00002000,
            Encrypted = 0x00004000,
            IntegrityStream = 0x00008000,
            Virtual = 0x00010000,
            NoScrubData = 0x00020000,
            Ea = 0x00040000,
            Pinned = 0x00080000,
            Unpinned = 0x00100000,
            RecallOnOpen = 0x00040000,
            RecallOnDataAccess = 0x00400000,
            InvalidFileAttributes = -1
        }
    }
}
