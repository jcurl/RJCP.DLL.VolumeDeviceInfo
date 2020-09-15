namespace VolumeInfo.IO.Storage
{
    using System;
    using System.Runtime.InteropServices;

    public partial class VolumeDeviceInfo
    {
        private readonly IOSVolumeDeviceInfo m_OS;
        private VolumeDeviceQuery m_DeviceQuery;

        /// <summary>
        /// Initializes a new instance of the <see cref="VolumeDeviceInfo"/> class.
        /// </summary>
        /// <param name="pathName">Path to the volume that should be queried.</param>
        /// <exception cref="PlatformNotSupportedException">This software only supports Windows NT.</exception>
        /// <exception cref="ArgumentNullException">
        /// The parameter <paramref name="pathName"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">The parameter <paramref name="pathName"/> is empty.</exception>
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
            string devicePathName = ResolveDevicePathNames(pathName);
            if (!string.IsNullOrEmpty(devicePathName)) GetDeviceInformation(devicePathName);
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
        /// Gets the drive letter for the drive in question.
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
        /// Returns <see langword="true"/> if the device supports command queueing, <see langword="false"/> othereise.
        /// </value>
        public bool CommandQueueing { get { return m_DeviceQuery != null && m_DeviceQuery.CommandQueueing; } }

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

        private string ResolveDevicePathNames(string pathName)
        {
            string volumePath;
            string volumeDevicePath;

            string volumeDrive = null;
            string volumeDosDevice = null;

            while (true) {
                if (pathName.StartsWith(@"\??\")) pathName = pathName.Substring(4);
                if (string.IsNullOrEmpty(pathName)) return null;

                volumePath = m_OS.GetVolumePathName(pathName);
                if (volumePath == null) {
                    if (IsDriveLetter(pathName)) {
                        string dosDevice = m_OS.QueryDosDevice(pathName.Substring(0, 2));
                        if (volumeDosDevice == null) {
                            volumeDrive = pathName.Substring(0, 2);
                            volumeDosDevice = dosDevice;
                        }
                        if (dosDevice != null && dosDevice.StartsWith(@"\??\")) {
                            pathName = dosDevice;
                            continue;
                        }

                        // This not drive which has been SUBST'd.
                        VolumeDosDevicePath = volumeDosDevice ?? string.Empty;
                        VolumeDrive = volumeDrive ?? string.Empty;
                        return null;
                    }
                    VolumeDosDevicePath = volumeDosDevice ?? string.Empty;
                    VolumeDrive = volumeDrive ?? string.Empty;
                    return null;
                }

                if (IsDriveLetter(volumePath)) {
                    string dosDevice = m_OS.QueryDosDevice(volumePath.Substring(0, 2));
                    if (volumeDosDevice == null) {
                        volumeDrive = volumePath.Substring(0, 2);
                        volumeDosDevice = dosDevice;
                    }
                    if (dosDevice != null && dosDevice.StartsWith(@"\??\")) {
                        pathName = dosDevice;
                        continue;
                    }
                }

                // Converts the volume path. to the Win32 device path, that we can query it with an IOCTL later. The
                // Win32 function GetVolumeNameForVolumeMountPoint adds a trailing slash, which needs to be removed for
                // some API, like the IOCTL.
                volumeDevicePath = m_OS.GetVolumeNameForVolumeMountPoint(volumePath);
                if (volumeDevicePath == null) {
                    if (IsDriveLetter(volumePath.Substring(0, 3))) {
                        string dosDevice = m_OS.QueryDosDevice(volumePath.Substring(0, 2));
                        if (volumeDosDevice == null) {
                            volumeDrive = volumePath.Substring(0, 2);
                            volumeDosDevice = dosDevice;
                        }
                        if (dosDevice != null && dosDevice.StartsWith(@"\??\")) {
                            pathName = dosDevice;
                            continue;
                        }

                        // This not drive which has been SUBST'd.
                        VolumeDosDevicePath = volumeDosDevice ?? string.Empty;
                        VolumeDrive = volumeDrive ?? string.Empty;
                        return null;
                    }
                    VolumeDosDevicePath = volumeDosDevice ?? string.Empty;
                    VolumeDrive = volumeDrive ?? string.Empty;
                    return null;
                }

                if (volumeDevicePath[volumeDevicePath.Length - 1] == System.IO.Path.DirectorySeparatorChar) {
                    volumeDevicePath = volumeDevicePath.Remove(volumeDevicePath.Length - 1, 1);
                }

                VolumePath = volumePath;
                VolumeDevicePath = volumeDevicePath;
                if (volumeDosDevice == null) {
                    if (IsDriveLetter(volumePath)) {
                        volumeDrive = volumePath.Substring(0, 2);
                        volumeDosDevice = m_OS.QueryDosDevice(volumePath.Substring(0, 2));
                    }
                }
                VolumeDosDevicePath = volumeDosDevice ?? string.Empty;
                VolumeDrive = volumeDrive ?? string.Empty;
                return VolumeDevicePath;
            }
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
            } finally {
                hDevice.Close();
            }
        }

#if DEBUG
        public static void DebugOutput(string pathName)
        {
            IOSVolumeDeviceInfo osInfo = new OSVolumeDeviceInfo();
            Console.WriteLine("  Debug: {0}", pathName);
            Console.WriteLine("    GetFileAttributes: {0}", osInfo.GetFileAttributes(pathName));

            string dosDevicePath = osInfo.QueryDosDevice(pathName);
            if (dosDevicePath == null) {
                Console.WriteLine("    QueryDosDevice() error: {0:X8}", osInfo.GetLastWin32Error());
            } else {
                Console.WriteLine("    QueryDosDevice() = {0}", dosDevicePath);
            }

            string volumePath = osInfo.GetVolumePathName(pathName);
            if (volumePath == null) {
                Console.WriteLine("    GetVolumePathName() error: {0:X8}", osInfo.GetLastWin32Error());
            } else {
                Console.WriteLine("    GetVolumePathName() = {0}", volumePath);
            }

            string volumeDevicePath = osInfo.GetVolumeNameForVolumeMountPoint(pathName);
            if (volumeDevicePath == null) {
                Console.WriteLine("    GetVolumeNameForVolumeMountPoint() error: {0:X8}", osInfo.GetLastWin32Error());
            } else {
                Console.WriteLine("    GetVolumeNameForVolumeMountPoint() = {0}", volumeDevicePath);
            }

            SafeHandle hDevice = null;
            try {
                hDevice = osInfo.CreateFileFromDevice(pathName);
                Console.WriteLine("    CreateFile(): OK");
            } catch (Exception ex) {
                Console.WriteLine("    CreateFile() error: {0:X8}", osInfo.GetLastWin32Error());
                Console.WriteLine("      Exception: {0}", ex.Message);
            } finally {
                if (hDevice != null) hDevice.Close();
            }
        }
#endif
    }
}
