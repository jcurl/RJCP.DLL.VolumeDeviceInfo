namespace VolumeTest
{
    using System;
    using Native;

    public static class Program
    {
        static int Main(string[] args)
        {
            foreach (string device in args) {
                VolumeDeviceInfo info = new VolumeDeviceInfo(device);
                Console.WriteLine("Device: {0}", device);
                Console.WriteLine("  Vendor: {0}", info.VendorId);
                Console.WriteLine("  Product: {0}; Revision {1}", info.ProductId, info.ProductRevision);
                Console.WriteLine("  SerialNumber: {0}", info.DeviceSerialNumber);
                Console.WriteLine("  Bus Type: {0}", info.BusType.ToDescription(true));
                Console.WriteLine("  Removable Media: {0}", info.RemovableMedia);
                Console.WriteLine("  Command Queueing: {0}", info.CommandQueueing);
                Console.WriteLine("  SCSI Device Type: {0}; SCSI Modifier: {1}", info.ScsiDeviceType.ToDescription(), info.ScsiDeviceModifier);
                Console.WriteLine("");
            }
            return 0;
        }
    }
}
