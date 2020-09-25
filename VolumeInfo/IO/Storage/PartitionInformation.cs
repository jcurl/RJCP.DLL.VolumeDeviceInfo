namespace VolumeInfo.IO.Storage
{
    /// <summary>
    /// Basic Partition information.
    /// </summary>
    public class PartitionInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PartitionInformation"/> class.
        /// </summary>
        /// <param name="style">The partition style.</param>
        public PartitionInformation(PartitionStyle style)
        {
            Style = style;
        }

        /// <summary>
        /// Gets or sets the style of the partition.
        /// </summary>
        /// <value>The style of the partition.</value>
        public PartitionStyle Style { get; private set; }

        /// <summary>
        /// Gets or sets the partition number.
        /// </summary>
        /// <value>The partition number, which is 1-based.</value>
        public int Number { get; set; }

        /// <summary>
        /// Gets or sets the offset, in bytes, to where the partition starts on the disk.
        /// </summary>
        /// <value>The offset, in bytes, to where the partition starts on the disk.</value>
        public long Offset { get; set; }

        /// <summary>
        /// Gets or sets the length, in bytes, of the partition.
        /// </summary>
        /// <value>The length, in bytes, of the partition.</value>
        public long Length { get; set; }
    }
}
