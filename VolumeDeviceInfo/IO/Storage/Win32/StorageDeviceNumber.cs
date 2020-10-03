namespace RJCP.IO.Storage.Win32
{
    using System;
    using static Native.Win32.WinIoCtl;

    internal class StorageDeviceNumber
    {
        public DeviceType DeviceType { get; set; }

        public int DeviceNumber { get; set; }

        public DeviceGuidFlags DeviceGuidFlags { get; set; }

        public Guid DeviceGuid { get; set; }

        public int PartitionNumber { get; set; }
    }
}
