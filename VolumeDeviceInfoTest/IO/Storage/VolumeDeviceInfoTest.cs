namespace RJCP.IO.Storage
{
    using System.Runtime.Versioning;
    using NUnit.Framework;

    [TestFixture]
    public class VolumeDeviceInfoTest
    {
        [Test]
        [Platform("Win")]
        [SupportedOSPlatform("windows")]
        public void BootPartition()
        {
            // Checks that in general we can query the volume information.
            VolumeDeviceInfo volumeInfo = VolumeDeviceInfo.Create(@"\\.\BootPartition");
            Assert.That(volumeInfo.Path, Is.EqualTo(@"\\.\BootPartition"));
            Assert.That(volumeInfo.DriveType, Is.EqualTo(DriveType.Fixed));
            Assert.That(volumeInfo.Volume.DriveLetter, Is.Not.Null.Or.Empty);
            Assert.That(volumeInfo.Volume.DevicePath, Is.Not.Null.Or.Empty);
            Assert.That(volumeInfo.Disk.IsMediaPresent, Is.True);
            Assert.That(volumeInfo.Disk.IsReadOnly, Is.False);
            Assert.That(volumeInfo.FileSystem.Label, Is.Not.Null);
            Assert.That(volumeInfo.FileSystem.Serial, Is.Not.Null);
            Assert.That(volumeInfo.FileSystem.Name, Is.Not.Null);
        }

        [Test]
        [Platform("Win")]
        [SupportedOSPlatform("windows")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Assertion", "NUnit2010:Use EqualConstraint for better assertion messages in case of failure.", Justification = "Specific Test")]
        public void ObjectEquality()
        {
            VolumeDeviceInfo bootPart = VolumeDeviceInfo.Create(@"\\.\BootPartition");
            VolumeDeviceInfo bootDrive = VolumeDeviceInfo.Create(bootPart.Volume.DevicePath);

            // Object Equality
            Assert.That(bootPart, Is.EqualTo(bootDrive));
            Assert.That(bootDrive, Is.EqualTo(bootPart));
            Assert.That(bootPart.Equals(bootDrive));
            Assert.That(bootDrive.Equals(bootPart));
            Assert.That(bootPart.ToString(), Is.EqualTo(bootDrive.ToString()));
            Assert.That(bootPart.GetHashCode(), Is.EqualTo(bootDrive.GetHashCode()));

            // Reference Equality
            Assert.That(bootPart != bootDrive);
            Assert.That(bootDrive != bootPart);
            Assert.That(ReferenceEquals(bootPart, bootDrive), Is.False);
        }
    }
}
