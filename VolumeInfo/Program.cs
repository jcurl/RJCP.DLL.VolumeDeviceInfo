namespace RJCP
{
    using System;
    using System.Collections.Generic;
    using IO.Storage;

    public static class Program
    {
        private class Options
        {
#if !SIGN
            public bool LogApi { get; set; }
#endif

            public bool ParseOption(string arg)
            {
                if (arg == null) throw new ArgumentNullException(nameof(arg));
                if (arg.Length < 2) return false;
                if (arg[0] != '-') return false;

#if !SIGN
                string option = arg.Substring(1);
                if (option.Equals("l", StringComparison.Ordinal) || option.Equals("log", StringComparison.OrdinalIgnoreCase)) {
                    LogApi = true;
                    return true;
                }
#endif

                return true;
            }
        }

        private static readonly HashSet<string> s_Paths = new HashSet<string>();

        static int Main(string[] args)
        {
            Options options = new Options();

            foreach (string device in args) {
                if (options.ParseOption(device)) continue;

                try {
                    Console.WriteLine("Device Path: {0}", device);
                    VolumeDeviceInfo info = VolumeDeviceInfo.Create(device);
#if !SIGN
                    if (options.LogApi) Capture(info);
#endif

                    Console.WriteLine("  Volume");
                    Console.WriteLine("    Drive Type      : {0}", info.DriveType);
                    Console.WriteLine("    Volume Path     : {0}", info.Volume.Path);
                    Console.WriteLine("    Volume Device   : {0}", info.Volume.DevicePath);
                    Console.WriteLine("    Volume Drive    : {0}", info.Volume.DriveLetter);
                    Console.WriteLine("    NT DOS Device   : {0}", info.Volume.DosDevicePath);
                    if (info.Partition != null) {
                        Console.WriteLine("  Partition");
                        Console.WriteLine("    Partition Style : {0}", info.Partition.Style);
                        Console.WriteLine("    Part Number     : {0}", info.Partition.Number);
                        Console.WriteLine("    Part Offset     : {0:X8}", info.Partition.Offset);
                        Console.WriteLine("    Part Length     : {0:F1} GB", info.Partition.Length / 1024.0 / 1024.0 / 1024.0);
                        switch (info.Partition.Style) {
                        case PartitionStyle.MasterBootRecord:
                            var mbrPart = (VolumeDeviceInfo.IMbrPartition)info.Partition;
                            Console.WriteLine("    MBR bootable    : {0}", mbrPart.Bootable);
                            Console.WriteLine("    MBR Type        : {0}", mbrPart.Type);
                            Console.WriteLine("    MBR Offset      : {0}", mbrPart.MbrSectorsOffset);
                            break;
                        case PartitionStyle.GuidPartitionTable:
                            var gptPart = (VolumeDeviceInfo.IGptPartition)info.Partition;
                            Console.WriteLine("    GPT Attributes  : {0}", gptPart.Attributes);
                            Console.WriteLine("    GPT Name        : {0}", gptPart.Name);
                            Console.WriteLine("    GPT Type        : {0}", gptPart.Type);
                            Console.WriteLine("    GPT Id          : {0}", gptPart.Id);
                            break;
                        }
                    }
                    if (info.FileSystem != null) {
                        Console.WriteLine("  File System");
                        Console.WriteLine("    Label           : {0}", info.FileSystem.Label);
                        Console.WriteLine("    Serial Number   : {0}", info.FileSystem.Serial);
                        Console.WriteLine("    Flags           : {0}", info.FileSystem.Flags);
                        Console.WriteLine("    File System     : {0}", info.FileSystem.Name);
                        Console.WriteLine("    Bytes Per Sector: {0}", info.FileSystem.BytesPerSector);
                        Console.WriteLine("    Sectors Per Clus: {0}", info.FileSystem.SectorsPerCluster);
                        Console.WriteLine("    User Free       : {0:F1} GB", info.FileSystem.UserFreeBytes / 1024.0 / 1024.0 / 1024.0);
                        Console.WriteLine("    Total Free      : {0:F1} GB", info.FileSystem.TotalFreeBytes / 1024.0 / 1024.0 / 1024.0);
                        Console.WriteLine("    Capacity        : {0:F1} GB", info.FileSystem.TotalBytes / 1024.0 / 1024.0 / 1024.0);
                    }
                    if (info.Disk != null) {
                        Console.WriteLine("  Device");
                        if (info.Disk.Extents != null) {
                            foreach (DiskExtent extent in info.Disk.Extents) {
                                Console.WriteLine("    Extent: {0}", extent.Device);
                                Console.WriteLine("      Offset        : {0:X8}", extent.StartingOffset);
                                Console.WriteLine("      Length        : {0:F1} GB", extent.ExtentLength / 1024.0 / 1024.0 / 1024.0);
                            }
                        }
                        if (info.Disk.Device != null) {
                            Console.WriteLine("    Vendor          : {0}", info.Disk.Device.VendorId);
                            Console.WriteLine("    Product         : {0}; Revision {1}", info.Disk.Device.ProductId, info.Disk.Device.ProductRevision);
                            Console.WriteLine("    SerialNumber    : {0}", info.Disk.Device.SerialNumber);
                            Console.WriteLine("    Bus Type        : {0}", info.Disk.Device.BusType.ToDescription(true));
                            Console.WriteLine("    SCSI Device Type: {0}; SCSI Modifier: {1}", info.Disk.Device.ScsiDeviceType.ToDescription(), info.Disk.Device.ScsiDeviceModifier);
                            Console.WriteLine("    Command Queueing: {0}", info.Disk.Device.HasCommandQueueing);
                            Console.WriteLine("    Device GUID:    : {0} ({1})", info.Disk.Device.Guid, info.Disk.Device.GuidFlags);
                        }
                        Console.WriteLine("    Removable Media : {0}", info.Disk.IsRemovableMedia);
                        Console.WriteLine("    Media Present   : {0}", info.Disk.IsMediaPresent);
                        Console.WriteLine("    Disk Read Only  : {0}", info.Disk.IsReadOnly);
                        if (info.Disk.Geometry != null) {
                            Console.WriteLine("    Cyl/Trk/Sec/Byte: {0}/{1}/{2}/{3} ({4:F1} GB)",
                                info.Disk.Geometry.Cylinders, info.Disk.Geometry.TracksPerCylinder,
                                info.Disk.Geometry.SectorsPerTrack, info.Disk.Geometry.BytesPerSector,
                                info.Disk.Geometry.Cylinders * info.Disk.Geometry.TracksPerCylinder *
                                info.Disk.Geometry.SectorsPerTrack * info.Disk.Geometry.BytesPerSector / 1024.0 / 1024.0 / 1024.0);
                            Console.WriteLine("    Bytes/Sector    : Physical {0}; Logical {1}",
                                info.Disk.Geometry.BytesPerPhysicalSector, info.Disk.Geometry.BytesPerSector);
                        }
                        Console.WriteLine("    Seek Penalty    : {0}", info.Disk.HasSeekPenalty);
                    }
                } catch (Exception ex) {
#if !SIGN
                    if (options.LogApi) Capture(device);
#endif
                    Console.WriteLine("  Error: {0}", ex.Message);
                }
                Console.WriteLine("");
            }

            return 0;
        }

#if !SIGN
        static void Capture(VolumeDeviceInfo info)
        {
            CaptureItem(info.Path);
            Capture(info.Path);
            Capture(info.Volume.Path);
            Capture(info.Volume.DevicePath);
            if (info.Disk?.Extents != null) {
                foreach (DiskExtent extent in info.Disk.Extents) {
                    Capture(extent.Device);
                }
            }
        }

        static void Capture(string path)
        {
            if (string.IsNullOrEmpty(path)) return;
            string noslash = path.TrimEnd(new[] { System.IO.Path.DirectorySeparatorChar, System.IO.Path.AltDirectorySeparatorChar });
            CaptureItem(noslash);
            CaptureItem(noslash + System.IO.Path.DirectorySeparatorChar);
        }

        static void CaptureItem(string path)
        {
            if (string.IsNullOrEmpty(path)) return;
            if (s_Paths.Contains(path)) return;
            s_Paths.Add(path);
            Console.WriteLine("  Logged: {0}", path);
            LogApi.LogDeviceData.Capture(path);
        }
#endif
    }
}
