namespace VolumeInfo.IO.Storage
{
    /// <summary>
    /// The SCSI Device Type.
    /// </summary>
    public enum ScsiDeviceType
    {
        /// <summary>
        /// Unknown SCSI Device Type.
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// A Direct Access Device, like a disk drive.
        /// </summary>
        DirectAccessDevice = 0,

        /// <summary>
        /// A Sequential Access Device, like a tape drive.
        /// </summary>
        SequentialAccessDevice = 1,

        /// <summary>
        /// A printer.
        /// </summary>
        PrinterDevice = 2,

        /// <summary>
        /// A Processor.
        /// </summary>
        ProcessorDevice = 3,

        /// <summary>
        /// The write once device.
        /// </summary>
        WriteOnceDevice = 4,

        /// <summary>
        /// A CD-ROM device.
        /// </summary>
        CdRomDevice = 5,

        /// <summary>
        /// An Optical Scanner.
        /// </summary>
        ScannerDevice = 6,

        /// <summary>
        /// An Optical Memory device, such as a magneto-optical drive.
        /// </summary>
        OpticalMemoryDevice = 7,

        /// <summary>
        /// The medium changer device.
        /// </summary>
        MediumChangerDevice = 8,

        /// <summary>
        /// The communications device.
        /// </summary>
        CommunicationsDevice = 9,

        /// <summary>
        /// An ASC IT8 (Graphic arts pre-press devices) device.
        /// </summary>
        AscIt8_A = 10,

        /// <summary>
        /// An ASC IT8 (Graphic arts pre-press devices) device.
        /// </summary>
        AscIt8_B = 11,

        /// <summary>
        /// A storage array controller device.
        /// </summary>
        StorageArrayControllerDevice = 12,

        /// <summary>
        /// An enclosure service device.
        /// </summary>
        EnclosureServiceDevice = 13,

        /// <summary>
        /// A simplified direct access device.
        /// </summary>
        SimplifiedDirectAccessDevice = 14,

        /// <summary>
        /// An optical card reader writer device.
        /// </summary>
        OpticalCardReaderWriterDevice = 15,

        /// <summary>
        /// A bridging expander.
        /// </summary>
        BridgingExpander = 16,

        /// <summary>
        /// An object based storage device.
        /// </summary>
        ObjectBasedStorageDevice = 17,

        /// <summary>
        /// An automation drive interface.
        /// </summary>
        AutomationDriveInterface = 18,

        /// <summary>
        /// A security manager device.
        /// </summary>
        SecurityManagerDevice = 19,

        /// <summary>
        /// A host managed zoned block device.
        /// </summary>
        HostManagedZonedBlockDevice = 20,

        /// <summary>
        /// A reduced media device.
        /// </summary>
        ReducedMediaDevice = 21,

        /// <summary>
        /// A well known logical unit device.
        /// </summary>
        WellKnownLogicalUnit = 30,

        /// <summary>
        /// An unknown, or no device.
        /// </summary>
        NoDevice = 31
    }

    /// <summary>
    /// Extensions for the <see cref="ScsiDeviceType"/>.
    /// </summary>
    public static class ScsiDeviceTypeExt
    {
        /// <summary>
        /// Converts the <see cref="ScsiDeviceType"/> enumeration to a more meaningful (printable) short description.
        /// </summary>
        /// <param name="deviceType">The SCSI device type.</param>
        /// <returns>A short description of the device</returns>
        public static string ToDescription(this ScsiDeviceType deviceType)
        {
            switch (deviceType) {
            case ScsiDeviceType.Unknown: return "Unknown";
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
            case ScsiDeviceType.SimplifiedDirectAccessDevice: return "Simplified Direct Access Device";
            case ScsiDeviceType.OpticalCardReaderWriterDevice: return "Optical Card Reader/Writer Device";
            case ScsiDeviceType.BridgingExpander: return "Bridging Expander";
            case ScsiDeviceType.ObjectBasedStorageDevice: return "Object-base Storage Device";
            case ScsiDeviceType.AutomationDriveInterface: return "Automation/Drive Device";
            case ScsiDeviceType.SecurityManagerDevice: return "Security Manager Device";
            case ScsiDeviceType.HostManagedZonedBlockDevice: return "Host Managed Zone Block Device";
            case ScsiDeviceType.ReducedMediaDevice: return "Reduced Multimedia Commands Device";
            case ScsiDeviceType.WellKnownLogicalUnit: return "Well known logical unit";
            case ScsiDeviceType.NoDevice: return "No/Unknown Device";
            default: return string.Format("Reserved Device: {0}", deviceType);
            }
        }
    }
}
