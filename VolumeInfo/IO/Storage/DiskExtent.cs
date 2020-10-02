namespace VolumeInfo.IO.Storage
{
    /// <summary>
    /// Describes a disk extent for a volume.
    /// </summary>
    /// <remarks>
    /// Useful for volumes that span multiple disks or partitions, this will describe information about where the volume
    /// spans. For Windows basic disks, the <see cref="Device"/> represents the disk number, the
    /// <see cref="StartingOffset"/> is usually the offset for where the partition starts, and the
    /// <see cref="ExtentLength"/> is the size of the partition. It gets interesting for Dynamic disks, that a volume
    /// spans multiple disks or partitions.
    /// </remarks>
    public class DiskExtent
    {
        /// <summary>
        /// Gets or sets the name of the device where the extent is stored.
        /// </summary>
        /// <value>The device.</value>
        public string Device { get; set; }

        /// <summary>
        /// Gets or sets the starting offset, in bytes, on the disk.
        /// </summary>
        /// <value>The starting offset, in bytes, on the disk.</value>
        public long StartingOffset { get; set; }

        /// <summary>
        /// Gets or sets the length of the extent, in bytes, on the disk.
        /// </summary>
        /// <value>The length of the extent, in bytes, on the disk.</value>
        public long ExtentLength { get; set; }
    }
}
