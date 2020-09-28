namespace VolumeInfo.IO.Storage
{
    using System;
    using System.IO;
    using NUnit.Framework;

    [TestFixture]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Correct case in the circumstances")]
    public class VolumeDeviceInfoTest_Win10v2004
    {
        private static readonly string Win10Sim = Path.Combine(TestContext.CurrentContext.TestDirectory, "Test", "Win32", "VolumeInfoTest.Win10v2004.xml");
        private static readonly string Win10SimNoMedia = Path.Combine(TestContext.CurrentContext.TestDirectory, "Test", "Win32", "VolumeInfoTest.Win10v2004.MediaNotPresent.xml");

        // The Physical Drive containing C: and the hidden partition
        private const string Phys0 = @"\\.\PhysicalDrive0";
        private const string Phys0S = @"\\.\PhysicalDrive0\";

        // The Physical Drive containing D:
        private const string Phys1 = @"\\.\PhysicalDrive1";
        private const string Phys1S = @"\\.\PhysicalDrive1\";

        // The Physical Drive containing E:
        private const string Phys2 = @"\\.\PhysicalDrive2";
        private const string Phys2S = @"\\.\PhysicalDrive2\";

        // The Physical Drive containing H:, which isn't mounted and has no media
        private const string Phys3 = @"\\.\PhysicalDrive3";
        private const string Phys3S = @"\\.\PhysicalDrive3\";

        // A hidden partition installed by Win10
        private const string Vol1 = @"\\.\HarddiskVolume1";
        private const string Vol1S = @"\\.\HarddiskVolume1\";
        private const string Vol1V = @"\\?\Volume{49c071ff-6931-498b-afad-d095ee3500d2}";

        // A 3.5" USB Floppy with disk present
        private const string A = @"A:";
        private const string AS = @"A:\";
        private const string AV = @"\\?\Volume{f35b67a3-fda7-11ea-b203-7085c2221e14}";
        private const string AVS = @"\\?\Volume{f35b67a3-fda7-11ea-b203-7085c2221e14}\";
        private const string AD = @"\Device\Floppy0";

        // The boot partition
        private const string C = @"C:";
        private const string CS = @"C:\";
        private const string CV = @"\\?\Volume{4a2248f3-ec20-4c41-b781-ff19fa7913e6}";
        private const string CVS = @"\\?\Volume{4a2248f3-ec20-4c41-b781-ff19fa7913e6}\";
        private const string CD = @"\Device\HarddiskVolume3";

        // An integrated SD-Card
        private const string D = @"D:";
        private const string DS = @"D:\";
        private const string DV = @"\\?\Volume{d5fcf8f3-1d12-11ea-913c-806e6f6e6963}";
        private const string DVS = @"\\?\Volume{d5fcf8f3-1d12-11ea-913c-806e6f6e6963}\";
        private const string DD = @"\Device\HarddiskVolume9";

        // A USB stick
        //  junction E:\efolder1\windows C:\windows
        //  junction E:\efolder1\calculus O:\Calculus  (junction to a sub dir on a SUBST'd drive)
        private const string E = @"E:";
        private const string ES = @"E:\";
        private const string EV = @"\\?\Volume{299b5220-f500-11ea-9168-985fd3d32a6a}";
        private const string EVS = @"\\?\Volume{299b5220-f500-11ea-9168-985fd3d32a6a}\";
        private const string ED = @"\Device\HarddiskVolume21";

        // A USB stick
        //  junction F:\ffolder1\efolder1 E:\efolder1
        private const string F = @"F:";
        private const string FS = @"F:\";
        private const string FV = @"\\?\Volume{58184c44-f6bc-11ea-9169-985fd3d32a6a}";
        private const string FD = @"\Device\HarddiskVolume22";

        // A Card Reader H:
        private const string H = @"H:";
        private const string HS = @"H:\";
        private const string HV = @"\\?\Volume{71045c21-add5-11e7-b180-806e6f6e6963}";
        private const string HVS = @"\\?\Volume{71045c21-add5-11e7-b180-806e6f6e6963}\";
        private const string HD = @"\Device\HarddiskVolume6";

        // A network share
        private const string M = @"M:";
        private const string MS = @"M:\";
        private const string MD = @"\Device\LanmanRedirector\;M:00000000001fec6c\localhost\share";

        // SUBST N: C:\
        private const string N = @"N:";
        private const string NS = @"N:\";
        private const string ND = @"\??\C:";

        // SUBST O: D:\books
        private const string O = @"O:";
        private const string OS = @"O:\";
        private const string OD = @"\??\D:\books";

        // SUBST P: N:\
        private const string P = @"P:";
        private const string PS = @"P:\";
        private const string PD = @"\??\N:";

        // ImDisk RAMDISK R:
        private const string R = @"R:";
        private const string RS = @"R:\";
        private const string RV = @"\\.\GLOBALROOT\Device\ImDisk0";
        private const string RVS = @"\\.\GLOBALROOT\Device\ImDisk0\";
        private const string RD = @"\Device\ImDisk0";

        [Test]
        public void DriveA()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"A:");
            Assert.That(vinfo.Path, Is.EqualTo(A));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(AS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(AV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(AD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(A));
            IsDriveFloppyA(vinfo);
        }

        [Test]
        public void DriveAS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"A:\");
            Assert.That(vinfo.Path, Is.EqualTo(AS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(AS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(AV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(AD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(A));
            IsDriveFloppyA(vinfo);
        }

        [Test]
        public void DriveAV()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"\\?\Volume{f35b67a3-fda7-11ea-b203-7085c2221e14}");
            Assert.That(vinfo.Path, Is.EqualTo(AV));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(AVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(AV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(AD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(A));
            IsDriveFloppyA(vinfo);
        }

        [Test]
        public void DriveAVS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"\\?\Volume{f35b67a3-fda7-11ea-b203-7085c2221e14}\");
            Assert.That(vinfo.Path, Is.EqualTo(AVS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(AVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(AV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(AD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(A));
            IsDriveFloppyA(vinfo);
        }

        private void IsDriveFloppyTeac(VolumeDeviceInfo vinfo)
        {
            Assert.That(vinfo.Disk.VendorId, Is.EqualTo("TEAC    "));
            Assert.That(vinfo.Disk.ProductId, Is.EqualTo("FD-05PUB        "));
            Assert.That(vinfo.Disk.ProductRevision, Is.EqualTo("3000"));
            Assert.That(vinfo.Disk.SerialNumber, Is.Empty);
            Assert.That(vinfo.Disk.BusType, Is.EqualTo(BusType.Usb));
            Assert.That(vinfo.Disk.IsRemovableMedia, Is.True);
            Assert.That(vinfo.Disk.HasCommandQueueing, Is.False);
            Assert.That(vinfo.Disk.ScsiDeviceType, Is.EqualTo(ScsiDeviceType.DirectAccessDevice));
            Assert.That(vinfo.Disk.ScsiDeviceModifier, Is.EqualTo(0));
            Assert.That(vinfo.Disk.GuidFlags, Is.EqualTo(DeviceGuidFlags.RandomDeviceGuidReasonNoHwId));
            Assert.That(vinfo.Disk.Guid.ToString(), Is.EqualTo("f35b67a2-fda7-11ea-b203-7085c2221e14"));
            Assert.That(vinfo.Disk.DeviceType, Is.EqualTo(DeviceType.Disk));
            Assert.That(vinfo.Disk.DeviceNumber, Is.EqualTo(0));
        }

        private void IsDriveFloppyA(VolumeDeviceInfo vinfo)
        {
            IsDriveFloppyTeac(vinfo);
            Assert.That(vinfo.Disk.IsMediaPresent, Is.True);
            Assert.That(vinfo.Disk.MediaType, Is.EqualTo(MediaType.F3_1Pt44_512));
            Assert.That(vinfo.Disk.Geometry.Cylinders, Is.EqualTo(80));
            Assert.That(vinfo.Disk.Geometry.TracksPerCylinder, Is.EqualTo(2));
            Assert.That(vinfo.Disk.Geometry.SectorsPerTrack, Is.EqualTo(18));
            Assert.That(vinfo.Disk.Geometry.BytesPerSector, Is.EqualTo(512));
            Assert.That(vinfo.Disk.Geometry.BytesPerPhysicalSector, Is.EqualTo(vinfo.Disk.Geometry.BytesPerSector));
            Assert.That(vinfo.Disk.HasSeekPenalty, Is.EqualTo(BoolUnknown.Unknown));
            Assert.That(vinfo.Partition, Is.Null);
            Assert.That(vinfo.FileSystem.Label, Is.Empty);
            Assert.That(vinfo.FileSystem.Serial, Is.EqualTo("0D18-1AEE"));
            Assert.That(vinfo.FileSystem.Name, Is.EqualTo("FAT"));
            Assert.That((int)vinfo.FileSystem.Flags, Is.EqualTo(0x80206));
            Assert.That(vinfo.FileSystem.BytesPerSector, Is.EqualTo(512));
            Assert.That(vinfo.FileSystem.SectorsPerCluster, Is.EqualTo(1));
            Assert.That(vinfo.FileSystem.TotalBytes, Is.EqualTo(1457664));
            Assert.That(vinfo.FileSystem.TotalFreeBytes, Is.EqualTo(120832));
            Assert.That(vinfo.FileSystem.UserFreeBytes, Is.EqualTo(120832));
        }

        [Test]
        public void DriveANoFloppy()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10SimNoMedia, @"A:");
            Assert.That(vinfo.Path, Is.EqualTo(A));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(AS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(AV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(AD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(A));
            IsDriveFloppyANoFloppy(vinfo);
        }

        [Test]
        public void DriveASNoFloppy()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10SimNoMedia, @"A:\");
            Assert.That(vinfo.Path, Is.EqualTo(AS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(AS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(AV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(AD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(A));
            IsDriveFloppyANoFloppy(vinfo);
        }

        [Test]
        public void DriveAVNoFloppy()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10SimNoMedia, @"\\?\Volume{f35b67a3-fda7-11ea-b203-7085c2221e14}");
            Assert.That(vinfo.Path, Is.EqualTo(AV));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(AVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(AV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(AD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(A));
            IsDriveFloppyANoFloppy(vinfo);
        }

        [Test]
        public void DriveAVSNoFloppy()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10SimNoMedia, @"\\?\Volume{f35b67a3-fda7-11ea-b203-7085c2221e14}\");
            Assert.That(vinfo.Path, Is.EqualTo(AVS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(AVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(AV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(AD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(A));
            IsDriveFloppyANoFloppy(vinfo);
        }

        private void IsDriveFloppyANoFloppy(VolumeDeviceInfo vinfo)
        {
            IsDriveFloppyTeac(vinfo);
            Assert.That(vinfo.Disk.IsMediaPresent, Is.False);
            Assert.That(vinfo.Disk.Geometry, Is.Null);
            Assert.That(vinfo.Disk.HasSeekPenalty, Is.EqualTo(BoolUnknown.Unknown));
            Assert.That(vinfo.Partition, Is.Null);
            Assert.That(vinfo.FileSystem, Is.Null);
        }

        [Test]
        public void PhysicalDrive0()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"\\.\PhysicalDrive0");
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
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"\\.\PhysicalDrive0\");
            Assert.That(vinfo.Path, Is.EqualTo(Phys0S));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Phys0S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(Phys0));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);     // Not a volume
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);       // Not a volume
            IsDrivePhys0(vinfo);
        }

        private void IsDrivePhys0(VolumeDeviceInfo vinfo)
        {
            IsPhysicalDrive0(vinfo);
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

        private void IsPhysicalDrive0(VolumeDeviceInfo vinfo)
        {
            Assert.That(vinfo.Disk.VendorId, Is.Empty);
            Assert.That(vinfo.Disk.ProductId, Is.EqualTo("SAMSUNG MZFLV512HCJH-000MV"));
            Assert.That(vinfo.Disk.ProductRevision, Is.EqualTo("BXV75M0Q"));
            Assert.That(vinfo.Disk.SerialNumber, Is.EqualTo("0025_3844_61B5_6586."));
            Assert.That(vinfo.Disk.BusType, Is.EqualTo(BusType.Nvme));
            Assert.That(vinfo.Disk.IsRemovableMedia, Is.False);
            Assert.That(vinfo.Disk.HasCommandQueueing, Is.True);
            Assert.That(vinfo.Disk.ScsiDeviceType, Is.EqualTo(ScsiDeviceType.DirectAccessDevice));
            Assert.That(vinfo.Disk.ScsiDeviceModifier, Is.EqualTo(0));
            Assert.That(vinfo.Disk.IsMediaPresent, Is.True);
            Assert.That(vinfo.Disk.IsReadOnly, Is.False);
            Assert.That(vinfo.Disk.GuidFlags, Is.EqualTo(DeviceGuidFlags.Page83DeviceGuid));
            Assert.That(vinfo.Disk.Guid.ToString(), Is.EqualTo("ba408eac-457b-e82e-f5ea-f5764f6a8c94"));
            Assert.That(vinfo.Disk.DeviceType, Is.EqualTo(DeviceType.Disk));
            Assert.That(vinfo.Disk.DeviceNumber, Is.EqualTo(0));
            Assert.That(vinfo.Disk.MediaType, Is.EqualTo(MediaType.FixedMedia));
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
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"C:");
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
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"C:\");
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
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"\\?\Volume{4a2248f3-ec20-4c41-b781-ff19fa7913e6}");
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
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"\\?\Volume{4a2248f3-ec20-4c41-b781-ff19fa7913e6}\");
            Assert.That(vinfo.Path, Is.EqualTo(CVS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(CVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(C));
            IsDriveSamsung(vinfo);
        }

        private void IsDriveSamsung(VolumeDeviceInfo vinfo)
        {
            IsPhysicalDrive0(vinfo);
            Assert.That(vinfo.Partition.Style, Is.EqualTo(PartitionStyle.GuidPartitionTable));
            Assert.That(vinfo.Partition.Number, Is.EqualTo(3));
            Assert.That(vinfo.Partition.Offset, Is.EqualTo(407896064));
            Assert.That(vinfo.Partition.Length, Is.EqualTo(510653366272));
            Assert.That(((VolumeDeviceInfo.IGptPartition)vinfo.Partition).Type.ToString(), Is.EqualTo("ebd0a0a2-b9e5-4433-87c0-68b6b72699c7"));
            Assert.That(((VolumeDeviceInfo.IGptPartition)vinfo.Partition).Id.ToString(), Is.EqualTo("4a2248f3-ec20-4c41-b781-ff19fa7913e6"));
            Assert.That(((VolumeDeviceInfo.IGptPartition)vinfo.Partition).Name, Is.EqualTo("Basic data partition"));
            Assert.That((int)((VolumeDeviceInfo.IGptPartition)vinfo.Partition).Attributes, Is.EqualTo(0));
            Assert.That(vinfo.FileSystem.Label, Is.EqualTo("Local Disk"));
            Assert.That(vinfo.FileSystem.Serial, Is.EqualTo("D470-C5ED"));
            Assert.That(vinfo.FileSystem.Name, Is.EqualTo("NTFS"));
            Assert.That((int)vinfo.FileSystem.Flags, Is.EqualTo(0x03E706FF));
            Assert.That(vinfo.FileSystem.BytesPerSector, Is.EqualTo(512));
            Assert.That(vinfo.FileSystem.SectorsPerCluster, Is.EqualTo(8));
            Assert.That(vinfo.FileSystem.TotalBytes, Is.EqualTo(510653362176));
            Assert.That(vinfo.FileSystem.TotalFreeBytes, Is.EqualTo(363201818624));
            Assert.That(vinfo.FileSystem.UserFreeBytes, Is.EqualTo(363201818624));
        }

        [Test]
        public void HarddiskVolume1()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"\\.\HarddiskVolume1");
            Assert.That(vinfo.Path, Is.EqualTo(Vol1));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Vol1S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(Vol1V));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);     // Not mounted
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);       // Not mounted
            IsDriveSamsungHiddenPart(vinfo);
        }

        [Test]
        public void HarddiskVolume1S()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"\\.\HarddiskVolume1\");
            Assert.That(vinfo.Path, Is.EqualTo(Vol1S));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Vol1S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(Vol1V));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);     // Not mounted
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);       // Not mounted
            IsDriveSamsungHiddenPart(vinfo);
        }

        private void IsDriveSamsungHiddenPart(VolumeDeviceInfo vinfo)
        {
            IsPhysicalDrive0(vinfo);
            Assert.That(vinfo.Partition.Style, Is.EqualTo(PartitionStyle.GuidPartitionTable));
            Assert.That(vinfo.Partition.Number, Is.EqualTo(1));
            Assert.That(vinfo.Partition.Offset, Is.EqualTo(1048576));
            Assert.That(vinfo.Partition.Length, Is.EqualTo(272629760));
            Assert.That(((VolumeDeviceInfo.IGptPartition)vinfo.Partition).Type.ToString(), Is.EqualTo("c12a7328-f81f-11d2-ba4b-00a0c93ec93b"));
            Assert.That(((VolumeDeviceInfo.IGptPartition)vinfo.Partition).Id.ToString(), Is.EqualTo("49c071ff-6931-498b-afad-d095ee3500d2"));
            Assert.That(((VolumeDeviceInfo.IGptPartition)vinfo.Partition).Name, Is.EqualTo("EFI system partition"));
            Assert.That(((VolumeDeviceInfo.IGptPartition)vinfo.Partition).Attributes, Is.EqualTo(EFIPartitionAttributes.GptBasicDataAttributeNoDriveLetter));
            Assert.That(vinfo.FileSystem.Label, Is.EqualTo("SYSTEM"));
            Assert.That(vinfo.FileSystem.Serial, Is.EqualTo("CC1F-1834"));
            Assert.That(vinfo.FileSystem.Name, Is.EqualTo("FAT32"));
            Assert.That((int)vinfo.FileSystem.Flags, Is.EqualTo(0x20206));
            Assert.That(vinfo.FileSystem.BytesPerSector, Is.EqualTo(512));
            Assert.That(vinfo.FileSystem.SectorsPerCluster, Is.EqualTo(8));
            Assert.That(vinfo.FileSystem.TotalBytes, Is.EqualTo(268435456));
            Assert.That(vinfo.FileSystem.TotalFreeBytes, Is.EqualTo(240005120));
            Assert.That(vinfo.FileSystem.UserFreeBytes, Is.EqualTo(240005120));
        }

        [Test]
        public void PhysicalDrive1()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"\\.\PhysicalDrive1");
            Assert.That(vinfo.Path, Is.EqualTo(Phys1));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Phys1S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(Phys1));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);     // Not a volume
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);       // Not a volume
            IsDrivePhys1(vinfo);
        }

        [Test]
        public void PhysicalDrive1S()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"\\.\PhysicalDrive1\");
            Assert.That(vinfo.Path, Is.EqualTo(Phys1S));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Phys1S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(Phys1));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);     // Not a volume
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);       // Not a volume
            IsDrivePhys1(vinfo);
        }

        private void IsDrivePhys1(VolumeDeviceInfo vinfo)
        {
            IsPhysicalDrive1(vinfo);
            Assert.That(vinfo.Partition.Style, Is.EqualTo(PartitionStyle.MasterBootRecord));
            Assert.That(vinfo.Partition.Number, Is.EqualTo(0));
            Assert.That(vinfo.Partition.Offset, Is.EqualTo(0));
            Assert.That(vinfo.Partition.Length, Is.EqualTo(64021856256));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Type, Is.EqualTo(0));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Bootable, Is.False);
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).MbrSectorsOffset, Is.EqualTo(0));
            Assert.That(vinfo.FileSystem, Is.Null);
        }

        private void IsPhysicalDrive1(VolumeDeviceInfo vinfo)
        {
            Assert.That(vinfo.Disk.VendorId, Is.EqualTo("Generic-"));
            Assert.That(vinfo.Disk.ProductId, Is.EqualTo("SD Card         "));
            Assert.That(vinfo.Disk.ProductRevision, Is.EqualTo("1.00"));
            Assert.That(vinfo.Disk.SerialNumber, Is.Empty);
            Assert.That(vinfo.Disk.BusType, Is.EqualTo(BusType.Usb));
            Assert.That(vinfo.Disk.IsRemovableMedia, Is.True);
            Assert.That(vinfo.Disk.HasCommandQueueing, Is.False);
            Assert.That(vinfo.Disk.ScsiDeviceType, Is.EqualTo(ScsiDeviceType.DirectAccessDevice));
            Assert.That(vinfo.Disk.ScsiDeviceModifier, Is.EqualTo(13));
            Assert.That(vinfo.Disk.IsMediaPresent, Is.True);
            Assert.That(vinfo.Disk.IsReadOnly, Is.False);
            Assert.That(vinfo.Disk.GuidFlags, Is.EqualTo(DeviceGuidFlags.RandomDeviceGuidReasonNoHwId));
            Assert.That(vinfo.Disk.Guid.ToString(), Is.EqualTo("537baabc-f7f9-11ea-9169-985fd3d32a6a"));
            Assert.That(vinfo.Disk.DeviceType, Is.EqualTo(DeviceType.Disk));
            Assert.That(vinfo.Disk.DeviceNumber, Is.EqualTo(1));
            Assert.That(vinfo.Disk.MediaType, Is.EqualTo(MediaType.RemovableMedia));
            Assert.That(vinfo.Disk.Geometry.Cylinders, Is.EqualTo(7783));
            Assert.That(vinfo.Disk.Geometry.TracksPerCylinder, Is.EqualTo(255));
            Assert.That(vinfo.Disk.Geometry.SectorsPerTrack, Is.EqualTo(63));
            Assert.That(vinfo.Disk.Geometry.BytesPerSector, Is.EqualTo(512));
            Assert.That(vinfo.Disk.Geometry.BytesPerPhysicalSector, Is.EqualTo(512));
            Assert.That(vinfo.Disk.HasSeekPenalty, Is.EqualTo(BoolUnknown.Unknown));
        }

        [Test]
        public void DriveD()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"D:");
            Assert.That(vinfo.Path, Is.EqualTo(D));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(DS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(DD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(D));
            IsDriveSdCard(vinfo);
        }

        [Test]
        public void DriveDS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"D:\");
            Assert.That(vinfo.Path, Is.EqualTo(DS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(DS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(DD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(D));
            IsDriveSdCard(vinfo);
        }

        [Test]
        public void DriveDV()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"\\?\Volume{d5fcf8f3-1d12-11ea-913c-806e6f6e6963}");
            Assert.That(vinfo.Path, Is.EqualTo(DV));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(DVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(DD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(D));
            IsDriveSdCard(vinfo);
        }

        [Test]
        public void DriveDVS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"\\?\Volume{d5fcf8f3-1d12-11ea-913c-806e6f6e6963}\");
            Assert.That(vinfo.Path, Is.EqualTo(DVS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(DVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(DD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(D));
            IsDriveSdCard(vinfo);
        }

        private void IsDriveSdCard(VolumeDeviceInfo vinfo)
        {
            IsPhysicalDrive1(vinfo);
            Assert.That(vinfo.Partition.Style, Is.EqualTo(PartitionStyle.MasterBootRecord));
            Assert.That(vinfo.Partition.Number, Is.EqualTo(1));
            Assert.That(vinfo.Partition.Offset, Is.EqualTo(16777216));
            Assert.That(vinfo.Partition.Length, Is.EqualTo(64005079040));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Type, Is.EqualTo(7));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Bootable, Is.False);
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).MbrSectorsOffset, Is.EqualTo(32768));
            Assert.That(vinfo.FileSystem.Label, Is.EqualTo("Samsung64GB"));
            Assert.That(vinfo.FileSystem.Serial, Is.EqualTo("0000-0000"));
            Assert.That(vinfo.FileSystem.Name, Is.EqualTo("exFAT"));
            Assert.That((int)vinfo.FileSystem.Flags, Is.EqualTo(0x00020206));
            Assert.That(vinfo.FileSystem.BytesPerSector, Is.EqualTo(512));
            Assert.That(vinfo.FileSystem.SectorsPerCluster, Is.EqualTo(256));
            Assert.That(vinfo.FileSystem.TotalBytes, Is.EqualTo(63988301824));
            Assert.That(vinfo.FileSystem.TotalFreeBytes, Is.EqualTo(37841797120));
            Assert.That(vinfo.FileSystem.UserFreeBytes, Is.EqualTo(37841797120));
        }

        [Test]
        public void PhysicalDrive2()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"\\.\PhysicalDrive2");
            Assert.That(vinfo.Path, Is.EqualTo(Phys2));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Phys2S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(Phys2));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);     // Not a volume
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);       // Not a volume
            IsDrivePhys2(vinfo);
        }

        [Test]
        public void PhysicalDrive2S()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"\\.\PhysicalDrive2\");
            Assert.That(vinfo.Path, Is.EqualTo(Phys2S));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Phys2S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(Phys2));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);     // Not a volume
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);       // Not a volume
            IsDrivePhys2(vinfo);
        }

        private void IsDrivePhys2(VolumeDeviceInfo vinfo)
        {
            IsPhysicalDrive2(vinfo);
            Assert.That(vinfo.Partition.Style, Is.EqualTo(PartitionStyle.MasterBootRecord));
            Assert.That(vinfo.Partition.Number, Is.EqualTo(0));
            Assert.That(vinfo.Partition.Offset, Is.EqualTo(0));
            Assert.That(vinfo.Partition.Length, Is.EqualTo(63043141632));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Type, Is.EqualTo(0));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Bootable, Is.False);
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).MbrSectorsOffset, Is.EqualTo(0));
            Assert.That(vinfo.FileSystem, Is.Null);
        }

        private void IsPhysicalDrive2(VolumeDeviceInfo vinfo)
        {
            Assert.That(vinfo.Disk.VendorId, Is.EqualTo("SanDisk"));
            Assert.That(vinfo.Disk.ProductId, Is.EqualTo("Cruzer Glide 3.0"));
            Assert.That(vinfo.Disk.ProductRevision, Is.EqualTo("1.00"));
            Assert.That(vinfo.Disk.SerialNumber, Is.EqualTo("4C530001120606116282"));
            Assert.That(vinfo.Disk.BusType, Is.EqualTo(BusType.Usb));
            Assert.That(vinfo.Disk.IsRemovableMedia, Is.True);
            Assert.That(vinfo.Disk.HasCommandQueueing, Is.False);
            Assert.That(vinfo.Disk.ScsiDeviceType, Is.EqualTo(ScsiDeviceType.DirectAccessDevice));
            Assert.That(vinfo.Disk.ScsiDeviceModifier, Is.EqualTo(0));
            Assert.That(vinfo.Disk.IsMediaPresent, Is.True);
            Assert.That(vinfo.Disk.IsReadOnly, Is.False);
            Assert.That(vinfo.Disk.GuidFlags, Is.EqualTo(DeviceGuidFlags.None));
            Assert.That(vinfo.Disk.Guid.ToString(), Is.EqualTo("a183a5b5-d50a-86a9-188b-739eab126a73"));
            Assert.That(vinfo.Disk.DeviceType, Is.EqualTo(DeviceType.Disk));
            Assert.That(vinfo.Disk.DeviceNumber, Is.EqualTo(2));
            Assert.That(vinfo.Disk.MediaType, Is.EqualTo(MediaType.RemovableMedia));
            Assert.That(vinfo.Disk.Geometry.Cylinders, Is.EqualTo(7664));
            Assert.That(vinfo.Disk.Geometry.TracksPerCylinder, Is.EqualTo(255));
            Assert.That(vinfo.Disk.Geometry.SectorsPerTrack, Is.EqualTo(63));
            Assert.That(vinfo.Disk.Geometry.BytesPerSector, Is.EqualTo(512));
            Assert.That(vinfo.Disk.Geometry.BytesPerPhysicalSector, Is.EqualTo(512));
            Assert.That(vinfo.Disk.HasSeekPenalty, Is.EqualTo(BoolUnknown.Unknown));
        }

        [Test]
        public void DriveE()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"E:");
            Assert.That(vinfo.Path, Is.EqualTo(E));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(ES));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(EV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(ED));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(E));
            IsDriveGlide(vinfo);
        }

        [Test]
        public void DriveES()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"E:\");
            Assert.That(vinfo.Path, Is.EqualTo(ES));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(ES));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(EV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(ED));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(E));
            IsDriveGlide(vinfo);
        }

        [Test]
        public void DriveEV()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"\\?\Volume{299b5220-f500-11ea-9168-985fd3d32a6a}");
            Assert.That(vinfo.Path, Is.EqualTo(EV));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(EVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(EV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(ED));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(E));
            IsDriveGlide(vinfo);
        }

        [Test]
        public void DriveEVS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"\\?\Volume{299b5220-f500-11ea-9168-985fd3d32a6a}\");
            Assert.That(vinfo.Path, Is.EqualTo(EVS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(EVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(EV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(ED));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(E));
            IsDriveGlide(vinfo);
        }

        private void IsDriveGlide(VolumeDeviceInfo vinfo)
        {
            IsPhysicalDrive2(vinfo);
            Assert.That(vinfo.Partition.Style, Is.EqualTo(PartitionStyle.MasterBootRecord));
            Assert.That(vinfo.Partition.Number, Is.EqualTo(1));
            Assert.That(vinfo.Partition.Offset, Is.EqualTo(1048576));
            Assert.That(vinfo.Partition.Length, Is.EqualTo(63041437696));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Type, Is.EqualTo(7));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Bootable, Is.False);
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).MbrSectorsOffset, Is.EqualTo(2048));
            Assert.That(vinfo.FileSystem.Label, Is.EqualTo("USB"));
            Assert.That(vinfo.FileSystem.Serial, Is.EqualTo("36F8-F5F6"));
            Assert.That(vinfo.FileSystem.Name, Is.EqualTo("NTFS"));
            Assert.That((int)vinfo.FileSystem.Flags, Is.EqualTo(0x03E706FF));
            Assert.That(vinfo.FileSystem.BytesPerSector, Is.EqualTo(512));
            Assert.That(vinfo.FileSystem.SectorsPerCluster, Is.EqualTo(8));
            Assert.That(vinfo.FileSystem.TotalBytes, Is.EqualTo(63041433600));
            Assert.That(vinfo.FileSystem.TotalFreeBytes, Is.EqualTo(62945357824));
            Assert.That(vinfo.FileSystem.UserFreeBytes, Is.EqualTo(62945357824));
        }

        [Test]
        public void PhysicalDrive3()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10SimNoMedia, @"\\.\PhysicalDrive3");
            Assert.That(vinfo.Path, Is.EqualTo(Phys3));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Phys3S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(Phys3));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);     // Not a volume
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);       // Not a volume
            IsDriveHNoCard(vinfo);
        }

        [Test]
        public void PhysicalDrive3S()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10SimNoMedia, @"\\.\PhysicalDrive3\");
            Assert.That(vinfo.Path, Is.EqualTo(Phys3S));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(Phys3S));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(Phys3));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);     // Not a volume
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);       // Not a volume
            IsDriveHNoCard(vinfo);
        }

        private void IsDriveHNoCard(VolumeDeviceInfo vinfo)
        {
            Assert.That(vinfo.Disk.VendorId, Is.EqualTo("DELL    "));
            Assert.That(vinfo.Disk.ProductId, Is.EqualTo("USB   HS-CF Card"));
            Assert.That(vinfo.Disk.ProductRevision, Is.EqualTo("7.12"));
            Assert.That(vinfo.Disk.SerialNumber, Is.EqualTo("00000208A3E0"));
            Assert.That(vinfo.Disk.BusType, Is.EqualTo(BusType.Usb));
            Assert.That(vinfo.Disk.IsRemovableMedia, Is.True);
            Assert.That(vinfo.Disk.HasCommandQueueing, Is.False);
            Assert.That(vinfo.Disk.ScsiDeviceType, Is.EqualTo(ScsiDeviceType.DirectAccessDevice));
            Assert.That(vinfo.Disk.ScsiDeviceModifier, Is.EqualTo(0));
            Assert.That(vinfo.Disk.IsMediaPresent, Is.False);
            Assert.That(vinfo.Disk.GuidFlags, Is.EqualTo(DeviceGuidFlags.None));
            Assert.That(vinfo.Disk.Guid.ToString(), Is.EqualTo("ab937ae9-e419-2f45-32f5-fa67c2e6c520"));
            Assert.That(vinfo.Disk.DeviceType, Is.EqualTo(DeviceType.Disk));
            Assert.That(vinfo.Disk.DeviceNumber, Is.EqualTo(3));
            Assert.That(vinfo.Disk.MediaType, Is.EqualTo(MediaType.Unknown));
            Assert.That(vinfo.Disk.Geometry, Is.Null);
            Assert.That(vinfo.Disk.HasSeekPenalty, Is.EqualTo(BoolUnknown.Unknown));
            Assert.That(vinfo.FileSystem, Is.Null);
            Assert.That(vinfo.Partition, Is.Null);
        }

        [Test]
        public void DriveHNoCard()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10SimNoMedia, @"H:");
            Assert.That(vinfo.Path, Is.EqualTo(H));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(HS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(HV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(HD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(H));
            IsDriveHNoCard(vinfo);
        }

        [Test]
        public void DriveHSNoCard()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10SimNoMedia, @"H:\");
            Assert.That(vinfo.Path, Is.EqualTo(HS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(HS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(HV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(HD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(H));
            IsDriveHNoCard(vinfo);
        }

        [Test]
        public void DriveHVNoCard()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10SimNoMedia, @"\\?\Volume{71045c21-add5-11e7-b180-806e6f6e6963}");
            Assert.That(vinfo.Path, Is.EqualTo(HV));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(HVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(HV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(HD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(H));
            IsDriveHNoCard(vinfo);
        }

        [Test]
        public void DriveHVSNoCard()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10SimNoMedia, @"\\?\Volume{71045c21-add5-11e7-b180-806e6f6e6963}\");
            Assert.That(vinfo.Path, Is.EqualTo(HVS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(HVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(HV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(HD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(H));
            IsDriveHNoCard(vinfo);
        }

        [Test]
        public void NetM()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"M:");
            Assert.That(vinfo.Path, Is.EqualTo(M));
            Assert.That(vinfo.Volume.Path, Is.Empty);        // Not a local mount
            Assert.That(vinfo.Volume.DevicePath, Is.Empty);  // Not a local mount
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(MD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(M));
            Assert.That(vinfo.Partition, Is.Null);
            Assert.That(vinfo.FileSystem, Is.Null);
        }

        [Test]
        public void NetMS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"M:\");
            Assert.That(vinfo.Path, Is.EqualTo(MS));
            Assert.That(vinfo.Volume.Path, Is.Empty);        // Not a local mount
            Assert.That(vinfo.Volume.DevicePath, Is.Empty);  // Not a local mount
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(MD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(M));
            Assert.That(vinfo.Partition, Is.Null);
            Assert.That(vinfo.FileSystem, Is.Null);
        }

        [Test]
        public void SubstN()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"N:");
            Assert.That(vinfo.Path, Is.EqualTo(N));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(CS));  // Drive N: is mapped to C:
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(ND));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(N));
            IsDriveSamsung(vinfo);
        }

        [Test]
        public void SubstNS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"N:\");
            Assert.That(vinfo.Path, Is.EqualTo(NS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(CS));  // Drive N: is mapped to C:
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(ND));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(N));
            IsDriveSamsung(vinfo);
        }

        [Test]
        public void SubstO()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"O:");
            Assert.That(vinfo.Path, Is.EqualTo(O));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(DS));  // Drive O: is mapped to D:\books
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(OD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(O));
            IsDriveSdCard(vinfo);
        }

        [Test]
        public void SubstOS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"O:\");
            Assert.That(vinfo.Path, Is.EqualTo(OS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(DS));  // Drive O: is mapped to D:\books
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(OD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(O));
            IsDriveSdCard(vinfo);
        }

        [Test]
        public void SubstOFolder()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"O:\Calculus");
            Assert.That(vinfo.Path, Is.EqualTo(@"O:\Calculus"));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(DS));  // Drive O: is mapped to D:\books
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(OD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(O));
            IsDriveSdCard(vinfo);
        }

        [Test]
        public void SubstP()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"P:");
            Assert.That(vinfo.Path, Is.EqualTo(P));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(CS));  // Drive P: is mapped to N:\
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(PD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(P));
            IsDriveSamsung(vinfo);
        }

        [Test]
        public void SubstPS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"P:\");
            Assert.That(vinfo.Path, Is.EqualTo(PS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(CS));  // Drive P: is mapped to N:\
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(PD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(P));
            IsDriveSamsung(vinfo);
        }

        [Test]
        public void ImDiskR()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"R:");
            Assert.That(vinfo.Path, Is.EqualTo(R));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(RVS));  // GetVolumePathName, GetVolumeNameForVolumeMountPoint fail
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(RV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(RD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(R));
            IsImDiskRamDisk(vinfo);
        }

        [Test]
        public void ImDiskRS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"R:\");
            Assert.That(vinfo.Path, Is.EqualTo(RS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(RVS));  // GetVolumePathName, GetVolumeNameForVolumeMountPoint fail
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(RV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(RD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(R));
            IsImDiskRamDisk(vinfo);
        }

        [Test]
        public void ImDiskRV()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"\\.\GLOBALROOT\Device\ImDisk0");
            Assert.That(vinfo.Path, Is.EqualTo(RV));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(RVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(RV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);// GetVolumePathName, GetVolumeNameForVolumeMountPoint fail
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);        // GetVolumePathName, GetVolumeNameForVolumeMountPoint fail
            IsImDiskRamDisk(vinfo);
        }

        [Test]
        public void ImDiskRVS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"\\.\GLOBALROOT\Device\ImDisk0\");
            Assert.That(vinfo.Path, Is.EqualTo(RVS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(RVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(RV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.Empty);// GetVolumePathName, GetVolumeNameForVolumeMountPoint fail
            Assert.That(vinfo.Volume.DriveLetter, Is.Empty);        // GetVolumePathName, GetVolumeNameForVolumeMountPoint fail
            IsImDiskRamDisk(vinfo);
        }

        private void IsImDiskRamDisk(VolumeDeviceInfo vinfo)
        {
            Assert.That(vinfo.Disk.VendorId, Is.Empty);                                    // Unsupported
            Assert.That(vinfo.Disk.ProductId, Is.Empty);                                   // Unsupported
            Assert.That(vinfo.Disk.ProductRevision, Is.Empty);                             // Unsupported
            Assert.That(vinfo.Disk.SerialNumber, Is.Empty);                                // Unsupported
            Assert.That(vinfo.Disk.BusType, Is.EqualTo(BusType.Unknown));                  // Unsupported
            Assert.That(vinfo.Disk.IsRemovableMedia, Is.False);
            Assert.That(vinfo.Disk.HasCommandQueueing, Is.False);                          // Unsupported
            Assert.That(vinfo.Disk.ScsiDeviceType, Is.EqualTo(ScsiDeviceType.Unknown));    // Unsupported
            Assert.That(vinfo.Disk.ScsiDeviceModifier, Is.EqualTo(0));
            Assert.That(vinfo.Disk.IsMediaPresent, Is.True);
            Assert.That(vinfo.Disk.IsReadOnly, Is.False);
            Assert.That(vinfo.FileSystem.Label, Is.EqualTo("RAMDISK"));
            Assert.That(vinfo.FileSystem.Serial, Is.EqualTo("C858-F289"));
            Assert.That(vinfo.FileSystem.Name, Is.EqualTo("NTFS"));
            Assert.That((int)vinfo.FileSystem.Flags, Is.EqualTo(0x03E706FF));
            Assert.That(vinfo.FileSystem.BytesPerSector, Is.EqualTo(512));
            Assert.That(vinfo.FileSystem.SectorsPerCluster, Is.EqualTo(8));
            Assert.That(vinfo.FileSystem.TotalBytes, Is.EqualTo(536866816));
            Assert.That(vinfo.FileSystem.TotalFreeBytes, Is.EqualTo(519839744));
            Assert.That(vinfo.FileSystem.UserFreeBytes, Is.EqualTo(519839744));
            Assert.That(vinfo.Disk.GuidFlags, Is.EqualTo(DeviceGuidFlags.None));           // Unsupported
            Assert.That(vinfo.Disk.Guid, Is.EqualTo(Guid.Empty));                          // Unsupported
            Assert.That(vinfo.Disk.DeviceType, Is.EqualTo(DeviceType.Unknown));            // Unsupported
            Assert.That(vinfo.Disk.DeviceNumber, Is.EqualTo(-1));                          // Unsupported
            Assert.That(vinfo.Disk.MediaType, Is.EqualTo(MediaType.FixedMedia));
            Assert.That(vinfo.Disk.Geometry.Cylinders, Is.EqualTo(130));
            Assert.That(vinfo.Disk.Geometry.TracksPerCylinder, Is.EqualTo(128));
            Assert.That(vinfo.Disk.Geometry.SectorsPerTrack, Is.EqualTo(63));
            Assert.That(vinfo.Disk.Geometry.BytesPerSector, Is.EqualTo(512));
            Assert.That(vinfo.Disk.Geometry.BytesPerPhysicalSector, Is.EqualTo(512));      // Unsupported
            Assert.That(vinfo.Disk.HasSeekPenalty, Is.EqualTo(BoolUnknown.Unknown));       // Unsupported
            Assert.That(vinfo.Partition.Style, Is.EqualTo(PartitionStyle.MasterBootRecord));
            Assert.That(vinfo.Partition.Number, Is.EqualTo(1));
            Assert.That(vinfo.Partition.Offset, Is.EqualTo(32256));
            Assert.That(vinfo.Partition.Length, Is.EqualTo(536870912));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Type, Is.EqualTo(6));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Bootable, Is.False);
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).MbrSectorsOffset, Is.EqualTo(1));
            Assert.That(vinfo.FileSystem.BytesPerSector, Is.EqualTo(512));
            Assert.That(vinfo.FileSystem.SectorsPerCluster, Is.EqualTo(8));
            Assert.That(vinfo.FileSystem.TotalBytes, Is.EqualTo(536866816));
            Assert.That(vinfo.FileSystem.TotalFreeBytes, Is.EqualTo(519839744));
            Assert.That(vinfo.FileSystem.UserFreeBytes, Is.EqualTo(519839744));
        }

        [Test]
        public void DriveCFolder()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"C:\Users\user\Desktop");
            Assert.That(vinfo.Path, Is.EqualTo(@"C:\Users\user\Desktop"));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(CS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(C));
            IsDriveSamsung(vinfo);
        }

        [Test]
        public void JunctionFromFToEToC()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"F:\ffolder1\efolder1\windows");
            Assert.That(vinfo.Path, Is.EqualTo(@"F:\ffolder1\efolder1\windows"));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(CS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(C));
            IsDriveSamsung(vinfo);
        }

        [Test]
        public void JunctionFromFToE()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"F:\ffolder1\efolder1");
            Assert.That(vinfo.Path, Is.EqualTo(@"F:\ffolder1\efolder1"));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(ES));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(EV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(ED));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(E));
            IsDriveGlide(vinfo);
        }

        [Test]
        public void JunctionFromFToO()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"F:\ffolder1\efolder1\calculus");
            Assert.That(vinfo.Path, Is.EqualTo(@"F:\ffolder1\efolder1\calculus"));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(DS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(OD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(O));
            IsDriveSdCard(vinfo);
        }

        [Test]
        public void JunctionFromEToO()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"E:\efolder1\calculus");
            Assert.That(vinfo.Path, Is.EqualTo(@"E:\efolder1\calculus"));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(DS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(OD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(O));
            IsDriveSdCard(vinfo);
        }

        [Test]
        public void DriveFFolder()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"F:\ffolder1");
            Assert.That(vinfo.Path, Is.EqualTo(@"F:\ffolder1"));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(FS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(FV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(FD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(F));
            IsDriveCruzer(vinfo);
        }

        private void IsDriveCruzer(VolumeDeviceInfo vinfo)
        {
            Assert.That(vinfo.Disk.VendorId, Is.EqualTo("SanDisk "));
            Assert.That(vinfo.Disk.ProductId, Is.EqualTo("U3 Cruzer Micro "));
            Assert.That(vinfo.Disk.ProductRevision, Is.EqualTo("8.02"));
            Assert.That(vinfo.Disk.SerialNumber, Is.EqualTo("43202207CA531669"));
            Assert.That(vinfo.Disk.BusType, Is.EqualTo(BusType.Usb));
            Assert.That(vinfo.Disk.IsRemovableMedia, Is.True);
            Assert.That(vinfo.Disk.HasCommandQueueing, Is.False);
            Assert.That(vinfo.Disk.ScsiDeviceType, Is.EqualTo(ScsiDeviceType.DirectAccessDevice));
            Assert.That(vinfo.Disk.ScsiDeviceModifier, Is.EqualTo(0));
            Assert.That(vinfo.Disk.IsMediaPresent, Is.True);
            Assert.That(vinfo.Disk.IsReadOnly, Is.False);
            Assert.That(vinfo.Disk.GuidFlags, Is.EqualTo(DeviceGuidFlags.None));
            Assert.That(vinfo.Disk.Guid.ToString(), Is.EqualTo("8efcc1d1-b02a-05d7-56a6-06aaae42be6a"));
            Assert.That(vinfo.Disk.DeviceType, Is.EqualTo(DeviceType.Disk));
            Assert.That(vinfo.Disk.DeviceNumber, Is.EqualTo(3));
            Assert.That(vinfo.Disk.MediaType, Is.EqualTo(MediaType.RemovableMedia));
            Assert.That(vinfo.Disk.Geometry.Cylinders, Is.EqualTo(492));
            Assert.That(vinfo.Disk.Geometry.TracksPerCylinder, Is.EqualTo(255));
            Assert.That(vinfo.Disk.Geometry.SectorsPerTrack, Is.EqualTo(63));
            Assert.That(vinfo.Disk.Geometry.BytesPerSector, Is.EqualTo(512));
            Assert.That(vinfo.Disk.Geometry.BytesPerPhysicalSector, Is.EqualTo(vinfo.Disk.Geometry.BytesPerSector)); // OS returns Error 1
            Assert.That(vinfo.Disk.HasSeekPenalty, Is.EqualTo(BoolUnknown.Unknown));
            Assert.That(vinfo.Partition.Style, Is.EqualTo(PartitionStyle.MasterBootRecord));
            Assert.That(vinfo.Partition.Number, Is.EqualTo(1));
            Assert.That(vinfo.Partition.Offset, Is.EqualTo(65536));
            Assert.That(vinfo.Partition.Length, Is.EqualTo(4051566592));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Type, Is.EqualTo(7));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Bootable, Is.True);
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).MbrSectorsOffset, Is.EqualTo(128));
            Assert.That(vinfo.FileSystem.Label, Is.Empty);
            Assert.That(vinfo.FileSystem.Serial, Is.EqualTo("F099-8836"));
            Assert.That(vinfo.FileSystem.Name, Is.EqualTo("NTFS"));
            Assert.That((int)vinfo.FileSystem.Flags, Is.EqualTo(0x03E706FF));
            Assert.That(vinfo.FileSystem.BytesPerSector, Is.EqualTo(512));
            Assert.That(vinfo.FileSystem.SectorsPerCluster, Is.EqualTo(8));
            Assert.That(vinfo.FileSystem.TotalBytes, Is.EqualTo(4051562496));
            Assert.That(vinfo.FileSystem.TotalFreeBytes, Is.EqualTo(4032421888));
            Assert.That(vinfo.FileSystem.UserFreeBytes, Is.EqualTo(4032421888));
        }

        [Test]
        public void DriveHarddisk3()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoSim(Win10Sim, @"\\.\HarddiskVolume3");
            Assert.That(vinfo.Path, Is.EqualTo(@"\\.\HarddiskVolume3"));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(@"\\.\HarddiskVolume3\"));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(C));
            IsDriveSamsung(vinfo);
        }

        [Test]
        public void UnmappedDriveZ()
        {
            Assert.That(() => {
                _ = new VolumeDeviceInfoSim(Win10Sim, @"Z:");
            }, Throws.TypeOf<FileNotFoundException>());
        }
    }
}
