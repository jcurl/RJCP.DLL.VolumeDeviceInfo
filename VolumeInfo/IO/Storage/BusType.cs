namespace VolumeInfo.IO.Storage
{
    using System;
    using Native.Win32;

    /// <summary>
    /// The bus type which the physical disk is attached with.
    /// </summary>
    public enum BusType
    {
        /// <summary>
        /// The bus type is not known.
        /// </summary>
        Unknown = 0x00,

        /// <summary>
        /// The device is attached using the SCSI protocol.
        /// </summary>
        Scsi = 0x1,

        /// <summary>
        /// The device is attached using the ATAPI bus.
        /// </summary>
        Atapi = 0x2,

        /// <summary>
        /// The device is attached using the ATA bus.
        /// </summary>
        Ata = 0x3,

        /// <summary>
        /// The device is attached using Firewire (IEEE1394).
        /// </summary>
        Ieee1394 = 0x4,

        /// <summary>
        /// The device is attached using Serial Storage Architecture.
        /// </summary>
        Ssa = 0x5,

        /// <summary>
        /// The device is attached using a Fibre channel.
        /// </summary>
        Fibre = 0x6,

        /// <summary>
        /// The device is attached via USB.
        /// </summary>
        Usb = 0x7,

        /// <summary>
        /// The device is attached via a RAID controller.
        /// </summary>
        RAID = 0x8,

        /// <summary>
        /// The device is attached using ISCSI.
        /// </summary>
        iScsi = 0x9,

        /// <summary>
        /// The device is attached using Serial attached SCSI.
        /// </summary>
        Sas = 0xA,

        /// <summary>
        /// The device is attached using Serial ATA.
        /// </summary>
        Sata = 0xB,

        /// <summary>
        /// The device is a Secure Digital device.
        /// </summary>
        Sd = 0xC,

        /// <summary>
        /// The device is a Multimedia Card.
        /// </summary>
        Mmc = 0xD,

        /// <summary>
        /// This is a Virtual Device (e.g. VHD).
        /// </summary>
        Virtual = 0xE,

        /// <summary>
        /// The file backed virtual device.
        /// </summary>
        FileBackedVirtual = 0xF,

        /// <summary>
        /// The device is using the Spaces driver.
        /// </summary>
        Spaces = 0x10,

        /// <summary>
        /// The device is a Non-Volatile Memory device, usually attached directly.
        /// </summary>
        Nvme = 0x11,

        /// <summary>
        /// The device is a Storage Class Memory.
        /// </summary>
        SCM = 0x12,

        /// <summary>
        /// The device is a Universal Flash Storage device.
        /// </summary>
        Ufs = 0x13,
    }


    /// <summary>
    /// Extensions for the <see cref="BusType"/>.
    /// </summary>
    public static class BusTypeExt
    {
        /// <summary>
        /// Converts the <see cref="BusType"/> enumeration to a more meaningful (printable) short description.
        /// </summary>
        /// <param name="busType">Type of the bus.</param>
        /// <returns>A short description of the device</returns>
        public static string ToDescription(this BusType busType)
        {
            switch (busType) {
            case BusType.Unknown: return "Unknown";
            case BusType.Scsi: return "SCSI";
            case BusType.Atapi: return "ATAPI";
            case BusType.Ata: return "ATA";
            case BusType.Ieee1394: return "IEEE1394";
            case BusType.Ssa: return "SSA";
            case BusType.Fibre: return "Fibre";
            case BusType.Usb: return "USB";
            case BusType.RAID: return "RAID";
            case BusType.iScsi: return "iSCSI";
            case BusType.Sas: return "SAS";
            case BusType.Sata: return "SATA";
            case BusType.Sd: return "SD";
            case BusType.Mmc: return "MMC";
            case BusType.Virtual: return "Virtual";
            case BusType.FileBackedVirtual: return "File Backed Virtual";
            case BusType.Spaces: return "Spaces";
            case BusType.Nvme: return "NVMe";
            case BusType.SCM: return "SCM";
            case BusType.Ufs: return "UFS";
            default:
                if (Enum.IsDefined(typeof(WinIoCtl.STORAGE_BUS_TYPE), (WinIoCtl.STORAGE_BUS_TYPE)busType))
                    return ((WinIoCtl.STORAGE_BUS_TYPE)busType).ToString();
                return string.Format("Bus Type: 0x{0:X2}", (int)busType);
            }
        }

        /// <summary>
        /// Converts the <see cref="BusType"/> enumeration to a more meaningful (printable), possible long, description.
        /// </summary>
        /// <param name="busType">Type of the bus.</param>
        /// <param name="extended">
        /// If set to <see langword="true"/>, provide a longer, more meaningful description.
        /// </param>
        /// <returns>A description of the device</returns>
        public static string ToDescription(this BusType busType, bool extended)
        {
            if (!extended) {
                switch (busType) {
                case BusType.Unknown: return "Unknown";
                case BusType.Scsi: return "SCSI";
                case BusType.Atapi: return "ATAPI";
                case BusType.Ata: return "ATA";
                case BusType.Ieee1394: return "IEEE1394";
                case BusType.Ssa: return "SSA";
                case BusType.Fibre: return "Fibre";
                case BusType.Usb: return "USB";
                case BusType.RAID: return "RAID";
                case BusType.iScsi: return "iSCSI";
                case BusType.Sas: return "SAS";
                case BusType.Sata: return "SATA";
                case BusType.Sd: return "SD";
                case BusType.Mmc: return "MMC";
                case BusType.Virtual: return "Virtual";
                case BusType.FileBackedVirtual: return "File Backed Virtual";
                case BusType.Spaces: return "Spaces";
                case BusType.Nvme: return "NVMe";
                case BusType.SCM: return "SCM";
                case BusType.Ufs: return "UFS";
                }
            } else {
                switch (busType) {
                case BusType.Unknown: return "Unknown";
                case BusType.Scsi: return "Small Computer Systems Interface (SCSI)";
                case BusType.Atapi: return "AT Attachment Packet Interface (ATAPI)";
                case BusType.Ata: return "AT Attachment (ATA)";
                case BusType.Ieee1394: return "IEEE1384 Firewire";
                case BusType.Ssa: return "Serial Storage Architecture (SSA)";
                case BusType.Fibre: return "Fibre";
                case BusType.Usb: return "Universal Serial Bus (USB)";
                case BusType.RAID: return "Redundant Array of Independent Disks (RAID)";
                case BusType.iScsi: return "Internet Small Computer Systems Interface (iSCSI)";
                case BusType.Sas: return "Serial Attached SCSI (SAS)";
                case BusType.Sata: return "Serial AT Attachment (SATA)";
                case BusType.Sd: return "Secure Digital (SD)";
                case BusType.Mmc: return "Multimedia Card (MMC)";
                case BusType.Virtual: return "Virtual";
                case BusType.FileBackedVirtual: return "File Backed Virtual";
                case BusType.Spaces: return "Spaces";
                case BusType.Nvme: return "Non-Volatile Memory Express (NVMe)";
                case BusType.SCM: return "Storage Class Memory (SCM)";
                case BusType.Ufs: return "Universal Flash Storage (UFS)";
                }
            }

            if (Enum.IsDefined(typeof(WinIoCtl.STORAGE_BUS_TYPE), (WinIoCtl.STORAGE_BUS_TYPE)busType))
                return ((WinIoCtl.STORAGE_BUS_TYPE)busType).ToString();
            return string.Format("Bus Type: 0x{0:X2}", (int)busType);
        }
    }
}
