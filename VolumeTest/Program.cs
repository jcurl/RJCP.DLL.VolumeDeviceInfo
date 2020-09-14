namespace VolumeTest
{
    using System;
    using Native;

    public static class Program
    {
        static int Main(string[] args)
        {
            foreach (string device in args) {
                Console.WriteLine("Device Path: {0}", device);

                VolumeDeviceInfo info;
                try {
#if DEBUG
                    VolumeDeviceInfo.DebugOutput(device);
#endif
                    info = new VolumeDeviceInfo(device);
                    Console.WriteLine("  Volume");
                    Console.WriteLine("    Volume Path     : {0}", info.VolumePath);
                    Console.WriteLine("    Volume Device   : {0}", info.VolumeDevicePath);
                    Console.WriteLine("    NT DOS Device   : {0}", info.VolumeDosDevicePath);
                    Console.WriteLine("  Device");
                    Console.WriteLine("    Vendor          : {0}", info.VendorId);
                    Console.WriteLine("    Product         : {0}; Revision {1}", info.ProductId, info.ProductRevision);
                    Console.WriteLine("    SerialNumber    : {0}", info.DeviceSerialNumber);
                    Console.WriteLine("    Bus Type        : {0}", info.BusType.ToDescription(true));
                    Console.WriteLine("    Removable Media : {0}", info.RemovableMedia);
                    Console.WriteLine("    Command Queueing: {0}", info.CommandQueueing);
                    Console.WriteLine("    SCSI Device Type: {0}; SCSI Modifier: {1}", info.ScsiDeviceType.ToDescription(), info.ScsiDeviceModifier);
                } catch (Exception ex) {
                    Console.WriteLine("  Error: {0}", ex.ToString());
                }
                Console.WriteLine("");
            }

            return 0;
        }
    }
}
