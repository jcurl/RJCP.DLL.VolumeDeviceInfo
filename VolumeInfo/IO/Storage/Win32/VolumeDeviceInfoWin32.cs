namespace VolumeInfo.IO.Storage.Win32
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Retrieve Volume Device Information on a Win32 Operating System.
    /// </summary>
    public class VolumeDeviceInfoWin32 : VolumeDeviceInfo
    {
        private readonly IOSVolumeDeviceInfo m_OS;

        private class VolumeData
        {
            public string VolumePath;
            public string VolumeDrive;
            public string VolumeDevicePath;
            public string VolumeDevicePathSlash;
            public string VolumeDosDevicePath;
            public bool MediaPresent;
            public bool DiskReadOnly;
            public int DriveType;
            public BoolUnknown HasSeekPenalty;
            public VolumeDeviceQuery DeviceQuery;
            public VolumeInfo VolumeQuery;
            public StorageDeviceNumber DeviceNumber;
            public DiskGeometry DiskGeometry;
            public StorageAccessAlignment Alignment;
            public PartitionInformation PartitionInfo;
            public DiskFreeSpace FreeSpace;
            public DiskExtent[] Extents;

            public bool IsFloppy
            {
                get
                {
                    // Floppy disk doesn't support MediaPresent, so we need to do an alternative check
                    if (DiskGeometry != null) {
                        switch (DiskGeometry.MediaType) {
                        case MediaType.F5_1pt2_512:
                        case MediaType.F3_1Pt44_512:
                        case MediaType.F3_2Pt88_512:
                        case MediaType.F3_20Pt8_512:
                        case MediaType.F3_720_512:
                        case MediaType.F5_360_512:
                        case MediaType.F5_320_512:
                        case MediaType.F5_320_1024:
                        case MediaType.F5_180_512:
                        case MediaType.F5_160_512:
                        case MediaType.F3_120M_512:
                        case MediaType.F3_640_512:
                        case MediaType.F5_640_512:
                        case MediaType.F5_720_512:
                        case MediaType.F3_1Pt2_512:
                        case MediaType.F3_1Pt23_1024:
                        case MediaType.F5_1Pt23_1024:
                        case MediaType.F3_128Mb_512:
                        case MediaType.F3_230Mb_512:
                        case MediaType.F8_256_128:
                        case MediaType.F3_200Mb_512:
                        case MediaType.F3_240M_512:
                        case MediaType.F3_32M_512:
                            return true;
                        }
                    }
                    return false;
                }
            }
        }

        private readonly VolumeData m_VolumeData = new VolumeData();

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeDeviceInfo"/> class.
        /// </summary>
        /// <param name="pathName">Path to the volume that should be queried.</param>
        /// <exception cref="PlatformNotSupportedException">This software only supports Windows NT.</exception>
        /// <exception cref="ArgumentNullException">
        /// The parameter <paramref name="pathName"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">The parameter <paramref name="pathName"/> is empty.</exception>
        /// <exception cref="System.IO.FileNotFoundException">The path is not recognized in the system.</exception>
        /// <remarks>
        /// This is a Windows only implementation that calls the Operating System direct to get information about the
        /// drive, including IOCTLs, from the path given.
        /// <para>The <paramref name="pathName"/> can be any of the following formats:</para>
        /// <list type="bullet">
        /// <item>A DOS drive letter, like <c>C:</c> or <c>C:\</c>.</item>
        /// <item>A Win32 device path, like <c>\\?\Bootpartition</c>.</item>
        /// </list>
        /// Note, that if you give an NT-path style, like <c>\Device\HarddiskVolume1</c>, it will be interpreted as a
        /// normal Win32 path, and the boot partition will be given.
        /// <para>
        /// The path name <paramref name="pathName"/> is specified as just a drive letter, e.g. <c>C:</c>, then the path
        /// used is the current directory for that drive, which may lead to unexpected results, e.g. the current path is
        /// within a junction that points to drive <c>D:\</c> which will result details for the junction, and not drive
        /// <c>C:\</c> as probably expected. Provide the trailing backslash in this case to ensure the path for the
        /// drive is returned as one would expect.
        /// </para>
        /// </remarks>
        public VolumeDeviceInfoWin32(string pathName) : this(new OSVolumeDeviceInfo(), pathName) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeDeviceInfo"/> class, specifically for abstracting the OS
        /// and testing.
        /// </summary>
        /// <param name="os">The object that simulates the OS calls.</param>
        /// <param name="pathName">Path to the volume that should be queried.</param>
        /// <exception cref="PlatformNotSupportedException">This software only supports Windows NT.</exception>
        /// <exception cref="ArgumentNullException">
        /// The parameter <paramref name="pathName"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">The parameter <paramref name="pathName"/> is empty.</exception>
        /// <remarks>
        /// This method is used to provide an implementation on how to access the Operating System, so this class isn't
        /// required to do this directly, making it testable for different OS behaviors.
        /// </remarks>
        internal VolumeDeviceInfoWin32(IOSVolumeDeviceInfo os, string pathName)
        {
            if (os == null) throw new ArgumentNullException(nameof(os));
            if (pathName == null) throw new ArgumentNullException(nameof(pathName));
            if (string.IsNullOrEmpty(pathName)) throw new ArgumentException("Path is empty", nameof(pathName));

            if (!Native.Platform.IsWinNT())
                throw new PlatformNotSupportedException();

            m_OS = os;
            Path = pathName;
            ResolveDevicePathNames(pathName);
            GetDeviceInformation();
            InitializeProperties();
        }

        private class VolumePathInfo : IVolumeInfo
        {
            private readonly VolumeData m_Data;

            public VolumePathInfo(VolumeData data) { m_Data = data; }

            public string Path { get { return m_Data.VolumePath ?? string.Empty; } }

            public string DevicePath { get { return m_Data.VolumeDevicePath ?? string.Empty; } }

            public string DriveLetter { get { return m_Data.VolumeDrive ?? string.Empty; } }

            public string DosDevicePath { get { return m_Data.VolumeDosDevicePath ?? string.Empty; } }
        }

        private class FileSystemInfo : IFileSystemInfo
        {
            private readonly VolumeData m_Data;

            public FileSystemInfo(VolumeData data) { m_Data = data; }

            public string Label { get { return m_Data.VolumeQuery?.VolumeLabel ?? string.Empty; } }

            public string Serial { get { return m_Data.VolumeQuery?.VolumeSerial ?? string.Empty; } }

            public FileSystemFlags Flags { get { return m_Data.VolumeQuery?.Flags ?? 0; } }

            public string Name { get { return m_Data.VolumeQuery?.FileSystem ?? string.Empty; } }

            public int BytesPerSector { get { return m_Data.FreeSpace?.BytesPerSector ?? 0; } }

            public int SectorsPerCluster { get { return m_Data.FreeSpace?.SectorsPerCluster ?? 0; } }

            public long UserFreeBytes { get { return m_Data.FreeSpace?.UserBytesFree ?? 0; } }

            public long TotalFreeBytes { get { return m_Data.FreeSpace?.TotalBytesFree ?? 0; } }

            public long TotalBytes { get { return m_Data.FreeSpace?.TotalBytes ?? 0; } }
        }

        private class GeometryInfo : IGeometryInfo
        {
            private readonly VolumeData m_Data;

            public GeometryInfo(VolumeData data) { m_Data = data; }

            public long Cylinders { get { return m_Data.DiskGeometry == null ? -1 : m_Data.DiskGeometry.Cylinders; } }

            public int TracksPerCylinder { get { return m_Data.DiskGeometry == null ? -1 : m_Data.DiskGeometry.TracksPerCylinder; } }

            public int SectorsPerTrack { get { return m_Data.DiskGeometry == null ? -1 : m_Data.DiskGeometry.SectorsPerTrack; } }

            public int BytesPerSector { get { return m_Data.DiskGeometry == null ? -1 : m_Data.DiskGeometry.BytesPerSector; } }

            public int BytesPerPhysicalSector { get { return m_Data.Alignment == null ? BytesPerSector : m_Data.Alignment.BytesPerPhysicalSector; } }
        }

        private class DeviceInfo : IDeviceInfo
        {
            private readonly VolumeData m_Data;

            public DeviceInfo(VolumeData data) { m_Data = data; }

            public string VendorId { get { return m_Data.DeviceQuery?.VendorId ?? string.Empty; } }

            public string ProductId { get { return m_Data.DeviceQuery?.ProductId ?? string.Empty; } }

            public string ProductRevision { get { return m_Data.DeviceQuery?.ProductRevision ?? string.Empty; } }

            public string SerialNumber { get { return m_Data.DeviceQuery?.DeviceSerialNumber ?? string.Empty; } }

            public BusType BusType { get { return m_Data.DeviceQuery == null ? BusType.Unknown : m_Data.DeviceQuery.BusType; } }

            public bool HasCommandQueueing { get { return m_Data.DeviceQuery != null && m_Data.DeviceQuery.CommandQueueing; } }

            public ScsiDeviceType ScsiDeviceType { get { return m_Data.DeviceQuery == null ? ScsiDeviceType.Unknown : m_Data.DeviceQuery.ScsiDeviceType; } }

            public int ScsiDeviceModifier { get { return m_Data.DeviceQuery == null ? 0 : m_Data.DeviceQuery.ScsiDeviceModifier; } }

            public DeviceGuidFlags GuidFlags { get { return m_Data.DeviceNumber == null ? DeviceGuidFlags.None : m_Data.DeviceNumber.DeviceGuidFlags; } }

            public Guid Guid { get { return m_Data.DeviceNumber == null ? Guid.Empty : m_Data.DeviceNumber.DeviceGuid; } }
        }

        private class DiskInfo : IDiskInfo
        {
            private readonly VolumeData m_Data;

            public DiskInfo(VolumeData data)
            {
                m_Data = data;
                if (!IsRemovableMedia || IsMediaPresent) Geometry = new GeometryInfo(data);
                if (m_Data.DeviceQuery != null) Device = new DeviceInfo(data);
            }

            public IDeviceInfo Device { get; set; }

            public DiskExtent[] Extents { get { return m_Data.Extents; } }

            public bool IsMediaPresent { get { return m_Data.MediaPresent; } }

            public bool IsRemovableMedia
            {
                get
                {
                    // Floppy drives don't return device queries, so need to get this information some other way.
                    if (m_Data.DeviceQuery != null) return m_Data.DeviceQuery.RemovableMedia;
                    if (m_Data.IsFloppy) return true;
                    if (m_Data.DiskGeometry != null) return m_Data.DiskGeometry.MediaType == MediaType.RemovableMedia;

                    // Pretty much everything has failed, and we're probably a floppy disk drive with no disk inserted.
                    // It is possible that other Device Drivers get here too, in which case we don't know what to do, so
                    // use the "magic" NT path to really determine if this is a floppy disk.
                    if (m_Data.VolumeDosDevicePath != null && m_Data.VolumeDosDevicePath.StartsWith(@"\Device\Floppy"))
                        return true;

                    return false;
                }
            }

            public BoolUnknown HasSeekPenalty { get { return m_Data.HasSeekPenalty; } }

            public bool IsReadOnly { get { return m_Data.DiskReadOnly; } }

            public IGeometryInfo Geometry { get; set; }
        }

        private class PartitionInfo : IPartitionInfo
        {
            protected readonly VolumeData m_Data;

            public PartitionInfo(VolumeData data) { m_Data = data; }

            public PartitionStyle Style { get { return m_Data.PartitionInfo.Style; } }

            public int Number { get { return m_Data.PartitionInfo.Number; } }

            public long Offset { get { return m_Data.PartitionInfo.Offset; } }

            public long Length { get { return m_Data.PartitionInfo.Length; } }
        }

        private class GptPartitionInfo : PartitionInfo, IGptPartition
        {
            private readonly GptPartition m_Gpt;

            public GptPartitionInfo(VolumeData data) : base(data)
            {
                m_Gpt = data.PartitionInfo as GptPartition;
            }

            public Guid Type { get { return m_Gpt == null ? Guid.Empty : m_Gpt.Type; } }

            public Guid Id { get { return m_Gpt == null ? Guid.Empty : m_Gpt.Id; } }

            public string Name { get { return m_Gpt?.Name ?? string.Empty; } }

            public EFIPartitionAttributes Attributes { get { return m_Gpt == null ? EFIPartitionAttributes.None : m_Gpt.Attributes; } }
        }

        private class MbrPartitionInfo : PartitionInfo, IMbrPartition
        {
            private readonly MbrPartition m_Mbr;

            public MbrPartitionInfo(VolumeData data) : base(data)
            {
                m_Mbr = data.PartitionInfo as MbrPartition;
            }

            public int Type { get { return m_Mbr == null ? -1 : m_Mbr.Type; } }

            public bool Bootable { get { return m_Mbr != null && m_Mbr.Bootable; } }

            public long MbrSectorsOffset { get { return m_Mbr == null ? 0 : m_Mbr.HiddenSectors; } }
        }

        private void ResolveDevicePathNames(string pathName)
        {
            string volumePath;
            string volumeDevicePath;
            string volumeDevicePathSlash = null;
            string volumeDrive = null;
            string volumeDosDevice = null;

            if (pathName.StartsWith(@"\??\")) pathName = pathName.Substring(4);

            int loop = 26;
            while (loop > 0) {
                loop--;
                if (string.IsNullOrEmpty(pathName)) break;

                // Finds the volume (win32 device path, or drive letter) with a trailing slash, for the path given.
                volumePath = m_OS.GetVolumePathName(pathName);
                if (volumePath == null) {
                    // GetVolumePath fails on drives that don't exist, or may fail on SUBST'd drives (e.g. Win10). If it
                    // is a SUBST'd drive, get the new path and loop again.
                    if (ParseDosDevice(pathName, ref volumeDosDevice, ref volumeDrive, ref pathName)) continue;

                    if (volumeDosDevice != null) {
                        // A drive letter that doesn't support GetVolumeNameForVolumeMountPoint, isn't subst'd.
                        volumePath = string.Format("\\\\.\\GLOBALROOT{0}\\", volumeDosDevice);
                    } else {
                        // A device that isn't a volume, i.e. a physical drive like '\\.\PhysicalDrive0'.
                        volumePath = string.Format("{0}\\",
                            pathName.TrimEnd(System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar));
                    }
                }

                // Check if the resultant path is a SUBST'd drive. Windows XP can get here. Win10 doesn't. If it is a
                // SUBST'd drive, then get the new path and loop again.
                if (ParseDosDevice(volumePath, ref volumeDosDevice, ref volumeDrive, ref pathName)) continue;

                // Converts the volume path to the Win32 device path, that we can query it with an IOCTL later. The
                // Win32 function GetVolumeNameForVolumeMountPoint adds a trailing slash, which needs to be removed for
                // some API, like the IOCTL.
                string volumeDevice = m_OS.GetVolumeNameForVolumeMountPoint(volumePath);
                if (volumeDevice == null) {
                    // There is no mount point for the drive given. It could be a SUBST'd drive with a path, in which
                    // case we take just the drive letter, get the new path from the SUBST'd drive and loop again.
                    if (ParseDosDevice(volumePath.Substring(0, 3), ref volumeDosDevice, ref volumeDrive, ref pathName)) continue;

                    // We got here, because the path can't be mapped to a volume, and the test above shows it's not
                    // SUBST'd. It could be a network drive, a physical drive, or a badly implemented driver (like the
                    // ImDisk driver that doesn't support GetVolumeNameForVolumeMountPoint()).
                    if (IsWin32Device(volumePath)) {
                        volumeDevice = volumePath;
                    } else {
                        // Probably a network drive, then we really don't have a volume device.
                        break;
                    }
                }
                if (volumeDevice[volumeDevice.Length - 1] == System.IO.Path.DirectorySeparatorChar) {
                    volumeDevicePath = volumeDevice.Remove(volumeDevice.Length - 1, 1);
                    volumeDevicePathSlash = volumeDevice;
                } else {
                    volumeDevicePath = volumeDevice;
                    volumeDevicePathSlash = string.Format("{0}{1}", volumeDevice, System.IO.Path.DirectorySeparatorChar);
                }

                m_VolumeData.VolumePath = volumePath;
                m_VolumeData.VolumeDevicePath = volumeDevicePath;
                m_VolumeData.VolumeDevicePathSlash = volumeDevicePathSlash;
                if (volumeDosDevice == null && IsDriveLetter(volumePath)) {
                    volumeDrive = volumePath.Substring(0, 2);
                    volumeDosDevice = m_OS.QueryDosDevice(volumePath.Substring(0, 2));
                }
                break;
            }

            // For some reason, the level of recursion when parsing the API is too high. This might be invalid data from
            // the OS, or a bug in the program. Obviously, we should never get here, but it's better than an infinite
            // loop.
            if (loop == 0) throw new InvalidOperationException("Operation took too long to complete");

            if (volumeDosDevice == null && volumeDevicePathSlash == null) {
                // We couldn't map the drive letter to a DOS device, and didn't find a mount. The drive probably doesn't
                // exist.
                throw new System.IO.FileNotFoundException("Path can't be resolved to a volume");
            }

            // In case a Win32 device was given, we now do a reverse lookup.
            if (volumeDrive == null)
                ResolveDriveLetter(volumeDevicePathSlash, ref volumeDrive, ref volumeDosDevice);

            m_VolumeData.VolumeDrive = volumeDrive ?? string.Empty;
            m_VolumeData.VolumeDosDevicePath = volumeDosDevice ?? string.Empty;
        }

        private bool ParseDosDevice(string path, ref string volumeDosDevice, ref string volumeDrive, ref string pathName)
        {
            if (IsDriveLetter(path)) {
                string drive = path.Substring(0, 2);
                string dosDevice = m_OS.QueryDosDevice(drive);
                if (dosDevice != null) {
                    if (volumeDosDevice == null) {
                        volumeDrive = drive;
                        volumeDosDevice = dosDevice;
                    }
                    if (dosDevice.StartsWith(@"\??\")) {
                        pathName = dosDevice.Substring(4);
                        return true;
                    }
                }

                // This is a drive letter, but not a SUBST'd drive (e.g. it could be a network path).
                return false;
            }

            // Not a drive letter (but may be another path, or a Win32 device path).
            return false;
        }

        private static bool IsDriveLetter(string path)
        {
            int pathLen = path.Length;
            if (pathLen < 2 || pathLen > 3) return false;
            if (pathLen == 3 && path[2] != System.IO.Path.DirectorySeparatorChar) return false;

            return ((path[0] >= 'a' && path[0] <= 'z') || (path[0] >= 'A' && path[0] <= 'Z')) && path[1] == ':';
        }

        private static bool IsWin32Device(string path)
        {
            if (path == null) return false;
            return path.StartsWith(@"\\.\") || path.StartsWith(@"\\?\");
        }

        private void GetDeviceInformation()
        {
            if (m_VolumeData.VolumeDevicePath == null && IsDriveLetter(m_VolumeData.VolumeDrive)) {
                string drive = string.Format("{0}{1}",
                    m_VolumeData.VolumeDrive.TrimEnd(new[] { System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar }),
                    System.IO.Path.DirectorySeparatorChar);
                m_VolumeData.DriveType = m_OS.GetDriveType(drive);
                return;
            }

            m_VolumeData.DriveType = m_OS.GetDriveType(m_VolumeData.VolumeDevicePathSlash);
            m_VolumeData.VolumeQuery = m_OS.GetVolumeInformation(m_VolumeData.VolumeDevicePathSlash);
            m_VolumeData.FreeSpace = m_OS.GetDiskFreeSpace(m_VolumeData.VolumeDevicePathSlash);

            // For floppy drives, m_OS.GetMediaPresent is false, even if media is present. Looking into the sources from
            // .NET, the DriveInfo class checks if the file attribute directory bit is set instead. So use that first,
            // and only if it fails, then use the IOCTL later.
            System.IO.FileAttributes attr = m_OS.GetFileAttributes(m_VolumeData.VolumeDevicePathSlash);
            if ((int)attr != -1) m_VolumeData.MediaPresent = (attr & System.IO.FileAttributes.Directory) != 0;

            SafeHandle hDevice = m_OS.CreateFileFromDevice(m_VolumeData.VolumeDevicePath);
            try {
                m_OS.RefreshVolume(hDevice);
                m_VolumeData.Extents = m_OS.GetDiskExtents(hDevice);
                m_VolumeData.DeviceQuery = m_OS.GetStorageDeviceProperty(hDevice);
                if ((int)attr == -1) m_VolumeData.MediaPresent = m_OS.GetMediaPresent(hDevice);
                m_VolumeData.HasSeekPenalty = m_OS.IncursSeekPenalty(hDevice);
                m_VolumeData.DeviceNumber = m_OS.GetDeviceNumberEx(hDevice);
                if (m_VolumeData.DeviceNumber == null) m_VolumeData.DeviceNumber = m_OS.GetDeviceNumber(hDevice);
                m_VolumeData.PartitionInfo = m_OS.GetPartitionInfo(hDevice);
                m_VolumeData.DiskGeometry = m_OS.GetDiskGeometry(hDevice);
                m_VolumeData.Alignment = m_OS.GetAlignment(hDevice);
                m_VolumeData.DiskReadOnly = m_OS.IsReadOnly(hDevice);
            } finally {
                hDevice.Close();
            }
        }

        private void ResolveDriveLetter(string volumeDevicePath, ref string volumeDrive, ref string volumeDosDevice)
        {
            char[] drivePath = new char[] { 'A', ':' };
            char[] driveFullPath = new char[] { 'A', ':', '\\' };
            int drives = m_OS.GetLogicalDrives();
            if (drives == 0) return;

            for (int drive = 0; drive < 26; drive++) {
                if ((drives & (1 << drive)) != 0) {
                    drivePath[0] = (char)('A' + (char)drive);

                    string dosVolume = new string(drivePath);
                    string dosDevice = m_OS.QueryDosDevice(dosVolume);
                    if (dosDevice != null && !dosDevice.StartsWith(@"\??\")) {
                        driveFullPath[0] = (char)('A' + (char)drive);
                        string dosVolumeFull = new string(driveFullPath);
                        string volume = m_OS.GetVolumeNameForVolumeMountPoint(dosVolumeFull);
                        if (volume != null && volume.Equals(volumeDevicePath)) {
                            volumeDrive = dosVolume;
                            volumeDosDevice = dosDevice;
                            return;
                        }
                    }
                }
            }
        }

        private void InitializeProperties()
        {
            Volume = new VolumePathInfo(m_VolumeData);
            if (m_VolumeData.VolumeDevicePath != null) {
                Disk = new DiskInfo(m_VolumeData);

                // It's possible that drivers return file system and partition information, even when no media is present.
                // In this case, the data is useless, and it's better that we don't present the information at all.
                if (!Disk.IsRemovableMedia || Disk.IsMediaPresent) {
                    if (m_VolumeData.VolumeQuery != null)
                        FileSystem = new FileSystemInfo(m_VolumeData);
                    if (m_VolumeData.PartitionInfo != null) {
                        // If there is no partition information, the property "Partition" is null.
                        switch (m_VolumeData.PartitionInfo.Style) {
                        case PartitionStyle.GuidPartitionTable:
                            Partition = new GptPartitionInfo(m_VolumeData);
                            break;
                        case PartitionStyle.MasterBootRecord:
                            Partition = new MbrPartitionInfo(m_VolumeData);
                            break;
                        default:
                            Partition = new PartitionInfo(m_VolumeData);
                            break;
                        }
                    }
                }
            }

            DriveType = GetDriveType();
        }

        private DriveType GetDriveType()
        {
            switch (m_VolumeData.DriveType) {
            case 0: // DRIVE_UNKNOWN:
            case 1: // DRIVE_NO_ROOT_DIR:
                return DriveType.Unknown;
            case 2: // DRIVE_REMOVABLE
                if (m_VolumeData.IsFloppy) return DriveType.Floppy;
                if (m_VolumeData.VolumeDosDevicePath != null && m_VolumeData.VolumeDosDevicePath.StartsWith(@"\Device\Floppy"))
                    return DriveType.Floppy;
                return DriveType.Removable;
            case 3: // DRIVE_FIXED
                return DriveType.Fixed;
            case 4: // DRIVE_REMOTE
                return DriveType.Remote;
            case 5: // DRIVE_CDROM
                return DriveType.CdRom;
            case 6: // DRIVE_RAMDISK
                return DriveType.RamDisk;
            }
            return DriveType.Unknown;
        }
    }
}
