namespace RJCP.IO.Storage
{
    /// <summary>
    /// The drives understood by <see cref="VolumeDeviceInfo"/>.
    /// </summary>
    /// <remarks>
    /// This enumeration is not related to any other existing implementation in .NET or the Windows API.
    /// </remarks>
    public enum DriveType
    {
        /// <summary>
        /// The drive type is not known, or the path is invalid.
        /// </summary>
        Unknown,

        /// <summary>
        /// This is a removable drive.
        /// </summary>
        Removable,

        /// <summary>
        /// This is a floppy drive.
        /// </summary>
        Floppy,

        /// <summary>
        /// This is a fixed drive, like a hard disk that is not removable.
        /// </summary>
        Fixed,

        /// <summary>
        /// A remote network drive.
        /// </summary>
        Remote,

        /// <summary>
        /// An optical CDRom like media.
        /// </summary>
        CdRom,

        /// <summary>
        /// A RAM disk.
        /// </summary>
        RamDisk,
    }
}
