namespace VolumeInfo.IO.Storage.Win32
{
    internal class VolumeInfo
    {
        public string VolumeLabel { get; set; }

        public string VolumeSerial { get; set; }

        public string FileSystem { get; set; }

        public FileSystemFlags Flags { get; set; }
    }
}
