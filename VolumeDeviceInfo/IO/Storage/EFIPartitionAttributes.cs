namespace RJCP.IO.Storage
{
    using System;

    /// <summary>
    /// Enhanced Firmware Interface partition attributes for GUID Partition Tables.
    /// </summary>
    [Flags]
    public enum EFIPartitionAttributes : long
    {
        /// <summary>
        /// There are no attributes set.
        /// </summary>
        None = 0,

        /// <summary>
        /// This is a system necessary partition for booting the Operating System.
        /// </summary>
        /// <remarks>
        /// This attribute must be set for OEM partitions, and Windows interprets this as a Basic Disk from an OEM.
        /// </remarks>
        GptAttributePlatformRequired = 0x0000000000000001,

        /// <summary>
        /// A legacy bootable partition.
        /// </summary>
        LegacyBIOSBootable = 0x0000000000000004,

        /// <summary>
        /// The partition should not be mounted with a drive letter by default.
        /// </summary>
        GptBasicDataAttributeNoDriveLetter = unchecked((long)0x8000000000000000),

        /// <summary>
        /// The partition should be hidden (on Windows, this partition is ignored by the mount manager and will not
        /// receive a volume path).
        /// </summary>
        GptBasicDataAttributeHidden = 0x4000000000000000,

        /// <summary>
        /// The partition describes a shadow copy of another partition.
        /// </summary>
        GptBasicDataAttributeShadowCopy = 0x2000000000000000,

        /// <summary>
        /// The partition is read only.
        /// </summary>
        GptBasicDataAttributeReadOnly = 0x1000000000000000
    }
}
