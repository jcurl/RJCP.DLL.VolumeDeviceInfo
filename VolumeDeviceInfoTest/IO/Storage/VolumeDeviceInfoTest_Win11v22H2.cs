namespace RJCP.IO.Storage
{
    using System;
    using System.IO;
    using NUnit.Framework;

    [TestFixture]
    public class VolumeDeviceInfoTest_Win11v222H2
    {
        private static readonly string Win11Sim = Path.Combine(TestContext.CurrentContext.TestDirectory,
            "Test", "Win32", "VolumeInfoTest.Win11.xml");

        // The Physical Drive containing C: and the hidden partition
        private const string Phys0 = @"\\.\PhysicalDrive0";
        private const string Phys0S = @"\\.\PhysicalDrive0\";

        // The boot partition
        private const string C = @"C:";
        private const string CS = @"C:\";
        private const string CV = @"\\?\Volume{16ae967d-e57e-4ad5-90ea-16e21a37b1e5}";
        private const string CVS = @"\\?\Volume{16ae967d-e57e-4ad5-90ea-16e21a37b1e5}\";
        private const string CD = @"\Device\HarddiskVolume3";

        [Test]
        public void PhysicalDrive0()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win11Sim, @"\\.\PhysicalDrive0");
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
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win11Sim, @"\\.\PhysicalDrive0\");
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
            Assert.That(vinfo.Partition.Length, Is.EqualTo(512110190592));
            Assert.That(((VolumeDeviceInfo.IGptPartition)vinfo.Partition).Type, Is.EqualTo(Guid.Empty));
            Assert.That(((VolumeDeviceInfo.IGptPartition)vinfo.Partition).Id.ToString(), Is.EqualTo("1180bb72-6dd1-4481-b0f3-9e0e1f1b61d0"));
            Assert.That(((VolumeDeviceInfo.IGptPartition)vinfo.Partition).Name, Is.Empty);
            Assert.That(((VolumeDeviceInfo.IGptPartition)vinfo.Partition).Attributes, Is.EqualTo(EFIPartitionAttributes.None));
            Assert.That(vinfo.FileSystem, Is.Null);
        }

        private static void IsPhysicalDrive0(VolumeDeviceInfo vinfo)
        {
            Assert.That(vinfo.DriveType, Is.EqualTo(DriveType.Fixed));
            Assert.That(vinfo.Disk.IsRemovableMedia, Is.False);
            Assert.That(vinfo.Disk.IsMediaPresent, Is.True);
            Assert.That(vinfo.Disk.IsReadOnly, Is.False);
            Assert.That(vinfo.Disk.Device.VendorId, Is.Empty);
            Assert.That(vinfo.Disk.Device.ProductId, Is.EqualTo("SAMSUNG MZFLV512HCJH-000MV"));
            Assert.That(vinfo.Disk.Device.ProductRevision, Is.EqualTo("BXV75M0Q"));
            Assert.That(vinfo.Disk.Device.SerialNumber, Is.EqualTo("0025_3844_61B5_6586."));
            Assert.That(vinfo.Disk.Device.BusType, Is.EqualTo(BusType.Nvme));
            Assert.That(vinfo.Disk.Device.HasCommandQueueing, Is.True);
            Assert.That(vinfo.Disk.Device.ScsiDeviceType, Is.EqualTo(ScsiDeviceType.DirectAccessDevice));
            Assert.That(vinfo.Disk.Device.ScsiDeviceModifier, Is.EqualTo(0));
            Assert.That(vinfo.Disk.Device.GuidFlags, Is.EqualTo(DeviceGuidFlags.Page83DeviceGuid));
            Assert.That(vinfo.Disk.Device.Guid.ToString(), Is.EqualTo("ba408eac-457b-e82e-f5ea-f5764f6a8c94"));
            Assert.That(vinfo.Disk.Geometry.Cylinders, Is.EqualTo(62260));
            Assert.That(vinfo.Disk.Geometry.TracksPerCylinder, Is.EqualTo(255));
            Assert.That(vinfo.Disk.Geometry.SectorsPerTrack, Is.EqualTo(63));
            Assert.That(vinfo.Disk.Geometry.BytesPerSector, Is.EqualTo(512));
            Assert.That(vinfo.Disk.Geometry.BytesPerPhysicalSector, Is.EqualTo(4096));
            Assert.That(vinfo.Disk.HasSeekPenalty, Is.EqualTo(BoolUnknown.False));
        }

        [Test]
        public void DriveC()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win11Sim, @"C:");
            Assert.That(vinfo.Path, Is.EqualTo(C));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(CS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(C));
            IsDriveSamsung(vinfo);
        }

        [Test]
        public void DriveCS()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win11Sim, @"C:\");
            Assert.That(vinfo.Path, Is.EqualTo(CS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(CS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(C));
            IsDriveSamsung(vinfo);
        }

        [Test]
        public void DriveCV()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win11Sim, @"\\?\Volume{16ae967d-e57e-4ad5-90ea-16e21a37b1e5}");
            Assert.That(vinfo.Path, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(CVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(C));
            IsDriveSamsung(vinfo);
        }

        [Test]
        public void DriveCVS()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win11Sim, @"\\?\Volume{16ae967d-e57e-4ad5-90ea-16e21a37b1e5}\");
            Assert.That(vinfo.Path, Is.EqualTo(CVS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(CVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(C));
            IsDriveSamsung(vinfo);
        }

        private static void IsDriveSamsung(VolumeDeviceInfo vinfo)
        {
            IsPhysicalDrive0(vinfo);
            Assert.That(vinfo.Disk.Extents, Has.Length.EqualTo(1));
            Assert.That(vinfo.Disk.Extents[0].Device, Is.EqualTo(@"\\.\PhysicalDrive0"));
            Assert.That(vinfo.Disk.Extents[0].StartingOffset, Is.EqualTo(vinfo.Partition.Offset));
            Assert.That(vinfo.Disk.Extents[0].ExtentLength, Is.EqualTo(vinfo.Partition.Length));
            Assert.That(vinfo.Partition.Style, Is.EqualTo(PartitionStyle.GuidPartitionTable));
            Assert.That(vinfo.Partition.Number, Is.EqualTo(3));
            Assert.That(vinfo.Partition.Offset, Is.EqualTo(122683392));
            Assert.That(vinfo.Partition.Length, Is.EqualTo(511390515200));
            Assert.That(((VolumeDeviceInfo.IGptPartition)vinfo.Partition).Type.ToString(), Is.EqualTo("ebd0a0a2-b9e5-4433-87c0-68b6b72699c7"));
            Assert.That(((VolumeDeviceInfo.IGptPartition)vinfo.Partition).Id.ToString(), Is.EqualTo("16ae967d-e57e-4ad5-90ea-16e21a37b1e5"));
            Assert.That(((VolumeDeviceInfo.IGptPartition)vinfo.Partition).Name, Is.EqualTo("Basic data partition"));
            Assert.That((int)((VolumeDeviceInfo.IGptPartition)vinfo.Partition).Attributes, Is.EqualTo(0));
            Assert.That(vinfo.FileSystem.Label, Is.EqualTo(string.Empty));
            Assert.That(vinfo.FileSystem.Serial, Is.EqualTo("F688-02DE"));
            Assert.That(vinfo.FileSystem.Name, Is.EqualTo("NTFS"));
            Assert.That((int)vinfo.FileSystem.Flags, Is.EqualTo(0x03E72EFF));
            Assert.That(vinfo.FileSystem.BytesPerSector, Is.EqualTo(512));
            Assert.That(vinfo.FileSystem.SectorsPerCluster, Is.EqualTo(8));
            Assert.That(vinfo.FileSystem.TotalBytes, Is.EqualTo(511390511104));
            Assert.That(vinfo.FileSystem.TotalFreeBytes, Is.EqualTo(437187723264));
            Assert.That(vinfo.FileSystem.UserFreeBytes, Is.EqualTo(437187723264));
        }

        [Test]
        public void DriveHarddisk3()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win11Sim, @"\\.\HarddiskVolume3");
            Assert.That(vinfo.Path, Is.EqualTo(@"\\.\HarddiskVolume3"));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(@"\\.\HarddiskVolume3\"));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(C));
            IsDriveSamsung(vinfo);
        }
    }
}
