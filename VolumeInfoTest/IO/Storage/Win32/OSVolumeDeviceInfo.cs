namespace VolumeInfo.IO.Storage.Win32
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Xml;

    public class OSVolumeDeviceInfo : IOSVolumeDeviceInfo
    {
        private const string RootNode = "VolumeInfoTest";
        private const string PathNode = "Path";
        private const string PathAttr = "path";

        private class ResultOrError<T>
        {
            public ResultOrError(T result) { Result = result; }

            public ResultOrError(int errorCode, bool throws)
            {
                ErrorCode = errorCode;
                Throws = throws;
            }

            public T Result { get; private set; }

            public int ErrorCode { get; private set; }

            public bool Throws { get; private set; }
        }

        public OSVolumeDeviceInfo(string fileName)
        {
            XmlDocument xmlDoc = new XmlDocument {
                XmlResolver = null
            };
            xmlDoc.Load(fileName);

            XmlNodeList paths = xmlDoc.SelectNodes($"/{RootNode}/{PathNode}");
            foreach (XmlElement path in paths) {
                ParsePath(path);
            }
        }

        private void ParsePath(XmlElement pathNode)
        {
            string path = pathNode.GetAttribute(PathAttr);
            Console.WriteLine("Adding path: {0}", path);
            AddItem(m_FileAttributes, path, pathNode["FileAttributes"]);
            AddItem(m_VolumePathName, path, pathNode["VolumePathName"]);
            AddItem(m_QueryDosDevice, path, pathNode["QueryDosDevice"]);
            AddItem(m_VolumeMountPoint, path, pathNode["VolumeNameForVolumeMountPoint"]);
            AddItem(m_CreateFileFromDevice, path, pathNode["CreateFileFromDevice"]);
            AddItem(m_MediaPresent, path, pathNode["MediaPresent"]);
            AddStorageDevice(m_StorageProperties, path, pathNode["StorageDeviceProperty"]);
            AddVolumeInfo(m_VolumeInfo, path, pathNode["VolumeInformation"]);
            AddDeviceNumber(m_DeviceNumber, path, pathNode["StorageDeviceNumber"]);
            AddDeviceNumberEx(m_DeviceNumberEx, path, pathNode["StorageDeviceNumberEx"]);
            AddGeometry(m_Geometry, path, pathNode["DiskGeometry"]);
        }

        private void AddStorageDevice(IDictionary<string, ResultOrError<VolumeDeviceQuery>> dictionary, string path, XmlElement node)
        {
            if (node == null) return;
            if (dictionary.ContainsKey(path)) return;

            // Because this is a complex type, The XML will always return the default value.
            ResultOrError<VolumeDeviceQuery> result = GetResultOrError<VolumeDeviceQuery>(node);
            if (result != null) {
                dictionary.Add(path, result);
                return;
            }

            string removableMedia = node["RemovableMedia"].Attributes["result"].Value;
            string cmdQueuing = node["CommandQueueing"].Attributes["result"].Value;
            string scsiDevType = node["ScsiDeviceType"].Attributes["result"].Value;
            string scsiDevMod = node["ScsiModifier"].Attributes["result"].Value;
            string busType = node["BusType"].Attributes["result"].Value;
            VolumeDeviceQuery devQuery = new VolumeDeviceQuery() {
                VendorId = node["VendorId"].Attributes["result"].Value,
                DeviceSerialNumber = node["DeviceSerialNumber"].Attributes["result"].Value,
                ProductId = node["ProductId"].Attributes["result"].Value,
                ProductRevision = node["ProductRevision"].Attributes["result"].Value,
                RemovableMedia = bool.Parse(removableMedia),
                CommandQueueing = bool.Parse(cmdQueuing),
                ScsiDeviceType = (ScsiDeviceType)int.Parse(scsiDevType, CultureInfo.InvariantCulture),
                ScsiDeviceModifier = int.Parse(scsiDevMod, CultureInfo.InvariantCulture),
                BusType = (BusType)int.Parse(busType, CultureInfo.InvariantCulture)
            };
            dictionary.Add(path, new ResultOrError<VolumeDeviceQuery>(devQuery));
        }

        private void AddVolumeInfo(IDictionary<string, ResultOrError<VolumeInfo>> dictionary, string path, XmlElement node)
        {
            if (node == null) return;
            if (dictionary.ContainsKey(path)) return;

            // Because this is a complex type, The XML will always return the default value.
            ResultOrError<VolumeInfo> result = GetResultOrError<VolumeInfo>(node);
            if (result != null) {
                dictionary.Add(path, result);
                return;
            }

            string flags = node["Flags"].Attributes["result"].Value;
            VolumeInfo volInfo = new VolumeInfo() {
                VolumeLabel = node["Label"].Attributes["result"].Value,
                VolumeSerial = node["SerialNumber"].Attributes["result"].Value,
                FileSystem = node["FileSystem"].Attributes["result"].Value,
                Flags = (FileSystemFlags)int.Parse(flags, CultureInfo.InvariantCulture)
            };
            dictionary.Add(path, new ResultOrError<VolumeInfo>(volInfo));
        }

        private void AddDeviceNumber(IDictionary<string, ResultOrError<StorageDeviceNumber>> dictionary, string path, XmlElement node)
        {
            if (node == null) return;
            if (dictionary.ContainsKey(path)) return;

            // Because this is a complex type, The XML will always return the default value.
            ResultOrError<StorageDeviceNumber> result = GetResultOrError<StorageDeviceNumber>(node);
            if (result != null) {
                dictionary.Add(path, result);
                return;
            }

            string deviceType = node["DeviceType"].Attributes["result"].Value;
            string deviceNumber = node["DeviceNumber"].Attributes["result"].Value;
            string devicePartition = node["DevicePartition"].Attributes["result"].Value;
            StorageDeviceNumber storageDevNum = new StorageDeviceNumber() {
                DeviceType = (DeviceType)int.Parse(deviceType, CultureInfo.InvariantCulture),
                DeviceNumber = int.Parse(deviceNumber, CultureInfo.InvariantCulture),
                PartitionNumber = int.Parse(devicePartition, CultureInfo.InvariantCulture),
            };
            dictionary.Add(path, new ResultOrError<StorageDeviceNumber>(storageDevNum));
        }

        private void AddDeviceNumberEx(IDictionary<string, ResultOrError<StorageDeviceNumber>> dictionary, string path, XmlElement node)
        {
            if (node == null) return;
            if (dictionary.ContainsKey(path)) return;

            // Because this is a complex type, The XML will always return the default value.
            ResultOrError<StorageDeviceNumber> result = GetResultOrError<StorageDeviceNumber>(node);
            if (result != null) {
                dictionary.Add(path, result);
                return;
            }

            string deviceType = node["DeviceType"].Attributes["result"].Value;
            string deviceGuidFlags = node["DeviceGuidFlags"].Attributes["result"].Value;
            string deviceNumber = node["DeviceNumber"].Attributes["result"].Value;
            string devicePartition = node["DevicePartition"].Attributes["result"].Value;
            StorageDeviceNumber storageDevNum = new StorageDeviceNumber() {
                DeviceType = (DeviceType)int.Parse(deviceType, CultureInfo.InvariantCulture),
                DeviceGuidFlags = (DeviceGuidFlags)int.Parse(deviceGuidFlags, CultureInfo.InvariantCulture),
                DeviceGuid = new Guid(node["DeviceGuid"].Attributes["result"].Value),
                DeviceNumber = int.Parse(deviceNumber, CultureInfo.InvariantCulture),
                PartitionNumber = int.Parse(devicePartition, CultureInfo.InvariantCulture),
            };
            dictionary.Add(path, new ResultOrError<StorageDeviceNumber>(storageDevNum));
        }

        private void AddGeometry(IDictionary<string, ResultOrError<DiskGeometry>> dictionary, string path, XmlElement node)
        {
            if (node == null) return;
            if (dictionary.ContainsKey(path)) return;

            // Because this is a complex type, The XML will always return the default value.
            ResultOrError<DiskGeometry> result = GetResultOrError<DiskGeometry>(node);
            if (result != null) {
                dictionary.Add(path, result);
                return;
            }

            string mediaType = node["MediaType"].Attributes["result"].Value;
            string cylinders = node["Cylinders"].Attributes["result"].Value;
            string tracks = node["TracksPerCylinder"].Attributes["result"].Value;
            string sectors = node["SectorsPerTrack"].Attributes["result"].Value;
            string bytes = node["BytesPerSector"].Attributes["result"].Value;
            DiskGeometry diskGeo = new DiskGeometry() {
                MediaType = (MediaType)int.Parse(mediaType, CultureInfo.InvariantCulture),
                Cylinders = int.Parse(cylinders, CultureInfo.InvariantCulture),
                TracksPerCylinder = int.Parse(tracks, CultureInfo.InvariantCulture),
                SectorsPerTrack = int.Parse(sectors, CultureInfo.InvariantCulture),
                BytesPerSector = int.Parse(bytes, CultureInfo.InvariantCulture),
            };
            dictionary.Add(path, new ResultOrError<DiskGeometry>(diskGeo));
        }

        private void AddItem<T>(IDictionary<string, ResultOrError<T>> dictionary, string path, XmlElement node)
        {
            if (node == null) return;
            if (dictionary.ContainsKey(path)) return;

            ResultOrError<T> result = GetResultOrError<T>(node);
            if (result == null)
                throw new ArgumentException("Complex types not supported", nameof(dictionary));

            dictionary.Add(path, result);
        }

        private ResultOrError<T> GetResultOrError<T>(XmlElement node)
        {
            string nodeResult = node.Attributes["result"]?.Value;
            string errorCodeStr = node.Attributes["error"]?.Value;
            string throwsStr = node.Attributes["throws"]?.Value;
            if (errorCodeStr != null) {
                int errorCode = int.Parse(errorCodeStr, CultureInfo.InvariantCulture);
                bool throws = false;
                if (throwsStr != null) throws = bool.Parse(throwsStr);
                return new ResultOrError<T>(errorCode, throws);
            } else {
                if (typeof(int).IsAssignableFrom(typeof(T))) {
                    int iResult = int.Parse(nodeResult, CultureInfo.InvariantCulture);
                    return new ResultOrError<T>((T)Convert.ChangeType(iResult, typeof(T)));
                } else if (typeof(bool).IsAssignableFrom(typeof(T))) {
                    bool bResult = bool.Parse(nodeResult);
                    return new ResultOrError<T>((T)Convert.ChangeType(bResult, typeof(T)));
                } else if (typeof(string).IsAssignableFrom(typeof(T))) {
                    return new ResultOrError<T>((T)Convert.ChangeType(nodeResult, typeof(T)));
                } else {
                    return null;
                }
            }
        }

        private T GetResultOrThrow<T>(Dictionary<string, ResultOrError<T>> dictionary, string pathName)
        {
            if (!dictionary.TryGetValue(pathName, out ResultOrError<T> result)) {
                string message = string.Format("Path name '{0}' not defined", pathName);
                throw new ArgumentException(message, nameof(pathName));
            }
            if (result.ErrorCode != 0) m_LastWin32Error = result.ErrorCode;
            if (result.Throws) throw new IOException("IO Exception");
            return result.Result;
        }

        private readonly Dictionary<string, ResultOrError<int>> m_FileAttributes = new Dictionary<string, ResultOrError<int>>();

        public FileAttributes GetFileAttributes(string pathName)
        {
            return (FileAttributes)GetResultOrThrow(m_FileAttributes, pathName);
        }

        private readonly Dictionary<string, ResultOrError<string>> m_VolumePathName = new Dictionary<string, ResultOrError<string>>();

        public string GetVolumePathName(string pathName)
        {
            return GetResultOrThrow(m_VolumePathName, pathName);
        }

        private readonly Dictionary<string, ResultOrError<string>> m_QueryDosDevice = new Dictionary<string, ResultOrError<string>>();

        public string QueryDosDevice(string dosDevice)
        {
            return GetResultOrThrow(m_QueryDosDevice, dosDevice);
        }

        private readonly Dictionary<string, ResultOrError<string>> m_VolumeMountPoint = new Dictionary<string, ResultOrError<string>>();

        public string GetVolumeNameForVolumeMountPoint(string pathName)
        {
            return GetResultOrThrow(m_VolumeMountPoint, pathName);
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

        private readonly Dictionary<string, ResultOrError<string>> m_CreateFileFromDevice = new Dictionary<string, ResultOrError<string>>();

        public SafeHandle CreateFileFromDevice(string pathName)
        {
            string result = GetResultOrThrow(m_CreateFileFromDevice, pathName);
            if (result == null) return null;

            SafeTestHandle handle = new SafeTestHandle(new IntPtr(1)) {
                PathName = pathName
            };
            return handle;
        }

        private static SafeTestHandle CheckHandle(SafeHandle hDevice)
        {
            if (hDevice == null) throw new ArgumentNullException(nameof(hDevice));
            if (hDevice.IsInvalid || hDevice.IsClosed) throw new ArgumentException("Handle is invalid or closed");
            if (!(hDevice is SafeTestHandle handle)) throw new ArgumentException("Handle is the wrong type");
            return handle;
        }

        private readonly Dictionary<string, ResultOrError<VolumeDeviceQuery>> m_StorageProperties = new Dictionary<string, ResultOrError<VolumeDeviceQuery>>();

        public VolumeDeviceQuery GetStorageDeviceProperty(SafeHandle hDevice)
        {
            SafeTestHandle handle = CheckHandle(hDevice);
            return GetResultOrThrow(m_StorageProperties, handle.PathName);
        }

        private readonly Dictionary<string, ResultOrError<VolumeInfo>> m_VolumeInfo = new Dictionary<string, ResultOrError<VolumeInfo>>();

        public VolumeInfo GetVolumeInformation(string devicePathName)
        {
            return GetResultOrThrow(m_VolumeInfo, devicePathName);
        }

        private readonly Dictionary<string, ResultOrError<bool>> m_MediaPresent = new Dictionary<string, ResultOrError<bool>>();

        public bool GetMediaPresent(SafeHandle hDevice)
        {
            SafeTestHandle handle = CheckHandle(hDevice);
            return GetResultOrThrow(m_MediaPresent, handle.PathName);
        }

        private readonly Dictionary<string, ResultOrError<StorageDeviceNumber>> m_DeviceNumber = new Dictionary<string, ResultOrError<StorageDeviceNumber>>();

        public StorageDeviceNumber GetDeviceNumber(SafeHandle hDevice)
        {
            SafeTestHandle handle = CheckHandle(hDevice);
            return GetResultOrThrow(m_DeviceNumber, handle.PathName);
        }

        private readonly Dictionary<string, ResultOrError<StorageDeviceNumber>> m_DeviceNumberEx = new Dictionary<string, ResultOrError<StorageDeviceNumber>>();

        public StorageDeviceNumber GetDeviceNumberEx(SafeHandle hDevice)
        {
            SafeTestHandle handle = CheckHandle(hDevice);
            return GetResultOrThrow(m_DeviceNumberEx, handle.PathName);
        }

        private readonly Dictionary<string, ResultOrError<DiskGeometry>> m_Geometry = new Dictionary<string, ResultOrError<DiskGeometry>>();

        public DiskGeometry GetDiskGeometry(SafeHandle hDevice)
        {
            SafeTestHandle handle = CheckHandle(hDevice);
            return GetResultOrThrow(m_Geometry, handle.PathName);
        }

        private int m_LastWin32Error;

        public int GetLastWin32Error()
        {
            return m_LastWin32Error;
        }
    }
}
