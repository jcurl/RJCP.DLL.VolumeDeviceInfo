namespace RJCP.IO.Storage
{
    using System;

    /// <summary>
    /// Windows File System Flags.
    /// </summary>
    /// <remarks>
    /// For more information, see
    /// https://docs.microsoft.com/en-us/windows/win32/api/fileapi/nf-fileapi-getvolumeinformationa.
    /// https://docs.microsoft.com/en-us/windows-hardware/drivers/ddi/ntifs/ns-ntifs-_file_fs_attribute_information.
    /// </remarks>
    [Flags]
    public enum FileSystemFlags
    {
        /// <summary>
        /// The file system supports case sensitive file names, so that two files with different case will be considered
        /// different files.
        /// </summary>
        CaseSensitiveSearch = 0x00000001,

        /// <summary>
        /// The file system will preserve the case of file names (this is not the same as being case sensitive).
        /// </summary>
        CasePreservedNames = 0x00000002,

        /// <summary>
        /// The file system supports Unicode file names.
        /// </summary>
        UnicodeOnDisk = 0x00000004,

        /// <summary>
        /// The file system supports persistent Access Control Lists. For example, FAT based file systems do not support
        /// access control lists.
        /// </summary>
        PersistentAcls = 0x00000008,

        /// <summary>
        /// The file system supports file-based compression.
        /// </summary>
        FileCompression = 0x00000010,

        /// <summary>
        /// The file system supports disk quotas.
        /// </summary>
        VolumeQuotas = 0x00000020,

        /// <summary>
        /// The file system supports sparse files.
        /// </summary>
        SupportsSparseFiles = 0x00000040,

        /// <summary>
        /// The file system supports reparse points. A reparse point is a directory that references a different volume.
        /// </summary>
        SupportsReparsePoints = 0x00000080,

        /// <summary>
        /// The file system supports remote storage.
        /// </summary>
        SupportsRemoteStorage = 0x00000100,

        /// <summary>
        /// Internal for driver filters, the file system can report additional actions taken during a clean up, such as
        /// deleting files. File system filters can use this to take additional actions in the post clean up process.
        /// </summary>
        ReturnsCleanupResultInfo = 0x00000200,

        /// <summary>
        /// The file system supports POSIX style delete and rename operations.
        /// </summary>
        SupportsPosixUnlinkRename = 0x00000400,

        /// <summary>
        /// The file system is compressed, for example, a DoubleSpace volume.
        /// </summary>
        VolumeIsCompressed = 0x00008000,

        /// <summary>
        /// The file system supports object identifiers.
        /// </summary>
        SupportsObjectIds = 0x00010000,

        /// <summary>
        /// The file system supports encryption via EFS (Encrypted File System), so that individual files can be
        /// encrypted.
        /// </summary>
        SupportsEncryption = 0x00020000,

        /// <summary>
        /// The file system supports named streams.
        /// </summary>
        NamedStreams = 0x00040000,

        /// <summary>
        /// The file system is read only.
        /// </summary>
        ReadOnlyVolume = 0x00080000,

        /// <summary>
        /// The file system only supports writing using a single sequential write and once only (e.g. CD-RW).
        /// </summary>
        SequentialWriteOnce = 0x00100000,

        /// <summary>
        /// The file system supports transactions.
        /// </summary>
        SupportsTransactions = 0x00200000,

        /// <summary>
        /// The file system supports hard links and junctions. Hard links are those where multiple paths can point to
        /// the same file on the same file system. Junctions are soft links for directories that can span file systems.
        /// </summary>
        SupportsHardLinks = 0x00400000,

        /// <summary>
        /// The file system supports extended attributes, which is application specific metadata associated with a file,
        /// but not part of the file itself. This is supported from Windows 7 / Windows Server 2008R2 and later.
        /// </summary>
        SupportsExtendedAttributes = 0x00800000,

        /// <summary>
        /// The file system supports open by FileId. This is supported from Windows 7 / Windows Server 2008R2 and later.
        /// This is a driver feature that allows a directory handle to be used to get information about the files in the
        /// directory.
        /// </summary>
        SupportsOpenByFileId = 0x01000000,

        /// <summary>
        /// The file system supports "Update Sequence Number" (USN) journals. This allows indexing software to identify
        /// changes since a particular entry for faster scanning. This is supported from Windows 7 / Windows Server
        /// 2008R2 and later.
        /// </summary>
        SupportsUsnJournal = 0x02000000,

        /// <summary>
        /// The file system supports integrity streams, e.g. as in ReFS.
        /// </summary>
        SupportsIntegrityStreams = 0x04000000,

        /// <summary>
        /// The file system supports block cloning, e.g. on ReFS.
        /// </summary>
        SupportsBlockRefCounting = 0x08000000,

        /// <summary>
        /// The file system tracks whether each cluster of a file contains valid data.
        /// </summary>
        SupportsSparseVdl = 0x10000000,

        /// <summary>
        /// This is a direct access volume, introduced since Windows 10 v1607.
        /// </summary>
        DaxVolume = 0x20000000,

        /// <summary>
        /// The file system supports ghosting.
        /// </summary>
        SupportsGhosting = 0x40000000
    }
}
