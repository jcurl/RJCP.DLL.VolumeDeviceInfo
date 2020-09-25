namespace VolumeInfo.IO.Storage.Win32
{
    public class MbrPartition : PartitionInformation
    {
        public MbrPartition() : base(PartitionStyle.MasterBootRecord) { }

        public int Type { get; set; }

        public bool Bootable { get; set; }

        public long HiddenSectors { get; set; }
    }
}
