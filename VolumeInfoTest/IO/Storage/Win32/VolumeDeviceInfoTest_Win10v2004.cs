namespace VolumeInfo.IO.Storage.Win32
{
    using System.IO;
    using NUnit.Framework;

    [TestFixture]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Correct case in the circumstances")]
    public class VolumeDeviceInfoTest_Win10v2004
    {
        private static readonly string Win10Sim = Path.Combine(TestContext.CurrentContext.TestDirectory, "Test", "Win32", "VolumeInfoTest.Win10v2004.xml");

        private class OSVolumeDeviceInfoWin10v2004 : OSVolumeDeviceInfo
        {
            public OSVolumeDeviceInfoWin10v2004() : base(Win10Sim) { }
        }

        private class VolumeDeviceInfoWin10v2004 : VolumeDeviceInfo
        {
            public VolumeDeviceInfoWin10v2004(string pathName) : base(new OSVolumeDeviceInfoWin10v2004(), pathName) { }
        }

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

        [Test]
        public void DriveC()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoWin10v2004(@"C:");
            Assert.That(vinfo.Path, Is.EqualTo(C));
            Assert.That(vinfo.VolumePath, Is.EqualTo(CS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(C));
            IsDriveSamsung(vinfo);
        }

        [Test]
        public void DriveCS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoWin10v2004(@"C:\");
            Assert.That(vinfo.Path, Is.EqualTo(CS));
            Assert.That(vinfo.VolumePath, Is.EqualTo(CS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(C));
            IsDriveSamsung(vinfo);
        }

        [Test]
        public void DriveCV()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoWin10v2004(@"\\?\Volume{4a2248f3-ec20-4c41-b781-ff19fa7913e6}");
            Assert.That(vinfo.Path, Is.EqualTo(CV));
            Assert.That(vinfo.VolumePath, Is.EqualTo(CVS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.Empty);
            Assert.That(vinfo.VolumeDrive, Is.Empty);
            IsDriveSamsung(vinfo);
        }

        [Test]
        public void DriveCVS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoWin10v2004(@"\\?\Volume{4a2248f3-ec20-4c41-b781-ff19fa7913e6}\");
            Assert.That(vinfo.Path, Is.EqualTo(CVS));
            Assert.That(vinfo.VolumePath, Is.EqualTo(CVS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.Empty);
            Assert.That(vinfo.VolumeDrive, Is.Empty);
            IsDriveSamsung(vinfo);
        }

        private void IsDriveSamsung(VolumeDeviceInfo vinfo)
        {
            Assert.That(vinfo.VendorId, Is.Empty);
            Assert.That(vinfo.ProductId, Is.EqualTo("SAMSUNG MZFLV512HCJH-000MV"));
            Assert.That(vinfo.ProductRevision, Is.EqualTo("BXV75M0Q"));
            Assert.That(vinfo.DeviceSerialNumber, Is.EqualTo("0025_3844_61B5_6586."));
            Assert.That(vinfo.BusType, Is.EqualTo(BusType.Nvme));
            Assert.That(vinfo.RemovableMedia, Is.False);
            Assert.That(vinfo.CommandQueueing, Is.True);
            Assert.That(vinfo.ScsiDeviceType, Is.EqualTo(ScsiDeviceType.DirectAccessDevice));
            Assert.That(vinfo.ScsiDeviceModifier, Is.EqualTo(0));
            Assert.That(vinfo.MediaPresent, Is.True);
            Assert.That(vinfo.VolumeLabel, Is.EqualTo("Local Disk"));
            Assert.That(vinfo.VolumeSerial, Is.EqualTo("D470-C5ED"));
            Assert.That(vinfo.FileSystem, Is.EqualTo("NTFS"));
            Assert.That((int)vinfo.FileSystemFlags, Is.EqualTo(0x03E706FF));
            Assert.That(vinfo.DeviceGuidFlags, Is.EqualTo(DeviceGuidFlags.Page83DeviceGuid));
            Assert.That(vinfo.DeviceGuid.ToString(), Is.EqualTo("ba408eac-457b-e82e-f5ea-f5764f6a8c94"));
            Assert.That(vinfo.DeviceType, Is.EqualTo(DeviceType.Disk));
            Assert.That(vinfo.DeviceNumber, Is.EqualTo(0));
            Assert.That(vinfo.DevicePartitionNumber, Is.EqualTo(3));
        }

        [Test]
        public void DriveD()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoWin10v2004(@"D:");
            Assert.That(vinfo.Path, Is.EqualTo(D));
            Assert.That(vinfo.VolumePath, Is.EqualTo(DS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(DD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(D));
            IsDriveSdCard(vinfo);
        }

        [Test]
        public void DriveDS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoWin10v2004(@"D:\");
            Assert.That(vinfo.Path, Is.EqualTo(DS));
            Assert.That(vinfo.VolumePath, Is.EqualTo(DS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(DD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(D));
            IsDriveSdCard(vinfo);
        }

        [Test]
        public void DriveDV()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoWin10v2004(@"\\?\Volume{d5fcf8f3-1d12-11ea-913c-806e6f6e6963}");
            Assert.That(vinfo.Path, Is.EqualTo(DV));
            Assert.That(vinfo.VolumePath, Is.EqualTo(DVS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.Empty);
            Assert.That(vinfo.VolumeDrive, Is.Empty);
            IsDriveSdCard(vinfo);
        }

        [Test]
        public void DriveDVS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoWin10v2004(@"\\?\Volume{d5fcf8f3-1d12-11ea-913c-806e6f6e6963}\");
            Assert.That(vinfo.Path, Is.EqualTo(DVS));
            Assert.That(vinfo.VolumePath, Is.EqualTo(DVS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.Empty);
            Assert.That(vinfo.VolumeDrive, Is.Empty);
            IsDriveSdCard(vinfo);
        }

        private void IsDriveSdCard(VolumeDeviceInfo vinfo)
        {
            Assert.That(vinfo.VendorId, Is.EqualTo("Generic-"));
            Assert.That(vinfo.ProductId, Is.EqualTo("SD Card         "));
            Assert.That(vinfo.ProductRevision, Is.EqualTo("1.00"));
            Assert.That(vinfo.DeviceSerialNumber, Is.Empty);
            Assert.That(vinfo.BusType, Is.EqualTo(BusType.Usb));
            Assert.That(vinfo.RemovableMedia, Is.True);
            Assert.That(vinfo.CommandQueueing, Is.False);
            Assert.That(vinfo.ScsiDeviceType, Is.EqualTo(ScsiDeviceType.DirectAccessDevice));
            Assert.That(vinfo.ScsiDeviceModifier, Is.EqualTo(13));
            Assert.That(vinfo.MediaPresent, Is.True);
            Assert.That(vinfo.VolumeLabel, Is.EqualTo("Samsung64GB"));
            Assert.That(vinfo.VolumeSerial, Is.EqualTo("0000-0000"));
            Assert.That(vinfo.FileSystem, Is.EqualTo("exFAT"));
            Assert.That((int)vinfo.FileSystemFlags, Is.EqualTo(0x00020206));
            Assert.That(vinfo.DeviceGuidFlags, Is.EqualTo(DeviceGuidFlags.RandomDeviceGuidReasonNoHwId));
            Assert.That(vinfo.DeviceGuid.ToString(), Is.EqualTo("537baabc-f7f9-11ea-9169-985fd3d32a6a"));
            Assert.That(vinfo.DeviceType, Is.EqualTo(DeviceType.Disk));
            Assert.That(vinfo.DeviceNumber, Is.EqualTo(1));
            Assert.That(vinfo.DevicePartitionNumber, Is.EqualTo(1));
        }

        [Test]
        public void DriveE()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoWin10v2004(@"E:");
            Assert.That(vinfo.Path, Is.EqualTo(E));
            Assert.That(vinfo.VolumePath, Is.EqualTo(ES));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(EV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(ED));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(E));
            IsDriveGlide(vinfo);
        }

        [Test]
        public void DriveES()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoWin10v2004(@"E:\");
            Assert.That(vinfo.Path, Is.EqualTo(ES));
            Assert.That(vinfo.VolumePath, Is.EqualTo(ES));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(EV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(ED));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(E));
            IsDriveGlide(vinfo);
        }

        [Test]
        public void DriveEV()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoWin10v2004(@"\\?\Volume{299b5220-f500-11ea-9168-985fd3d32a6a}");
            Assert.That(vinfo.Path, Is.EqualTo(EV));
            Assert.That(vinfo.VolumePath, Is.EqualTo(EVS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(EV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.Empty);
            Assert.That(vinfo.VolumeDrive, Is.Empty);
            IsDriveGlide(vinfo);
        }

        [Test]
        public void DriveEVS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoWin10v2004(@"\\?\Volume{299b5220-f500-11ea-9168-985fd3d32a6a}\");
            Assert.That(vinfo.Path, Is.EqualTo(EVS));
            Assert.That(vinfo.VolumePath, Is.EqualTo(EVS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(EV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.Empty);
            Assert.That(vinfo.VolumeDrive, Is.Empty);
            IsDriveGlide(vinfo);
        }

        private void IsDriveGlide(VolumeDeviceInfo vinfo)
        {
            Assert.That(vinfo.VendorId, Is.EqualTo("SanDisk"));
            Assert.That(vinfo.ProductId, Is.EqualTo("Cruzer Glide 3.0"));
            Assert.That(vinfo.ProductRevision, Is.EqualTo("1.00"));
            Assert.That(vinfo.DeviceSerialNumber, Is.EqualTo("4C530001120606116282"));
            Assert.That(vinfo.BusType, Is.EqualTo(BusType.Usb));
            Assert.That(vinfo.RemovableMedia, Is.True);
            Assert.That(vinfo.CommandQueueing, Is.False);
            Assert.That(vinfo.ScsiDeviceType, Is.EqualTo(ScsiDeviceType.DirectAccessDevice));
            Assert.That(vinfo.ScsiDeviceModifier, Is.EqualTo(0));
            Assert.That(vinfo.MediaPresent, Is.True);
            Assert.That(vinfo.VolumeLabel, Is.EqualTo("USB"));
            Assert.That(vinfo.VolumeSerial, Is.EqualTo("36F8-F5F6"));
            Assert.That(vinfo.FileSystem, Is.EqualTo("NTFS"));
            Assert.That((int)vinfo.FileSystemFlags, Is.EqualTo(0x03E706FF));
            Assert.That(vinfo.DeviceGuidFlags, Is.EqualTo(DeviceGuidFlags.None));
            Assert.That(vinfo.DeviceGuid.ToString(), Is.EqualTo("a183a5b5-d50a-86a9-188b-739eab126a73"));
            Assert.That(vinfo.DeviceType, Is.EqualTo(DeviceType.Disk));
            Assert.That(vinfo.DeviceNumber, Is.EqualTo(2));
            Assert.That(vinfo.DevicePartitionNumber, Is.EqualTo(1));
        }

        [Test]
        public void DriveCFolder()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoWin10v2004(@"C:\Users\user\Desktop");
            Assert.That(vinfo.Path, Is.EqualTo(@"C:\Users\user\Desktop"));
            Assert.That(vinfo.VolumePath, Is.EqualTo(CS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(C));
            IsDriveSamsung(vinfo);
        }

        [Test]
        public void NetM()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoWin10v2004(@"M:");
            Assert.That(vinfo.Path, Is.EqualTo(M));
            Assert.That(vinfo.VolumePath, Is.Empty);        // Not a local mount
            Assert.That(vinfo.VolumeDevicePath, Is.Empty);  // Not a local mount
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(MD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(M));
        }

        [Test]
        public void NetMS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoWin10v2004(@"M:\");
            Assert.That(vinfo.Path, Is.EqualTo(MS));
            Assert.That(vinfo.VolumePath, Is.Empty);        // Not a local mount
            Assert.That(vinfo.VolumeDevicePath, Is.Empty);  // Not a local mount
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(MD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(M));
        }

        [Test]
        public void SubstN()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoWin10v2004(@"N:");
            Assert.That(vinfo.Path, Is.EqualTo(N));
            Assert.That(vinfo.VolumePath, Is.EqualTo(CS));  // Drive N: is mapped to C:
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(ND));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(N));
            IsDriveSamsung(vinfo);
        }

        [Test]
        public void SubstNS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoWin10v2004(@"N:\");
            Assert.That(vinfo.Path, Is.EqualTo(NS));
            Assert.That(vinfo.VolumePath, Is.EqualTo(CS));  // Drive N: is mapped to C:
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(ND));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(N));
            IsDriveSamsung(vinfo);
        }

        [Test]
        public void SubstO()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoWin10v2004(@"O:");
            Assert.That(vinfo.Path, Is.EqualTo(O));
            Assert.That(vinfo.VolumePath, Is.EqualTo(DS));  // Drive O: is mapped to D:\books
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(OD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(O));
            IsDriveSdCard(vinfo);
        }

        [Test]
        public void SubstOS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoWin10v2004(@"O:\");
            Assert.That(vinfo.Path, Is.EqualTo(OS));
            Assert.That(vinfo.VolumePath, Is.EqualTo(DS));  // Drive O: is mapped to D:\books
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(OD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(O));
            IsDriveSdCard(vinfo);
        }

        [Test]
        public void SubstOFolder()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoWin10v2004(@"O:\Calculus");
            Assert.That(vinfo.Path, Is.EqualTo(@"O:\Calculus"));
            Assert.That(vinfo.VolumePath, Is.EqualTo(DS));  // Drive O: is mapped to D:\books
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(OD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(O));
            IsDriveSdCard(vinfo);
        }

        [Test]
        public void SubstP()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoWin10v2004(@"P:");
            Assert.That(vinfo.Path, Is.EqualTo(P));
            Assert.That(vinfo.VolumePath, Is.EqualTo(CS));  // Drive P: is mapped to N:\
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(PD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(P));
            IsDriveSamsung(vinfo);
        }

        [Test]
        public void SubstPS()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoWin10v2004(@"P:\");
            Assert.That(vinfo.Path, Is.EqualTo(PS));
            Assert.That(vinfo.VolumePath, Is.EqualTo(CS));  // Drive P: is mapped to N:\
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(PD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(P));
            IsDriveSamsung(vinfo);
        }

        [Test]
        public void JunctionFromFToEToC()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoWin10v2004(@"F:\ffolder1\efolder1\windows");
            Assert.That(vinfo.Path, Is.EqualTo(@"F:\ffolder1\efolder1\windows"));
            Assert.That(vinfo.VolumePath, Is.EqualTo(CS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(CD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(C));
            IsDriveSamsung(vinfo);
        }

        [Test]
        public void JunctionFromFToE()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoWin10v2004(@"F:\ffolder1\efolder1");
            Assert.That(vinfo.Path, Is.EqualTo(@"F:\ffolder1\efolder1"));
            Assert.That(vinfo.VolumePath, Is.EqualTo(ES));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(EV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(ED));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(E));
            IsDriveGlide(vinfo);
        }

        [Test]
        public void JunctionFromFToO()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoWin10v2004(@"F:\ffolder1\efolder1\calculus");
            Assert.That(vinfo.Path, Is.EqualTo(@"F:\ffolder1\efolder1\calculus"));
            Assert.That(vinfo.VolumePath, Is.EqualTo(DS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(OD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(O));
            IsDriveSdCard(vinfo);
        }

        [Test]
        public void JunctionFromEToO()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoWin10v2004(@"E:\efolder1\calculus");
            Assert.That(vinfo.Path, Is.EqualTo(@"E:\efolder1\calculus"));
            Assert.That(vinfo.VolumePath, Is.EqualTo(DS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(DV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(OD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(O));
            IsDriveSdCard(vinfo);
        }

        [Test]
        public void DriveFFolder()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoWin10v2004(@"F:\ffolder1");
            Assert.That(vinfo.Path, Is.EqualTo(@"F:\ffolder1"));
            Assert.That(vinfo.VolumePath, Is.EqualTo(FS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(FV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(FD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(F));
            IsDriveCruzer(vinfo);
        }

        private void IsDriveCruzer(VolumeDeviceInfo vinfo)
        {
            Assert.That(vinfo.VendorId, Is.EqualTo("SanDisk "));
            Assert.That(vinfo.ProductId, Is.EqualTo("U3 Cruzer Micro "));
            Assert.That(vinfo.ProductRevision, Is.EqualTo("8.02"));
            Assert.That(vinfo.DeviceSerialNumber, Is.EqualTo("43202207CA531669"));
            Assert.That(vinfo.BusType, Is.EqualTo(BusType.Usb));
            Assert.That(vinfo.RemovableMedia, Is.True);
            Assert.That(vinfo.CommandQueueing, Is.False);
            Assert.That(vinfo.ScsiDeviceType, Is.EqualTo(ScsiDeviceType.DirectAccessDevice));
            Assert.That(vinfo.ScsiDeviceModifier, Is.EqualTo(0));
            Assert.That(vinfo.MediaPresent, Is.True);
            Assert.That(vinfo.VolumeLabel, Is.Empty);
            Assert.That(vinfo.VolumeSerial, Is.EqualTo("F099-8836"));
            Assert.That(vinfo.FileSystem, Is.EqualTo("NTFS"));
            Assert.That((int)vinfo.FileSystemFlags, Is.EqualTo(0x03E706FF));
            Assert.That(vinfo.DeviceGuidFlags, Is.EqualTo(DeviceGuidFlags.None));
            Assert.That(vinfo.DeviceGuid.ToString(), Is.EqualTo("8efcc1d1-b02a-05d7-56a6-06aaae42be6a"));
            Assert.That(vinfo.DeviceType, Is.EqualTo(DeviceType.Disk));
            Assert.That(vinfo.DeviceNumber, Is.EqualTo(3));
            Assert.That(vinfo.DevicePartitionNumber, Is.EqualTo(1));
        }

        [Test]
        public void DriveHarddisk3()
        {
            VolumeDeviceInfo vinfo = new VolumeDeviceInfoWin10v2004(@"\\.\HarddiskVolume3");
            Assert.That(vinfo.Path, Is.EqualTo(@"\\.\HarddiskVolume3"));
            Assert.That(vinfo.VolumePath, Is.EqualTo(@"\\.\HarddiskVolume3\"));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(CV));
            Assert.That(vinfo.VolumeDosDevicePath, Is.Empty);
            Assert.That(vinfo.VolumeDrive, Is.Empty);
            IsDriveSamsung(vinfo);
        }

        [Test]
        public void UnmappedDriveZ()
        {
            Assert.That(() => {
                _ = new VolumeDeviceInfoWin10v2004(@"Z:");
            }, Throws.TypeOf<FileNotFoundException>());
        }
    }
}
