namespace VolumeInfo.IO.Storage
{
    using System;

    /// <summary>
    /// Gather additional information about Disks, Volumes and File systems for a given path.
    /// </summary>
    public abstract class VolumeDeviceInfo
    {
        public static VolumeDeviceInfo Create(string path)
        {
            if (Native.Platform.IsWinNT()) return new Win32.VolumeDeviceInfoWin32(path);
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeDeviceInfo"/> class.
        /// </summary>
        /// <remarks>
        /// This class cannot be instantiated direct from user code, instead, use the static
        /// <see cref="Create(string)"/> method to instantiate such an instance.
        /// </remarks>
        protected VolumeDeviceInfo() { }

        /// <summary>
        /// Gets the path as given to the constructor of this class <see cref="VolumeDeviceInfo"/>.
        /// </summary>
        /// <value>The path to check.</value>
        public string Path { get; protected set; }

        /// <summary>
        /// Gets the type of the drive.
        /// </summary>
        /// <value>
        /// The type of the drive.
        /// </value>
        public DriveType DriveType { get; protected set; }

        /// <summary>
        /// All properties relevant for Volume information.
        /// </summary>
        public interface IVolumeInfo
        {
            /// <summary>
            /// Gets the volume mount point where the specified path is mounted.
            /// </summary>
            /// <value>
            /// The volume mount point. This can be a drive letter, or a Win32 device path, of the actual volume which
            /// is related to the <see cref="Path"/>. This is calculated after all reparse points.
            /// </value>
            string Path { get; }

            /// <summary>
            /// Gets the Win32 device path for the volume.
            /// </summary>
            /// <value>The Win32 device path for the volume.</value>
            string DevicePath { get; }

            /// <summary>
            /// Gets the drive letter for the volume device in question.
            /// </summary>
            /// <value>The drive letter for the <see cref="Path"/>.</value>
            string DriveLetter { get; }

            /// <summary>
            /// Gets the NT path, volume DOS device path.
            /// </summary>
            /// <value>The NT path, volume dos device path.</value>
            string DosDevicePath { get; }
        }

        /// <summary>
        /// Gets volume information.
        /// </summary>
        /// <value>Volume information.</value>
        public IVolumeInfo Volume { get; protected set; }

        /// <summary>
        /// All properties relevant for the file system.
        /// </summary>
        public interface IFileSystemInfo
        {
            /// <summary>
            /// Gets the volume label.
            /// </summary>
            /// <value>The volume label.</value>
            string Label { get; }

            /// <summary>
            /// Gets the volume serial.
            /// </summary>
            /// <value>The volume serial.</value>
            string Serial { get; }

            /// <summary>
            /// Gets the file system flags for the volume.
            /// </summary>
            /// <value>The file system flags for the volume.</value>
            FileSystemFlags Flags { get; }

            /// <summary>
            /// Gets the name of the file system for the volume.
            /// </summary>
            /// <value>The name of the file system for the volume.</value>
            string Name { get; }

            /// <summary>
            /// Gets the bytes per sector for the file system.
            /// </summary>
            /// <value>
            /// The bytes per sector for the file system.
            /// </value>
            int BytesPerSector { get; }

            /// <summary>
            /// Gets the sectors per cluster for the file system.
            /// </summary>
            /// <value>
            /// The sectors per cluster for the file system.
            /// </value>
            int SectorsPerCluster { get; }

            /// <summary>
            /// Gets the amount of free bytes available to the user.
            /// </summary>
            /// <value>
            /// The amount of free bytes available to the user.
            /// </value>
            long UserFreeBytes { get; }

            /// <summary>
            /// Gets the total free bytes for the file system.
            /// </summary>
            /// <value>
            /// The total free bytes for the file system.
            /// </value>
            long TotalFreeBytes { get; }

            /// <summary>
            /// Gets the total number of bytes for the file system.
            /// </summary>
            /// <value>
            /// The total bytes for the file system.
            /// </value>
            long TotalBytes { get; }
        }

        /// <summary>
        /// Gets file system information.
        /// </summary>
        /// <value>File system information.</value>
        /// <remarks>
        /// This property can return <see langword="null"/> if there is no volume mounted information. This can occur
        /// for example for floppy drives, card readers, etc.
        /// </remarks>
        public IFileSystemInfo FileSystem { get; protected set; }

        /// <summary>
        /// Provides geometry information about the disk, if present.
        /// </summary>
        public interface IGeometryInfo
        {

            /// <summary>
            /// Gets the number of cylinders for the disk the volume is part of.
            /// </summary>
            /// <value>The disk cylinders.</value>
            /// <remarks>This is part of the disk geometry to get the number of Cylinders (total) for the disk.</remarks>
            long Cylinders { get; }

            /// <summary>
            /// Gets the disk tracks per cylinder.
            /// </summary>
            /// <value>The disk tracks per cylinder.</value>
            /// <remarks>
            /// This is part of the disk geometry to get the number of Tracks per Cylinder for the disk.
            /// </remarks>
            int TracksPerCylinder { get; }

            /// <summary>
            /// Gets the disk sectors per track.
            /// </summary>
            /// <value>The disk sectors per track.</value>
            /// <remarks>This is part of the disk geometry to get the number of Sectors per Track for the disk.</remarks>
            int SectorsPerTrack { get; }

            /// <summary>
            /// Gets the (logical) disk bytes per sector.
            /// </summary>
            /// <value>The disk bytes per sector.</value>
            /// <remarks>
            /// This is the number of bytes per sector for the disk geometry, and is a logical value (it is not related
            /// to the size of the actual sectors of the media itself).
            /// </remarks>
            int BytesPerSector { get; }

            /// <summary>
            /// Gets the physical disk bytes per sector.
            /// </summary>
            /// <value>The disk physical bytes per physical sector.</value>
            /// <remarks>
            /// If this value cannot be determined, then it is the same as the <see cref="DiskBytesPerSector"/>.
            /// </remarks>
            int BytesPerPhysicalSector { get; }
        }

        /// <summary>
        /// Properties relevant for the physical drive
        /// </summary>
        public interface IDeviceInfo
        {
            /// <summary>
            /// Gets the vendor identifier for the device.
            /// </summary>
            /// <value>The vendor identifier for the device.</value>
            string VendorId { get; }

            /// <summary>
            /// Gets the product identifier for the device.
            /// </summary>
            /// <value>The product identifier for the device.</value>
            string ProductId { get; }

            /// <summary>
            /// Gets the product revision for the device.
            /// </summary>
            /// <value>The product revision for the device.</value>
            string ProductRevision { get; }

            /// <summary>
            /// Gets the device serial number.
            /// </summary>
            /// <value>The device serial number.</value>
            string SerialNumber { get; }

            /// <summary>
            /// Gets a value indicating if the device supports command queueing.
            /// </summary>
            /// <value>
            /// Returns <see langword="true"/> if the device supports command queueing, <see langword="false"/>
            /// otherwise.
            /// </value>
            bool HasCommandQueueing { get; }

            /// <summary>
            /// Gets the SCSI device type for the device.
            /// </summary>
            /// <value>The SCSI device type for the device.</value>
            ScsiDeviceType ScsiDeviceType { get; }

            /// <summary>
            /// Gets the SCSI device modifier for the SCSI device type.
            /// </summary>
            /// <value>The SCSI device modifier for the SCSI device type.</value>
            int ScsiDeviceModifier { get; }

            /// <summary>
            /// Gets the type of the bus the device is attached to.
            /// </summary>
            /// <value>The type of the bus the device is attached to.</value>
            BusType BusType { get; }

            /// <summary>
            /// Gets the source of the device unique identifier.
            /// </summary>
            /// <value>The source of the device unique identifier.</value>
            DeviceGuidFlags GuidFlags { get; }

            /// <summary>
            /// Gets the device unique identifier.
            /// </summary>
            /// <value>The device unique identifier.</value>
            Guid Guid { get; }
        }

        /// <summary>
        /// All properties relevant for the disk.
        /// </summary>
        public interface IDiskInfo
        {
            IDeviceInfo Device { get; }

            /// <summary>
            /// Gets the disk extents for this volume.
            /// </summary>
            /// <value>An array of the disk extents for this volume.</value>
            /// <remarks>
            /// If the device doesn't support disk extents (e.g. a floppy disk), then this parameter will be
            /// <see langword="null"/>. Typically on Windows, if this is a basic disk, there is only one extent, and
            /// this provides the physical drive device name. The <see cref="DiskExtent.StartingOffset"/> and
            /// <see cref="DiskExtent.ExtentLength"/> are equal to the partition parameters
            /// <see cref="IPartitionInfo.Offset"/> and <see cref="IPartitionInfo.Length"/> respectively (but for
            /// multi-disk/partition volumes, the <see cref="VolumeDeviceInfo.Partition"/> is probably not available and
            /// is <see langword="null"/>.
            /// </remarks>
            DiskExtent[] Extents { get; }

            /// <summary>
            /// Gets a value indicating if the device is of removable media.
            /// </summary>
            /// <value>
            /// Returns <see langword="true"/> if the device is removable; otherwise, <see langword="false"/>.
            /// </value>
            bool IsRemovableMedia { get; }

            /// <summary>
            /// Gets if media was present at the time the device was queried on instantiation.
            /// </summary>
            /// <value><see langword="true"/> if was present; otherwise, <see langword="false"/>.</value>
            bool IsMediaPresent { get; }

            /// <summary>
            /// Gets a value indicating whether the disk will incur a seek penalty.
            /// </summary>
            /// <value>
            /// Is <see cref="BoolUnknown.True"/> if the disk will incur a seek penalty (such as a mechanical HDD), or
            /// <see cref="BoolUnknown.False"/> if not (such as an SSD). If this value cannot be determined, then
            /// <see cref="BoolUnknown.Unknown"/> is returned.
            /// </value>
            BoolUnknown HasSeekPenalty { get; }

            /// <summary>
            /// Gets a value indicating whether the disk is read-only.
            /// </summary>
            /// <value><see langword="true"/> if the disk is read only; otherwise, <see langword="false"/>.</value>
            bool IsReadOnly { get; }

            /// <summary>
            /// Gets information about the geometry of the disk.
            /// </summary>
            /// <value>
            /// Information about the geometry of the disk.
            /// </value>
            /// <remarks>
            /// This property may be <see langword="null"/>, if no disk is present.
            /// </remarks>
            IGeometryInfo Geometry { get; }
        }

        /// <summary>
        /// Gets the disk relevant information.
        /// </summary>
        /// <value>The disk relevant information.</value>
        public IDiskInfo Disk { get; protected set; }

        /// <summary>
        /// Provides information about the partition for the volume or physical device.
        /// </summary>
        public interface IPartitionInfo
        {
            /// <summary>
            /// Gets the style of the partition.
            /// </summary>
            /// <value>The style of the partition.</value>
            PartitionStyle Style { get; }

            /// <summary>
            /// Gets the partition number.
            /// </summary>
            /// <value>The partition number.</value>
            /// <remarks>
            /// The partition number is so that the first partition is the value 1. Some devices return a partition
            /// number of zero, indicating there is no partition structure, or this is the entire device.
            /// </remarks>
            int Number { get; }

            /// <summary>
            /// Gets the offset, in bytes, to where the partition starts.
            /// </summary>
            /// <value>The offset, in bytes, to where the partition starts..</value>
            long Offset { get; }

            /// <summary>
            /// Gets the length of the partition in bytes.
            /// </summary>
            /// <value>The length of the partition in bytes.</value>
            long Length { get; }
        }

        /// <summary>
        /// Information about a GUID Partition Table.
        /// </summary>
        public interface IGptPartition : IPartitionInfo
        {
            /// <summary>
            /// Gets the type of the partition table.
            /// </summary>
            /// <value>The type of the partition table.</value>
            Guid Type { get; }

            /// <summary>
            /// Gets the identifier for the partition.
            /// </summary>
            /// <value>The identifier for the partition.</value>
            Guid Id { get; }

            /// <summary>
            /// Gets the name for the partition.
            /// </summary>
            /// <value>The name for the partition.</value>
            string Name { get; }

            /// <summary>
            /// Gets the attributes associated with the GUID partition.
            /// </summary>
            /// <value>The attributes associated with the GUID partition.</value>
            EFIPartitionAttributes Attributes { get; }
        }

        /// <summary>
        /// Information about a Master Boot Record Disk.
        /// </summary>
        public interface IMbrPartition : IPartitionInfo
        {
            /// <summary>
            /// Gets or sets the type of the partition.
            /// </summary>
            /// <value>The type of the partition.</value>
            /// <remarks>
            /// For a list of partition values, see https://www.win.tue.nl/~aeb/partitions/partition_types-1.html.
            /// </remarks>
            int Type { get; }

            /// <summary>
            /// Gets or sets a value indicating whether this <see cref="MbrPartition"/> is bootable.
            /// </summary>
            /// <value><see langword="true"/> if bootable; otherwise, <see langword="false"/>.</value>
            bool Bootable { get; }

            /// <summary>
            /// Gets or sets the offset of the partition as the number of sectors.
            /// </summary>
            /// <value>The offset of the partition as the number of sectors</value>
            /// <remarks>
            /// The "hidden sectors" is a term used in Microsoft documentation. It is the size of the MBR (in sectors),
            /// to where the first partition starts. This is primarily useful in determining if a partition might be
            /// aligned correctly on 512e hard disks (these are drives that have native 4k sectors, but report 512 byte
            /// sectors for compatibility). If the sector offset is not a multiple of the native sector size, there is
            /// likely write performance degradation as each write doesn't align with the physical media (regardless of
            /// the sector size of the underlying filesystem itself).
            /// <para>
            /// As an example, Windows XP chooses 63, which is not aligned for a 4k 512e drive, even if the NTFS is
            /// formatted for 4096 bytes per sector. This is because each write starts with an offset of 63 sectors plus
            /// the position in the partition itself.
            /// </para>
            /// </remarks>
            long MbrSectorsOffset { get; }
        }

        /// <summary>
        /// Gets the partition information for the path provided.
        /// </summary>
        /// <value>The partition information for the path provided.</value>
        /// <remarks>
        /// Depending on the <see cref="PartitionStyle"/>, this class can by typecasted to get more information, e.g.
        /// <list type="bullet">
        /// <item>When <see cref="PartitionStyle.MasterBootRecord"/>, type cast to <see cref="IMbrPartition"/>.</item>
        /// <item>When <see cref="PartitionStyle.GuidPartitionTable"/>, type cast to <see cref="IGptPartition"/>.</item>
        /// </list>
        /// <para>
        /// This property can return <see langword="null"/> if there is no partition information. This can occur for
        /// example for floppy drives.
        /// </para>
        /// </remarks>
        public IPartitionInfo Partition { get; protected set; }
    }
}
