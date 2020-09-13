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
        /// <param name="devicePathName">
        /// Name of the device path. Under Windows, this is the NT path, such as \\.\PhysicalDrive0.
        /// </param>
        /// <exception cref="PlatformNotSupportedException">This software only supports Windows NT.</exception>
        public VolumeDeviceInfo(string devicePathName)
        {
            if (devicePathName == null) throw new ArgumentNullException(nameof(devicePathName));

            if (!Platform.IsWinNT())
                throw new PlatformNotSupportedException();

            GetDeviceInformation(devicePathName);
            DevicePath = devicePathName;
        }

        /// <summary>
        /// Gets the path of the device in the system;
        /// </summary>
        /// <value>The path of the device.</value>
        public string DevicePath { get; private set; }

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
        public BusType BusType { get; private set; }

        /// <summary>
        /// Gets the SCSI device type for the device.
        /// </summary>
        /// <value>The SCSI device type for the device.</value>
        public ScsiDeviceType ScsiDeviceType { get; private set; }

        /// <summary>
        /// Gets the SCSI device modifier for the SCSI device type.
        /// </summary>
        /// <value>
        /// The SCSI device modifier for the SCSI device type.
        /// </value>
        public int ScsiDeviceModifier { get; private set; }

        private void GetDeviceInformation(string devicePathName)
        {
            SafeObjectHandle hDevice = CreateFile(devicePathName, 0, FileShare.FILE_SHARE_READ | FileShare.FILE_SHARE_WRITE, IntPtr.Zero, CreationDisposition.OPEN_EXISTING, 0, SafeObjectHandle.Null);
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

        private static string GetDevicePathFromWindowsPath(string drivePath)
        {
            StringBuilder devicePath = new StringBuilder(1024);
            uint cchars = QueryDosDevice(drivePath, devicePath, devicePath.Capacity);
            if (cchars == 0) {
                int e = Marshal.GetHRForLastWin32Error();
                Marshal.ThrowExceptionForHR(e, INVALID_HANDLE_VALUE);
                throw new System.IO.IOException("Couldn't get device path", e);
            }
            return devicePath.ToString();
        }
    }
}
