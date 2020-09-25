namespace VolumeInfo.IO.Storage
{
    using System.IO;
    using NUnit.Framework;

    [TestFixture]
    public class VolumeDeviceInfoTest_WinXpSP3
    {
        private static readonly string WinXPSim = Path.Combine(TestContext.CurrentContext.TestDirectory, "Test", "Win32", "VolumeInfoTest.WinXPSP3.xml");

        private class OSVolumeDeviceInfoWinXpSP3 : Win32.OSVolumeDeviceInfo
        {
            public OSVolumeDeviceInfoWinXpSP3() : base(WinXPSim) { }
        }

        private class VolumeDeviceInfoXpSP3 : VolumeDeviceInfo
        {
            public VolumeDeviceInfoXpSP3(string pathName) : base(new OSVolumeDeviceInfoWinXpSP3(), pathName) { }
        }

        public const string C = @"C:";
        public const string CS = @"C:\";
        public const string CD = @"\Device\HarddiskVolume1";
        public const string CV = @"\\?\Volume{77f8a1bc-e9e9-11ea-95c7-806d6172696f}";
        public const string CVS = @"\\?\Volume{77f8a1bc-e9e9-11ea-95c7-806d6172696f}\";

        public const string D = @"D:";
        public const string DS = @"D:\";
        public const string DD = @"\Device\CdRom0";
        public const string DV = @"\\?\Volume{77f8a1ba-e9e9-11ea-95c7-806d6172696f}";
        public const string DVS = @"\\?\Volume{77f8a1ba-e9e9-11ea-95c7-806d6172696f}\";

        public const string M = @"M:";
        public const string MS = @"M:\";
        public const string MD = @"\Device\LanmanRedirector\;M:000000000000dc0d\devtest\share";

        public const string N = @"N:";
        public const string NS = @"N:\";
        public const string ND = @"\??\C:";
        public const string NV = CV;
        public const string NVS = CVS;

        public const string O = @"O:";
        public const string OS = @"O:\";
        public const string OD = @"\??\D:\I386";
        public const string OV = CV;
        public const string OVS = CVS;

        public const string Z = @"Z:";
        public const string ZS = @"Z:\";

        [Test]
        public void DriveC()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoXpSP3(@"C:");
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
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoXpSP3(@"C:\");
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
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoXpSP3(@"\\?\Volume{77f8a1bc-e9e9-11ea-95c7-806d6172696f}");
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
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoXpSP3(@"\\?\Volume{77f8a1bc-e9e9-11ea-95c7-806d6172696f}\");
            Assert.That(vinfo.Path, Is.EqualTo(CVS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(CVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(C));
            IsDriveBoot(vinfo);
        }

        [Test]
        public void DriveCFolder()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoXpSP3(@"C:\Documents and Settings\User\My Documents");
            Assert.That(vinfo.Path, Is.EqualTo(@"C:\Documents and Settings\User\My Documents"));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(CS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(C));
            IsDriveBoot(vinfo);
        }

        private void IsDriveBoot(VolumeDeviceInfo vinfo)
        {
            Assert.That(vinfo.Disk.VendorId, Is.Empty);
            Assert.That(vinfo.Disk.ProductId, Is.EqualTo("VMware Virtual IDE Hard Drive"));
            Assert.That(vinfo.Disk.ProductRevision, Is.EqualTo("00000001"));
            Assert.That(vinfo.Disk.SerialNumber, Is.EqualTo("3030303030303030303"));
            Assert.That(vinfo.Disk.BusType, Is.EqualTo(BusType.Ata));
            Assert.That(vinfo.Disk.IsRemovableMedia, Is.False);
            Assert.That(vinfo.Disk.HasCommandQueueing, Is.False);
            Assert.That(vinfo.Disk.ScsiDeviceType, Is.EqualTo(ScsiDeviceType.DirectAccessDevice));
            Assert.That(vinfo.Disk.ScsiDeviceModifier, Is.EqualTo(0));
            Assert.That(vinfo.Disk.IsMediaPresent, Is.True);
            Assert.That(vinfo.FileSystem.Label, Is.Empty);
            Assert.That(vinfo.FileSystem.Serial, Is.EqualTo("544D-DD66"));
            Assert.That(vinfo.FileSystem.Name, Is.EqualTo("NTFS"));
            Assert.That((int)vinfo.FileSystem.Flags, Is.EqualTo(0x000700FF));
            Assert.That(vinfo.Disk.GuidFlags, Is.EqualTo(DeviceGuidFlags.None));  // Windows XP doesn't support GUIDs
            Assert.That(vinfo.Disk.Guid.ToString(), Is.EqualTo("00000000-0000-0000-0000-000000000000"));
            Assert.That(vinfo.Disk.DeviceType, Is.EqualTo(DeviceType.Disk));
            Assert.That(vinfo.Disk.DeviceNumber, Is.EqualTo(0));
            Assert.That(vinfo.Disk.MediaType, Is.EqualTo(MediaType.FixedMedia));
            Assert.That(vinfo.Disk.Cylinders, Is.EqualTo(5221));
            Assert.That(vinfo.Disk.TracksPerCylinder, Is.EqualTo(255));
            Assert.That(vinfo.Disk.SectorsPerTrack, Is.EqualTo(63));
            Assert.That(vinfo.Disk.BytesPerSector, Is.EqualTo(512));
            Assert.That(vinfo.Disk.HasSeekPenalty, Is.EqualTo(BoolUnknown.False));
            Assert.That(vinfo.Disk.BytesPerPhysicalSector, Is.EqualTo(vinfo.Disk.BytesPerSector)); // Not supported on WinXP
            Assert.That(vinfo.Partition.Style, Is.EqualTo(PartitionStyle.MasterBootRecord));
            Assert.That(vinfo.Partition.Number, Is.EqualTo(1));
            Assert.That(vinfo.Partition.Offset, Is.EqualTo(32256));
            Assert.That(vinfo.Partition.Length, Is.EqualTo(42935929344));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Type, Is.EqualTo(7));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Bootable, Is.True);
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).MbrSectorsOffset, Is.EqualTo(63));
        }

        [Test]
        public void DriveD()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoXpSP3(@"D:");
            Assert.That(vinfo.Path, Is.EqualTo(D));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(DS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(DD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(D));
            IsDriveCdRom(vinfo);
        }

        [Test]
        public void DriveDS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoXpSP3(@"D:\");
            Assert.That(vinfo.Path, Is.EqualTo(DS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(DS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(DD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(D));
            IsDriveCdRom(vinfo);
        }

        [Test]
        public void DriveDV()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoXpSP3(@"\\?\Volume{77f8a1ba-e9e9-11ea-95c7-806d6172696f}");
            Assert.That(vinfo.Path, Is.EqualTo(DV));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(DVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(DD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(D));
            IsDriveCdRom(vinfo);
        }

        [Test]
        public void DriveDVS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoXpSP3(@"\\?\Volume{77f8a1ba-e9e9-11ea-95c7-806d6172696f}\");
            Assert.That(vinfo.Path, Is.EqualTo(DVS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(DVS));
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(DD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(D));
            IsDriveCdRom(vinfo);
        }

        private void IsDriveCdRom(VolumeDeviceInfo vinfo)
        {
            Assert.That(vinfo.Disk.VendorId, Is.Empty);
            Assert.That(vinfo.Disk.ProductId, Is.EqualTo("NECVMWar VMware IDE CDR10"));
            Assert.That(vinfo.Disk.ProductRevision, Is.EqualTo("1.00"));
            Assert.That(vinfo.Disk.SerialNumber, Is.EqualTo("3031303030303030303"));
            Assert.That(vinfo.Disk.BusType, Is.EqualTo(BusType.Atapi));
            Assert.That(vinfo.Disk.IsRemovableMedia, Is.True);
            Assert.That(vinfo.Disk.HasCommandQueueing, Is.False);
            Assert.That(vinfo.Disk.ScsiDeviceType, Is.EqualTo(ScsiDeviceType.CdRomDevice));
            Assert.That(vinfo.Disk.ScsiDeviceModifier, Is.EqualTo(0));
            Assert.That(vinfo.Disk.IsMediaPresent, Is.True);
            Assert.That(vinfo.FileSystem.Label, Is.EqualTo("winxpsp3_090429"));
            Assert.That(vinfo.FileSystem.Serial, Is.EqualTo("1686-338B"));
            Assert.That(vinfo.FileSystem.Name, Is.EqualTo("CDFS"));
            Assert.That((int)vinfo.FileSystem.Flags, Is.EqualTo(0x00080005));
            Assert.That(vinfo.Disk.GuidFlags, Is.EqualTo(DeviceGuidFlags.None));  // Windows XP doesn't support GUIDs
            Assert.That(vinfo.Disk.Guid.ToString(), Is.EqualTo("00000000-0000-0000-0000-000000000000"));
            Assert.That(vinfo.Disk.DeviceType, Is.EqualTo(DeviceType.CdRom));
            Assert.That(vinfo.Disk.DeviceNumber, Is.EqualTo(0));
            Assert.That(vinfo.Disk.MediaType, Is.EqualTo(MediaType.RemovableMedia));
            Assert.That(vinfo.Disk.Cylinders, Is.EqualTo(904));
            Assert.That(vinfo.Disk.TracksPerCylinder, Is.EqualTo(64));
            Assert.That(vinfo.Disk.SectorsPerTrack, Is.EqualTo(32));
            Assert.That(vinfo.Disk.BytesPerSector, Is.EqualTo(2048));
            Assert.That(vinfo.Disk.HasSeekPenalty, Is.EqualTo(BoolUnknown.True));
            Assert.That(vinfo.Disk.BytesPerPhysicalSector, Is.EqualTo(vinfo.Disk.BytesPerSector)); // Not supported on WinXP
            Assert.That(vinfo.Partition.Style, Is.EqualTo(PartitionStyle.MasterBootRecord));
            Assert.That(vinfo.Partition.Number, Is.EqualTo(0));
            Assert.That(vinfo.Partition.Offset, Is.EqualTo(0));
            Assert.That(vinfo.Partition.Length, Is.EqualTo(3794823168));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Type, Is.EqualTo(11));
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).Bootable, Is.False);
            Assert.That(((VolumeDeviceInfo.IMbrPartition)vinfo.Partition).MbrSectorsOffset, Is.EqualTo(0));
        }

        [Test]
        public void NetM()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoXpSP3(@"M:");
            Assert.That(vinfo.Path, Is.EqualTo(M));
            Assert.That(vinfo.Volume.Path, Is.Empty);        // Not a local mount
            Assert.That(vinfo.Volume.DevicePath, Is.Empty);  // Not a local mount
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(MD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(M));
        }

        [Test]
        public void NetMS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoXpSP3(@"M:\");
            Assert.That(vinfo.Path, Is.EqualTo(MS));
            Assert.That(vinfo.Volume.Path, Is.Empty);        // Not a local mount
            Assert.That(vinfo.Volume.DevicePath, Is.Empty);  // Not a local mount
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(MD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(M));
        }

        [Test]
        public void SubstN()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoXpSP3(@"N:");
            Assert.That(vinfo.Path, Is.EqualTo(N));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(CS));  // Drive N: is mapped to C:
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(ND));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(N));
            IsDriveBoot(vinfo);
        }

        [Test]
        public void SubstNS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoXpSP3(@"N:\");
            Assert.That(vinfo.Path, Is.EqualTo(NS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(CS));  // Drive N: is mapped to C:
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(ND));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(N));
            IsDriveBoot(vinfo);
        }

        [Test]
        public void SubstO()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoXpSP3(@"O:");
            Assert.That(vinfo.Path, Is.EqualTo(O));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(DS));  // Drive O: is mapped to D:\I386
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(OD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(O));
            IsDriveCdRom(vinfo);
        }

        [Test]
        public void SubstOS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoXpSP3(@"O:\");
            Assert.That(vinfo.Path, Is.EqualTo(OS));
            Assert.That(vinfo.Volume.Path, Is.EqualTo(DS));  // Drive O: is mapped to D:\I386
            Assert.That(vinfo.Volume.DevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.Volume.DosDevicePath, Is.EqualTo(OD));
            Assert.That(vinfo.Volume.DriveLetter, Is.EqualTo(O));
            IsDriveCdRom(vinfo);
        }

        [Test]
        public void UnmappedDriveZ()
        {
            Assert.That(() => {
                _ = new VolumeDeviceInfoXpSP3(@"Z:");
            }, Throws.TypeOf<FileNotFoundException>());
        }
    }
}
