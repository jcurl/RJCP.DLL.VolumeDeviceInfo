namespace VolumeTest.Native
{
    using System;

    public enum BusType
    {
        Unknown = 0x00,
        Scsi = 0x1,
        Atapi = 0x2,
        Ata = 0x3,
        Ieee1394 = 0x4,
        Ssa = 0x5,
        Fibre = 0x6,
        Usb = 0x7,
        RAID = 0x8,
        iScsi = 0x9,
        Sas = 0xA,
        Sata = 0xB,
        Sd = 0xC,
        Mmc = 0xD,
        Virtual = 0xE,
        FileBackedVirtual = 0xF,
        Spaces = 0x10,
        Nvme = 0x11,
        SCM = 0x12,
        Ufs = 0x13,
    }


    public static class BusTypeExt
    {
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
                if (Enum.IsDefined(typeof(Win32.WinIoCtl.STORAGE_BUS_TYPE), (Win32.WinIoCtl.STORAGE_BUS_TYPE)busType))
                    return ((Win32.WinIoCtl.STORAGE_BUS_TYPE)busType).ToString();
                return string.Format("Bus Type: 0x{0:X2}", (int)busType);
            }
        }

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

            if (Enum.IsDefined(typeof(Win32.WinIoCtl.STORAGE_BUS_TYPE), (Win32.WinIoCtl.STORAGE_BUS_TYPE)busType))
                return ((Win32.WinIoCtl.STORAGE_BUS_TYPE)busType).ToString();
            return string.Format("Bus Type: 0x{0:X2}", (int)busType);
        }
    }
}
