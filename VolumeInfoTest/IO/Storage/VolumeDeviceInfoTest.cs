namespace VolumeInfo.IO.Storage
{
    using NUnit.Framework;

    [TestFixture]
    public class VolumeDeviceInfoTest
    {
        [Test]
        public void BootPartition()
        {
            // Checks that in general we can query the volume information.
            VolumeDeviceInfo volumeInfo = new VolumeDeviceInfo(@"\\.\BootPartition");
            Assert.That(volumeInfo.Path, Is.EqualTo(@"\\.\BootPartition"));
            Assert.That(volumeInfo.VolumeDrive, Is.Not.Null.Or.Empty);
            Assert.That(volumeInfo.VolumeDevicePath, Is.Not.Null.Or.Empty);
        }
    }
}
