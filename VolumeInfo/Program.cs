namespace VolumeInfo
{
    using System;
    using IO.Storage;

    public static class Program
    {
        private class Options
        {
            public bool LogApi { get; set; }

            public bool ParseOption(string arg)
            {
                if (arg == null) throw new ArgumentNullException(nameof(arg));
                if (arg.Length < 2) return false;
                if (arg[0] != '-') return false;

                string option = arg.Substring(1);
                if (option.Equals("l", StringComparison.Ordinal) || option.Equals("log", StringComparison.OrdinalIgnoreCase)) {
                    LogApi = true;
                    return true;
                }

                return true;
            }
        }

        static int Main(string[] args)
        {
            Options options = new Options();

            foreach (string device in args) {
                if (options.ParseOption(device)) continue;

                Console.WriteLine("Device Path: {0}", device);

                if (options.LogApi) LogApi.LogDeviceData.Capture(device);

                VolumeDeviceInfo info;
                try {
                    info = new VolumeDeviceInfo(device);
                    Console.WriteLine("  Volume");
                    Console.WriteLine("    Volume Path     : {0}", info.VolumePath);
                    Console.WriteLine("    Volume Device   : {0}", info.VolumeDevicePath);
                    Console.WriteLine("    Volume Drive    : {0}", info.VolumeDrive);
                    Console.WriteLine("    NT DOS Device   : {0}", info.VolumeDosDevicePath);
                    Console.WriteLine("    Label           : {0}", info.VolumeLabel);
                    Console.WriteLine("    Serial Number   : {0}", info.VolumeSerial);
                    Console.WriteLine("    Flags           : {0}", info.FileSystemFlags);
                    Console.WriteLine("    File System     : {0}", info.FileSystem);
                    Console.WriteLine("  Device");
                    Console.WriteLine("    Vendor          : {0}", info.VendorId);
                    Console.WriteLine("    Product         : {0}; Revision {1}", info.ProductId, info.ProductRevision);
                    Console.WriteLine("    SerialNumber    : {0}", info.DeviceSerialNumber);
                    Console.WriteLine("    Bus Type        : {0}", info.BusType.ToDescription(true));
                    Console.WriteLine("    SCSI Device Type: {0}; SCSI Modifier: {1}", info.ScsiDeviceType.ToDescription(), info.ScsiDeviceModifier);
                    Console.WriteLine("    Command Queueing: {0}", info.CommandQueueing);
                    Console.WriteLine("    Removable Media : {0}", info.RemovableMedia);
                    Console.WriteLine("    Media Present   : {0}", info.MediaPresent);
                    Console.WriteLine("    Disk Read Only  : {0}", info.IsDiskReadOnly);
                    Console.WriteLine("    Device GUID:    : {0} ({1})", info.DeviceGuid, info.DeviceGuidFlags);
                    Console.WriteLine("    Device Number   : {0} #{1}", info.DeviceType, info.DeviceNumber);
                    Console.WriteLine("    Partition Number: {0}", info.DevicePartitionNumber);
                    Console.WriteLine("    Media Type      : {0}", info.DiskMediaType);
                    Console.WriteLine("    Cyl/Trk/Sec/Byte: {0}/{1}/{2}/{3} ({4:F1} GB)", info.DiskCylinders, info.DiskTracksPerCylinder, info.DiskSectorsPerTrack, info.DiskBytesPerSector,
                        info.DiskCylinders * info.DiskTracksPerCylinder * info.DiskSectorsPerTrack * info.DiskBytesPerSector / 1024.0 / 1024.0 / 1024.0);
                    Console.WriteLine("    Bytes/Sector    : Physical {0}; Logical {1}", info.DiskBytesPerPhysicalSector, info.DiskBytesPerSector);
                    Console.WriteLine("    Seek Penalty    : {0}", info.HasSeekPenalty);
                } catch (Exception ex) {
                    Console.WriteLine("  Error: {0}", ex.Message);
                }
                Console.WriteLine("");
            }

            return 0;
        }
    }
}
