namespace VolumeInfo.IO.Storage.Win32
{
    internal class DiskFreeSpace
    {
        public int SectorsPerCluster { get; set; }

        public int BytesPerSector { get; set; }

        public long UserBytesFree { get; set; }

        public long TotalBytesFree { get; set; }

        public long TotalBytes { get; set; }
    }
}
