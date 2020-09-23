namespace VolumeInfo.IO.Storage
{
    using System;

    [Flags]
    public enum DeviceGuidFlags
    {
        None = 0,
        RandomDeviceGuidReasonConflict = 1,
        RandomDeviceGuidReasonNoHwId = 2,
        Page83DeviceGuid = 4
    }
}
