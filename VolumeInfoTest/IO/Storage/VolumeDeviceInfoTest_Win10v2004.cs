namespace VolumeInfo.IO.Storage
{
    using NUnit.Framework;

    /// <summary>
    /// See <see cref="Win10.OSVolumeDeviceInfoWin10v2004"/> for definitions of the drives and tests being performed.
    /// </summary>
    [TestFixture]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Correct case in the circumstances")]
    public class VolumeDeviceInfoTest_Win10v2004
    {
        [Test]
        public void DriveC()
        {
            VolumeDeviceInfo vinfo = new Win10.VolumeDeviceInfoWin10v2004(@"C:");
            Assert.That(vinfo.Path, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.C));
            Assert.That(vinfo.VolumePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.CS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VC));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.CD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.C));
            IsDrive3(vinfo);
        }

        [Test]
        public void DriveCS()
        {
            VolumeDeviceInfo vinfo = new Win10.VolumeDeviceInfoWin10v2004(@"C:\");
            Assert.That(vinfo.Path, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.CS));
            Assert.That(vinfo.VolumePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.CS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VC));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.CD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.C));
            IsDrive3(vinfo);
        }

        [Test]
        public void DriveCV()
        {
            VolumeDeviceInfo vinfo = new Win10.VolumeDeviceInfoWin10v2004(@"\\?\Volume{4a2248f3-ec20-4c41-b781-ff19fa7913e6}");
            Assert.That(vinfo.Path, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VC));
            Assert.That(vinfo.VolumePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VCS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VC));
            Assert.That(vinfo.VolumeDosDevicePath, Is.Empty);
            Assert.That(vinfo.VolumeDrive, Is.Empty);
            IsDrive3(vinfo);
        }

        [Test]
        public void DriveCVS()
        {
            VolumeDeviceInfo vinfo = new Win10.VolumeDeviceInfoWin10v2004(@"\\?\Volume{4a2248f3-ec20-4c41-b781-ff19fa7913e6}\");
            Assert.That(vinfo.Path, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VCS));
            Assert.That(vinfo.VolumePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VCS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VC));
            Assert.That(vinfo.VolumeDosDevicePath, Is.Empty);
            Assert.That(vinfo.VolumeDrive, Is.Empty);
            IsDrive3(vinfo);
        }

        private void IsDrive3(VolumeDeviceInfo vinfo)
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
        }

        [Test]
        public void DriveD()
        {
            VolumeDeviceInfo vinfo = new Win10.VolumeDeviceInfoWin10v2004(@"D:");
            Assert.That(vinfo.Path, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.D));
            Assert.That(vinfo.VolumePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.DS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VD));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.DD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.D));
            IsDrive6(vinfo);
        }

        [Test]
        public void DriveDS()
        {
            VolumeDeviceInfo vinfo = new Win10.VolumeDeviceInfoWin10v2004(@"D:\");
            Assert.That(vinfo.Path, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.DS));
            Assert.That(vinfo.VolumePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.DS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VD));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.DD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.D));
            IsDrive6(vinfo);
        }

        [Test]
        public void DriveDV()
        {
            VolumeDeviceInfo vinfo = new Win10.VolumeDeviceInfoWin10v2004(@"\\?\Volume{d5fcf8f3-1d12-11ea-913c-806e6f6e6963}");
            Assert.That(vinfo.Path, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VD));
            Assert.That(vinfo.VolumePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VDS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VD));
            Assert.That(vinfo.VolumeDosDevicePath, Is.Empty);
            Assert.That(vinfo.VolumeDrive, Is.Empty);
            IsDrive6(vinfo);
        }

        [Test]
        public void DriveDVS()
        {
            VolumeDeviceInfo vinfo = new Win10.VolumeDeviceInfoWin10v2004(@"\\?\Volume{d5fcf8f3-1d12-11ea-913c-806e6f6e6963}\");
            Assert.That(vinfo.Path, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VDS));
            Assert.That(vinfo.VolumePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VDS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VD));
            Assert.That(vinfo.VolumeDosDevicePath, Is.Empty);
            Assert.That(vinfo.VolumeDrive, Is.Empty);
            IsDrive6(vinfo);
        }

        private void IsDrive6(VolumeDeviceInfo vinfo)
        {
            Assert.That(vinfo.VendorId, Is.EqualTo("Generic-"));
            Assert.That(vinfo.ProductId, Is.EqualTo("SD Card"));
            Assert.That(vinfo.ProductRevision, Is.EqualTo("1.00"));
            Assert.That(vinfo.DeviceSerialNumber, Is.Empty);
            Assert.That(vinfo.BusType, Is.EqualTo(BusType.Usb));
            Assert.That(vinfo.RemovableMedia, Is.True);
            Assert.That(vinfo.CommandQueueing, Is.False);
            Assert.That(vinfo.ScsiDeviceType, Is.EqualTo(ScsiDeviceType.DirectAccessDevice));
            Assert.That(vinfo.ScsiDeviceModifier, Is.EqualTo(13));
        }

        [Test]
        public void DriveE()
        {
            VolumeDeviceInfo vinfo = new Win10.VolumeDeviceInfoWin10v2004(@"E:");
            Assert.That(vinfo.Path, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.E));
            Assert.That(vinfo.VolumePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.ES));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VE));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.ED));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.E));
            IsDrive5(vinfo);
        }

        [Test]
        public void DriveES()
        {
            VolumeDeviceInfo vinfo = new Win10.VolumeDeviceInfoWin10v2004(@"E:\");
            Assert.That(vinfo.Path, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.ES));
            Assert.That(vinfo.VolumePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.ES));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VE));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.ED));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.E));
            IsDrive5(vinfo);
        }

        [Test]
        public void DriveEV()
        {
            VolumeDeviceInfo vinfo = new Win10.VolumeDeviceInfoWin10v2004(@"\\?\Volume{299b5220-f500-11ea-9168-985fd3d32a6a}");
            Assert.That(vinfo.Path, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VE));
            Assert.That(vinfo.VolumePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VES));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VE));
            Assert.That(vinfo.VolumeDosDevicePath, Is.Empty);
            Assert.That(vinfo.VolumeDrive, Is.Empty);
            IsDrive5(vinfo);
        }

        [Test]
        public void DriveEVS()
        {
            VolumeDeviceInfo vinfo = new Win10.VolumeDeviceInfoWin10v2004(@"\\?\Volume{299b5220-f500-11ea-9168-985fd3d32a6a}\");
            Assert.That(vinfo.Path, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VES));
            Assert.That(vinfo.VolumePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VES));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VE));
            Assert.That(vinfo.VolumeDosDevicePath, Is.Empty);
            Assert.That(vinfo.VolumeDrive, Is.Empty);
            IsDrive5(vinfo);
        }

        private void IsDrive5(VolumeDeviceInfo vinfo)
        {
            Assert.That(vinfo.VendorId, Is.EqualTo("SanDisk"));
            Assert.That(vinfo.ProductId, Is.EqualTo("Cruzer Glide 3.0"));
            Assert.That(vinfo.ProductRevision, Is.EqualTo("1.00"));
            Assert.That(vinfo.DeviceSerialNumber, Is.EqualTo("4C530001620606126282"));
            Assert.That(vinfo.BusType, Is.EqualTo(BusType.Usb));
            Assert.That(vinfo.RemovableMedia, Is.True);
            Assert.That(vinfo.CommandQueueing, Is.False);
            Assert.That(vinfo.ScsiDeviceType, Is.EqualTo(ScsiDeviceType.DirectAccessDevice));
            Assert.That(vinfo.ScsiDeviceModifier, Is.EqualTo(0));
        }

        [Test]
        public void DriveCFolder()
        {
            VolumeDeviceInfo vinfo = new Win10.VolumeDeviceInfoWin10v2004(@"C:\Users\user\Desktop");
            Assert.That(vinfo.Path, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.CF));
            Assert.That(vinfo.VolumePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.CS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VC));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.CD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.C));
            IsDrive3(vinfo);
        }

        [Test]
        public void NetM()
        {
            VolumeDeviceInfo vinfo = new Win10.VolumeDeviceInfoWin10v2004(@"M:");
            Assert.That(vinfo.Path, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.M));
            Assert.That(vinfo.VolumePath, Is.Empty);        // Not a local mount
            Assert.That(vinfo.VolumeDevicePath, Is.Empty);  // Not a local mount
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.MD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.M));
        }

        [Test]
        public void NetMS()
        {
            VolumeDeviceInfo vinfo = new Win10.VolumeDeviceInfoWin10v2004(@"M:\");
            Assert.That(vinfo.Path, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.MS));
            Assert.That(vinfo.VolumePath, Is.Empty);        // Not a local mount
            Assert.That(vinfo.VolumeDevicePath, Is.Empty);  // Not a local mount
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.MD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.M));
        }

        [Test]
        public void SubstN()
        {
            VolumeDeviceInfo vinfo = new Win10.VolumeDeviceInfoWin10v2004(@"N:");
            Assert.That(vinfo.Path, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.N));
            Assert.That(vinfo.VolumePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.CS));  // Drive N: is mapped to C:
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VC));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.ND));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.N));
            IsDrive3(vinfo);
        }

        [Test]
        public void SubstNS()
        {
            VolumeDeviceInfo vinfo = new Win10.VolumeDeviceInfoWin10v2004(@"N:\");
            Assert.That(vinfo.Path, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.NS));
            Assert.That(vinfo.VolumePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.CS));  // Drive N: is mapped to C:
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VC));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.ND));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.N));
            IsDrive3(vinfo);
        }

        [Test]
        public void SubstO()
        {
            VolumeDeviceInfo vinfo = new Win10.VolumeDeviceInfoWin10v2004(@"O:");
            Assert.That(vinfo.Path, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.O));
            Assert.That(vinfo.VolumePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.DS));  // Drive O: is mapped to D:\books
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VD));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.OD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.O));
            IsDrive6(vinfo);
        }

        [Test]
        public void SubstOS()
        {
            VolumeDeviceInfo vinfo = new Win10.VolumeDeviceInfoWin10v2004(@"O:\");
            Assert.That(vinfo.Path, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.OS));
            Assert.That(vinfo.VolumePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.DS));  // Drive O: is mapped to D:\books
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VD));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.OD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.O));
            IsDrive6(vinfo);
        }

        [Test]
        public void SubstP()
        {
            VolumeDeviceInfo vinfo = new Win10.VolumeDeviceInfoWin10v2004(@"P:");
            Assert.That(vinfo.Path, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.P));
            Assert.That(vinfo.VolumePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.CS));  // Drive P: is mapped to N:\
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VC));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.PD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.P));
            IsDrive3(vinfo);
        }

        [Test]
        public void SubstPS()
        {
            VolumeDeviceInfo vinfo = new Win10.VolumeDeviceInfoWin10v2004(@"P:\");
            Assert.That(vinfo.Path, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.PS));
            Assert.That(vinfo.VolumePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.CS));  // Drive P: is mapped to N:\
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VC));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.PD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.P));
            IsDrive3(vinfo);
        }

        [Test]
        public void JunctionFromEToDToC()
        {
            VolumeDeviceInfo vinfo = new Win10.VolumeDeviceInfoWin10v2004(@"E:\efolder1\dfolder1\winlink");
            Assert.That(vinfo.Path, Is.EqualTo(@"E:\efolder1\dfolder1\winlink"));
            Assert.That(vinfo.VolumePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.CS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VC));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.CD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.C));
            IsDrive3(vinfo);
        }

        [Test]
        public void JunctionFromEToD()
        {
            VolumeDeviceInfo vinfo = new Win10.VolumeDeviceInfoWin10v2004(@"E:\efolder1\dfolder1");
            Assert.That(vinfo.Path, Is.EqualTo(@"E:\efolder1\dfolder1"));
            Assert.That(vinfo.VolumePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.DS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.VD));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.DD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(Win10.OSVolumeDeviceInfoWin10v2004.D));
            IsDrive6(vinfo);
        }
    }
}
