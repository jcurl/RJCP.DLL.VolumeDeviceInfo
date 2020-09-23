namespace VolumeInfo.IO.Storage
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using Win32;

    public partial class VolumeDeviceInfo
    {
        private readonly IOSVolumeDeviceInfo m_OS;
        private VolumeDeviceQuery m_DeviceQuery;
        private VolumeInfo m_VolumeQuery;
        private StorageDeviceNumber m_DeviceNumber;
        private DiskGeometry m_DiskGeometry;

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeDeviceInfo"/> class.
        /// </summary>
        /// <param name="pathName">Path to the volume that should be queried.</param>
        /// <exception cref="PlatformNotSupportedException">This software only supports Windows NT.</exception>
        /// <exception cref="ArgumentNullException">
        /// The parameter <paramref name="pathName"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">The parameter <paramref name="pathName"/> is empty.</exception>
        /// <exception cref="FileNotFoundException">The path is not recognized in the system.</exception>
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
        public VolumeDeviceInfo(string pathName) : this(new OSVolumeDeviceInfo(), pathName) { }

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
        protected VolumeDeviceInfo(IOSVolumeDeviceInfo os, string pathName)
        {
            if (os == null) throw new ArgumentNullException(nameof(os));
            if (pathName == null) throw new ArgumentNullException(nameof(pathName));
            if (string.IsNullOrEmpty(pathName)) throw new ArgumentException("Path is empty", nameof(pathName));

            if (!Native.Platform.IsWinNT())
                throw new PlatformNotSupportedException();

            m_OS = os;
            Path = pathName;
            string devicePathName = ResolveDevicePathNames(pathName);
            if (!string.IsNullOrEmpty(devicePathName)) {
                GetDeviceInformation(devicePathName);
                GetVolumeInformation(VolumeDevicePathSlash);
            }
        }

        /// <summary>
        /// Gets the path as given to the constructor of this class <see cref="VolumeDeviceInfo"/>.
        /// </summary>
        /// <value>
        /// The path to check.
        /// </value>
        public string Path { get; private set; }

        /// <summary>
        /// Gets the volume mount point where the specified path is mounted.
        /// </summary>
        /// <value>
        /// The volume mount point. This can be a drive letter, or a Win32 device path, of the actual volume which is
        /// related to the <see cref="Path"/>. This is calculated after all reparse points.
        /// </value>
        public string VolumePath { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the drive letter for the volume device in question.
        /// </summary>
        /// <value>
        /// The drive letter for the <see cref="Path"/>.
        /// </value>
        public string VolumeDrive { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the Win32 device path for the volume.
        /// </summary>
        /// <value>
        /// The Win32 device path for the volume.
        /// </value>
        public string VolumeDevicePath { get; private set; } = string.Empty;

        private string VolumeDevicePathSlash { get; set; } = string.Empty;

        /// <summary>
        /// Gets the NT path, volume DOS device path.
        /// </summary>
        /// <value>
        /// The NT path, volume dos device path.
        /// </value>
        public string VolumeDosDevicePath { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the vendor identifier for the device.
        /// </summary>
        /// <value>The vendor identifier for the device.</value>
        public string VendorId { get { return m_DeviceQuery?.VendorId ?? string.Empty; } }

        /// <summary>
        /// Gets the product identifier for the device.
        /// </summary>
        /// <value>The product identifier for the device.</value>
        public string ProductId { get { return m_DeviceQuery?.ProductId ?? string.Empty; } }

        /// <summary>
        /// Gets the product revision for the device.
        /// </summary>
        /// <value>The product revision for the device.</value>
        public string ProductRevision { get { return m_DeviceQuery?.ProductRevision ?? string.Empty; } }

        /// <summary>
        /// Gets the device serial number.
        /// </summary>
        /// <value>The device serial number.</value>
        public string DeviceSerialNumber { get { return m_DeviceQuery?.DeviceSerialNumber ?? string.Empty; } }

        /// <summary>
        /// Gets a value indicating if the device is of removable media.
        /// </summary>
        /// <value>Returns <see langword="true"/> if the device is removable; otherwise, <see langword="false"/>.</value>
        public bool RemovableMedia { get { return m_DeviceQuery != null && m_DeviceQuery.RemovableMedia; } }

        /// <summary>
        /// Gets a value indicating if the device supports command queueing.
        /// </summary>
        /// <value>
        /// Returns <see langword="true"/> if the device supports command queueing, <see langword="false"/> otherwise.
        /// </value>
        public bool CommandQueueing { get { return m_DeviceQuery != null && m_DeviceQuery.CommandQueueing; } }

        /// <summary>
        /// Gets if media was present at the time the device was queried on instantiation.
        /// </summary>
        /// <value><see langword="true"/> if was present; otherwise, <see langword="false"/>.</value>
        public bool MediaPresent { get; private set; }

        /// <summary>
        /// Gets the type of the bus the device is attached to.
        /// </summary>
        /// <value>The type of the bus the device is attached to.</value>
        public BusType BusType { get { return m_DeviceQuery == null ? BusType.Unknown : m_DeviceQuery.BusType; } }

        /// <summary>
        /// Gets the SCSI device type for the device.
        /// </summary>
        /// <value>The SCSI device type for the device.</value>
        public ScsiDeviceType ScsiDeviceType { get { return m_DeviceQuery == null ? ScsiDeviceType.Unknown : m_DeviceQuery.ScsiDeviceType; } }

        /// <summary>
        /// Gets the SCSI device modifier for the SCSI device type.
        /// </summary>
        /// <value>
        /// The SCSI device modifier for the SCSI device type.
        /// </value>
        public int ScsiDeviceModifier { get { return m_DeviceQuery == null ? 0 : m_DeviceQuery.ScsiDeviceModifier; } }

        /// <summary>
        /// Gets the volume label.
        /// </summary>
        /// <value>
        /// The volume label.
        /// </value>
        public string VolumeLabel { get { return m_VolumeQuery?.VolumeLabel ?? string.Empty; } }

        /// <summary>
        /// Gets the volume serial.
        /// </summary>
        /// <value>
        /// The volume serial.
        /// </value>
        public string VolumeSerial { get { return m_VolumeQuery?.VolumeSerial ?? string.Empty; } }

        /// <summary>
        /// Gets the name of the file system for the volume.
        /// </summary>
        /// <value>
        /// The name of the file system for the volume.
        /// </value>
        public string FileSystem { get { return m_VolumeQuery?.FileSystem ?? string.Empty; } }

        /// <summary>
        /// Gets the file system flags for the volume.
        /// </summary>
        /// <value>
        /// The file system flags for the volume.
        /// </value>
        public FileSystemFlags FileSystemFlags { get { return m_VolumeQuery == null ? 0 : m_VolumeQuery.Flags; } }

        /// <summary>
        /// Gets the Device Type where the volume is found.
        /// </summary>
        /// <value>
        /// The type of the device.
        /// </value>
        public DeviceType DeviceType { get { return m_DeviceNumber == null ? DeviceType.Unknown : m_DeviceNumber.DeviceType; } }

        /// <summary>
        /// Gets the device number assigned in the system for this volume.
        /// </summary>
        /// <value>The device number.</value>
        /// <remarks>
        /// This value is unique in the system for the given <see cref="DeviceType"/>. THe Device Number remains
        /// constant until the device is removed, or the system is restarted.
        /// </remarks>
        public int DeviceNumber { get { return m_DeviceNumber == null ? -1 : m_DeviceNumber.DeviceNumber; } }

        /// <summary>
        /// Gets the partition number assigned in the system for this volume.
        /// </summary>
        /// <value>
        /// The partition number assigned in the system for this volume.
        /// </value>
        public int DevicePartitionNumber { get { return m_DeviceNumber == null ? -1 : m_DeviceNumber.PartitionNumber; } }

        /// <summary>
        /// Gets the source of the device unique identifier.
        /// </summary>
        /// <value>
        /// The source of the device unique identifier.
        /// </value>
        public DeviceGuidFlags DeviceGuidFlags { get { return m_DeviceNumber == null ? DeviceGuidFlags.None : m_DeviceNumber.DeviceGuidFlags; } }

        /// <summary>
        /// Gets the device unique identifier.
        /// </summary>
        /// <value>
        /// The device unique identifier.
        /// </value>
        public Guid DeviceGuid { get { return m_DeviceNumber == null ? Guid.Empty : m_DeviceNumber.DeviceGuid; } }

        /// <summary>
        /// Gets the type of the disk media, if it's removable, fixed or a Floppy disk.
        /// </summary>
        /// <value>
        /// The type of the disk media.
        /// </value>
        public MediaType DiskMediaType { get { return m_DiskGeometry == null ? MediaType.Unknown : m_DiskGeometry.MediaType; } }

        /// <summary>
        /// Gets the number of cylinders for the disk the volume is part of.
        /// </summary>
        /// <value>
        /// The disk cylinders.
        /// </value>
        /// <remarks>
        /// This is part of the disk geometry to get the number of Cylinders (total) for the disk.
        /// </remarks>
        public long DiskCylinders { get { return m_DiskGeometry == null ? -1 : m_DiskGeometry.Cylinders; } }

        /// <summary>
        /// Gets the disk tracks per cylinder.
        /// </summary>
        /// <value>
        /// The disk tracks per cylinder.
        /// </value>
        /// <remarks>
        /// This is part of the disk geometry to get the number of Tracks per Cylinder for the disk.
        /// </remarks>
        public int DiskTracksPerCylinder { get { return m_DiskGeometry == null ? -1 : m_DiskGeometry.TracksPerCylinder; } }

        /// <summary>
        /// Gets the disk sectors per track.
        /// </summary>
        /// <value>
        /// The disk sectors per track.
        /// </value>
        /// <remarks>
        /// This is part of the disk geometry to get the number of Sectors per Track for the disk.
        /// </remarks>
        public int DiskSectorsPerTrack { get { return m_DiskGeometry == null ? -1 : m_DiskGeometry.SectorsPerTrack; } }

        /// <summary>
        /// Gets the (logical) disk bytes per sector.
        /// </summary>
        /// <value>
        /// The disk bytes per sector.
        /// </value>
        /// <remarks>
        /// This is the number of bytes per sector for the disk geometry, and is a logical value (it is not related to
        /// the size of the actual sectors of the media itself).
        /// </remarks>
        public int DiskBytesPerSector { get { return m_DiskGeometry == null ? -1 : m_DiskGeometry.BytesPerSector; } }

        private string ResolveDevicePathNames(string pathName)
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
                    break;
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
                    break;
                }
                if (volumeDevice[volumeDevice.Length - 1] == System.IO.Path.DirectorySeparatorChar) {
                    volumeDevicePath = volumeDevice.Remove(volumeDevice.Length - 1, 1);
                    volumeDevicePathSlash = volumeDevice;
                } else {
                    volumeDevicePath = volumeDevice;
                    volumeDevicePathSlash = string.Format("{0}{1}", volumeDevice, System.IO.Path.DirectorySeparatorChar);
                }

                VolumePath = volumePath;
                VolumeDevicePath = volumeDevicePath;
                VolumeDevicePathSlash = volumeDevicePathSlash;
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
                throw new FileNotFoundException("Path can't be resolved to a volume");
            }

            VolumeDrive = volumeDrive ?? string.Empty;
            VolumeDosDevicePath = volumeDosDevice ?? string.Empty;
            return VolumeDevicePath;
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

        private void GetDeviceInformation(string devicePathName)
        {
            SafeHandle hDevice = m_OS.CreateFileFromDevice(devicePathName);
            try {
                m_DeviceQuery = m_OS.GetStorageDeviceProperty(hDevice);
                MediaPresent = m_OS.GetMediaPresent(hDevice);
                m_DeviceNumber = m_OS.GetDeviceNumberEx(hDevice);
                if (m_DeviceNumber == null) m_DeviceNumber = m_OS.GetDeviceNumber(hDevice);
                m_DiskGeometry = m_OS.GetDiskGeometry(hDevice);
            } finally {
                hDevice.Close();
            }
        }

        private void GetVolumeInformation(string devicePathName)
        {
            m_VolumeQuery = m_OS.GetVolumeInformation(devicePathName);
        }
    }
}
