namespace VolumeInfo.Native.Win32
{
    using System;

    internal partial class WinIoCtl
    {
        [Flags]
        public enum StorageDeviceFlags
        {
            RandomDeviceGuidReasonConflict = 1,
            RandomDeviceGuidReasonNoHwId = 2,
            Page83DeviceGuid = 4
        }
    }
}
