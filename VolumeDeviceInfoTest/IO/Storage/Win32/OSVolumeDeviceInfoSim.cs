namespace RJCP.IO.Storage.Win32
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Xml;
    using static Native.Win32.WinIoCtl;

    internal class OSVolumeDeviceInfoSim : IOSVolumeDeviceInfo
    {
        private const string RootNode = "VolumeInfoTest";
        private const string PathNode = "Path";
        private const string PathAttr = "path";

        private class ResultOrError<T>
        {
            public ResultOrError(T result) { Result = result; }

            public ResultOrError(T result, int errorCode)
            {
                ErrorCode = errorCode;
                Result = result;
            }

            public ResultOrError(int errorCode, bool throws)
            {
                ErrorCode = errorCode;
                Throws = throws;
            }

            public T Result { get; private set; }

            public int ErrorCode { get; private set; }

            public bool Throws { get; private set; }
        }

        public OSVolumeDeviceInfoSim(string fileName)
        {
            XmlDocument xmlDoc = new XmlDocument {
                XmlResolver = null
            };
            xmlDoc.Load(fileName);

            m_LogicalDrives = LoadGetLogicalDrives((XmlElement)xmlDoc.SelectSingleNode($"/{RootNode}/GetLogicalDrives"));
            XmlNodeList paths = xmlDoc.SelectNodes($"/{RootNode}/{PathNode}");
            foreach (XmlElement path in paths) {
                ParsePath(path);
            }
        }

        private static int LoadGetLogicalDrives(XmlElement pathNode)
        {
            if (pathNode == null) return 0;
            string nodeResult = pathNode.Attributes["result"]?.Value;
            return int.Parse(nodeResult, CultureInfo.InvariantCulture);
        }

        private void ParsePath(XmlElement pathNode)
        {
            string path = pathNode.GetAttribute(PathAttr);
            AddItem(m_FileAttributes, path, pathNode["FileAttributes"]);
            AddItem(m_VolumePathName, path, pathNode["VolumePathName"]);
            AddItem(m_QueryDosDevice, path, pathNode["QueryDosDevice"]);
            AddItem(m_VolumeMountPoint, path, pathNode["VolumeNameForVolumeMountPoint"]);
            AddItem(m_CreateFileFromDevice, path, pathNode["CreateFileFromDevice"]);
            AddItem(m_DiskUpdateProperties, path, pathNode["DiskUpdateProperties"]);
            AddItem(m_MediaPresent, path, pathNode["MediaPresent"]);
            AddItem(m_ReadOnly, path, pathNode["DiskReadOnly"]);
            AddItem(m_DriveType, path, pathNode["DriveType"]);
            AddStorageDevice(m_StorageProperties, path, pathNode["StorageDeviceProperty"]);
            AddVolumeInfo(m_VolumeInfo, path, pathNode["VolumeInformation"]);
            AddDiskFreeSpace(m_DiskFree, path, pathNode["DiskFreeSpace"]);
            AddDeviceNumber(m_DeviceNumber, path, pathNode["StorageDeviceNumber"]);
            AddDeviceNumberEx(m_DeviceNumberEx, path, pathNode["StorageDeviceNumberEx"]);
            AddGeometry(m_Geometry, path, pathNode["DiskGeometry"]);
            AddAlignment(m_Alignment, path, pathNode["StorageAlignment"]);
            AddItem(m_SeekPenalty, path, pathNode["SeekPenalty"]);
            AddPartitionInfo(m_PartitionInfo, path, pathNode["PartitionInformation"]);
            AddDiskExtents(m_Extents, path, pathNode["DiskExtents"]);
        }

        private static void AddStorageDevice(IDictionary<string, ResultOrError<VolumeDeviceQuery>> dictionary, string path, XmlElement node)
        {
            if (node == null) return;
            if (dictionary.ContainsKey(path)) return;

            // Because this is a complex type, The XML will always return the default value.
            ResultOrError<VolumeDeviceQuery> result = GetResultOrError<VolumeDeviceQuery>(node);
            if (result != null) {
                dictionary.Add(path, result);
                return;
            }

            unchecked {
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
        }

        private static void AddVolumeInfo(IDictionary<string, ResultOrError<VolumeInfo>> dictionary, string path, XmlElement node)
        {
            if (node == null) return;
            if (dictionary.ContainsKey(path)) return;

            // Because this is a complex type, The XML will always return the default value.
            ResultOrError<VolumeInfo> result = GetResultOrError<VolumeInfo>(node);
            if (result != null) {
                dictionary.Add(path, result);
                return;
            }

            unchecked {
                string flags = node["Flags"].Attributes["result"].Value;
                VolumeInfo volInfo = new VolumeInfo() {
                    VolumeLabel = node["Label"].Attributes["result"].Value,
                    VolumeSerial = node["SerialNumber"].Attributes["result"].Value,
                    FileSystem = node["FileSystem"].Attributes["result"].Value,
                    Flags = (FileSystemFlags)int.Parse(flags, CultureInfo.InvariantCulture)
                };
                dictionary.Add(path, new ResultOrError<VolumeInfo>(volInfo));
            }
        }

        private static void AddDiskFreeSpace(IDictionary<string, ResultOrError<DiskFreeSpace>> dictionary, string path, XmlElement node)
        {
            if (node == null) return;
            if (dictionary.ContainsKey(path)) return;

            // Because this is a complex type, The XML will always return the default value.
            ResultOrError<DiskFreeSpace> result = GetResultOrError<DiskFreeSpace>(node);
            if (result != null) {
                dictionary.Add(path, result);
                return;
            }

            unchecked {
                string sectorsPerCluster = node["SectorsPerCluster"].Attributes["result"].Value;
                string bytesPerSector = node["BytesPerSector"].Attributes["result"].Value;
                string totalBytes = node["TotalBytes"].Attributes["result"].Value;
                string totalBytesFree = node["TotalBytesFree"].Attributes["result"].Value;
                string userBytesFree = node["UserBytesFree"].Attributes["result"].Value;
                DiskFreeSpace freeInfo = new DiskFreeSpace() {
                    SectorsPerCluster = int.Parse(sectorsPerCluster, CultureInfo.InvariantCulture),
                    BytesPerSector = int.Parse(bytesPerSector, CultureInfo.InvariantCulture),
                    TotalBytes = long.Parse(totalBytes, CultureInfo.InvariantCulture),
                    TotalBytesFree = long.Parse(totalBytesFree, CultureInfo.InvariantCulture),
                    UserBytesFree = long.Parse(userBytesFree, CultureInfo.InvariantCulture),
                };
                dictionary.Add(path, new ResultOrError<DiskFreeSpace>(freeInfo));
            }
        }

        private static void AddDeviceNumber(IDictionary<string, ResultOrError<StorageDeviceNumber>> dictionary, string path, XmlElement node)
        {
            if (node == null) return;
            if (dictionary.ContainsKey(path)) return;

            // Because this is a complex type, The XML will always return the default value.
            ResultOrError<StorageDeviceNumber> result = GetResultOrError<StorageDeviceNumber>(node);
            if (result != null) {
                dictionary.Add(path, result);
                return;
            }

            unchecked {
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
        }

        private static void AddDeviceNumberEx(IDictionary<string, ResultOrError<StorageDeviceNumber>> dictionary, string path, XmlElement node)
        {
            if (node == null) return;
            if (dictionary.ContainsKey(path)) return;

            // Because this is a complex type, The XML will always return the default value.
            ResultOrError<StorageDeviceNumber> result = GetResultOrError<StorageDeviceNumber>(node);
            if (result != null) {
                dictionary.Add(path, result);
                return;
            }

            unchecked {
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
        }

        private static void AddGeometry(IDictionary<string, ResultOrError<DiskGeometry>> dictionary, string path, XmlElement node)
        {
            if (node == null) return;
            if (dictionary.ContainsKey(path)) return;

            // Because this is a complex type, The XML will always return the default value.
            ResultOrError<DiskGeometry> result = GetResultOrError<DiskGeometry>(node);
            if (result != null) {
                dictionary.Add(path, result);
                return;
            }

            unchecked {
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
        }

        private static void AddAlignment(IDictionary<string, ResultOrError<StorageAccessAlignment>> dictionary, string path, XmlElement node)
        {
            if (node == null) return;
            if (dictionary.ContainsKey(path)) return;

            // Because this is a complex type, The XML will always return the default value.
            ResultOrError<StorageAccessAlignment> result = GetResultOrError<StorageAccessAlignment>(node);
            if (result != null) {
                dictionary.Add(path, result);
                return;
            }

            unchecked {
                string bytesPerCacheLine = node["BytesPerCacheLine"].Attributes["result"].Value;
                string bytesOffsetForCacheAlignment = node["BytesOffsetForCacheAlignment"].Attributes["result"].Value;
                string bytesPerLogicalSector = node["BytesPerLogicalSector"].Attributes["result"].Value;
                string bytesPerPhysicalSector = node["BytesPerPhysicalSector"].Attributes["result"].Value;
                string bytesOffsetForSectorAlignment = node["BytesOffsetForSectorAlignment"].Attributes["result"].Value;
                StorageAccessAlignment diskAlignment = new StorageAccessAlignment() {
                    BytesPerCacheLine = int.Parse(bytesPerCacheLine, CultureInfo.InvariantCulture),
                    BytesOffsetForCacheAlignment = int.Parse(bytesOffsetForCacheAlignment, CultureInfo.InvariantCulture),
                    BytesPerLogicalSector = int.Parse(bytesPerLogicalSector, CultureInfo.InvariantCulture),
                    BytesPerPhysicalSector = int.Parse(bytesPerPhysicalSector, CultureInfo.InvariantCulture),
                    BytesOffsetForSectorAlignment = int.Parse(bytesOffsetForSectorAlignment, CultureInfo.InvariantCulture),
                };
                dictionary.Add(path, new ResultOrError<StorageAccessAlignment>(diskAlignment));
            }
        }

        private static void AddPartitionInfo(IDictionary<string, ResultOrError<PartitionInformation>> dictionary, string path, XmlElement node)
        {
            if (node == null) return;
            if (dictionary.ContainsKey(path)) return;

            // Because this is a complex type, The XML will always return the default value.
            ResultOrError<PartitionInformation> result = GetResultOrError<PartitionInformation>(node);
            if (result != null) {
                dictionary.Add(path, result);
                return;
            }

            unchecked {
                string style = node["Style"].Attributes["result"].Value;
                string number = node["Number"].Attributes["result"].Value;
                string offset = node["Offset"].Attributes["result"].Value;
                string length = node["Length"].Attributes["result"].Value;
                PartitionStyle partStyle = (PartitionStyle)int.Parse(style, CultureInfo.InvariantCulture);
                int partNumber = int.Parse(number, CultureInfo.InvariantCulture);
                long partOffset = long.Parse(offset, CultureInfo.InvariantCulture);
                long partLength = long.Parse(length, CultureInfo.InvariantCulture);
                switch (partStyle) {
                case PartitionStyle.MasterBootRecord:
                    dictionary.Add(path, new ResultOrError<PartitionInformation>(
                        new MbrPartition() {
                            Number = partNumber,
                            Offset = partOffset,
                            Length = partLength,
                            Bootable = bool.Parse(node["MbrBootable"].Attributes["result"].Value),
                            HiddenSectors = int.Parse(node["MbrOffset"].Attributes["result"].Value),
                            Type = int.Parse(node["MbrType"].Attributes["result"].Value)
                        }));
                    break;
                case PartitionStyle.GuidPartitionTable:
                    dictionary.Add(path, new ResultOrError<PartitionInformation>(
                        new GptPartition() {
                            Number = partNumber,
                            Offset = partOffset,
                            Length = partLength,
                            Type = new Guid(node["GptType"].Attributes["result"].Value),
                            Id = new Guid(node["GptId"].Attributes["result"].Value),
                            Name = node["GptName"].Attributes["result"].Value,
                            Attributes = (EFIPartitionAttributes)long.Parse(node["GptAttributes"].Attributes["result"].Value, CultureInfo.InvariantCulture)
                        }));
                    break;
                default:
                    dictionary.Add(path, new ResultOrError<PartitionInformation>(
                        new PartitionInformation(partStyle) {
                            Number = partNumber,
                            Offset = partOffset,
                            Length = partLength
                        }));
                    break;
                }
            }
        }

        private static void AddDiskExtents(IDictionary<string, ResultOrError<DiskExtent[]>> dictionary, string path, XmlElement node)
        {
            if (node == null) return;
            if (dictionary.ContainsKey(path)) return;

            // Because this is a complex type, The XML will always return the default value.
            ResultOrError<DiskExtent[]> result = GetResultOrError<DiskExtent[]>(node);
            if (result != null) {
                dictionary.Add(path, result);
                return;
            }

            unchecked {
                // There may be zero or more disk extents.
                XmlNodeList extentNodes = node.SelectNodes("DiskExtent");
                List<DiskExtent> extents = new List<DiskExtent>();
                foreach (XmlElement extentNode in extentNodes) {
                    string offset = extentNode["Offset"].Attributes["result"].Value;
                    string length = extentNode["Length"].Attributes["result"].Value;
                    extents.Add(new DiskExtent() {
                        Device = extentNode["Device"].Attributes["result"].Value,
                        StartingOffset = long.Parse(offset, CultureInfo.InvariantCulture),
                        ExtentLength = long.Parse(length, CultureInfo.InvariantCulture),
                    });
                }

                dictionary.Add(path, new ResultOrError<DiskExtent[]>(extents.ToArray()));
            }
        }

        private static void AddItem<T>(IDictionary<string, ResultOrError<T>> dictionary, string path, XmlElement node)
        {
            if (node == null) return;
            if (dictionary.ContainsKey(path)) return;

            ResultOrError<T> result = GetResultOrError<T>(node);
            if (result == null)
                throw new ArgumentException("Complex types not supported", nameof(dictionary));

            dictionary.Add(path, result);
        }

        private static ResultOrError<T> GetResultOrError<T>(XmlElement node)
        {
            string nodeResult = node.Attributes["result"]?.Value;
            string errorCodeStr = node.Attributes["error"]?.Value;
            string throwsStr = node.Attributes["throws"]?.Value;
            int errorCode = 0;
            if (errorCodeStr != null) {
                errorCode = int.Parse(errorCodeStr, CultureInfo.InvariantCulture);
                if (throwsStr != null) {
                    bool throws = bool.Parse(throwsStr);
                    return new ResultOrError<T>(errorCode, throws);
                }
            }

            unchecked {
                if (nodeResult != null) {
                    if (typeof(BoolUnknown).IsAssignableFrom(typeof(T))) {
                        BoolUnknown buResult = (BoolUnknown)Enum.Parse(typeof(BoolUnknown), nodeResult, true);
                        return new ResultOrError<T>((T)Convert.ChangeType(buResult, typeof(T)), errorCode);
                    } else if (typeof(int).IsAssignableFrom(typeof(T))) {
                        int iResult = int.Parse(nodeResult, CultureInfo.InvariantCulture);
                        return new ResultOrError<T>((T)Convert.ChangeType(iResult, typeof(T)), errorCode);
                    } else if (typeof(bool).IsAssignableFrom(typeof(T))) {
                        bool bResult = bool.Parse(nodeResult);
                        return new ResultOrError<T>((T)Convert.ChangeType(bResult, typeof(T)), errorCode);
                    } else if (typeof(string).IsAssignableFrom(typeof(T))) {
                        return new ResultOrError<T>((T)Convert.ChangeType(nodeResult, typeof(T)), errorCode);
                    } else {
                        return null;
                    }
                } else {
                    return new ResultOrError<T>(errorCode, false);
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

        private readonly Dictionary<string, ResultOrError<bool>> m_DiskUpdateProperties = new Dictionary<string, ResultOrError<bool>>();

        public bool RefreshVolume(SafeHandle hDevice)
        {
            SafeTestHandle handle = CheckHandle(hDevice);
            return GetResultOrThrow(m_DiskUpdateProperties, handle.PathName);
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

        private readonly Dictionary<string, ResultOrError<DiskFreeSpace>> m_DiskFree = new Dictionary<string, ResultOrError<DiskFreeSpace>>();

        public DiskFreeSpace GetDiskFreeSpace(string devicePathName)
        {
            return GetResultOrThrow(m_DiskFree, devicePathName);
        }

        private readonly Dictionary<string, ResultOrError<int>> m_DriveType = new Dictionary<string, ResultOrError<int>>();

        public int GetDriveType(string devicePathName)
        {
            return GetResultOrThrow(m_DriveType, devicePathName);
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

        private readonly Dictionary<string, ResultOrError<StorageAccessAlignment>> m_Alignment = new Dictionary<string, ResultOrError<StorageAccessAlignment>>();

        public StorageAccessAlignment GetAlignment(SafeHandle hDevice)
        {
            SafeTestHandle handle = CheckHandle(hDevice);
            return GetResultOrThrow(m_Alignment, handle.PathName);
        }

        private readonly Dictionary<string, ResultOrError<BoolUnknown>> m_SeekPenalty = new Dictionary<string, ResultOrError<BoolUnknown>>();

        public BoolUnknown IncursSeekPenalty(SafeHandle hDevice)
        {
            SafeTestHandle handle = CheckHandle(hDevice);
            return GetResultOrThrow(m_SeekPenalty, handle.PathName);
        }

        private readonly Dictionary<string, ResultOrError<bool>> m_ReadOnly = new Dictionary<string, ResultOrError<bool>>();

        public bool IsReadOnly(SafeHandle hDevice)
        {
            SafeTestHandle handle = CheckHandle(hDevice);
            return GetResultOrThrow(m_ReadOnly, handle.PathName);
        }

        private readonly Dictionary<string, ResultOrError<PartitionInformation>> m_PartitionInfo = new Dictionary<string, ResultOrError<PartitionInformation>>();

        public PartitionInformation GetPartitionInfo(SafeHandle hDevice)
        {
            SafeTestHandle handle = CheckHandle(hDevice);
            return GetResultOrThrow(m_PartitionInfo, handle.PathName);
        }

        private readonly Dictionary<string, ResultOrError<DiskExtent[]>> m_Extents = new Dictionary<string, ResultOrError<DiskExtent[]>>();

        public DiskExtent[] GetDiskExtents(SafeHandle hDevice)
        {
            SafeTestHandle handle = CheckHandle(hDevice);
            return GetResultOrThrow(m_Extents, handle.PathName);
        }

        private readonly int m_LogicalDrives;

        public int GetLogicalDrives()
        {
            return m_LogicalDrives;
        }

        private int m_LastWin32Error;

        public int GetLastWin32Error()
        {
            return m_LastWin32Error;
        }
    }
}
