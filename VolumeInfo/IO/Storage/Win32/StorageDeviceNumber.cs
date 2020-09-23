using System;

namespace VolumeInfo.IO.Storage.Win32
{
    public class StorageDeviceNumber
    {
        public DeviceType DeviceType { get; set; }

        public int DeviceNumber { get; set; }

        public DeviceGuidFlags DeviceGuidFlags { get; set; }

        public Guid DeviceGuid { get; set; }

        public int PartitionNumber { get; set; }
    }
}
