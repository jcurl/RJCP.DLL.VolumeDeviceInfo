namespace VolumeInfo.IO.Storage.Win32
{
    internal class VolumeDeviceQuery
    {
        public string VendorId { get; set; } = string.Empty;

        public string DeviceSerialNumber { get; set; } = string.Empty;

        public string ProductId { get; set; } = string.Empty;

        public string ProductRevision { get; set; } = string.Empty;

        public bool RemovableMedia { get; set; }

        public bool CommandQueueing { get; set; }

        public ScsiDeviceType ScsiDeviceType { get; set; } = ScsiDeviceType.Unknown;

        public int ScsiDeviceModifier { get; set; }

        public BusType BusType { get; set; } = BusType.Unknown;
    }
}
