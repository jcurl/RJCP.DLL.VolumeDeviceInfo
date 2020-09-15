namespace VolumeInfo.IO.Storage
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;

    public abstract class OSVolumeDeviceInfo : IOSVolumeDeviceInfo
    {
        private class ResultOrError<T>
        {
            public ResultOrError(T result) { Result = result; }

            public ResultOrError(int errorCode) { ErrorCode = errorCode; }

            public T Result { get; private set; }

            public int ErrorCode { get; private set; }
        }

        private readonly Dictionary<string, FileAttributes> m_FileAttributes = new Dictionary<string, FileAttributes>();

        protected void SetFileAttributes(string pathName, FileAttributes attributes)
        {
            if (m_FileAttributes.ContainsKey(pathName))
                throw new ArgumentException("Path name already added");
            m_FileAttributes.Add(pathName, attributes);
        }

        public FileAttributes GetFileAttributes(string pathName)
        {
            if (!m_FileAttributes.TryGetValue(pathName, out FileAttributes attributes))
                return (FileAttributes)(-1);
            return attributes;
        }

        private readonly Dictionary<string, ResultOrError<string>> m_VolumePathName = new Dictionary<string, ResultOrError<string>>();

        protected void SetVolumePathName(string pathName, string result)
        {
            if (m_VolumePathName.ContainsKey(pathName))
                throw new ArgumentException("Path name already added");
            m_VolumePathName.Add(pathName, new ResultOrError<string>(result));
        }

        protected void SetVolumePathName(string pathName, int error)
        {
            if (m_VolumePathName.ContainsKey(pathName))
                throw new ArgumentException("Path name already added");
            m_VolumePathName.Add(pathName, new ResultOrError<string>(error));
        }

        public string GetVolumePathName(string pathName)
        {
            if (!m_VolumePathName.TryGetValue(pathName, out ResultOrError<string> result))
                throw new ArgumentException("Unhandled path");

            if (result.Result == null) {
                SetLastWin32Error(result.ErrorCode);
                return null;
            }
            return result.Result;
        }

        private readonly Dictionary<string, ResultOrError<string>> m_QueryDosDevice = new Dictionary<string, ResultOrError<string>>();

        protected void SetQueryDosDevice(string pathName, string result)
        {
            if (m_QueryDosDevice.ContainsKey(pathName))
                throw new ArgumentException("Path name already added");
            m_QueryDosDevice.Add(pathName, new ResultOrError<string>(result));
        }

        protected void SetQueryDosDevice(string pathName, int error)
        {
            if (m_QueryDosDevice.ContainsKey(pathName))
                throw new ArgumentException("Path name already added");
            m_QueryDosDevice.Add(pathName, new ResultOrError<string>(error));
        }

        public string QueryDosDevice(string dosDevice)
        {
            if (dosDevice == null) throw new ArgumentNullException(nameof(dosDevice));

            if (dosDevice.Length != 2) {
                SetLastWin32Error(0xA2);    // ERROR_BAD_PATHNAME
                return null;
            }

            if (dosDevice[1] != ':' ||
                (dosDevice[0] < 'a' || dosDevice[0] > 'z') && (dosDevice[0] < 'A' || dosDevice[0] > 'Z')) {
                SetLastWin32Error(0x02);    // ERROR_FILE_NOT_FOUND
                return null;
            }

            if (!m_QueryDosDevice.TryGetValue(dosDevice, out ResultOrError<string> result)) {
                SetLastWin32Error(0x02);
                return null;
            }

            if (result.Result == null) {
                SetLastWin32Error(result.ErrorCode);
                return null;
            }
            return result.Result;
        }

        private readonly Dictionary<string, ResultOrError<string>> m_VolumeNameForVolumeMountPoint = new Dictionary<string, ResultOrError<string>>();

        protected void SetVolumeNameForVolumeMountPoint(string pathName, string result)
        {
            if (m_VolumeNameForVolumeMountPoint.ContainsKey(pathName))
                throw new ArgumentException("Path name already added");
            m_VolumeNameForVolumeMountPoint.Add(pathName, new ResultOrError<string>(result));
        }

        protected void SetVolumeNameForVolumeMountPoint(string pathName, int error)
        {
            if (m_VolumeNameForVolumeMountPoint.ContainsKey(pathName))
                throw new ArgumentException("Path name already added");
            m_VolumeNameForVolumeMountPoint.Add(pathName, new ResultOrError<string>(error));
        }

        public string GetVolumeNameForVolumeMountPoint(string pathName)
        {
            if (!m_VolumeNameForVolumeMountPoint.TryGetValue(pathName, out ResultOrError<string> result))
                throw new ArgumentException("Unhandled path");

            if (result.Result == null) {
                SetLastWin32Error(result.ErrorCode);
                return null;
            }
            return result.Result;
        }

        private class SafeTestHandle : SafeHandle
        {
            public SafeTestHandle() : base(new IntPtr(-1), true) { }

            public SafeTestHandle(IntPtr preexistingHandle, bool ownsHandle = true)
            : base(new IntPtr(-1), ownsHandle)
            {
                SetHandle(preexistingHandle);
            }

            public override bool IsInvalid
            {
                get { return handle.Equals(new IntPtr(-1)) || handle == IntPtr.Zero; }
            }

            protected override bool ReleaseHandle()
            {
                return true;
            }

            public string PathName { get; set; }
        }

        private readonly Dictionary<string, int> m_CreateFailError = new Dictionary<string, int>();

        protected void SetCreateFileFromDeviceError(string pathName, int errorCode)
        {
            if (m_CreateFailError.ContainsKey(pathName))
                throw new ArgumentException("Path name already added");
            m_CreateFailError.Add(pathName, errorCode);
        }

        public SafeHandle CreateFileFromDevice(string pathName)
        {
            if (m_CreateFailError.TryGetValue(pathName, out int result) && result != 0) {
                SetLastWin32Error(result);
                int e = Marshal.GetHRForLastWin32Error();
                Marshal.ThrowExceptionForHR(e, new IntPtr(-1));
                throw new IOException("Couldn't open device for reading", e);
            }

            SafeTestHandle handle = new SafeTestHandle(new IntPtr(1)) {
                PathName = pathName
            };

            return handle;
        }

        private readonly Dictionary<string, ResultOrError<VolumeDeviceQuery>> m_StorageDeviceProperty = new Dictionary<string, ResultOrError<VolumeDeviceQuery>>();

        protected void SetStorageDeviceProperty(string pathName, VolumeDeviceQuery result)
        {
            if (m_StorageDeviceProperty.ContainsKey(pathName))
                throw new ArgumentException("Path name already added");
            m_StorageDeviceProperty.Add(pathName, new ResultOrError<VolumeDeviceQuery>(result));
        }

        protected void SetStorageDeviceProperty(string pathName, int error)
        {
            if (m_StorageDeviceProperty.ContainsKey(pathName))
                throw new ArgumentException("Path name already added");
            m_StorageDeviceProperty.Add(pathName, new ResultOrError<VolumeDeviceQuery>(error));
        }

        public VolumeDeviceQuery GetStorageDeviceProperty(SafeHandle hDevice)
        {
            if (hDevice == null) throw new ArgumentNullException(nameof(hDevice));
            if (hDevice.IsInvalid || hDevice.IsClosed) throw new ArgumentException("Handle is invalid or closed");
            if (!(hDevice is SafeTestHandle handle)) throw new ArgumentException("Handle is the wrong type");

            if (!m_StorageDeviceProperty.TryGetValue(handle.PathName, out ResultOrError<VolumeDeviceQuery> result)) {
                throw new ArgumentException("Unhandled path");
            }

            if (result.Result == null) {
                SetLastWin32Error(result.ErrorCode);
                return null;
            }
            return result.Result;
        }

        private int m_Win32Error;

        protected void SetLastWin32Error(int error) { m_Win32Error = error; }

        public int GetLastWin32Error() { return m_Win32Error; }
    }
}
