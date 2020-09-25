namespace VolumeInfo.IO.Storage
{
    /// <summary>
    /// Details of a partition, specific for MBR type partitions.
    /// </summary>
    public class MbrPartition : PartitionInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MbrPartition"/> class.
        /// </summary>
        public MbrPartition() : base(PartitionStyle.MasterBootRecord) { }

        /// <summary>
        /// Gets or sets the type of the partition.
        /// </summary>
        /// <value>The type of the partition.</value>
        /// <remarks>
        /// For a list of partition values, see https://www.win.tue.nl/~aeb/partitions/partition_types-1.html.
        /// </remarks>
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MbrPartition"/> is bootable.
        /// </summary>
        /// <value><see langword="true"/> if bootable; otherwise, <see langword="false"/>.</value>
        public bool Bootable { get; set; }

        /// <summary>
        /// Gets or sets the offset of the partition as the number of sectors.
        /// </summary>
        /// <value>The offset of the partition as the number of sectors</value>
        /// <remarks>
        /// The "hidden sectors" is a term used in Microsoft documentation. It is the size of the MBR (in sectors), to
        /// where the first partition starts. This is primarily useful in determining if a partition might be aligned
        /// correctly on 512e hard disks (these are drives that have native 4k sectors, but report 512 byte sectors for
        /// compatibility). If the sector offset is not a multiple of the native sector size, there is likely write
        /// performance degradation as each write doesn't align with the physical media (regardless of the sector size
        /// of the underlying filesystem itself).
        /// <para>
        /// As an example, Windows XP chooses 63, which is not aligned for a 4k 512e drive, even if the NTFS is
        /// formatted for 4096 bytes per sector. This is because each write starts with an offset of 63 sectors plus the
        /// position in the partition itself.
        /// </para>
        /// </remarks>
        public long HiddenSectors { get; set; }
    }
}
