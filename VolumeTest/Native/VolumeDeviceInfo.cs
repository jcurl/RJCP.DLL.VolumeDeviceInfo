namespace VolumeTest.Native
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;
    using Native.Win32;
    using static Native.Win32.Kernel32;
    using static Native.Win32.WinIoCtl;

    public class VolumeDeviceInfo
    {
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
        public VolumeDeviceInfo(string pathName)
        {
            if (pathName == null) throw new ArgumentNullException(nameof(pathName));
            if (string.IsNullOrEmpty(pathName)) throw new ArgumentException("Path is empty", nameof(pathName));

            if (!Platform.IsWinNT())
                throw new PlatformNotSupportedException();

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
        /// The volume mount point.
        /// </value>
        public string VolumePath { get; private set; } = string.Empty;

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
        public string VendorId { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the product identifier for the device.
        /// </summary>
        /// <value>The product identifier for the device.</value>
        public string ProductId { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the product revision for the device.
        /// </summary>
        /// <value>The product revision for the device.</value>
        public string ProductRevision { get; private set; } = string.Empty;

        /// <summary>
        /// Gets the device serial number.
        /// </summary>
        /// <value>The device serial number.</value>
        public string DeviceSerialNumber { get; private set; } = string.Empty;

        /// <summary>
        /// Gets a value indicating if the device is of removable media.
        /// </summary>
        /// <value>Returns <see langword="true"/> if the device is removable; otherwise, <see langword="false"/>.</value>
        public bool RemovableMedia { get; private set; }

        /// <summary>
        /// Gets a value indicating if the device supports command queueing.
        /// </summary>
        /// <value>
        /// Returns <see langword="true"/> if the device supports command queueing, <see langword="false"/> othereise.
        /// </value>
        public bool CommandQueueing { get; private set; }

        /// <summary>
        /// Gets the type of the bus the device is attached to.
        /// </summary>
        /// <value>The type of the bus the device is attached to.</value>
        public BusType BusType { get; private set; } = BusType.Unknown;

        /// <summary>
        /// Gets the SCSI device type for the device.
        /// </summary>
        /// <value>The SCSI device type for the device.</value>
        public ScsiDeviceType ScsiDeviceType { get; private set; } = ScsiDeviceType.Unknown;

        /// <summary>
        /// Gets the SCSI device modifier for the SCSI device type.
        /// </summary>
        /// <value>
        /// The SCSI device modifier for the SCSI device type.
        /// </value>
        public int ScsiDeviceModifier { get; private set; }

        private void GetDeviceInformation(string devicePathName)
        {
            SafeObjectHandle hDevice = CreateFile(devicePathName, 0,
                FileShare.FILE_SHARE_READ | FileShare.FILE_SHARE_WRITE, IntPtr.Zero, CreationDisposition.OPEN_EXISTING,
                0, SafeObjectHandle.Null);
            if (hDevice == null || hDevice.IsInvalid) {
                int e = Marshal.GetHRForLastWin32Error();
                Marshal.ThrowExceptionForHR(e, INVALID_HANDLE_VALUE);
                throw new System.IO.IOException("Couldn't open device for reading", e);
            }

            try {
                GetStorageDeviceProperty(hDevice);
            } catch {
                throw;
            } finally {
                hDevice.Close();
            }
        }

        private void GetStorageDeviceProperty(SafeObjectHandle hDevice)
        {
            SafeAllocHandle<STORAGE_PROPERTY_QUERY> storagePropertyQueryPtr = null;
            SafeAllocHandle<STORAGE_DESCRIPTOR_HEADER> storageDescriptorHeaderPtr = null;
            SafeAllocHandle<STORAGE_DEVICE_DESCRIPTOR> storageDeviceDescriptorPtr = null;
            try {
                STORAGE_PROPERTY_QUERY storagePropertyQuery = new STORAGE_PROPERTY_QUERY {
                    PropertyId = (uint)STORAGE_PROPERTY_ID.StorageDeviceProperty,
                    QueryType = (uint)STORAGE_QUERY_TYPE.PropertyStandardQuery
                };
                storagePropertyQueryPtr = new SafeAllocHandle<STORAGE_PROPERTY_QUERY>(storagePropertyQuery);

                // Get the necessary output buffer size
                STORAGE_DESCRIPTOR_HEADER storageDescriptorHeader = new STORAGE_DESCRIPTOR_HEADER {
                    Version = 0,
                    Size = 0
                };
                storageDescriptorHeaderPtr = new SafeAllocHandle<STORAGE_DESCRIPTOR_HEADER>(storageDescriptorHeader);

                bool success = DeviceIoControl(hDevice, IOCTL_STORAGE_QUERY_PROPERTY,
                    storagePropertyQueryPtr, storagePropertyQueryPtr.SizeOf,
                    storageDescriptorHeaderPtr, storageDescriptorHeaderPtr.SizeOf,
                    out uint bytesReturns, IntPtr.Zero);
                if (!success) {
                    int e = Marshal.GetHRForLastWin32Error();
                    Marshal.ThrowExceptionForHR(e, INVALID_HANDLE_VALUE);
                    throw new System.IO.IOException("Couldn't get storage header descriptor", e);
                }

                STORAGE_DESCRIPTOR_HEADER storageDescriptorHeaderResult = storageDescriptorHeaderPtr.ToStructure();

                storageDeviceDescriptorPtr = new SafeAllocHandle<STORAGE_DEVICE_DESCRIPTOR>((int)storageDescriptorHeaderResult.Size);
                success = DeviceIoControl(hDevice, IOCTL_STORAGE_QUERY_PROPERTY,
                    storagePropertyQueryPtr, storagePropertyQueryPtr.SizeOf,
                    storageDeviceDescriptorPtr, storageDeviceDescriptorPtr.SizeOf,
                    out bytesReturns, IntPtr.Zero);
                if (!success) {
                    int e = Marshal.GetHRForLastWin32Error();
                    Marshal.ThrowExceptionForHR(e, INVALID_HANDLE_VALUE);
                    throw new System.IO.IOException("Couldn't get storage descriptor", e);
                }

                STORAGE_DEVICE_DESCRIPTOR storageDeviceDescriptor = storageDeviceDescriptorPtr.ToStructure();
                if (storageDeviceDescriptor.VendorIdOffset != 0)
                    VendorId = storageDeviceDescriptorPtr.ToStringAnsi((int)storageDeviceDescriptor.VendorIdOffset);
                if (storageDeviceDescriptor.SerialNumberOffset != 0)
                    DeviceSerialNumber = storageDeviceDescriptorPtr.ToStringAnsi((int)storageDeviceDescriptor.SerialNumberOffset);
                if (storageDeviceDescriptor.ProductIdOffset != 0)
                    ProductId = storageDeviceDescriptorPtr.ToStringAnsi((int)storageDeviceDescriptor.ProductIdOffset);
                if (storageDeviceDescriptor.ProductRevisionOffset != 0)
                    ProductRevision = storageDeviceDescriptorPtr.ToStringAnsi((int)storageDeviceDescriptor.ProductRevisionOffset);
                RemovableMedia = storageDeviceDescriptor.RemovableMedia;
                CommandQueueing = storageDeviceDescriptor.CommandQueueing;
                ScsiDeviceType = (ScsiDeviceType)storageDeviceDescriptor.DeviceType;
                ScsiDeviceModifier = storageDeviceDescriptor.DeviceTypeModifier;
                BusType = (BusType)storageDeviceDescriptor.BusType;
            } finally {
                if (storagePropertyQueryPtr != null) storagePropertyQueryPtr.Close();
                if (storageDescriptorHeaderPtr != null) storageDescriptorHeaderPtr.Close();
                if (storageDeviceDescriptorPtr != null) storageDescriptorHeaderPtr.Close();
            }
        }

        private string ResolveDevicePathNames(string pathName)
        {
            Path = pathName;

            // GetVolumePathName behaves as following
            // * c: => C:\
            // * C:\ => C:\
            // * \\.\HarddiskVolume3 => \\.\HarddiskVolume3\
            // * \ => C:\ (the boot partition)
            StringBuilder volumePath = new StringBuilder(1024);
            if (GetVolumePathName(pathName, volumePath, volumePath.Capacity)) {
                VolumePath = volumePath.ToString();

                // Converts the volume path. to the Win32 device path, that we can query it with an IOCTL later. The
                // Win32 function GetVolumeNameForVolumeMountPoint adds a trailing slash, which needs to be removed for
                // some API, like the IOCTL.
                StringBuilder volumeDevicePath = new StringBuilder(1024);
                if (GetVolumeNameForVolumeMountPoint(VolumePath, volumeDevicePath, volumeDevicePath.Capacity)) {
                    if (volumeDevicePath[volumeDevicePath.Length - 1] == System.IO.Path.DirectorySeparatorChar) {
                        volumeDevicePath.Remove(volumeDevicePath.Length - 1, 1);
                    }
                    VolumeDevicePath = volumeDevicePath.ToString();
                } else {
                    // This isn't a volume or isn't mounted.
                    return string.Empty;
                }

                StringBuilder dosDevicePath = new StringBuilder(1024);
                uint tlength = QueryDosDevice(VolumePath.Substring(0, 2), dosDevicePath, dosDevicePath.Capacity);
                if (tlength > 0) {
                    VolumeDosDevicePath = dosDevicePath.ToString();
                }
                return VolumeDevicePath;
            } else {
                StringBuilder dosDevicePath = new StringBuilder(1024);
                uint tlength = QueryDosDevice(pathName, dosDevicePath, dosDevicePath.Capacity);
                if (tlength > 0) {
                    VolumeDosDevicePath = dosDevicePath.ToString();
                }

                // This could be a valid path (we don't know until it's opened later), but just not mounted as a volume.
                return pathName;
            }
        }

#if DEBUG
        public static void DebugOutput(string pathName)
        {
            Console.WriteLine("  Debug: {0}", pathName);
            int e;

            var attr = GetFileAttributes(pathName);
            Console.WriteLine("    GetFileAttributes() = {0}", attr);

            StringBuilder dosDevicePath = new StringBuilder(1024);
            uint tlength = QueryDosDevice(pathName, dosDevicePath, dosDevicePath.Capacity);
            if (tlength > 0) {
                Console.WriteLine("    QueryDosDevice() = {0}", dosDevicePath);
            } else {
                e = Marshal.GetLastWin32Error();
                Console.WriteLine("    QueryDosDevice() error: {0:X8}", e);
            }

            StringBuilder volumePath = new StringBuilder(1024);
            if (GetVolumePathName(pathName, volumePath, volumePath.Capacity)) {
                Console.WriteLine("    GetVolumePathName() = {0}", volumePath);
            } else {
                e = Marshal.GetLastWin32Error();
                Console.WriteLine("    GetVolumePathName() error: {0:X8}", e);
            }

            StringBuilder volumeDevicePath = new StringBuilder(1024);
            if (GetVolumeNameForVolumeMountPoint(pathName, volumeDevicePath, volumeDevicePath.Capacity)) {
                Console.WriteLine("    GetVolumeNameForVolumeMountPoint() = {0}", volumeDevicePath);
            } else {
                e = Marshal.GetLastWin32Error();
                Console.WriteLine("    GetVolumeNameForVolumeMountPoint() error: {0:X8}", e);
            }

            SafeObjectHandle hDevice = CreateFile(pathName, 0,
                FileShare.FILE_SHARE_READ | FileShare.FILE_SHARE_WRITE, IntPtr.Zero, CreationDisposition.OPEN_EXISTING,
                0, SafeObjectHandle.Null);
            if (hDevice == null || hDevice.IsInvalid) {
                e = Marshal.GetLastWin32Error();
                Console.WriteLine("    CreateFile() error: {0:X8}", e);
            } else {
                Console.WriteLine("    CreateFile(): OK");
                hDevice.Close();
            }
        }
#endif
    }
}
