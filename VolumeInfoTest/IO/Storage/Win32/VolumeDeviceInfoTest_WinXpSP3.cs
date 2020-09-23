namespace VolumeInfo.IO.Storage.Win32
{
    using System.IO;
    using NUnit.Framework;

    [TestFixture]
    public class VolumeDeviceInfoTest_WinXpSP3
    {
        private static readonly string WinXPSim = Path.Combine(TestContext.CurrentContext.TestDirectory, "Test", "Win32", "VolumeInfoTest.WinXPSP3.xml");

        private class OSVolumeDeviceInfoWinXpSP3 : OSVolumeDeviceInfo
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
            Assert.That(vinfo.VolumePath, Is.EqualTo(CS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(C));
            IsDriveBoot(vinfo);
        }

        [Test]
        public void DriveCS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoXpSP3(@"C:\");
            Assert.That(vinfo.Path, Is.EqualTo(CS));
            Assert.That(vinfo.VolumePath, Is.EqualTo(CS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(C));
            IsDriveBoot(vinfo);
        }

        [Test]
        public void DriveCV()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoXpSP3(@"\\?\Volume{77f8a1bc-e9e9-11ea-95c7-806d6172696f}");
            Assert.That(vinfo.Path, Is.EqualTo(CV));
            Assert.That(vinfo.VolumePath, Is.EqualTo(CVS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.Empty);
            Assert.That(vinfo.VolumeDrive, Is.Empty);
            IsDriveBoot(vinfo);
        }

        [Test]
        public void DriveCVS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoXpSP3(@"\\?\Volume{77f8a1bc-e9e9-11ea-95c7-806d6172696f}\");
            Assert.That(vinfo.Path, Is.EqualTo(CVS));
            Assert.That(vinfo.VolumePath, Is.EqualTo(CVS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.Empty);
            Assert.That(vinfo.VolumeDrive, Is.Empty);
            IsDriveBoot(vinfo);
        }

        [Test]
        public void DriveCFolder()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoXpSP3(@"C:\Documents and Settings\User\My Documents");
            Assert.That(vinfo.Path, Is.EqualTo(@"C:\Documents and Settings\User\My Documents"));
            Assert.That(vinfo.VolumePath, Is.EqualTo(CS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(C));
            IsDriveBoot(vinfo);
        }

        private void IsDriveBoot(VolumeDeviceInfo vinfo)
        {
            Assert.That(vinfo.VendorId, Is.Empty);
            Assert.That(vinfo.ProductId, Is.EqualTo("VMware Virtual IDE Hard Drive"));
            Assert.That(vinfo.ProductRevision, Is.EqualTo("00000001"));
            Assert.That(vinfo.DeviceSerialNumber, Is.EqualTo("3030303030303030303"));
            Assert.That(vinfo.BusType, Is.EqualTo(BusType.Ata));
            Assert.That(vinfo.RemovableMedia, Is.False);
            Assert.That(vinfo.CommandQueueing, Is.False);
            Assert.That(vinfo.ScsiDeviceType, Is.EqualTo(ScsiDeviceType.DirectAccessDevice));
            Assert.That(vinfo.ScsiDeviceModifier, Is.EqualTo(0));
            Assert.That(vinfo.MediaPresent, Is.True);
            Assert.That(vinfo.VolumeLabel, Is.Empty);
            Assert.That(vinfo.VolumeSerial, Is.EqualTo("544D-DD66"));
            Assert.That(vinfo.FileSystem, Is.EqualTo("NTFS"));
            Assert.That((int)vinfo.FileSystemFlags, Is.EqualTo(0x000700FF));
            Assert.That(vinfo.DeviceGuidFlags, Is.EqualTo(DeviceGuidFlags.None));  // Windows XP doesn't support GUIDs
            Assert.That(vinfo.DeviceGuid.ToString(), Is.EqualTo("00000000-0000-0000-0000-000000000000"));
            Assert.That(vinfo.DeviceType, Is.EqualTo(DeviceType.Disk));
            Assert.That(vinfo.DeviceNumber, Is.EqualTo(0));
            Assert.That(vinfo.DevicePartitionNumber, Is.EqualTo(1));
            Assert.That(vinfo.DiskMediaType, Is.EqualTo(MediaType.FixedMedia));
            Assert.That(vinfo.DiskCylinders, Is.EqualTo(5221));
            Assert.That(vinfo.DiskTracksPerCylinder, Is.EqualTo(255));
            Assert.That(vinfo.DiskSectorsPerTrack, Is.EqualTo(63));
            Assert.That(vinfo.DiskBytesPerSector, Is.EqualTo(512));
            Assert.That(vinfo.HasSeekPenalty, Is.EqualTo(BoolUnknown.False));
        }

        [Test]
        public void DriveD()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoXpSP3(@"D:");
            Assert.That(vinfo.Path, Is.EqualTo(D));
            Assert.That(vinfo.VolumePath, Is.EqualTo(DS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(DD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(D));
            IsDriveCdRom(vinfo);
        }

        [Test]
        public void DriveDS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoXpSP3(@"D:\");
            Assert.That(vinfo.Path, Is.EqualTo(DS));
            Assert.That(vinfo.VolumePath, Is.EqualTo(DS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(DD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(D));
            IsDriveCdRom(vinfo);
        }

        [Test]
        public void DriveDV()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoXpSP3(@"\\?\Volume{77f8a1ba-e9e9-11ea-95c7-806d6172696f}");
            Assert.That(vinfo.Path, Is.EqualTo(DV));
            Assert.That(vinfo.VolumePath, Is.EqualTo(DVS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.Empty);
            Assert.That(vinfo.VolumeDrive, Is.Empty);
            IsDriveCdRom(vinfo);
        }

        [Test]
        public void DriveDVS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoXpSP3(@"\\?\Volume{77f8a1ba-e9e9-11ea-95c7-806d6172696f}\");
            Assert.That(vinfo.Path, Is.EqualTo(DVS));
            Assert.That(vinfo.VolumePath, Is.EqualTo(DVS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.Empty);
            Assert.That(vinfo.VolumeDrive, Is.Empty);
            IsDriveCdRom(vinfo);
        }

        private void IsDriveCdRom(VolumeDeviceInfo vinfo)
        {
            Assert.That(vinfo.VendorId, Is.Empty);
            Assert.That(vinfo.ProductId, Is.EqualTo("NECVMWar VMware IDE CDR10"));
            Assert.That(vinfo.ProductRevision, Is.EqualTo("1.00"));
            Assert.That(vinfo.DeviceSerialNumber, Is.EqualTo("3031303030303030303"));
            Assert.That(vinfo.BusType, Is.EqualTo(BusType.Atapi));
            Assert.That(vinfo.RemovableMedia, Is.True);
            Assert.That(vinfo.CommandQueueing, Is.False);
            Assert.That(vinfo.ScsiDeviceType, Is.EqualTo(ScsiDeviceType.CdRomDevice));
            Assert.That(vinfo.ScsiDeviceModifier, Is.EqualTo(0));
            Assert.That(vinfo.MediaPresent, Is.True);
            Assert.That(vinfo.VolumeLabel, Is.EqualTo("winxpsp3_090429"));
            Assert.That(vinfo.VolumeSerial, Is.EqualTo("1686-338B"));
            Assert.That(vinfo.FileSystem, Is.EqualTo("CDFS"));
            Assert.That((int)vinfo.FileSystemFlags, Is.EqualTo(0x00080005));
            Assert.That(vinfo.DeviceGuidFlags, Is.EqualTo(DeviceGuidFlags.None));  // Windows XP doesn't support GUIDs
            Assert.That(vinfo.DeviceGuid.ToString(), Is.EqualTo("00000000-0000-0000-0000-000000000000"));
            Assert.That(vinfo.DeviceType, Is.EqualTo(DeviceType.CdRom));
            Assert.That(vinfo.DeviceNumber, Is.EqualTo(0));
            Assert.That(vinfo.DevicePartitionNumber, Is.EqualTo(-1));  // No partitions
            Assert.That(vinfo.DiskMediaType, Is.EqualTo(MediaType.RemovableMedia));
            Assert.That(vinfo.DiskCylinders, Is.EqualTo(904));
            Assert.That(vinfo.DiskTracksPerCylinder, Is.EqualTo(64));
            Assert.That(vinfo.DiskSectorsPerTrack, Is.EqualTo(32));
            Assert.That(vinfo.DiskBytesPerSector, Is.EqualTo(2048));
            Assert.That(vinfo.HasSeekPenalty, Is.EqualTo(BoolUnknown.True));
        }

        [Test]
        public void NetM()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoXpSP3(@"M:");
            Assert.That(vinfo.Path, Is.EqualTo(M));
            Assert.That(vinfo.VolumePath, Is.Empty);        // Not a local mount
            Assert.That(vinfo.VolumeDevicePath, Is.Empty);  // Not a local mount
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(MD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(M));
        }

        [Test]
        public void NetMS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoXpSP3(@"M:\");
            Assert.That(vinfo.Path, Is.EqualTo(MS));
            Assert.That(vinfo.VolumePath, Is.Empty);        // Not a local mount
            Assert.That(vinfo.VolumeDevicePath, Is.Empty);  // Not a local mount
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(MD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(M));
        }

        [Test]
        public void SubstN()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoXpSP3(@"N:");
            Assert.That(vinfo.Path, Is.EqualTo(N));
            Assert.That(vinfo.VolumePath, Is.EqualTo(CS));  // Drive N: is mapped to C:
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(ND));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(N));
            IsDriveBoot(vinfo);
        }

        [Test]
        public void SubstNS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoXpSP3(@"N:\");
            Assert.That(vinfo.Path, Is.EqualTo(NS));
            Assert.That(vinfo.VolumePath, Is.EqualTo(CS));  // Drive N: is mapped to C:
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(ND));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(N));
            IsDriveBoot(vinfo);
        }

        [Test]
        public void SubstO()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoXpSP3(@"O:");
            Assert.That(vinfo.Path, Is.EqualTo(O));
            Assert.That(vinfo.VolumePath, Is.EqualTo(DS));  // Drive O: is mapped to D:\I386
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(OD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(O));
            IsDriveCdRom(vinfo);
        }

        [Test]
        public void SubstOS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoXpSP3(@"O:\");
            Assert.That(vinfo.Path, Is.EqualTo(OS));
            Assert.That(vinfo.VolumePath, Is.EqualTo(DS));  // Drive O: is mapped to D:\I386
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(OD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(O));
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
