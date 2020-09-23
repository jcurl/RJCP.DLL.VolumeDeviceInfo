namespace VolumeInfo.Native.Win32
{
    using System.Runtime.InteropServices;
    using IO.Storage;

    internal partial class WinIoCtl
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct DISK_GEOMETRY
        {
            public long Cylinders;
            public MediaType MediaType;
            public int TracksPerCylinder;
            public int SectorsPerTrack;
            public int BytesPerSector;
        }
    }
}
