namespace VolumeTest.Native
{
    public enum ScsiDeviceType
    {
        DirectAccessDevice = 0,
        SequentialAccessDevice = 1,
        PrinterDevice = 2,
        ProcessorDevice = 3,
        WriteOnceDevice = 4,
        CdRomDevice = 5,
        ScannerDevice = 6,
        OpticalMemoryDevice = 7,
        MediumChangerDevice = 8,
        CommunicationsDevice = 9,
        AscIt8_A = 10,
        AscIt8_B = 11,
        StorageArrayControllerDevice = 12,
        EnclosureServiceDevice = 13,
        SimplifedDirectAccessDevice = 14,
        OpticalCardReaderWriterDevice = 15,
        BridgingExpander = 16,
        ObjectBasedStorageDevice = 17,
        AutomationDriveInterface = 18,
        SecurityManagerDevice = 19,
        HostManagedZonedBlockDevice = 20,
        ReducedMediaDevice = 21,
        WellKnownLogicalUnit = 30,
        UnknownDevice = 31
    }

    public static class ScsiDeviceTypeExt
    {
        public static string ToDescription(this ScsiDeviceType deviceType)
        {
            switch (deviceType) {
            case ScsiDeviceType.DirectAccessDevice: return "Direct Access Device";
            case ScsiDeviceType.SequentialAccessDevice: return "Sequential Access Device";
            case ScsiDeviceType.PrinterDevice: return "Printer Device";
            case ScsiDeviceType.ProcessorDevice: return "Processor Device";
            case ScsiDeviceType.WriteOnceDevice: return "Write Once Device";
            case ScsiDeviceType.CdRomDevice: return "CD-ROM Device";
            case ScsiDeviceType.ScannerDevice: return "Scanner Device";
            case ScsiDeviceType.OpticalMemoryDevice: return "Optical Memory Device";
            case ScsiDeviceType.MediumChangerDevice: return "Medium Changer Device";
            case ScsiDeviceType.CommunicationsDevice: return "Communications Device";
            case ScsiDeviceType.AscIt8_A: return "ASC IT8 Device (0x0A)";
            case ScsiDeviceType.AscIt8_B: return "ASC IT8 Device (0x0B)";
            case ScsiDeviceType.StorageArrayControllerDevice: return "Storage Array Controller Device";
            case ScsiDeviceType.EnclosureServiceDevice: return "Enclosure Services Device";
            case ScsiDeviceType.SimplifedDirectAccessDevice: return "Simplified Direct Access Device";
            case ScsiDeviceType.OpticalCardReaderWriterDevice: return "Optical Card Reader/Writer Device";
            case ScsiDeviceType.BridgingExpander: return "Bridging Expander";
            case ScsiDeviceType.ObjectBasedStorageDevice: return "Object-base Storage Device";
            case ScsiDeviceType.AutomationDriveInterface: return "Automation/Drive Device";
            case ScsiDeviceType.SecurityManagerDevice: return "Security Manager Device";
            case ScsiDeviceType.HostManagedZonedBlockDevice: return "Host Managed Zone Block Device";
            case ScsiDeviceType.ReducedMediaDevice: return "Reduced Multimedia Commands Device";
            case ScsiDeviceType.WellKnownLogicalUnit: return "Well known logical unit";
            case ScsiDeviceType.UnknownDevice: return "Unknown Device";
            default: return string.Format("Reserved Device: {0}", deviceType);
            }
        }
    }
}
