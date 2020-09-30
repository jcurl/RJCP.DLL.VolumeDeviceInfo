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
            Assert.That(volumeInfo.Volume.DriveLetter, Is.Not.Null.Or.Empty);
            Assert.That(volumeInfo.Volume.DevicePath, Is.Not.Null.Or.Empty);
            Assert.That(volumeInfo.Volume.DriveType, Is.EqualTo(DriveType.Fixed));
            Assert.That(volumeInfo.Disk.IsMediaPresent, Is.True);
            Assert.That(volumeInfo.Disk.IsReadOnly, Is.False);
            Assert.That(volumeInfo.FileSystem.Label, Is.Not.Null);
            Assert.That(volumeInfo.FileSystem.Serial, Is.Not.Null);
            Assert.That(volumeInfo.FileSystem.Name, Is.Not.Null);
        }
    }
}
