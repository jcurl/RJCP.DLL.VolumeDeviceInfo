namespace VolumeInfo.IO.Storage
{
    using System;
    using System.IO;
    using NUnit.Framework;

    [TestFixture]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase",
        Justification = "Correct case in the circumstances")]
    public class VolumeDeviceInfoTest_Win2K3R2
    {
        private static readonly string Win2k3r2Sim = Path.Combine(TestContext.CurrentContext.TestDirectory,
            "Test", "Win32", "VolumeInfoTest.Win2k3r2.xml");

        // Drive C:
        private const string Phys0 = @"\\.\PhysicalDrive0";
        private const string Phys0S = @"\\.\PhysicalDrive0\";

        // The Physical Drive containing the first extent for drive E:
        private const string Phys1 = @"\\.\PhysicalDrive1";
        private const string Phys1S = @"\\.\PhysicalDrive1\";

        // The Physical Drive containing the second extent for drive E:
        private const string Phys2 = @"\\.\PhysicalDrive2";
        private const string Phys2S = @"\\.\PhysicalDrive2\";

        // The Physical Drive containing the two partitions for a single volume F:
        private const string Phys3 = @"\\.\PhysicalDrive3";
        private const string Phys3S = @"\\.\PhysicalDrive3\";

        // The boot partition C:
        private const string C = @"C:";
        private const string CS = @"C:\";
        private const string CV = @"\\?\Volume{73ff8621-041a-11eb-bfe1-806e6f6e6963}";
        private const string CVS = @"\\?\Volume{73ff8621-041a-11eb-bfe1-806e6f6e6963}\";
        private const string CD = @"\Device\HarddiskVolume1";

        // Mirrored Volume E:
        private const string E = @"E:";
        private const string ES = @"E:\";
        private const string EV = @"\\?\Volume{78d41c0e-348a-49e2-b7de-b92956381bd3}";
        private const string EVS = @"\\?\Volume{78d41c0e-348a-49e2-b7de-b92956381bd3}\";
        private const string ED = @"\Device\HarddiskDmVolumes\Xxx-d02058760efDg0\Volume1";

        // Multipartition F:
        private const string F = @"F:";
        private const string FS = @"F:\";
        private const string FV = @"\\?\Volume{273d5d74-ead4-4847-8846-941c99522a66}";
        private const string FVS = @"\\?\Volume{273d5d74-ead4-4847-8846-941c99522a66}\";
        private const string FD = @"\Device\HarddiskDmVolumes\Xxx-d02058760efDg0\Volume2";

        [Test]
        public void PhysicalDrive0()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win2k3r2Sim, @"\\.\PhysicalDrive0");
            Assert.That(vinfo.Path, Is.EqualTo(Phys0));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Phys0S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(Phys0));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);     // Physical drive
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);       // Physical drive
            Assert.That(vinfo.Disk.Extents, Is.Null);
            IsDrivePhys0(vinfo);
        }

        [Test]
        public void PhysicalDrive0S()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win2k3r2Sim, @"\\.\PhysicalDrive0\");
            Assert.That(vinfo.Path, Is.EqualTo(Phys0S));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Phys0S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(Phys0));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);     // Physical drive
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);       // Physical drive
            Assert.That(vinfo.Disk.Extents, Is.Null);
            IsDrivePhys0(vinfo);
        }

        private void IsDrivePhys0(VolumeDeviceInfo vinfo)
        {
            IsPhysicalDrive0(vinfo);
            Assert.That(vinfo.Disk.Extents, Is.Null);
            Assert.That(vinfo.Partition.Style, Is.EqualTo(PartitionStyle.MasterBootRecord));
            Assert.That(vinfo.Partition.Number, Is.EqualTo(0));
            Assert.That(vinfo.Partition.Offset, Is.EqualTo(0));
            Assert.That(vinfo.Partition.Length, Is.EqualTo(10737418240));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Type, Is.EqualTo(0));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Bootable, Is.False);
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).MbrSectorsOffset, Is.EqualTo(0));
            Assert.That(vinfo.FileSystem, Is.Null);
        }

        public void IsPhysicalDrive0(VolumeDeviceInfo vinfo)
        {
            Assert.That(vinfo.DriveType, Is.EqualTo(DriveType.Fixed));
            Assert.That(vinfo.Disk.Device.VendorId, Is.EqualTo("VMware, "));
            Assert.That(vinfo.Disk.Device.ProductId, Is.EqualTo("VMware Virtual S"));
            Assert.That(vinfo.Disk.Device.ProductRevision, Is.EqualTo("1.0 "));
            Assert.That(vinfo.Disk.Device.SerialNumber, Is.Empty);
            Assert.That(vinfo.Disk.Device.BusType, Is.EqualTo(BusType.Scsi));
            Assert.That(vinfo.Disk.Device.HasCommandQueueing, Is.True);
            Assert.That(vinfo.Disk.Device.ScsiDeviceType, Is.EqualTo(ScsiDeviceType.DirectAccessDevice));
            Assert.That(vinfo.Disk.Device.ScsiDeviceModifier, Is.EqualTo(0));
            Assert.That(vinfo.Disk.Device.GuidFlags, Is.EqualTo(DeviceGuidFlags.None));
            Assert.That(vinfo.Disk.Device.Guid, Is.EqualTo(Guid.Empty));
            Assert.That(vinfo.Disk.IsRemovableMedia, Is.False);
            Assert.That(vinfo.Disk.IsMediaPresent, Is.True);
            Assert.That(vinfo.Disk.IsReadOnly, Is.False);
            Assert.That(vinfo.Disk.Geometry.Cylinders, Is.EqualTo(1468));
            Assert.That(vinfo.Disk.Geometry.TracksPerCylinder, Is.EqualTo(255));
            Assert.That(vinfo.Disk.Geometry.SectorsPerTrack, Is.EqualTo(56));
            Assert.That(vinfo.Disk.Geometry.BytesPerSector, Is.EqualTo(512));
            Assert.That(vinfo.Disk.Geometry.BytesPerPhysicalSector, Is.EqualTo(512));
            Assert.That(vinfo.Disk.HasSeekPenalty, Is.EqualTo(BoolUnknown.Unknown));
        }

        [Test]
        public void DriveC()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win2k3r2Sim, @"C:");
            Assert.That(vinfo.Path, Is.EqualTo(C));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(CS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(C));
            IsBootDrive(vinfo);
        }

        [Test]
        public void DriveCS()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win2k3r2Sim, @"C:\");
            Assert.That(vinfo.Path, Is.EqualTo(CS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(CS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(C));
            IsBootDrive(vinfo);
        }

        [Test]
        public void DriveCV()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win2k3r2Sim, @"\\?\Volume{73ff8621-041a-11eb-bfe1-806e6f6e6963}");
            Assert.That(vinfo.Path, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(CVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(C));
            IsBootDrive(vinfo);
        }

        [Test]
        public void DriveCVS()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win2k3r2Sim, @"\\?\Volume{73ff8621-041a-11eb-bfe1-806e6f6e6963}\");
            Assert.That(vinfo.Path, Is.EqualTo(CVS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(CVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(C));
            IsBootDrive(vinfo);
        }

        private void IsBootDrive(VolumeDeviceInfo vinfo)
        {
            IsPhysicalDrive0(vinfo);
            Assert.That(vinfo.Disk.Extents.Length, Is.EqualTo(1));
            Assert.That(vinfo.Disk.Extents[0].Device, Is.EqualTo(@"\\.\PhysicalDrive0"));
            Assert.That(vinfo.Disk.Extents[0].StartingOffset, Is.EqualTo(vinfo.Partition.Offset));
            Assert.That(vinfo.Disk.Extents[0].ExtentLength, Is.EqualTo(vinfo.Partition.Length));
            Assert.That(vinfo.Partition.Style, Is.EqualTo(PartitionStyle.MasterBootRecord));
            Assert.That(vinfo.Partition.Number, Is.EqualTo(1));
            Assert.That(vinfo.Partition.Offset, Is.EqualTo(28672));
            Assert.That(vinfo.Partition.Length, Is.EqualTo(10725736448));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Type, Is.EqualTo(7));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Bootable, Is.True);
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).MbrSectorsOffset, Is.EqualTo(56));
            Assert.That(vinfo.FileSystem.Label, Is.Empty);
            Assert.That(vinfo.FileSystem.Serial, Is.EqualTo("D444-C292"));
            Assert.That(vinfo.FileSystem.Name, Is.EqualTo("NTFS"));
            Assert.That((int)vinfo.FileSystem.Flags, Is.EqualTo(0x700FF));
            Assert.That(vinfo.FileSystem.BytesPerSector, Is.EqualTo(512));
            Assert.That(vinfo.FileSystem.SectorsPerCluster, Is.EqualTo(8));
            Assert.That(vinfo.FileSystem.TotalBytes, Is.EqualTo(10725732352));
            Assert.That(vinfo.FileSystem.TotalFreeBytes, Is.EqualTo(6526672896));
            Assert.That(vinfo.FileSystem.UserFreeBytes, Is.EqualTo(6526672896));
        }

        [Test]
        public void PhysicalDrive1()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win2k3r2Sim, @"\\.\PhysicalDrive1");
            Assert.That(vinfo.Path, Is.EqualTo(Phys1));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Phys1S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(Phys1));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);     // Physical drive
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);       // Physical drive
            Assert.That(vinfo.Disk.Extents, Is.Null);
            IsDrivePhys1(vinfo);
        }

        [Test]
        public void PhysicalDrive1S()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win2k3r2Sim, @"\\.\PhysicalDrive1\");
            Assert.That(vinfo.Path, Is.EqualTo(Phys1S));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Phys1S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(Phys1));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);     // Physical drive
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);       // Physical drive
            Assert.That(vinfo.Disk.Extents, Is.Null);
            IsDrivePhys1(vinfo);
        }

        private void IsDrivePhys1(VolumeDeviceInfo vinfo)
        {
            IsPhysicalDrive1(vinfo);
            Assert.That(vinfo.Disk.Extents, Is.Null);
            Assert.That(vinfo.Partition.Style, Is.EqualTo(PartitionStyle.MasterBootRecord));
            Assert.That(vinfo.Partition.Number, Is.EqualTo(0));
            Assert.That(vinfo.Partition.Offset, Is.EqualTo(0));
            Assert.That(vinfo.Partition.Length, Is.EqualTo(10737418240));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Type, Is.EqualTo(0));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Bootable, Is.False);
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).MbrSectorsOffset, Is.EqualTo(0));
            Assert.That(vinfo.FileSystem, Is.Null);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S4144:Methods should not have identical implementations",
            Justification = "Test Case must keep them separate as they're physically different devices")]
        public void IsPhysicalDrive1(VolumeDeviceInfo vinfo)
        {
            Assert.That(vinfo.DriveType, Is.EqualTo(DriveType.Fixed));
            Assert.That(vinfo.Disk.Device.VendorId, Is.EqualTo("VMware, "));
            Assert.That(vinfo.Disk.Device.ProductId, Is.EqualTo("VMware Virtual S"));
            Assert.That(vinfo.Disk.Device.ProductRevision, Is.EqualTo("1.0 "));
            Assert.That(vinfo.Disk.Device.SerialNumber, Is.Empty);
            Assert.That(vinfo.Disk.Device.BusType, Is.EqualTo(BusType.Scsi));
            Assert.That(vinfo.Disk.Device.HasCommandQueueing, Is.True);
            Assert.That(vinfo.Disk.Device.ScsiDeviceType, Is.EqualTo(ScsiDeviceType.DirectAccessDevice));
            Assert.That(vinfo.Disk.Device.ScsiDeviceModifier, Is.EqualTo(0));
            Assert.That(vinfo.Disk.Device.GuidFlags, Is.EqualTo(DeviceGuidFlags.None));
            Assert.That(vinfo.Disk.Device.Guid, Is.EqualTo(Guid.Empty));
            Assert.That(vinfo.Disk.IsRemovableMedia, Is.False);
            Assert.That(vinfo.Disk.IsMediaPresent, Is.True);
            Assert.That(vinfo.Disk.IsReadOnly, Is.False);
            Assert.That(vinfo.Disk.Geometry.Cylinders, Is.EqualTo(1468));
            Assert.That(vinfo.Disk.Geometry.TracksPerCylinder, Is.EqualTo(255));
            Assert.That(vinfo.Disk.Geometry.SectorsPerTrack, Is.EqualTo(56));
            Assert.That(vinfo.Disk.Geometry.BytesPerSector, Is.EqualTo(512));
            Assert.That(vinfo.Disk.Geometry.BytesPerPhysicalSector, Is.EqualTo(512));
            Assert.That(vinfo.Disk.HasSeekPenalty, Is.EqualTo(BoolUnknown.Unknown));
        }

        [Test]
        public void PhysicalDrive2()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win2k3r2Sim, @"\\.\PhysicalDrive2");
            Assert.That(vinfo.Path, Is.EqualTo(Phys2));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Phys2S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(Phys2));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);     // Physical drive
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);       // Physical drive
            Assert.That(vinfo.Disk.Extents, Is.Null);
            IsDrivePhys2(vinfo);
        }

        [Test]
        public void PhysicalDrive2S()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win2k3r2Sim, @"\\.\PhysicalDrive2\");
            Assert.That(vinfo.Path, Is.EqualTo(Phys2S));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Phys2S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(Phys2));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);     // Physical drive
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);       // Physical drive
            Assert.That(vinfo.Disk.Extents, Is.Null);
            IsDrivePhys2(vinfo);
        }

        private void IsDrivePhys2(VolumeDeviceInfo vinfo)
        {
            IsPhysicalDrive2(vinfo);
            Assert.That(vinfo.Disk.Extents, Is.Null);
            Assert.That(vinfo.Partition.Style, Is.EqualTo(PartitionStyle.MasterBootRecord));
            Assert.That(vinfo.Partition.Number, Is.EqualTo(0));
            Assert.That(vinfo.Partition.Offset, Is.EqualTo(0));
            Assert.That(vinfo.Partition.Length, Is.EqualTo(10737418240));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Type, Is.EqualTo(0));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Bootable, Is.False);
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).MbrSectorsOffset, Is.EqualTo(0));
            Assert.That(vinfo.FileSystem, Is.Null);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S4144:Methods should not have identical implementations",
            Justification = "Test Case must keep them separate as they're physically different devices")]
        public void IsPhysicalDrive2(VolumeDeviceInfo vinfo)
        {
            Assert.That(vinfo.DriveType, Is.EqualTo(DriveType.Fixed));
            Assert.That(vinfo.Disk.Device.VendorId, Is.EqualTo("VMware, "));
            Assert.That(vinfo.Disk.Device.ProductId, Is.EqualTo("VMware Virtual S"));
            Assert.That(vinfo.Disk.Device.ProductRevision, Is.EqualTo("1.0 "));
            Assert.That(vinfo.Disk.Device.SerialNumber, Is.Empty);
            Assert.That(vinfo.Disk.Device.BusType, Is.EqualTo(BusType.Scsi));
            Assert.That(vinfo.Disk.Device.HasCommandQueueing, Is.True);
            Assert.That(vinfo.Disk.Device.ScsiDeviceType, Is.EqualTo(ScsiDeviceType.DirectAccessDevice));
            Assert.That(vinfo.Disk.Device.ScsiDeviceModifier, Is.EqualTo(0));
            Assert.That(vinfo.Disk.Device.GuidFlags, Is.EqualTo(DeviceGuidFlags.None));
            Assert.That(vinfo.Disk.Device.Guid, Is.EqualTo(Guid.Empty));
            Assert.That(vinfo.Disk.IsRemovableMedia, Is.False);
            Assert.That(vinfo.Disk.IsMediaPresent, Is.True);
            Assert.That(vinfo.Disk.IsReadOnly, Is.False);
            Assert.That(vinfo.Disk.Geometry.Cylinders, Is.EqualTo(1468));
            Assert.That(vinfo.Disk.Geometry.TracksPerCylinder, Is.EqualTo(255));
            Assert.That(vinfo.Disk.Geometry.SectorsPerTrack, Is.EqualTo(56));
            Assert.That(vinfo.Disk.Geometry.BytesPerSector, Is.EqualTo(512));
            Assert.That(vinfo.Disk.Geometry.BytesPerPhysicalSector, Is.EqualTo(512));
            Assert.That(vinfo.Disk.HasSeekPenalty, Is.EqualTo(BoolUnknown.Unknown));
        }

        [Test]
        public void DriveE()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win2k3r2Sim, @"E:");
            Assert.That(vinfo.Path, Is.EqualTo(E));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(ES));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(EV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(ED));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(E));
            IsMirrorDrive(vinfo);
        }

        [Test]
        public void DriveES()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win2k3r2Sim, @"E:\");
            Assert.That(vinfo.Path, Is.EqualTo(ES));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(ES));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(EV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(ED));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(E));
            IsMirrorDrive(vinfo);
        }

        [Test]
        public void DriveEV()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win2k3r2Sim, @"\\?\Volume{78d41c0e-348a-49e2-b7de-b92956381bd3}");
            Assert.That(vinfo.Path, Is.EqualTo(EV));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(EVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(EV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(ED));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(E));
            IsMirrorDrive(vinfo);
        }

        [Test]
        public void DriveEVS()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win2k3r2Sim, @"\\?\Volume{78d41c0e-348a-49e2-b7de-b92956381bd3}\");
            Assert.That(vinfo.Path, Is.EqualTo(EVS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(EVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(EV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(ED));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(E));
            IsMirrorDrive(vinfo);
        }

        private void IsMirrorDrive(VolumeDeviceInfo vinfo)
        {
            Assert.That(vinfo.DriveType, Is.EqualTo(DriveType.Fixed));
            Assert.That(vinfo.Disk.Extents.Length, Is.EqualTo(2));   // This is a mirrored volume
            Assert.That(vinfo.Disk.Extents[0].Device, Is.EqualTo(@"\\.\PhysicalDrive2"));
            Assert.That(vinfo.Disk.Extents[0].StartingOffset, Is.EqualTo(28672));
            Assert.That(vinfo.Disk.Extents[0].ExtentLength, Is.EqualTo(10732175360));
            Assert.That(vinfo.Disk.Extents[1].Device, Is.EqualTo(@"\\.\PhysicalDrive1"));
            Assert.That(vinfo.Disk.Extents[1].StartingOffset, Is.EqualTo(28672));
            Assert.That(vinfo.Disk.Extents[1].ExtentLength, Is.EqualTo(10732175360));
            Assert.That(vinfo.Partition, Is.Null); // This is a multivolume, no single partition. Need to get from the physical drive
            Assert.That(vinfo.FileSystem.Label, Is.EqualTo("MIRROR"));
            Assert.That(vinfo.FileSystem.Serial, Is.EqualTo("28E7-CE46"));
            Assert.That(vinfo.FileSystem.Name, Is.EqualTo("NTFS"));
            Assert.That((int)vinfo.FileSystem.Flags, Is.EqualTo(0x700FF));
            Assert.That(vinfo.FileSystem.BytesPerSector, Is.EqualTo(512));
            Assert.That(vinfo.FileSystem.SectorsPerCluster, Is.EqualTo(8));
            Assert.That(vinfo.FileSystem.TotalBytes, Is.EqualTo(10732171264));
            Assert.That(vinfo.FileSystem.TotalFreeBytes, Is.EqualTo(10675417088));
            Assert.That(vinfo.FileSystem.UserFreeBytes, Is.EqualTo(10675417088));
        }

        [Test]
        public void PhysicalDrive3()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win2k3r2Sim, @"\\.\PhysicalDrive3");
            Assert.That(vinfo.Path, Is.EqualTo(Phys3));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Phys3S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(Phys3));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);     // Physical drive
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);       // Physical drive
            IsDrivePhys3(vinfo);
        }

        [Test]
        public void PhysicalDrive3S()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win2k3r2Sim, @"\\.\PhysicalDrive3\");
            Assert.That(vinfo.Path, Is.EqualTo(Phys3S));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Phys3S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(Phys3));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);     // Physical drive
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);       // Physical drive
            Assert.That(vinfo.Disk.Extents, Is.Null);
            IsDrivePhys3(vinfo);
        }

        private void IsDrivePhys3(VolumeDeviceInfo vinfo)
        {
            IsPhysicalDrive3(vinfo);
            Assert.That(vinfo.Disk.Extents, Is.Null);
            Assert.That(vinfo.Partition.Style, Is.EqualTo(PartitionStyle.MasterBootRecord));
            Assert.That(vinfo.Partition.Number, Is.EqualTo(0));
            Assert.That(vinfo.Partition.Offset, Is.EqualTo(0));
            Assert.That(vinfo.Partition.Length, Is.EqualTo(16106127360));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Type, Is.EqualTo(0));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Bootable, Is.False);
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).MbrSectorsOffset, Is.EqualTo(0));
            Assert.That(vinfo.FileSystem, Is.Null);
        }

        public void IsPhysicalDrive3(VolumeDeviceInfo vinfo)
        {
            Assert.That(vinfo.DriveType, Is.EqualTo(DriveType.Fixed));
            Assert.That(vinfo.Disk.Device.VendorId, Is.EqualTo("VMware, "));
            Assert.That(vinfo.Disk.Device.ProductId, Is.EqualTo("VMware Virtual S"));
            Assert.That(vinfo.Disk.Device.ProductRevision, Is.EqualTo("1.0 "));
            Assert.That(vinfo.Disk.Device.SerialNumber, Is.Empty);
            Assert.That(vinfo.Disk.Device.BusType, Is.EqualTo(BusType.Scsi));
            Assert.That(vinfo.Disk.Device.HasCommandQueueing, Is.True);
            Assert.That(vinfo.Disk.Device.ScsiDeviceType, Is.EqualTo(ScsiDeviceType.DirectAccessDevice));
            Assert.That(vinfo.Disk.Device.ScsiDeviceModifier, Is.EqualTo(0));
            Assert.That(vinfo.Disk.Device.GuidFlags, Is.EqualTo(DeviceGuidFlags.None));
            Assert.That(vinfo.Disk.Device.Guid, Is.EqualTo(Guid.Empty));
            Assert.That(vinfo.Disk.IsRemovableMedia, Is.False);
            Assert.That(vinfo.Disk.IsMediaPresent, Is.True);
            Assert.That(vinfo.Disk.IsReadOnly, Is.False);
            Assert.That(vinfo.Disk.Geometry.Cylinders, Is.EqualTo(1958));
            Assert.That(vinfo.Disk.Geometry.TracksPerCylinder, Is.EqualTo(255));
            Assert.That(vinfo.Disk.Geometry.SectorsPerTrack, Is.EqualTo(63));
            Assert.That(vinfo.Disk.Geometry.BytesPerSector, Is.EqualTo(512));
            Assert.That(vinfo.Disk.Geometry.BytesPerPhysicalSector, Is.EqualTo(512));
            Assert.That(vinfo.Disk.HasSeekPenalty, Is.EqualTo(BoolUnknown.Unknown));
        }

        [Test]
        public void DriveF()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win2k3r2Sim, @"F:");
            Assert.That(vinfo.Path, Is.EqualTo(F));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(FS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(FV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(FD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(F));
            IsMultiPartDrive(vinfo);
        }

        [Test]
        public void DriveFS()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win2k3r2Sim, @"F:\");
            Assert.That(vinfo.Path, Is.EqualTo(FS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(FS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(FV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(FD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(F));
            IsMultiPartDrive(vinfo);
        }

        [Test]
        public void DriveFV()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win2k3r2Sim, @"\\?\Volume{273d5d74-ead4-4847-8846-941c99522a66}");
            Assert.That(vinfo.Path, Is.EqualTo(FV));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(FVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(FV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(FD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(F));
            IsMultiPartDrive(vinfo);
        }

        [Test]
        public void DriveFVS()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win2k3r2Sim, @"\\?\Volume{273d5d74-ead4-4847-8846-941c99522a66}\");
            Assert.That(vinfo.Path, Is.EqualTo(FVS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(FVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(FV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(FD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(F));
            IsMultiPartDrive(vinfo);
        }

        private void IsMultiPartDrive(VolumeDeviceInfo vinfo)
        {
            Assert.That(vinfo.DriveType, Is.EqualTo(DriveType.Fixed));
            Assert.That(vinfo.Disk.Extents.Length, Is.EqualTo(2));   // This is a spanning volume
            Assert.That(vinfo.Disk.Extents[0].Device, Is.EqualTo(@"\\.\PhysicalDrive3"));
            Assert.That(vinfo.Disk.Extents[0].StartingOffset, Is.EqualTo(32256));
            Assert.That(vinfo.Disk.Extents[0].ExtentLength, Is.EqualTo(4294967296));
            Assert.That(vinfo.Disk.Extents[1].Device, Is.EqualTo(@"\\.\PhysicalDrive3"));
            Assert.That(vinfo.Disk.Extents[1].StartingOffset, Is.EqualTo(8589966848));
            Assert.That(vinfo.Disk.Extents[1].ExtentLength, Is.EqualTo(7514095616));
            Assert.That(vinfo.Partition, Is.Null); // This is a multipartition, no single partition. Need to get from the physical drive
            Assert.That(vinfo.FileSystem.Label, Is.EqualTo("Simple"));
            Assert.That(vinfo.FileSystem.Serial, Is.EqualTo("54D5-9860"));
            Assert.That(vinfo.FileSystem.Name, Is.EqualTo("NTFS"));
            Assert.That((int)vinfo.FileSystem.Flags, Is.EqualTo(0x700FF));
            Assert.That(vinfo.FileSystem.BytesPerSector, Is.EqualTo(512));
            Assert.That(vinfo.FileSystem.SectorsPerCluster, Is.EqualTo(8));
            Assert.That(vinfo.FileSystem.TotalBytes, Is.EqualTo(11809058816));
            Assert.That(vinfo.FileSystem.TotalFreeBytes, Is.EqualTo(11784658944));
            Assert.That(vinfo.FileSystem.UserFreeBytes, Is.EqualTo(11784658944));
        }
    }
}
