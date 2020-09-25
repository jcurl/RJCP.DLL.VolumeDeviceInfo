namespace VolumeInfo.IO.Storage
{
    /// <summary>
    /// Type of Partition.
    /// </summary>
    public enum PartitionStyle
    {
        /// <summary>
        /// The master boot record.
        /// </summary>
        MasterBootRecord = 0,

        /// <summary>
        /// The GUID Partition Table.
        /// </summary>
        GuidPartitionTable = 1,

        /// <summary>
        /// Raw partition.
        /// </summary>
        Raw = 2
    }
}
