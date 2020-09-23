namespace VolumeInfo.IO.Storage.Win32
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;
    using Native.Win32;
    using static Native.Win32.Kernel32;
    using static Native.Win32.WinIoCtl;

    internal class OSVolumeDeviceInfo : IOSVolumeDeviceInfo
    {
        private readonly StringBuilder m_StringBuffer = new StringBuilder(1024);

        public System.IO.FileAttributes GetFileAttributes(string pathName)
        {
            return (System.IO.FileAttributes)Kernel32.GetFileAttributes(pathName);
        }

        public string GetVolumePathName(string pathName)
        {
            if (!Kernel32.GetVolumePathName(pathName, m_StringBuffer, m_StringBuffer.Capacity)) {
                m_Win32Error = Marshal.GetLastWin32Error();
                return null;
            }
            return m_StringBuffer.ToString();
        }

        public string QueryDosDevice(string dosDevice)
        {
            if (Kernel32.QueryDosDevice(dosDevice, m_StringBuffer, m_StringBuffer.Capacity) == 0) {
                m_Win32Error = Marshal.GetLastWin32Error();
                return null;
            }
            return m_StringBuffer.ToString();
        }

        public string GetVolumeNameForVolumeMountPoint(string pathName)
        {
            if (!Kernel32.GetVolumeNameForVolumeMountPoint(pathName, m_StringBuffer, m_StringBuffer.Capacity)) {
                m_Win32Error = Marshal.GetLastWin32Error();
                return null;
            }
            return m_StringBuffer.ToString();
        }

        public SafeHandle CreateFileFromDevice(string pathName)
        {
            SafeObjectHandle hDevice = CreateFile(pathName, 0,
                FileShare.FILE_SHARE_READ | FileShare.FILE_SHARE_WRITE, IntPtr.Zero, CreationDisposition.OPEN_EXISTING,
                0, SafeObjectHandle.Null);
            if (hDevice == null || hDevice.IsInvalid) {
                m_Win32Error = Marshal.GetLastWin32Error();
                int e = Marshal.GetHRForLastWin32Error();
                Marshal.ThrowExceptionForHR(e, INVALID_HANDLE_VALUE);
                throw new System.IO.IOException("Couldn't open device for reading", e);
            }
            return hDevice;
        }

        public VolumeDeviceQuery GetStorageDeviceProperty(SafeHandle hDevice)
        {
            VolumeDeviceQuery volumeDeviceQuery = new VolumeDeviceQuery();

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
                    m_Win32Error = Marshal.GetLastWin32Error();
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
                    m_Win32Error = Marshal.GetLastWin32Error();
                    int e = Marshal.GetHRForLastWin32Error();
                    Marshal.ThrowExceptionForHR(e, INVALID_HANDLE_VALUE);
                    throw new System.IO.IOException("Couldn't get storage descriptor", e);
                }

                STORAGE_DEVICE_DESCRIPTOR storageDeviceDescriptor = storageDeviceDescriptorPtr.ToStructure();
                if (storageDeviceDescriptor.VendorIdOffset != 0)
                    volumeDeviceQuery.VendorId = storageDeviceDescriptorPtr.ToStringAnsi((int)storageDeviceDescriptor.VendorIdOffset);
                if (storageDeviceDescriptor.SerialNumberOffset != 0)
                    volumeDeviceQuery.DeviceSerialNumber = storageDeviceDescriptorPtr.ToStringAnsi((int)storageDeviceDescriptor.SerialNumberOffset);
                if (storageDeviceDescriptor.ProductIdOffset != 0)
                    volumeDeviceQuery.ProductId = storageDeviceDescriptorPtr.ToStringAnsi((int)storageDeviceDescriptor.ProductIdOffset);
                if (storageDeviceDescriptor.ProductRevisionOffset != 0)
                    volumeDeviceQuery.ProductRevision = storageDeviceDescriptorPtr.ToStringAnsi((int)storageDeviceDescriptor.ProductRevisionOffset);
                volumeDeviceQuery.RemovableMedia = storageDeviceDescriptor.RemovableMedia;
                volumeDeviceQuery.CommandQueueing = storageDeviceDescriptor.CommandQueueing;
                volumeDeviceQuery.ScsiDeviceType = (ScsiDeviceType)storageDeviceDescriptor.DeviceType;
                volumeDeviceQuery.ScsiDeviceModifier = storageDeviceDescriptor.DeviceTypeModifier;
                volumeDeviceQuery.BusType = (BusType)storageDeviceDescriptor.BusType;
            } finally {
                if (storagePropertyQueryPtr != null) storagePropertyQueryPtr.Close();
                if (storageDescriptorHeaderPtr != null) storageDescriptorHeaderPtr.Close();
                if (storageDeviceDescriptorPtr != null) storageDescriptorHeaderPtr.Close();
            }

            return volumeDeviceQuery;
        }

        public VolumeInfo GetVolumeInformation(string devicePathName)
        {
            StringBuilder volumeName = new StringBuilder(256);
            StringBuilder fileSystem = new StringBuilder(256);

            bool success = Kernel32.GetVolumeInformation(devicePathName,
                volumeName, volumeName.Capacity, out uint volumeSerial,
                out uint _, out FileSystemFlags flags, fileSystem, fileSystem.Capacity);
            if (!success) {
                m_Win32Error = Marshal.GetLastWin32Error();
                return null;
            }

            return new VolumeInfo() {
                VolumeLabel = volumeName.ToString(),
                VolumeSerial = string.Format("{0:X4}-{1:X4}", (volumeSerial & 0xFFFF0000) >> 16, volumeSerial & 0xFFFF),
                Flags = flags,
                FileSystem = fileSystem.ToString(),
            };
        }

        public bool GetMediaPresent(SafeHandle hDevice)
        {
            return DeviceIoControl(hDevice, IOCTL_STORAGE_CHECK_VERIFY2,
                IntPtr.Zero, 0, IntPtr.Zero, 0, out uint _, IntPtr.Zero);
        }

        public StorageDeviceNumber GetDeviceNumber(SafeHandle hDevice)
        {
            StorageDeviceNumber deviceNumber = new StorageDeviceNumber();
            SafeAllocHandle<STORAGE_DEVICE_NUMBER> storageDeviceNumberPtr = null;
            try {
                storageDeviceNumberPtr = new SafeAllocHandle<STORAGE_DEVICE_NUMBER>();
                bool success = DeviceIoControl(hDevice, IOCTL_STORAGE_GET_DEVICE_NUMBER, IntPtr.Zero, 0,
                    storageDeviceNumberPtr, storageDeviceNumberPtr.SizeOf, out uint bytesReturns, IntPtr.Zero);
                if (!success) {
                    m_Win32Error = Marshal.GetLastWin32Error();
                    return null;
                }
                STORAGE_DEVICE_NUMBER storageDeviceNumber = storageDeviceNumberPtr.ToStructure();
                deviceNumber.DeviceType = storageDeviceNumber.DeviceType;
                deviceNumber.DeviceNumber = storageDeviceNumber.DeviceNumber;
                deviceNumber.PartitionNumber = storageDeviceNumber.PartitionNumber;
            } finally {
                if (storageDeviceNumberPtr != null) storageDeviceNumberPtr.Close();
            }
            return deviceNumber;
        }

        public StorageDeviceNumber GetDeviceNumberEx(SafeHandle hDevice)
        {
            StorageDeviceNumber deviceNumber = new StorageDeviceNumber();
            SafeAllocHandle<STORAGE_DEVICE_NUMBER_EX> storageDeviceNumberPtr = null;
            try {
                storageDeviceNumberPtr = new SafeAllocHandle<STORAGE_DEVICE_NUMBER_EX>();
                bool success = DeviceIoControl(hDevice, IOCTL_STORAGE_GET_DEVICE_NUMBER_EX, IntPtr.Zero, 0,
                    storageDeviceNumberPtr, storageDeviceNumberPtr.SizeOf, out uint bytesReturns, IntPtr.Zero);
                if (!success) {
                    m_Win32Error = Marshal.GetLastWin32Error();
                    return null;
                }
                STORAGE_DEVICE_NUMBER_EX storageDeviceNumber = storageDeviceNumberPtr.ToStructure();
                deviceNumber.DeviceType = storageDeviceNumber.DeviceType;
                deviceNumber.DeviceNumber = storageDeviceNumber.DeviceNumber;
                deviceNumber.DeviceGuidFlags = (DeviceGuidFlags)storageDeviceNumber.Flags;
                deviceNumber.DeviceGuid = new Guid(storageDeviceNumber.DeviceGuid);
                deviceNumber.PartitionNumber = storageDeviceNumber.PartitionNumber;
            } finally {
                if (storageDeviceNumberPtr != null) storageDeviceNumberPtr.Close();
            }
            return deviceNumber;
        }

        private int m_Win32Error;

        public int GetLastWin32Error() { return m_Win32Error; }
    }
}
