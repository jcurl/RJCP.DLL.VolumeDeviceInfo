namespace RJCP.LogApi
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Xml;
    using IO.Storage;
    using IO.Storage.Win32;

    internal class LogDeviceData
    {
        private const string RootNode = "VolumeInfoTest";
        private const string PathNode = "Path";
        private const string PathAttr = "path";

        public static void Capture(string devicePath)
        {
            ThrowHelper.ThrowIfNull(devicePath);

            LogDeviceData data = new();
            data.QueryDeviceParameters(devicePath);
            data.Save();
        }

        private readonly XmlDocument m_Document;
        private readonly string m_FileName;

        private LogDeviceData()
        {
            m_FileName = string.Format("VolumeInfoTest.{0}.xml", Environment.MachineName);
            m_Document = InitializeXmlDocument(m_FileName);
        }

        private static XmlDocument InitializeXmlDocument(string fileName)
        {
            if (File.Exists(fileName)) return LoadXmlDocument(fileName);
            return NewXmlDocument();
        }

        private static XmlDocument LoadXmlDocument(string fileName)
        {
            XmlDocument xmlDoc = new() {
                XmlResolver = null
            };
            xmlDoc.Load(fileName);

            if (xmlDoc.SelectSingleNode($"/{RootNode}") is null) {
                return NewXmlDocument();
            }
            return xmlDoc;
        }

        private static XmlDocument NewXmlDocument()
        {
            XmlDocument xmlDoc = new() {
                XmlResolver = null
            };

            XmlDeclaration xmlDec = xmlDoc.CreateXmlDeclaration("1.0", System.Text.Encoding.UTF8.BodyName, string.Empty);
            xmlDoc.AppendChild(xmlDec);

            XmlElement xmlRoot = xmlDoc.CreateElement($"{RootNode}");
            xmlDoc.AppendChild(xmlRoot);
            return xmlDoc;
        }

        private void Save()
        {
            m_Document.Save(m_FileName);
        }

        private void QueryDeviceParameters(string devicePath)
        {
            XmlElement rootNode = (XmlElement)m_Document.SelectSingleNode($"/{RootNode}");
            XmlElement pathNode = CreateDevicePath(devicePath);

            IOSVolumeDeviceInfo vinfo = new OSVolumeDeviceInfo();
            QueryApi(pathNode, "FileAttributes", vinfo, () => {
                return ((int)vinfo.GetFileAttributes(devicePath)).ToString(CultureInfo.InvariantCulture);
            });
            QueryApi(pathNode, "VolumePathName", vinfo, () => { return vinfo.GetVolumePathName(devicePath); });
            QueryApi(pathNode, "QueryDosDevice", vinfo, () => { return vinfo.QueryDosDevice(devicePath); });
            QueryApi(pathNode, "VolumeNameForVolumeMountPoint", vinfo, () => { return vinfo.GetVolumeNameForVolumeMountPoint(devicePath); });
            QueryVolumeInfo(pathNode, vinfo, devicePath);
            QueryFreeSpace(pathNode, vinfo, devicePath);
            QueryApi(pathNode, "DriveType", vinfo, () => { return vinfo.GetDriveType(devicePath); });

            using (SafeHandle hDevice = QueryApi(pathNode, "CreateFileFromDevice", vinfo, () => { return vinfo.CreateFileFromDevice(devicePath); })) {
                if (hDevice is not null && !hDevice.IsInvalid) {
                    QueryApi(pathNode, "DiskUpdateProperties", vinfo, true, () => { return vinfo.RefreshVolume(hDevice); });
                    QueryStorageProperty(pathNode, vinfo, hDevice);
                    QueryDeviceNumber(pathNode, vinfo, hDevice);
                    QueryDeviceNumberEx(pathNode, vinfo, hDevice);
                    QueryDiskGeometry(pathNode, vinfo, hDevice);
                    QueryDiskAlignment(pathNode, vinfo, hDevice);
                    QueryPartition(pathNode, vinfo, hDevice);
                    QueryDiskExtents(pathNode, vinfo, hDevice);
                    QueryApi(pathNode, "MediaPresent", vinfo, () => { return vinfo.GetMediaPresent(hDevice); });
                    QueryApi(pathNode, "DiskReadOnly", vinfo, () => { return vinfo.IsReadOnly(hDevice); });
                    QueryApi(pathNode, "SeekPenalty", vinfo, () => { return vinfo.IncursSeekPenalty(hDevice); });
                }
            }
            rootNode.AppendChild(pathNode);
        }

        private void QueryStorageProperty(XmlElement pathNode, IOSVolumeDeviceInfo vinfo, SafeHandle hDevice)
        {
            VolumeDeviceQuery query = QueryApi(pathNode, "StorageDeviceProperty", vinfo, () => {
                return vinfo.GetStorageDeviceProperty(hDevice);
            }, out XmlElement storageNode);

            if (query is null) return;
            WriteApiResult(storageNode, "VendorId", query.VendorId);
            WriteApiResult(storageNode, "DeviceSerialNumber", query.DeviceSerialNumber);
            WriteApiResult(storageNode, "ProductId", query.ProductId);
            WriteApiResult(storageNode, "ProductRevision", query.ProductRevision);
            WriteApiResult(storageNode, "RemovableMedia", query.RemovableMedia);
            WriteApiResult(storageNode, "CommandQueueing", query.CommandQueueing);
            WriteApiResult(storageNode, "ScsiDeviceType", (int)query.ScsiDeviceType);
            WriteApiResult(storageNode, "ScsiModifier", query.ScsiDeviceModifier);
            WriteApiResult(storageNode, "BusType", (int)query.BusType);
        }

        private void QueryDeviceNumber(XmlElement pathNode, IOSVolumeDeviceInfo vinfo, SafeHandle hDevice)
        {
            StorageDeviceNumber device = QueryApi(pathNode, "StorageDeviceNumber", vinfo, () => {
                return vinfo.GetDeviceNumber(hDevice);
            }, out XmlElement deviceNode);

            if (device is null) return;
            WriteApiResult(deviceNode, "DeviceType", (int)device.DeviceType);
            WriteApiResult(deviceNode, "DeviceNumber", device.DeviceNumber);
            WriteApiResult(deviceNode, "DevicePartition", device.PartitionNumber);
        }

        private void QueryDeviceNumberEx(XmlElement pathNode, IOSVolumeDeviceInfo vinfo, SafeHandle hDevice)
        {
            StorageDeviceNumber device = QueryApi(pathNode, "StorageDeviceNumberEx", vinfo, () => {
                return vinfo.GetDeviceNumberEx(hDevice);
            }, out XmlElement deviceNode);

            if (device is null) return;
            WriteApiResult(deviceNode, "DeviceType", (int)device.DeviceType);
            WriteApiResult(deviceNode, "DeviceGuidFlags", (int)device.DeviceGuidFlags);
            WriteApiResult(deviceNode, "DeviceGuid", device.DeviceGuid.ToString());
            WriteApiResult(deviceNode, "DeviceNumber", device.DeviceNumber);
            WriteApiResult(deviceNode, "DevicePartition", device.PartitionNumber);
        }

        private void QueryDiskGeometry(XmlElement pathNode, IOSVolumeDeviceInfo vinfo, SafeHandle hDevice)
        {
            DiskGeometry diskGeo = QueryApi(pathNode, "DiskGeometry", vinfo, () => {
                return vinfo.GetDiskGeometry(hDevice);
            }, out XmlElement geoNode);

            if (diskGeo is null) return;
            WriteApiResult(geoNode, "MediaType", (int)diskGeo.MediaType);
            WriteApiResult(geoNode, "Cylinders", diskGeo.Cylinders);
            WriteApiResult(geoNode, "TracksPerCylinder", diskGeo.TracksPerCylinder);
            WriteApiResult(geoNode, "SectorsPerTrack", diskGeo.SectorsPerTrack);
            WriteApiResult(geoNode, "BytesPerSector", diskGeo.BytesPerSector);
        }

        private void QueryDiskAlignment(XmlElement pathNode, IOSVolumeDeviceInfo vinfo, SafeHandle hDevice)
        {
            StorageAccessAlignment diskAlignment = QueryApi(pathNode, "StorageAlignment", vinfo, () => {
                return vinfo.GetAlignment(hDevice);
            }, out XmlElement alignNode);

            if (diskAlignment is null) return;
            WriteApiResult(alignNode, "BytesPerCacheLine", diskAlignment.BytesPerCacheLine);
            WriteApiResult(alignNode, "BytesOffsetForCacheAlignment", diskAlignment.BytesOffsetForCacheAlignment);
            WriteApiResult(alignNode, "BytesPerLogicalSector", diskAlignment.BytesPerLogicalSector);
            WriteApiResult(alignNode, "BytesPerPhysicalSector", diskAlignment.BytesPerPhysicalSector);
            WriteApiResult(alignNode, "BytesOffsetForSectorAlignment", diskAlignment.BytesOffsetForSectorAlignment);
        }

        private void QueryPartition(XmlElement pathNode, IOSVolumeDeviceInfo vinfo, SafeHandle hDevice)
        {
            PartitionInformation partInfo = QueryApi(pathNode, "PartitionInformation", vinfo, () => {
                return vinfo.GetPartitionInfo(hDevice);
            }, out XmlElement partNode);

            if (partInfo is null) return;
            WriteApiResult(partNode, "Style", (int)partInfo.Style);
            WriteApiResult(partNode, "Number", partInfo.Number);
            WriteApiResult(partNode, "Offset", partInfo.Offset);
            WriteApiResult(partNode, "Length", partInfo.Length);

            switch (partInfo.Style) {
            case PartitionStyle.MasterBootRecord:
                MbrPartition mbrInfo = (MbrPartition)partInfo;
                WriteApiResult(partNode, "MbrType", mbrInfo.Type);
                WriteApiResult(partNode, "MbrBootable", mbrInfo.Bootable);
                WriteApiResult(partNode, "MbrOffset", mbrInfo.HiddenSectors);
                break;
            case PartitionStyle.GuidPartitionTable:
                GptPartition gptInfo = (GptPartition)partInfo;
                WriteApiResult(partNode, "GptAttributes", (long)gptInfo.Attributes);
                WriteApiResult(partNode, "GptId", gptInfo.Id.ToString());
                WriteApiResult(partNode, "GptType", gptInfo.Type.ToString());
                WriteApiResult(partNode, "GptName", gptInfo.Name);
                break;
            }
        }

        private void QueryDiskExtents(XmlElement pathNode, IOSVolumeDeviceInfo vinfo, SafeHandle hDevice)
        {
            DiskExtent[] extents = QueryApi(pathNode, "DiskExtents", vinfo, () => {
                return vinfo.GetDiskExtents(hDevice);
            }, out XmlElement extNode);

            if (extents is null) return;

            foreach (DiskExtent extent in extents) {
                XmlElement entry = WriteApiResult(extNode, "DiskExtent", string.Empty);
                WriteApiResult(entry, "Device", extent.Device);
                WriteApiResult(entry, "Offset", extent.StartingOffset);
                WriteApiResult(entry, "Length", extent.ExtentLength);
            }
        }

        private void QueryVolumeInfo(XmlElement pathNode, IOSVolumeDeviceInfo vinfo, string pathName)
        {
            VolumeInfo info = QueryApi(pathNode, "VolumeInformation", vinfo, () => {
                return vinfo.GetVolumeInformation(pathName);
            }, out XmlElement volumeInfoNode);

            if (info is null) return;
            WriteApiResult(volumeInfoNode, "Label", info.VolumeLabel);
            WriteApiResult(volumeInfoNode, "SerialNumber", info.VolumeSerial);
            WriteApiResult(volumeInfoNode, "FileSystem", info.FileSystem);
            WriteApiResult(volumeInfoNode, "Flags", (int)info.Flags);
        }

        private void QueryFreeSpace(XmlElement pathNode, IOSVolumeDeviceInfo vinfo, string pathName)
        {
            DiskFreeSpace space = QueryApi(pathNode, "DiskFreeSpace", vinfo, () => {
                return vinfo.GetDiskFreeSpace(pathName);
            }, out XmlElement spaceInfo);

            if (space is null) return;
            WriteApiResult(spaceInfo, "SectorsPerCluster", space.SectorsPerCluster);
            WriteApiResult(spaceInfo, "BytesPerSector", space.BytesPerSector);
            WriteApiResult(spaceInfo, "TotalBytes", space.TotalBytes);
            WriteApiResult(spaceInfo, "TotalBytesFree", space.TotalBytesFree);
            WriteApiResult(spaceInfo, "UserBytesFree", space.UserBytesFree);
        }

        private XmlElement CreateDevicePath(string devicePath)
        {
            string pathElementAttrQuery = $"/{RootNode}/{PathNode}[@{PathAttr}='{devicePath}']";
            XmlNodeList pathNodes = m_Document.SelectNodes(pathElementAttrQuery);
            if (pathNodes is not null) {
                foreach (XmlNode node in pathNodes) {
                    node.ParentNode.RemoveChild(node);
                }
            }

            XmlElement pathNode = m_Document.CreateElement($"{PathNode}");
            XmlAttribute pathAttr = m_Document.CreateAttribute($"{PathAttr}");
            pathAttr.Value = devicePath;
            pathNode.Attributes.Append(pathAttr);
            return pathNode;
        }

        private T QueryApi<T>(XmlElement parent, string elementName, IOSVolumeDeviceInfo vinfo, Func<T> method)
        {
            return QueryApi(parent, elementName, vinfo, method, out _);
        }

        private T QueryApi<T>(XmlElement parent, string elementName, IOSVolumeDeviceInfo vinfo, Func<T> method, out XmlElement node)
        {
            return QueryApi(parent, elementName, vinfo, false, method, out node);
        }

        private T QueryApi<T>(XmlElement parent, string elementName, IOSVolumeDeviceInfo vinfo, bool errorIfFalse, Func<T> method)
        {
            return QueryApi(parent, elementName, vinfo, errorIfFalse, method, out _);
        }

        private T QueryApi<T>(XmlElement parent, string elementName, IOSVolumeDeviceInfo vinfo, bool errorIfFalse, Func<T> method, out XmlElement node)
        {
            T result;
            try {
                result = method();
            } catch {
                node = WriteApiResult(parent, elementName, null, vinfo.GetLastWin32Error(), true);
                return default;
            }

            if (result is string sResult) {
                node = WriteApiResult(parent, elementName, sResult);
            } else if (result is long lResult) {
                node = WriteApiResult(parent, elementName, lResult);
            } else if (result is int iResult) {
                node = WriteApiResult(parent, elementName, iResult);
            } else if (result is bool bResult) {
                if (!bResult && errorIfFalse) {
                    node = WriteApiResult(parent, elementName, bResult, vinfo.GetLastWin32Error());
                } else {
                    node = WriteApiResult(parent, elementName, bResult);
                }
            } else if (result is BoolUnknown buResult) {
                if (buResult == BoolUnknown.Unknown) {
                    node = WriteApiResult(parent, elementName, buResult.ToString(), vinfo.GetLastWin32Error(), false);
                } else {
                    node = WriteApiResult(parent, elementName, buResult.ToString());
                }
            } else if (result is object) {
                // The result is an object and is not null, so the call was successful, we just can't serialize it, as
                // it's complex.
                node = WriteApiResult(parent, elementName, string.Empty);
            } else {
                // The result was null.
                node = WriteApiResult(parent, elementName, null, vinfo.GetLastWin32Error(), false);
            }
            return result;
        }

        private XmlElement WriteApiResult(XmlElement parent, string elementName, string result)
        {
            return WriteApiResult(parent, elementName, result, 0, false);
        }

        private XmlElement WriteApiResult(XmlElement parent, string elementName, bool result)
        {
            return WriteApiResult(parent, elementName, result.ToString(CultureInfo.InvariantCulture), 0, false);
        }

        private XmlElement WriteApiResult(XmlElement parent, string elementName, bool result, int errorCode)
        {
            return WriteApiResult(parent, elementName, result.ToString(CultureInfo.InvariantCulture), errorCode, false);
        }

        private XmlElement WriteApiResult(XmlElement parent, string elementName, int result)
        {
            return WriteApiResult(parent, elementName, result.ToString(CultureInfo.InvariantCulture), 0, false);
        }

        private XmlElement WriteApiResult(XmlElement parent, string elementName, long result)
        {
            return WriteApiResult(parent, elementName, result.ToString(CultureInfo.InvariantCulture), 0, false);
        }

        private XmlElement WriteApiResult(XmlElement parent, string elementName, string result, int errorCode, bool throws)
        {
            XmlElement node = m_Document.CreateElement(elementName);

            if (result is not null) {
                XmlAttribute attr = m_Document.CreateAttribute("result");
                attr.Value = result;
                node.Attributes.Append(attr);

            }

            if (errorCode != 0) {
                XmlAttribute errorAttr = m_Document.CreateAttribute("error");
                errorAttr.Value = errorCode.ToString(CultureInfo.InvariantCulture);
                node.Attributes.Append(errorAttr);
            }

            if (throws) {
                XmlAttribute throwsAttr = m_Document.CreateAttribute("throws");
                throwsAttr.Value = throws.ToString();
                node.Attributes.Append(throwsAttr);
            }

            if (parent is not null) parent.AppendChild(node);
            return node;
        }
    }
}
