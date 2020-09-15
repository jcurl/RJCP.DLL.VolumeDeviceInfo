namespace VolumeInfo.IO.Storage.WinXP
{
    using System.IO;

    public class OSVolumeDeviceInfoXpSP3 : OSVolumeDeviceInfo
    {
        private static readonly VolumeDeviceQuery HarddiskVolume1 = new VolumeDeviceQuery() {
            VendorId = string.Empty,
            ProductId = "VMware Virtual IDE Hard Drive",
            ProductRevision = "00000001",
            DeviceSerialNumber = "3030303030303030303",
            BusType = BusType.Ata,
            RemovableMedia = false,
            CommandQueueing = false,
            ScsiDeviceType = ScsiDeviceType.DirectAccessDevice,
            ScsiDeviceModifier = 0
        };

        public const string C = @"C:";
        public const string CS = @"C:\";
        public const string CD = @"\Device\HarddiskVolume1";
        public const string VC = @"\\?\Volume{77f8a1bc-e9e9-11ea-95c7-806d6172696f}";
        public const string VCS = @"\\?\Volume{77f8a1bc-e9e9-11ea-95c7-806d6172696f}\";

        public const string CF = @"C:\Documents and Settings\User\Desktop";

        public const string M = @"M:";
        public const string MS = @"M:\";
        public const string MD = @"\Device\LanmanRedirector\;M:000000000000d8c6\devtest\share";

        public const string N = @"N:";
        public const string NS = @"N:\";
        public const string ND = @"\??\C:";
        public const string VN = VC;
        public const string VNS = VCS;

        public const string O = @"O:";
        public const string OS = @"O:\";
        public const string OD = @"\??\C:\WINDOWS";
        public const string VO = VC;
        public const string VOS = VCS;

        public OSVolumeDeviceInfoXpSP3()
        {
            // Drive C, is also the boot partition. Data is obtained from a test instance of Windows XP SP3 in a Virtual
            // Machine.
            SetFileAttributes(C, FileAttributes.Directory);
            SetQueryDosDevice(C, CD);
            SetVolumePathName(C, CS);
            SetVolumeNameForVolumeMountPoint(C, 0x7B);
            SetCreateFileFromDeviceError(C, 0x05);

            SetFileAttributes(CS, FileAttributes.Directory | FileAttributes.System | FileAttributes.Hidden | FileAttributes.Archive);
            SetVolumePathName(CS, CS);
            SetVolumeNameForVolumeMountPoint(CS, VCS);
            SetCreateFileFromDeviceError(CS, 0x03);

            SetVolumePathName(VC, VCS);
            SetVolumeNameForVolumeMountPoint(VC, 0x7B);
            SetStorageDeviceProperty(VC, HarddiskVolume1);

            SetFileAttributes(VCS, FileAttributes.Directory | FileAttributes.System | FileAttributes.Hidden | FileAttributes.Archive);
            SetVolumePathName(VCS, VCS);
            SetVolumeNameForVolumeMountPoint(VCS, VCS);
            SetCreateFileFromDeviceError(VCS, 0x03);

            // A path in the file system.
            SetFileAttributes(CF, FileAttributes.Directory);
            SetVolumePathName(CF, CS);
            SetVolumeNameForVolumeMountPoint(CF, 0x7B);
            SetCreateFileFromDeviceError(CF, 0x05);

            // Network File System
            SetFileAttributes(M, FileAttributes.Directory);
            SetQueryDosDevice(M, MD);
            SetVolumePathName(M, MS);
            SetVolumeNameForVolumeMountPoint(M, 0x7B);
            SetCreateFileFromDeviceError(M, 0x7B);

            SetFileAttributes(MS, FileAttributes.Directory);
            SetVolumePathName(MS, MS);
            SetVolumeNameForVolumeMountPoint(MS, 0x7B);
            SetCreateFileFromDeviceError(MS, 0x02);

            // N: is from "SUBST N: C:\"
            SetFileAttributes(N, FileAttributes.Directory | FileAttributes.System | FileAttributes.Hidden | FileAttributes.Archive);
            SetQueryDosDevice(N, ND);
            SetVolumePathName(N, NS);
            SetVolumeNameForVolumeMountPoint(N, 0x7B);
            SetCreateFileFromDeviceError(N, 0x05);

            SetFileAttributes(NS, FileAttributes.Directory | FileAttributes.System | FileAttributes.Hidden | FileAttributes.Archive);
            SetVolumePathName(NS, NS);
            SetVolumeNameForVolumeMountPoint(NS, VNS);
            SetCreateFileFromDeviceError(NS, 0x03);

            // O: is from "SUBST O: C:\WINDOWS"
            SetFileAttributes(O, FileAttributes.Directory);
            SetQueryDosDevice(O, OD);
            SetVolumePathName(O, OS);
            SetVolumeNameForVolumeMountPoint(O, 0x7B);
            SetCreateFileFromDeviceError(O, 0x05);

            SetFileAttributes(OS, FileAttributes.Directory);
            SetVolumePathName(OS, OS);
            SetVolumeNameForVolumeMountPoint(OS, 0x57);
            SetCreateFileFromDeviceError(OS, 0x03);

            SetFileAttributes(@"C:\WINDOWS", FileAttributes.Directory);
            SetVolumePathName(@"C:\WINDOWS", CS);
            SetVolumeNameForVolumeMountPoint(@"C:\WINDOWS", 0x7B);
            SetCreateFileFromDeviceError(@"C:\WINDOWS", 0x05);
        }
    }
}
