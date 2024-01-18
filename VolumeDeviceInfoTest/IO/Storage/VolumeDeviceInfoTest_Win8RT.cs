namespace RJCP.IO.Storage
{
    using System;
    using System.IO;
    using NUnit.Framework;

    [TestFixture]
    public class VolumeDeviceInfoTest_Win8RT
    {
        private static readonly string Win8RT = Path.Combine(TestContext.CurrentContext.TestDirectory,
            "Test", "Win32", "VolumeInfoTest.Win8RT.xml");

        // The Physical Drive containing C:
        private const string Phys0 = @"\\.\PhysicalDrive0";
        private const string Phys0S = @"\\.\PhysicalDrive0\";

        public const string C = @"C:";
        public const string CS = @"C:\";
        public const string CD = @"\Device\HarddiskVolume4";
        public const string CV = @"\\?\Volume{83569dd5-908d-49d5-afe4-cdd0c6262cb4}";
        public const string CVS = @"\\?\Volume{83569dd5-908d-49d5-afe4-cdd0c6262cb4}\";

        private const string Phys1 = @"\\.\PhysicalDrive1";
        private const string Phys1S = @"\\.\PhysicalDrive1\";

        public const string D = @"D:";
        public const string DS = @"D:\";
        public const string DD = @"\Device\HarddiskVolume6";
        public const string DV = @"\\?\Volume{c1daecf7-b56f-11ee-9332-6045bd87413c}";
        public const string DVS = @"\\?\Volume{c1daecf7-b56f-11ee-9332-6045bd87413c}\";

        [Test]
        public void PhysicalDrive0()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win8RT, @"\\.\PhysicalDrive0");
            Assert.That(vinfo.Path, Is.EqualTo(Phys0));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Phys0S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(Phys0));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);     // Not a volume
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);       // Not a volume
            IsDrivePhys0(vinfo);
        }

        [Test]
        public void PhysicalDrive0S()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win8RT, @"\\.\PhysicalDrive0\");
            Assert.That(vinfo.Path, Is.EqualTo(Phys0S));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Phys0S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(Phys0));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);     // Not a volume
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);       // Not a volume
            IsDrivePhys0(vinfo);
        }

        private static void IsDrivePhys0(VolumeDeviceInfo vinfo)
        {
            IsPhysicalDrive0(vinfo);
            Assert.That(vinfo.Disk.Extents, Is.Null);
            Assert.That(vinfo.Partition.Style, Is.EqualTo(PartitionStyle.GuidPartitionTable));
            Assert.That(vinfo.Partition.Number, Is.EqualTo(0));
            Assert.That(vinfo.Partition.Offset, Is.EqualTo(0));
            Assert.That(vinfo.Partition.Length, Is.EqualTo(31268536320));
            Assert.That(((VolumeDeviceInfo.IGptPartition)vinfo.Partition).Type, Is.EqualTo(Guid.Empty));
            Assert.That(((VolumeDeviceInfo.IGptPartition)vinfo.Partition).Id.ToString(), Is.EqualTo("d1c64bb8-c9d9-46bf-aa51-179b74613353"));
            Assert.That(((VolumeDeviceInfo.IGptPartition)vinfo.Partition).Name, Is.Empty);
            Assert.That(((VolumeDeviceInfo.IGptPartition)vinfo.Partition).Attributes, Is.EqualTo(EFIPartitionAttributes.None));
            Assert.That(vinfo.FileSystem, Is.Null);
        }

        private static void IsPhysicalDrive0(VolumeDeviceInfo vinfo)
        {
            Assert.That(vinfo.DriveType, Is.EqualTo(DriveType.Fixed));
            Assert.That(vinfo.Disk.Device.VendorId, Is.EqualTo("VID:15"));
            Assert.That(vinfo.Disk.Device.ProductId, Is.EqualTo("MBG4GA"));
            Assert.That(vinfo.Disk.Device.ProductRevision, Is.EqualTo("0.6"));
            Assert.That(vinfo.Disk.Device.SerialNumber, Is.EqualTo("1710a3a6"));
            Assert.That(vinfo.Disk.Device.BusType, Is.EqualTo(BusType.Sd));
            Assert.That(vinfo.Disk.Device.HasCommandQueueing, Is.False);
            Assert.That(vinfo.Disk.Device.ScsiDeviceType, Is.EqualTo(ScsiDeviceType.DirectAccessDevice));
            Assert.That(vinfo.Disk.Device.ScsiDeviceModifier, Is.EqualTo(0));
            Assert.That(vinfo.Disk.Device.GuidFlags, Is.EqualTo(DeviceGuidFlags.None));
            Assert.That(vinfo.Disk.Device.Guid.ToString(), Is.EqualTo("00000000-0000-0000-0000-000000000000"));
            Assert.That(vinfo.Disk.IsRemovableMedia, Is.False);
            Assert.That(vinfo.Disk.IsMediaPresent, Is.True);
            Assert.That(vinfo.Disk.IsReadOnly, Is.False);
            Assert.That(vinfo.Disk.Geometry.Cylinders, Is.EqualTo(3801));
            Assert.That(vinfo.Disk.Geometry.TracksPerCylinder, Is.EqualTo(255));
            Assert.That(vinfo.Disk.Geometry.SectorsPerTrack, Is.EqualTo(63));
            Assert.That(vinfo.Disk.Geometry.BytesPerSector, Is.EqualTo(512));
            Assert.That(vinfo.Disk.Geometry.BytesPerPhysicalSector, Is.EqualTo(4096));
            Assert.That(vinfo.Disk.HasSeekPenalty, Is.EqualTo(BoolUnknown.False));
        }

        [Test]
        public void DriveC()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win8RT, @"C:");
            Assert.That(vinfo.Path, Is.EqualTo(C));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(CS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(C));
            IsDriveBoot(vinfo);
        }

        [Test]
        public void DriveCS()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win8RT, @"C:\");
            Assert.That(vinfo.Path, Is.EqualTo(CS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(CS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(C));
            IsDriveBoot(vinfo);
        }

        [Test]
        public void DriveCV()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win8RT, @"\\?\Volume{83569dd5-908d-49d5-afe4-cdd0c6262cb4}");
            Assert.That(vinfo.Path, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(CVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(C));
            IsDriveBoot(vinfo);
        }

        [Test]
        public void DriveCVS()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win8RT, @"\\?\Volume{83569dd5-908d-49d5-afe4-cdd0c6262cb4}\");
            Assert.That(vinfo.Path, Is.EqualTo(CVS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(CVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(C));
            IsDriveBoot(vinfo);
        }

        private static void IsDriveBoot(VolumeDeviceInfo vinfo)
        {
            IsPhysicalDrive0(vinfo);
            Assert.That(vinfo.Disk.Extents, Has.Length.EqualTo(1));
            Assert.That(vinfo.Disk.Extents[0].Device, Is.EqualTo(@"\\.\PhysicalDrive0"));
            Assert.That(vinfo.Disk.Extents[0].StartingOffset, Is.EqualTo(vinfo.Partition.Offset));
            Assert.That(vinfo.Disk.Extents[0].ExtentLength, Is.EqualTo(vinfo.Partition.Length));
            Assert.That(vinfo.Partition.Style, Is.EqualTo(PartitionStyle.GuidPartitionTable));
            Assert.That(vinfo.Partition.Number, Is.EqualTo(4));
            Assert.That(vinfo.Partition.Offset, Is.EqualTo(670040064));
            Assert.That(vinfo.Partition.Length, Is.EqualTo(27137146880));
            Assert.That(((VolumeDeviceInfo.IGptPartition)vinfo.Partition).Type.ToString(), Is.EqualTo("ebd0a0a2-b9e5-4433-87c0-68b6b72699c7"));
            Assert.That(((VolumeDeviceInfo.IGptPartition)vinfo.Partition).Id.ToString(), Is.EqualTo("83569dd5-908d-49d5-afe4-cdd0c6262cb4"));
            Assert.That(((VolumeDeviceInfo.IGptPartition)vinfo.Partition).Name, Is.EqualTo("Basic data partition"));
            Assert.That(((VolumeDeviceInfo.IGptPartition)vinfo.Partition).Attributes, Is.EqualTo(EFIPartitionAttributes.None));
            Assert.That(vinfo.FileSystem.Label, Is.EqualTo("Windows"));
            Assert.That(vinfo.FileSystem.Serial, Is.EqualTo("3AD8-78B8"));
            Assert.That(vinfo.FileSystem.Name, Is.EqualTo("NTFS"));
            Assert.That((int)vinfo.FileSystem.Flags, Is.EqualTo(0x03E700FF));
        }

        [Test]
        public void PhysicalDrive1()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win8RT, @"\\.\PhysicalDrive1");
            Assert.That(vinfo.Path, Is.EqualTo(Phys1));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Phys1S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(Phys1));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);
            IsDrivePhys1(vinfo);
        }

        [Test]
        public void PhysicalDrive1S()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win8RT, @"\\.\PhysicalDrive1\");
            Assert.That(vinfo.Path, Is.EqualTo(Phys1S));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Phys1S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(Phys1));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);
            IsDrivePhys1(vinfo);
        }

        private static void IsDrivePhys1(VolumeDeviceInfo vinfo)
        {
            IsPhysicalDrive1(vinfo);
            Assert.That(vinfo.Disk.Extents, Is.Null);
            Assert.That(vinfo.Partition.Style, Is.EqualTo(PartitionStyle.MasterBootRecord));
            Assert.That(vinfo.Partition.Number, Is.EqualTo(0));
            Assert.That(vinfo.Partition.Offset, Is.EqualTo(0));
            Assert.That(vinfo.Partition.Length, Is.EqualTo(262144000));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Type, Is.EqualTo(0));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Bootable, Is.False);
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Offset, Is.EqualTo(0));
            Assert.That(vinfo.FileSystem, Is.Null);
        }

        private static void IsPhysicalDrive1(VolumeDeviceInfo vinfo)
        {
            Assert.That(vinfo.DriveType, Is.EqualTo(DriveType.Fixed));
            Assert.That(vinfo.Disk.Device.VendorId, Is.EqualTo("Generic "));
            Assert.That(vinfo.Disk.Device.ProductId, Is.EqualTo("Flash Disk      "));
            Assert.That(vinfo.Disk.Device.ProductRevision, Is.EqualTo("7.77"));
            Assert.That(vinfo.Disk.Device.SerialNumber, Is.EqualTo("\x1F"));
            Assert.That(vinfo.Disk.Device.BusType, Is.EqualTo(BusType.Usb));
            Assert.That(vinfo.Disk.Device.HasCommandQueueing, Is.False);
            Assert.That(vinfo.Disk.Device.ScsiDeviceType, Is.EqualTo(ScsiDeviceType.DirectAccessDevice));
            Assert.That(vinfo.Disk.Device.ScsiDeviceModifier, Is.EqualTo(0));
            Assert.That(vinfo.Disk.Device.GuidFlags, Is.EqualTo(DeviceGuidFlags.None));
            Assert.That(vinfo.Disk.Device.Guid.ToString(), Is.EqualTo("00000000-0000-0000-0000-000000000000"));
            Assert.That(vinfo.Disk.IsRemovableMedia, Is.False);
            Assert.That(vinfo.Disk.IsMediaPresent, Is.True);
            Assert.That(vinfo.Disk.IsReadOnly, Is.False);
            Assert.That(vinfo.Disk.Geometry.Cylinders, Is.EqualTo(31));
            Assert.That(vinfo.Disk.Geometry.TracksPerCylinder, Is.EqualTo(255));
            Assert.That(vinfo.Disk.Geometry.SectorsPerTrack, Is.EqualTo(63));
            Assert.That(vinfo.Disk.Geometry.BytesPerSector, Is.EqualTo(512));
            Assert.That(vinfo.Disk.Geometry.BytesPerPhysicalSector, Is.EqualTo(512));
            Assert.That(vinfo.Disk.HasSeekPenalty, Is.EqualTo(BoolUnknown.Unknown));
        }

        [Test]
        public void DriveD()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win8RT, @"D:");
            Assert.That(vinfo.Path, Is.EqualTo(D));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(DS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(DD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(D));
            IsDriveUsb(vinfo);
        }

        [Test]
        public void DriveDS()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win8RT, @"D:\");
            Assert.That(vinfo.Path, Is.EqualTo(DS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(DS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(DD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(D));
            IsDriveUsb(vinfo);
        }

        [Test]
        public void DriveDV()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win8RT, @"\\?\Volume{c1daecf7-b56f-11ee-9332-6045bd87413c}");
            Assert.That(vinfo.Path, Is.EqualTo(DV));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(DVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(DD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(D));
            IsDriveUsb(vinfo);
        }

        [Test]
        public void DriveDVS()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win8RT, @"\\?\Volume{c1daecf7-b56f-11ee-9332-6045bd87413c}\");
            Assert.That(vinfo.Path, Is.EqualTo(DVS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(DVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(DD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(D));
            IsDriveUsb(vinfo);
        }

        private static void IsDriveUsb(VolumeDeviceInfo vinfo)
        {
            IsPhysicalDrive1(vinfo);
            Assert.That(vinfo.Disk.Extents, Has.Length.EqualTo(1));
            Assert.That(vinfo.Disk.Extents[0].Device, Is.EqualTo(@"\\.\PhysicalDrive1"));
            Assert.That(vinfo.Disk.Extents[0].StartingOffset, Is.EqualTo(vinfo.Partition.Offset));
            Assert.That(vinfo.Disk.Extents[0].ExtentLength, Is.EqualTo(vinfo.Partition.Length));
            Assert.That(vinfo.Partition.Style, Is.EqualTo(PartitionStyle.MasterBootRecord));
            Assert.That(vinfo.Partition.Number, Is.EqualTo(1));
            Assert.That(vinfo.Partition.Offset, Is.EqualTo(32256));
            Assert.That(vinfo.Partition.Length, Is.EqualTo(262111744));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Type, Is.EqualTo(14));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Bootable, Is.True);
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Offset, Is.EqualTo(32256));
            Assert.That(vinfo.FileSystem.Label, Is.EqualTo(""));
            Assert.That(vinfo.FileSystem.Serial, Is.EqualTo("8ECC-9782"));
            Assert.That(vinfo.FileSystem.Name, Is.EqualTo("FAT"));
            Assert.That((int)vinfo.FileSystem.Flags, Is.EqualTo(0x00000006));
        }
    }
}
