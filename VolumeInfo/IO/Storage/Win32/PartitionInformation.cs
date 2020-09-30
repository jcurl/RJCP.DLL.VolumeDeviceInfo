namespace VolumeInfo.IO.Storage.Win32
{
    internal class PartitionInformation
    {
        public PartitionInformation(PartitionStyle style)
        {
            Style = style;
        }

        public PartitionStyle Style { get; private set; }

        public int Number { get; set; }

        public long Offset { get; set; }

        public long Length { get; set; }
    }
}
