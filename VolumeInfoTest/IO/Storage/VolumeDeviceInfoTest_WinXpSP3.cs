namespace VolumeInfo.IO.Storage
{
    using NUnit.Framework;

    /// <summary>
    /// See <see cref="WinXP.OSVolumeDeviceInfoXpSP3"/> for definitions of the drives and tests being performed.
    /// </summary>
    [TestFixture]
    public class VolumeDeviceInfoTest_WinXpSP3
    {
        [Test]
        public void DriveC()
        {
            VolumeDeviceInfo vinfo = new WinXP.VolumeDeviceInfoXpSP3(@"C:");
            Assert.That(vinfo.Path, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.C));
            Assert.That(vinfo.VolumePath, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.CS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.VC));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.CD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.C));
            IsDrive1(vinfo);
        }

        [Test]
        public void DriveCS()
        {
            VolumeDeviceInfo vinfo = new WinXP.VolumeDeviceInfoXpSP3(@"C:\");
            Assert.That(vinfo.Path, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.CS));
            Assert.That(vinfo.VolumePath, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.CS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.VC));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.CD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.C));
            IsDrive1(vinfo);
        }

        [Test]
        public void DriveCV()
        {
            VolumeDeviceInfo vinfo = new WinXP.VolumeDeviceInfoXpSP3(@"\\?\Volume{77f8a1bc-e9e9-11ea-95c7-806d6172696f}");
            Assert.That(vinfo.Path, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.VC));
            Assert.That(vinfo.VolumePath, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.VCS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.VC));
            Assert.That(vinfo.VolumeDosDevicePath, Is.Empty);
            Assert.That(vinfo.VolumeDrive, Is.Empty);
            IsDrive1(vinfo);
        }

        [Test]
        public void DriveCVS()
        {
            VolumeDeviceInfo vinfo = new WinXP.VolumeDeviceInfoXpSP3(@"\\?\Volume{77f8a1bc-e9e9-11ea-95c7-806d6172696f}\");
            Assert.That(vinfo.Path, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.VCS));
            Assert.That(vinfo.VolumePath, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.VCS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.VC));
            Assert.That(vinfo.VolumeDosDevicePath, Is.Empty);
            Assert.That(vinfo.VolumeDrive, Is.Empty);
            IsDrive1(vinfo);
        }

        private void IsDrive1(VolumeDeviceInfo vinfo)
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
        }

        [Test]
        public void DriveCFolder()
        {
            VolumeDeviceInfo vinfo = new WinXP.VolumeDeviceInfoXpSP3(@"C:\Documents and Settings\User\Desktop");
            Assert.That(vinfo.Path, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.CF));
            Assert.That(vinfo.VolumePath, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.CS));
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.VC));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.CD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.C));
            IsDrive1(vinfo);
        }

        [Test]
        public void NetM()
        {
            VolumeDeviceInfo vinfo = new WinXP.VolumeDeviceInfoXpSP3(@"M:");
            Assert.That(vinfo.Path, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.M));
            Assert.That(vinfo.VolumePath, Is.Empty);        // Not a local mount
            Assert.That(vinfo.VolumeDevicePath, Is.Empty);  // Not a local mount
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.MD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.M));
        }

        [Test]
        public void NetMS()
        {
            VolumeDeviceInfo vinfo = new WinXP.VolumeDeviceInfoXpSP3(@"M:\");
            Assert.That(vinfo.Path, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.MS));
            Assert.That(vinfo.VolumePath, Is.Empty);        // Not a local mount
            Assert.That(vinfo.VolumeDevicePath, Is.Empty);  // Not a local mount
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.MD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.M));
        }

        [Test]
        public void SubstN()
        {
            VolumeDeviceInfo vinfo = new WinXP.VolumeDeviceInfoXpSP3(@"N:");
            Assert.That(vinfo.Path, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.N));
            Assert.That(vinfo.VolumePath, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.CS));  // Drive N: is mapped to C:
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.VC));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.ND));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.N));
            IsDrive1(vinfo);
        }

        [Test]
        public void SubstNS()
        {
            VolumeDeviceInfo vinfo = new WinXP.VolumeDeviceInfoXpSP3(@"N:\");
            Assert.That(vinfo.Path, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.NS));
            Assert.That(vinfo.VolumePath, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.CS));  // Drive N: is mapped to C:
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.VC));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.ND));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.N));
            IsDrive1(vinfo);
        }

        [Test]
        public void SubstO()
        {
            VolumeDeviceInfo vinfo = new WinXP.VolumeDeviceInfoXpSP3(@"O:");
            Assert.That(vinfo.Path, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.O));
            Assert.That(vinfo.VolumePath, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.CS));  // Drive O: is mapped to C:\WINDOWS
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.VC));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.OD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.O));
            IsDrive1(vinfo);
        }

        [Test]
        public void SubstOS()
        {
            VolumeDeviceInfo vinfo = new WinXP.VolumeDeviceInfoXpSP3(@"O:\");
            Assert.That(vinfo.Path, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.OS));
            Assert.That(vinfo.VolumePath, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.CS));  // Drive O: is mapped to C:\WINDOWS
            Assert.That(vinfo.VolumeDevicePath, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.VC));
            Assert.That(vinfo.VolumeDosDevicePath, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.OD));
            Assert.That(vinfo.VolumeDrive, Is.EqualTo(WinXP.OSVolumeDeviceInfoXpSP3.O));
            IsDrive1(vinfo);
        }
    }
}
