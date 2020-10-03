namespace VolumeInfo.IO.Storage
{
    using System;
    using System.IO;
    using NUnit.Framework;

    [TestFixture]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase",
        Justification = "Correct case in the circumstances")]
    public class VolumeDeviceInfoTest_Win7SP1
    {
        private static readonly string Win7Sim = Path.Combine(TestContext.CurrentContext.TestDirectory,
            "Test", "Win32", "VolumeInfoTest.Win7SP1.xml");

        // The boot drive containing C: and two Linux partitions
        private const string Phys0 = @"\\.\PhysicalDrive0";
        private const string Phys0S = @"\\.\PhysicalDrive0\";

        // The Physical Drive containing F:
        private const string Phys1 = @"\\.\PhysicalDrive1";
        private const string Phys1S = @"\\.\PhysicalDrive1\";

        // A hidden partition installed by Win7
        private const string Vol1 = @"\\.\HarddiskVolume1";
        private const string Vol1S = @"\\.\HarddiskVolume1\";
        private const string Vol1V = @"\\?\Volume{5391b1c6-9a5d-11e6-9dbc-806e6f6e6963}";

        // Drive C:
        private const string Vol2 = @"\\.\HarddiskVolume2";
        private const string Vol2S = @"\\.\HarddiskVolume2\";

        // Linux partition
        private const string Vol3 = @"\\.\HarddiskVolume3";
        private const string Vol3S = @"\\.\HarddiskVolume3\";

        // Linux partition
        private const string Vol4 = @"\\.\HarddiskVolume4";
        private const string Vol4S = @"\\.\HarddiskVolume4\";

        // Drive F:
        private const string Vol5 = @"\\.\HarddiskVolume5";
        private const string Vol5S = @"\\.\HarddiskVolume5\";

        // The boot partition C:
        private const string C = @"C:";
        private const string CS = @"C:\";
        private const string CV = @"\\?\Volume{5391b1c7-9a5d-11e6-9dbc-806e6f6e6963}";
        private const string CVS = @"\\?\Volume{5391b1c7-9a5d-11e6-9dbc-806e6f6e6963}\";
        private const string CD = @"\Device\HarddiskVolume2";

        // CD-ROM D:
        private const string D = @"D:";
        private const string DS = @"D:\";
        private const string DV = @"\\?\Volume{5391b1cd-9a5d-11e6-9dbc-806e6f6e6963}";
        private const string DVS = @"\\?\Volume{5391b1cd-9a5d-11e6-9dbc-806e6f6e6963}\";
        private const string DD = @"\Device\CdRom0";

        // Virtual CD-ROM E:
        private const string E = @"E:";
        private const string ES = @"E:\";
        private const string EV = @"\\?\Volume{64314999-9ae3-11e6-b310-0021865a69b8}";
        private const string EVS = @"\\?\Volume{64314999-9ae3-11e6-b310-0021865a69b8}\";
        private const string ED = @"\Device\CdRom1";

        // External HDD F:
        private const string F = @"F:";
        private const string FS = @"F:\";
        private const string FV = @"\\?\Volume{455b7dcd-e19f-11e6-b6bc-0021865a69b8}";
        private const string FVS = @"\\?\Volume{455b7dcd-e19f-11e6-b6bc-0021865a69b8}\";
        private const string FD = @"\Device\HarddiskVolume5";

        [Test]
        public void PhysicalDrive0()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win7Sim, @"\\.\PhysicalDrive0");
            Assert.That(vinfo.Path, Is.EqualTo(Phys0));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Phys0S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(Phys0));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);     // Physical drive
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);       // Physical drive
            Assert.That(vinfo.Volume.DriveType, Is.EqualTo(DriveType.Fixed));
            IsDrivePhys0(vinfo);
        }

        [Test]
        public void PhysicalDrive0S()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win7Sim, @"\\.\PhysicalDrive0\");
            Assert.That(vinfo.Path, Is.EqualTo(Phys0S));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Phys0S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(Phys0));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);     // Physical drive
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);       // Physical drive
            Assert.That(vinfo.Volume.DriveType, Is.EqualTo(DriveType.Fixed));
            IsDrivePhys0(vinfo);
        }

        private void IsDrivePhys0(VolumeDeviceInfo vinfo)
        {
            IsPhysicalDrive0(vinfo);
            Assert.That(vinfo.Disk.Extents, Is.Null);
            Assert.That(vinfo.Partition.Style, Is.EqualTo(PartitionStyle.MasterBootRecord));
            Assert.That(vinfo.Partition.Number, Is.EqualTo(0));
            Assert.That(vinfo.Partition.Offset, Is.EqualTo(0));
            Assert.That(vinfo.Partition.Length, Is.EqualTo(320072933376));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Type, Is.EqualTo(0));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Bootable, Is.False);
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).MbrSectorsOffset, Is.EqualTo(0));
            Assert.That(vinfo.FileSystem, Is.Null);
        }

        public void IsPhysicalDrive0(VolumeDeviceInfo vinfo)
        {
            Assert.That(vinfo.Disk.Device.VendorId, Is.Empty);
            Assert.That(vinfo.Disk.Device.ProductId, Is.EqualTo("HGST HEJ423232H9E300"));
            Assert.That(vinfo.Disk.Device.ProductRevision, Is.EqualTo("F6BOA170"));
            Assert.That(vinfo.Disk.Device.SerialNumber, Is.EqualTo("2020202020203646303030304e46304758334338"));
            Assert.That(vinfo.Disk.Device.BusType, Is.EqualTo(BusType.Sata));
            Assert.That(vinfo.Disk.Device.HasCommandQueueing, Is.False);
            Assert.That(vinfo.Disk.Device.ScsiDeviceType, Is.EqualTo(ScsiDeviceType.DirectAccessDevice));
            Assert.That(vinfo.Disk.Device.ScsiDeviceModifier, Is.EqualTo(0));
            Assert.That(vinfo.Disk.Device.GuidFlags, Is.EqualTo(DeviceGuidFlags.None));
            Assert.That(vinfo.Disk.Device.Guid, Is.EqualTo(Guid.Empty));
            Assert.That(vinfo.Disk.Device.DeviceType, Is.EqualTo(DeviceType.Disk));
            Assert.That(vinfo.Disk.Device.DeviceNumber, Is.EqualTo(0));
            Assert.That(vinfo.Disk.IsRemovableMedia, Is.False);
            Assert.That(vinfo.Disk.IsMediaPresent, Is.True);
            Assert.That(vinfo.Disk.IsReadOnly, Is.False);
            Assert.That(vinfo.Disk.MediaType, Is.EqualTo(MediaType.FixedMedia));
            Assert.That(vinfo.Disk.Geometry.Cylinders, Is.EqualTo(41345));
            Assert.That(vinfo.Disk.Geometry.TracksPerCylinder, Is.EqualTo(240));
            Assert.That(vinfo.Disk.Geometry.SectorsPerTrack, Is.EqualTo(63));
            Assert.That(vinfo.Disk.Geometry.BytesPerSector, Is.EqualTo(512));
            Assert.That(vinfo.Disk.Geometry.BytesPerPhysicalSector, Is.EqualTo(4096));
            Assert.That(vinfo.Disk.HasSeekPenalty, Is.EqualTo(BoolUnknown.True));
        }

        [Test]
        public void DriveC()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win7Sim, @"C:");
            Assert.That(vinfo.Path, Is.EqualTo(C));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(CS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(C));
            Assert.That(vinfo.Volume.DriveType, Is.EqualTo(DriveType.Fixed));
            IsBootDrive(vinfo);
        }

        [Test]
        public void DriveCS()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win7Sim, @"C:\");
            Assert.That(vinfo.Path, Is.EqualTo(CS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(CS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(C));
            Assert.That(vinfo.Volume.DriveType, Is.EqualTo(DriveType.Fixed));
            IsBootDrive(vinfo);
        }

        [Test]
        public void DriveCV()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win7Sim, @"\\?\Volume{5391b1c7-9a5d-11e6-9dbc-806e6f6e6963}");
            Assert.That(vinfo.Path, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(CVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(C));
            Assert.That(vinfo.Volume.DriveType, Is.EqualTo(DriveType.Fixed));
            IsBootDrive(vinfo);
        }

        [Test]
        public void DriveCVS()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win7Sim, @"\\?\Volume{5391b1c7-9a5d-11e6-9dbc-806e6f6e6963}\");
            Assert.That(vinfo.Path, Is.EqualTo(CVS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(CVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(C));
            Assert.That(vinfo.Volume.DriveType, Is.EqualTo(DriveType.Fixed));
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
            Assert.That(vinfo.Partition.Number, Is.EqualTo(2));
            Assert.That(vinfo.Partition.Offset, Is.EqualTo(105906176));
            Assert.That(vinfo.Partition.Length, Is.EqualTo(251552333824));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Type, Is.EqualTo(7));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Bootable, Is.False);
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).MbrSectorsOffset, Is.EqualTo(206848));
            Assert.That(vinfo.FileSystem.Label, Is.Empty);
            Assert.That(vinfo.FileSystem.Serial, Is.EqualTo("8C28-A8AD"));
            Assert.That(vinfo.FileSystem.Name, Is.EqualTo("NTFS"));
            Assert.That((int)vinfo.FileSystem.Flags, Is.EqualTo(0x03E700FF));
            Assert.That(vinfo.FileSystem.BytesPerSector, Is.EqualTo(512));
            Assert.That(vinfo.FileSystem.SectorsPerCluster, Is.EqualTo(8));
            Assert.That(vinfo.FileSystem.TotalBytes, Is.EqualTo(251552329728));
            Assert.That(vinfo.FileSystem.TotalFreeBytes, Is.EqualTo(176125943808));
            Assert.That(vinfo.FileSystem.UserFreeBytes, Is.EqualTo(176125943808));
        }

        [Test]
        public void HarddiskVolume1()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win7Sim, @"\\.\HarddiskVolume1");
            Assert.That(vinfo.Path, Is.EqualTo(Vol1));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Vol1S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(Vol1V));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);     // Unmounted volume
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);       // Unmounted volume
            Assert.That(vinfo.Volume.DriveType, Is.EqualTo(DriveType.Fixed));
            IsHarddiskVolume1(vinfo);
        }

        [Test]
        public void HarddiskVolume1S()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win7Sim, @"\\.\HarddiskVolume1\");
            Assert.That(vinfo.Path, Is.EqualTo(Vol1S));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Vol1S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(Vol1V));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);     // Unmounted volume
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);       // Unmounted volume
            Assert.That(vinfo.Volume.DriveType, Is.EqualTo(DriveType.Fixed));
            IsHarddiskVolume1(vinfo);
        }

        private void IsHarddiskVolume1(VolumeDeviceInfo vinfo)
        {
            IsPhysicalDrive0(vinfo);
            Assert.That(vinfo.Disk.Extents.Length, Is.EqualTo(1));
            Assert.That(vinfo.Disk.Extents[0].Device, Is.EqualTo(@"\\.\PhysicalDrive0"));
            Assert.That(vinfo.Disk.Extents[0].StartingOffset, Is.EqualTo(vinfo.Partition.Offset));
            Assert.That(vinfo.Disk.Extents[0].ExtentLength, Is.EqualTo(vinfo.Partition.Length));
            Assert.That(vinfo.Partition.Style, Is.EqualTo(PartitionStyle.MasterBootRecord));
            Assert.That(vinfo.Partition.Number, Is.EqualTo(1));
            Assert.That(vinfo.Partition.Offset, Is.EqualTo(1048576));
            Assert.That(vinfo.Partition.Length, Is.EqualTo(104857600));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Type, Is.EqualTo(7));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Bootable, Is.True);
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).MbrSectorsOffset, Is.EqualTo(2048));
            Assert.That(vinfo.FileSystem.Label, Is.EqualTo("System Reserved"));
            Assert.That(vinfo.FileSystem.Serial, Is.EqualTo("D021-4443"));
            Assert.That(vinfo.FileSystem.Name, Is.EqualTo("NTFS"));
            Assert.That((int)vinfo.FileSystem.Flags, Is.EqualTo(0x03E700FF));
            Assert.That(vinfo.FileSystem.BytesPerSector, Is.EqualTo(512));
            Assert.That(vinfo.FileSystem.SectorsPerCluster, Is.EqualTo(8));
            Assert.That(vinfo.FileSystem.TotalBytes, Is.EqualTo(104853504));
            Assert.That(vinfo.FileSystem.TotalFreeBytes, Is.EqualTo(74989568));
            Assert.That(vinfo.FileSystem.UserFreeBytes, Is.EqualTo(74989568));
        }

        [Test]
        public void HarddiskVolume2()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win7Sim, @"\\.\HarddiskVolume2");
            Assert.That(vinfo.Path, Is.EqualTo(Vol2));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Vol2S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(C));
            Assert.That(vinfo.Volume.DriveType, Is.EqualTo(DriveType.Fixed));
            IsBootDrive(vinfo);
        }

        [Test]
        public void HarddiskVolume2S()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win7Sim, @"\\.\HarddiskVolume2\");
            Assert.That(vinfo.Path, Is.EqualTo(Vol2S));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Vol2S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(C));
            Assert.That(vinfo.Volume.DriveType, Is.EqualTo(DriveType.Fixed));
            IsBootDrive(vinfo);
        }

        [Test]
        public void HarddiskVolume3()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win7Sim, @"\\.\HarddiskVolume3");
            Assert.That(vinfo.Path, Is.EqualTo(Vol3));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Vol3S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(Vol3));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);     // Unknown volume
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);       // Unknown volume
            Assert.That(vinfo.Volume.DriveType, Is.EqualTo(DriveType.Fixed));
            IsHarddiskVolume3(vinfo);
        }

        [Test]
        public void HarddiskVolume3S()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win7Sim, @"\\.\HarddiskVolume3\");
            Assert.That(vinfo.Path, Is.EqualTo(Vol3S));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Vol3S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(Vol3));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);     // Unknown volume
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);       // Unknown volume
            Assert.That(vinfo.Volume.DriveType, Is.EqualTo(DriveType.Fixed));
            IsHarddiskVolume3(vinfo);
        }

        private void IsHarddiskVolume3(VolumeDeviceInfo vinfo)
        {
            IsPhysicalDrive0(vinfo);
            Assert.That(vinfo.Disk.Extents.Length, Is.EqualTo(1));
            Assert.That(vinfo.Disk.Extents[0].Device, Is.EqualTo(@"\\.\PhysicalDrive0"));
            Assert.That(vinfo.Disk.Extents[0].StartingOffset, Is.EqualTo(vinfo.Partition.Offset));
            Assert.That(vinfo.Disk.Extents[0].ExtentLength, Is.EqualTo(vinfo.Partition.Length));
            Assert.That(vinfo.Partition.Style, Is.EqualTo(PartitionStyle.MasterBootRecord));
            Assert.That(vinfo.Partition.Number, Is.EqualTo(4));
            Assert.That(vinfo.Partition.Offset, Is.EqualTo(251659288576));
            Assert.That(vinfo.Partition.Length, Is.EqualTo(64187531264));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Type, Is.EqualTo(131));  // Linux
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Bootable, Is.False);
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).MbrSectorsOffset, Is.EqualTo(1));
            Assert.That(vinfo.FileSystem, Is.Null);
        }

        [Test]
        public void HarddiskVolume4()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win7Sim, @"\\.\HarddiskVolume4");
            Assert.That(vinfo.Path, Is.EqualTo(Vol4));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Vol4S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(Vol4));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);     // Unknown volume
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);       // Unknown volume
            Assert.That(vinfo.Volume.DriveType, Is.EqualTo(DriveType.Fixed));
            IsHarddiskVolume4(vinfo);
        }

        [Test]
        public void HarddiskVolume4S()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win7Sim, @"\\.\HarddiskVolume4\");
            Assert.That(vinfo.Path, Is.EqualTo(Vol4S));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Vol4S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(Vol4));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);     // Unknown volume
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);       // Unknown volume
            Assert.That(vinfo.Volume.DriveType, Is.EqualTo(DriveType.Fixed));
            IsHarddiskVolume4(vinfo);
        }

        private void IsHarddiskVolume4(VolumeDeviceInfo vinfo)
        {
            IsPhysicalDrive0(vinfo);
            Assert.That(vinfo.Disk.Extents.Length, Is.EqualTo(1));
            Assert.That(vinfo.Disk.Extents[0].Device, Is.EqualTo(@"\\.\PhysicalDrive0"));
            Assert.That(vinfo.Disk.Extents[0].StartingOffset, Is.EqualTo(vinfo.Partition.Offset));
            Assert.That(vinfo.Disk.Extents[0].ExtentLength, Is.EqualTo(vinfo.Partition.Length));
            Assert.That(vinfo.Partition.Style, Is.EqualTo(PartitionStyle.MasterBootRecord));
            Assert.That(vinfo.Partition.Number, Is.EqualTo(3));
            Assert.That(vinfo.Partition.Offset, Is.EqualTo(315846819840));
            Assert.That(vinfo.Partition.Length, Is.EqualTo(4225761280));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Type, Is.EqualTo(130));  // Linux
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Bootable, Is.False);
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).MbrSectorsOffset, Is.EqualTo(125366274));
            Assert.That(vinfo.FileSystem, Is.Null);
        }

        [Test]
        public void DriveD()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win7Sim, @"D:");
            Assert.That(vinfo.Path, Is.EqualTo(D));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(DS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(DD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(D));
            IsCdRom(vinfo);
        }

        [Test]
        public void DriveDS()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win7Sim, @"D:\");
            Assert.That(vinfo.Path, Is.EqualTo(DS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(DS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(DD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(D));
            IsCdRom(vinfo);
        }

        [Test]
        public void DriveDV()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win7Sim, @"\\?\Volume{5391b1cd-9a5d-11e6-9dbc-806e6f6e6963}");
            Assert.That(vinfo.Path, Is.EqualTo(DV));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(DVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(DD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(D));
            IsCdRom(vinfo);
        }

        [Test]
        public void DriveDVS()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win7Sim, @"\\?\Volume{5391b1cd-9a5d-11e6-9dbc-806e6f6e6963}\");
            Assert.That(vinfo.Path, Is.EqualTo(DVS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(DVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(DD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(D));
            IsCdRom(vinfo);
        }

        private void IsCdRom(VolumeDeviceInfo vinfo)
        {
            Assert.That(vinfo.Volume.DriveType, Is.EqualTo(DriveType.CdRom));
            Assert.That(vinfo.Disk.Extents, Is.Null);
            Assert.That(vinfo.Disk.Device.VendorId, Is.Empty);
            Assert.That(vinfo.Disk.Device.ProductId, Is.EqualTo("HL-DT-ST DVDRAM GSA-U10N"));
            Assert.That(vinfo.Disk.Device.ProductRevision, Is.EqualTo("1.05"));
            Assert.That(vinfo.Disk.Device.SerialNumber, Is.Empty);
            Assert.That(vinfo.Disk.Device.BusType, Is.EqualTo(BusType.Atapi));
            Assert.That(vinfo.Disk.Device.HasCommandQueueing, Is.False);
            Assert.That(vinfo.Disk.Device.ScsiDeviceType, Is.EqualTo(ScsiDeviceType.CdRomDevice));
            Assert.That(vinfo.Disk.Device.ScsiDeviceModifier, Is.EqualTo(0));
            Assert.That(vinfo.Disk.Device.GuidFlags, Is.EqualTo(DeviceGuidFlags.None));
            Assert.That(vinfo.Disk.Device.Guid, Is.EqualTo(Guid.Empty));
            Assert.That(vinfo.Disk.Device.DeviceType, Is.EqualTo(DeviceType.CdRom));
            Assert.That(vinfo.Disk.Device.DeviceNumber, Is.EqualTo(0));
            Assert.That(vinfo.Disk.IsRemovableMedia, Is.True);
            Assert.That(vinfo.Disk.IsMediaPresent, Is.False);
            Assert.That(vinfo.Disk.IsReadOnly, Is.True);
            Assert.That(vinfo.Disk.MediaType, Is.EqualTo(MediaType.Unknown));
            Assert.That(vinfo.Disk.Geometry, Is.Null);
            Assert.That(vinfo.Disk.HasSeekPenalty, Is.EqualTo(BoolUnknown.True));
            Assert.That(vinfo.Partition, Is.Null);
            Assert.That(vinfo.FileSystem, Is.Null);
        }

        [Test]
        public void DriveE()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win7Sim, @"E:");
            Assert.That(vinfo.Path, Is.EqualTo(E));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(ES));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(EV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(ED));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(E));
            IsVirtCdRom(vinfo);
        }

        [Test]
        public void DriveES()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win7Sim, @"E:\");
            Assert.That(vinfo.Path, Is.EqualTo(ES));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(ES));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(EV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(ED));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(E));
            IsVirtCdRom(vinfo);
        }

        [Test]
        public void DriveEV()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win7Sim, @"\\?\Volume{64314999-9ae3-11e6-b310-0021865a69b8}");
            Assert.That(vinfo.Path, Is.EqualTo(EV));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(EVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(EV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(ED));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(E));
            IsVirtCdRom(vinfo);
        }

        [Test]
        public void DriveEVS()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win7Sim, @"\\?\Volume{64314999-9ae3-11e6-b310-0021865a69b8}\");
            Assert.That(vinfo.Path, Is.EqualTo(EVS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(EVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(EV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(ED));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(E));
            IsVirtCdRom(vinfo);
        }

        private void IsVirtCdRom(VolumeDeviceInfo vinfo)
        {
            Assert.That(vinfo.Volume.DriveType, Is.EqualTo(DriveType.CdRom));
            Assert.That(vinfo.Disk.Extents, Is.Null);
            Assert.That(vinfo.Disk.Device.VendorId, Is.EqualTo("ELBY    "));
            Assert.That(vinfo.Disk.Device.ProductId, Is.EqualTo("CLONEDRIVE      "));
            Assert.That(vinfo.Disk.Device.ProductRevision, Is.EqualTo("1.4 "));
            Assert.That(vinfo.Disk.Device.SerialNumber, Is.EqualTo("\r("));
            Assert.That(vinfo.Disk.Device.BusType, Is.EqualTo(BusType.Scsi));
            Assert.That(vinfo.Disk.Device.HasCommandQueueing, Is.False);
            Assert.That(vinfo.Disk.Device.ScsiDeviceType, Is.EqualTo(ScsiDeviceType.CdRomDevice));
            Assert.That(vinfo.Disk.Device.ScsiDeviceModifier, Is.EqualTo(0));
            Assert.That(vinfo.Disk.Device.GuidFlags, Is.EqualTo(DeviceGuidFlags.None));
            Assert.That(vinfo.Disk.Device.Guid, Is.EqualTo(Guid.Empty));
            Assert.That(vinfo.Disk.Device.DeviceType, Is.EqualTo(DeviceType.CdRom));
            Assert.That(vinfo.Disk.Device.DeviceNumber, Is.EqualTo(1));
            Assert.That(vinfo.Disk.IsRemovableMedia, Is.True);
            Assert.That(vinfo.Disk.IsMediaPresent, Is.False);
            Assert.That(vinfo.Disk.IsReadOnly, Is.True);
            Assert.That(vinfo.Disk.MediaType, Is.EqualTo(MediaType.Unknown));
            Assert.That(vinfo.Disk.Geometry, Is.Null);
            Assert.That(vinfo.Disk.HasSeekPenalty, Is.EqualTo(BoolUnknown.Unknown));
            Assert.That(vinfo.Partition, Is.Null);
            Assert.That(vinfo.FileSystem, Is.Null);
        }

        [Test]
        public void PhysicalDrive1()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win7Sim, @"\\.\PhysicalDrive1");
            Assert.That(vinfo.Path, Is.EqualTo(Phys1));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Phys1S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(Phys1));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);     // Physical drive
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);       // Physical drive
            Assert.That(vinfo.Volume.DriveType, Is.EqualTo(DriveType.Fixed));
            IsDrivePhys1(vinfo);
        }

        [Test]
        public void PhysicalDrive1S()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win7Sim, @"\\.\PhysicalDrive1\");
            Assert.That(vinfo.Path, Is.EqualTo(Phys1S));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Phys1S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(Phys1));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);     // Physical drive
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);       // Physical drive
            Assert.That(vinfo.Volume.DriveType, Is.EqualTo(DriveType.Fixed));
            IsDrivePhys1(vinfo);
        }

        private void IsDrivePhys1(VolumeDeviceInfo vinfo)
        {
            IsPhysicalDrive1(vinfo);
            Assert.That(vinfo.Disk.Extents, Is.Null);
            Assert.That(vinfo.Partition.Style, Is.EqualTo(PartitionStyle.MasterBootRecord));
            Assert.That(vinfo.Partition.Number, Is.EqualTo(0));
            Assert.That(vinfo.Partition.Offset, Is.EqualTo(0));
            Assert.That(vinfo.Partition.Length, Is.EqualTo(320072933376));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Type, Is.EqualTo(0));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Bootable, Is.False);
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).MbrSectorsOffset, Is.EqualTo(0));
            Assert.That(vinfo.FileSystem, Is.Null);
        }

        public void IsPhysicalDrive1(VolumeDeviceInfo vinfo)
        {
            Assert.That(vinfo.Disk.Device.VendorId, Is.EqualTo("WD      "));
            Assert.That(vinfo.Disk.Device.ProductId, Is.EqualTo("3200AAJ External"));
            Assert.That(vinfo.Disk.Device.ProductRevision, Is.EqualTo("1.06"));
            Assert.That(vinfo.Disk.Device.SerialNumber, Is.EqualTo("E"));
            Assert.That(vinfo.Disk.Device.BusType, Is.EqualTo(BusType.Usb));
            Assert.That(vinfo.Disk.Device.HasCommandQueueing, Is.False);
            Assert.That(vinfo.Disk.Device.ScsiDeviceType, Is.EqualTo(ScsiDeviceType.DirectAccessDevice));
            Assert.That(vinfo.Disk.Device.ScsiDeviceModifier, Is.EqualTo(0));
            Assert.That(vinfo.Disk.Device.GuidFlags, Is.EqualTo(DeviceGuidFlags.None));
            Assert.That(vinfo.Disk.Device.Guid, Is.EqualTo(Guid.Empty));
            Assert.That(vinfo.Disk.Device.DeviceType, Is.EqualTo(DeviceType.Disk));
            Assert.That(vinfo.Disk.Device.DeviceNumber, Is.EqualTo(1));
            Assert.That(vinfo.Disk.IsRemovableMedia, Is.False);
            Assert.That(vinfo.Disk.IsMediaPresent, Is.True);
            Assert.That(vinfo.Disk.IsReadOnly, Is.False);
            Assert.That(vinfo.Disk.MediaType, Is.EqualTo(MediaType.FixedMedia));
            Assert.That(vinfo.Disk.Geometry.Cylinders, Is.EqualTo(38913));
            Assert.That(vinfo.Disk.Geometry.TracksPerCylinder, Is.EqualTo(255));
            Assert.That(vinfo.Disk.Geometry.SectorsPerTrack, Is.EqualTo(63));
            Assert.That(vinfo.Disk.Geometry.BytesPerSector, Is.EqualTo(512));
            Assert.That(vinfo.Disk.Geometry.BytesPerPhysicalSector, Is.EqualTo(512));
            Assert.That(vinfo.Disk.HasSeekPenalty, Is.EqualTo(BoolUnknown.Unknown));
        }

        [Test]
        public void DriveF()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win7Sim, @"F:");
            Assert.That(vinfo.Path, Is.EqualTo(F));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(FS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(FV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(FD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(F));
            Assert.That(vinfo.Volume.DriveType, Is.EqualTo(DriveType.Fixed));
            IsExtDrive(vinfo);
        }

        [Test]
        public void DriveFS()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win7Sim, @"F:\");
            Assert.That(vinfo.Path, Is.EqualTo(FS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(FS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(FV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(FD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(F));
            Assert.That(vinfo.Volume.DriveType, Is.EqualTo(DriveType.Fixed));
            IsExtDrive(vinfo);
        }

        [Test]
        public void DriveFV()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win7Sim, @"\\?\Volume{455b7dcd-e19f-11e6-b6bc-0021865a69b8}");
            Assert.That(vinfo.Path, Is.EqualTo(FV));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(FVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(FV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(FD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(F));
            Assert.That(vinfo.Volume.DriveType, Is.EqualTo(DriveType.Fixed));
            IsExtDrive(vinfo);
        }

        [Test]
        public void DriveFVS()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win7Sim, @"\\?\Volume{455b7dcd-e19f-11e6-b6bc-0021865a69b8}\");
            Assert.That(vinfo.Path, Is.EqualTo(FVS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(FVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(FV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(FD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(F));
            Assert.That(vinfo.Volume.DriveType, Is.EqualTo(DriveType.Fixed));
            IsExtDrive(vinfo);
        }

        private void IsExtDrive(VolumeDeviceInfo vinfo)
        {
            IsPhysicalDrive1(vinfo);
            Assert.That(vinfo.Disk.Extents.Length, Is.EqualTo(1));
            Assert.That(vinfo.Disk.Extents[0].Device, Is.EqualTo(@"\\.\PhysicalDrive1"));
            Assert.That(vinfo.Disk.Extents[0].StartingOffset, Is.EqualTo(vinfo.Partition.Offset));
            Assert.That(vinfo.Disk.Extents[0].ExtentLength, Is.EqualTo(vinfo.Partition.Length));
            Assert.That(vinfo.Partition.Style, Is.EqualTo(PartitionStyle.MasterBootRecord));
            Assert.That(vinfo.Partition.Number, Is.EqualTo(1));
            Assert.That(vinfo.Partition.Offset, Is.EqualTo(1048576));
            Assert.That(vinfo.Partition.Length, Is.EqualTo(320070483968));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Type, Is.EqualTo(7));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Bootable, Is.False);
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).MbrSectorsOffset, Is.EqualTo(2048));
            Assert.That(vinfo.FileSystem.Label, Is.EqualTo("WD320"));
            Assert.That(vinfo.FileSystem.Serial, Is.EqualTo("6C45-9ED2"));
            Assert.That(vinfo.FileSystem.Name, Is.EqualTo("NTFS"));
            Assert.That((int)vinfo.FileSystem.Flags, Is.EqualTo(0x03E700FF));
            Assert.That(vinfo.FileSystem.BytesPerSector, Is.EqualTo(512));
            Assert.That(vinfo.FileSystem.SectorsPerCluster, Is.EqualTo(8));
            Assert.That(vinfo.FileSystem.TotalBytes, Is.EqualTo(320070479872));
            Assert.That(vinfo.FileSystem.TotalFreeBytes, Is.EqualTo(292682526720));
            Assert.That(vinfo.FileSystem.UserFreeBytes, Is.EqualTo(292682526720));
        }

        [Test]
        public void HarddiskVolume5()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win7Sim, @"\\.\HarddiskVolume5");
            Assert.That(vinfo.Path, Is.EqualTo(Vol5));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Vol5S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(FV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(FD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(F));
            Assert.That(vinfo.Volume.DriveType, Is.EqualTo(DriveType.Fixed));
            IsExtDrive(vinfo);
        }

        [Test]
        public void HarddiskVolume5S()
        {
            VolumeDeviceInfo vinfo = new Win32.VolumeDeviceInfoWin32Sim(Win7Sim, @"\\.\HarddiskVolume5\");
            Assert.That(vinfo.Path, Is.EqualTo(Vol5S));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Vol5S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(FV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(FD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(F));
            Assert.That(vinfo.Volume.DriveType, Is.EqualTo(DriveType.Fixed));
            IsExtDrive(vinfo);
        }
    }
}
