namespace VolumeInfo.IO.Storage.Win10
{
    using System.IO;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Correct case in the circumstances")]
    public class OSVolumeDeviceInfoWin10v2004 : OSVolumeDeviceInfo
    {
        private static readonly VolumeDeviceQuery HarddiskVolume3 = new VolumeDeviceQuery() {
            VendorId = string.Empty,
            ProductId = "SAMSUNG MZFLV512HCJH-000MV",
            ProductRevision = "BXV75M0Q",
            DeviceSerialNumber = "0025_3844_61B5_6586.",
            BusType = BusType.Nvme,
            RemovableMedia = false,
            CommandQueueing = true,
            ScsiDeviceType = ScsiDeviceType.DirectAccessDevice,
            ScsiDeviceModifier = 0
        };

        private static readonly VolumeDeviceQuery HarddiskVolume5 = new VolumeDeviceQuery() {
            VendorId = "SanDisk",
            ProductId = "Cruzer Glide 3.0",
            ProductRevision = "1.00",
            DeviceSerialNumber = "4C530001620606126282",
            BusType = BusType.Usb,
            RemovableMedia = true,
            CommandQueueing = false,
            ScsiDeviceType = ScsiDeviceType.DirectAccessDevice,
            ScsiDeviceModifier = 0
        };

        private static readonly VolumeDeviceQuery HarddiskVolume6 = new VolumeDeviceQuery() {
            VendorId = "Generic-",
            ProductId = "SD Card",
            ProductRevision = "1.00",
            DeviceSerialNumber = string.Empty,
            BusType = BusType.Usb,
            RemovableMedia = true,
            CommandQueueing = false,
            ScsiDeviceType = ScsiDeviceType.DirectAccessDevice,
            ScsiDeviceModifier = 13
        };

        public const string C = @"C:";
        public const string CS = @"C:\";
        public const string CD = @"\Device\HarddiskVolume3";
        public const string VC = @"\\?\Volume{4a2248f3-ec20-4c41-b781-ff19fa7913e6}";
        public const string VCS = @"\\?\Volume{4a2248f3-ec20-4c41-b781-ff19fa7913e6}\";

        public const string CF = @"C:\Users\user\Desktop";

        public const string D = @"D:";
        public const string DS = @"D:\";
        public const string DD = @"\Device\HarddiskVolume6";
        public const string VD = @"\\?\Volume{d5fcf8f3-1d12-11ea-913c-806e6f6e6963}";
        public const string VDS = @"\\?\Volume{d5fcf8f3-1d12-11ea-913c-806e6f6e6963}\";

        public const string E = @"E:";
        public const string ES = @"E:\";
        public const string ED = @"\Device\HarddiskVolume5";
        public const string VE = @"\\?\Volume{299b5220-f500-11ea-9168-985fd3d32a6a}";
        public const string VES = @"\\?\Volume{299b5220-f500-11ea-9168-985fd3d32a6a}\";

        public const string M = @"M:";
        public const string MS = @"M:\";
        public const string MD = @"\Device\LanmanRedirector\;M:00000000001fec6c\localhost\share";

        public const string N = @"N:";
        public const string NS = @"N:\";
        public const string ND = @"\??\C:";
        public const string VN = VC;
        public const string VNS = VCS;

        public const string O = @"O:";
        public const string OS = @"O:\";
        public const string OD = @"\??\D:\books";
        public const string VO = VD;
        public const string VOS = VDS;

        public const string P = @"P:";
        public const string PS = @"P:\";
        public const string PD = @"\??\N:";
        public const string VP = VC;
        public const string VPS = VCS;

        public OSVolumeDeviceInfoWin10v2004()
        {
            // Drive C, is also the boot partition. Data is obtained from a test instance of Windows XP SP3 in a Virtual
            // Machine.
            SetFileAttributes(C, FileAttributes.Directory);
            SetQueryDosDevice(C, CD);
            SetVolumePathName(C, CS);
            SetVolumeNameForVolumeMountPoint(C, 0x7B);
            SetCreateFileFromDeviceError(C, 0x05);

            SetFileAttributes(CS, FileAttributes.Directory | FileAttributes.System | FileAttributes.Hidden);
            SetVolumePathName(CS, CS);
            SetVolumeNameForVolumeMountPoint(CS, VCS);
            SetCreateFileFromDeviceError(CS, 0x03);

            SetVolumePathName(VC, VCS);
            SetVolumeNameForVolumeMountPoint(VC, 0x7B);
            SetStorageDeviceProperty(VC, HarddiskVolume3);

            SetFileAttributes(VCS, FileAttributes.Directory | FileAttributes.System | FileAttributes.Hidden);
            SetVolumePathName(VCS, VCS);
            SetVolumeNameForVolumeMountPoint(VCS, VCS);
            SetCreateFileFromDeviceError(VCS, 0x03);

            // Drive D - Will be used for Junctions
            SetFileAttributes(D, FileAttributes.Directory);
            SetQueryDosDevice(D, DD);
            SetVolumePathName(D, DS);
            SetVolumeNameForVolumeMountPoint(D, 0x7B);
            SetCreateFileFromDeviceError(D, 0x05);

            SetFileAttributes(DS, FileAttributes.Directory);
            SetVolumePathName(DS, DS);
            SetVolumeNameForVolumeMountPoint(DS, VDS);
            SetCreateFileFromDeviceError(DS, 0x03);

            SetVolumePathName(VD, VDS);
            SetVolumeNameForVolumeMountPoint(VD, 0x7B);
            SetStorageDeviceProperty(VD, HarddiskVolume6);

            SetFileAttributes(VDS, FileAttributes.Directory);
            SetVolumePathName(VDS, VDS);
            SetVolumeNameForVolumeMountPoint(VDS, VDS);
            SetCreateFileFromDeviceError(VDS, 0x03);

            // Drive E - Will be used for Junctions
            SetFileAttributes(E, FileAttributes.Directory | FileAttributes.System | FileAttributes.Hidden);
            SetQueryDosDevice(E, ED);
            SetVolumePathName(E, ES);
            SetVolumeNameForVolumeMountPoint(E, 0x7B);
            SetCreateFileFromDeviceError(E, 0x05);

            SetFileAttributes(ES, FileAttributes.Directory | FileAttributes.System | FileAttributes.Hidden);
            SetVolumePathName(ES, ES);
            SetVolumeNameForVolumeMountPoint(ES, VES);
            SetCreateFileFromDeviceError(ES, 0x03);

            SetVolumePathName(VE, VES);
            SetVolumeNameForVolumeMountPoint(VE, 0x7B);
            SetStorageDeviceProperty(VE, HarddiskVolume5);

            SetFileAttributes(VES, FileAttributes.Directory | FileAttributes.System | FileAttributes.Hidden);
            SetVolumePathName(VES, VES);
            SetVolumeNameForVolumeMountPoint(VES, VES);
            SetCreateFileFromDeviceError(VES, 0x03);

            // A path in the file system.
            SetFileAttributes(CF, FileAttributes.Directory | FileAttributes.ReadOnly);
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
            SetVolumeNameForVolumeMountPoint(MS, 0x57);    // Changed from WinXP
            SetCreateFileFromDeviceError(MS, 0x7B);        // Changed from WinXP

            // N: is from "SUBST N: C:\"
            SetFileAttributes(N, FileAttributes.Directory | FileAttributes.System | FileAttributes.Hidden);
            SetQueryDosDevice(N, ND);
            SetVolumePathName(N, NS);
            SetVolumeNameForVolumeMountPoint(N, 0x7B);
            SetCreateFileFromDeviceError(N, 0x05);

            SetFileAttributes(NS, FileAttributes.Directory | FileAttributes.System | FileAttributes.Hidden);
            SetVolumePathName(NS, NS);
            SetVolumeNameForVolumeMountPoint(NS, VNS);
            SetCreateFileFromDeviceError(NS, 0x03);

            // O: is from "SUBST O: D:\books"
            SetFileAttributes(O, FileAttributes.Directory);
            SetQueryDosDevice(O, OD);
            SetVolumePathName(O, 0x57);                    // Changed from WinXP
            SetVolumeNameForVolumeMountPoint(O, 0x7B);
            SetCreateFileFromDeviceError(O, 0x05);

            SetFileAttributes(OS, FileAttributes.Directory);
            SetVolumePathName(OS, 0x57);                   // Changed from WinXP
            SetVolumeNameForVolumeMountPoint(OS, 0x57);
            SetCreateFileFromDeviceError(OS, 0x03);

            SetFileAttributes(@"D:\books", FileAttributes.Directory);
            SetVolumePathName(@"D:\books", DS);
            SetVolumeNameForVolumeMountPoint(@"D:\books", 0x7B);
            SetCreateFileFromDeviceError(@"D:\books", 0x05);

            // P: is from "SUBST P: N:\" (so a recursive subst)
            SetFileAttributes(P, FileAttributes.Directory | FileAttributes.System | FileAttributes.Hidden);
            SetQueryDosDevice(P, PD);
            SetVolumePathName(P, PS);
            SetVolumeNameForVolumeMountPoint(P, 0x7B);
            SetCreateFileFromDeviceError(P, 0x05);

            SetFileAttributes(PS, FileAttributes.Directory | FileAttributes.System | FileAttributes.Hidden);
            SetVolumePathName(PS, PS);
            SetVolumeNameForVolumeMountPoint(PS, VPS);
            SetCreateFileFromDeviceError(PS, 0x03);

            // junction e:\efolder1\dfolder1 d:\dfolder1
            // junction d:\dfolder1\winlink c:\windows
            SetFileAttributes(@"E:\efolder1", FileAttributes.Directory);
            SetVolumePathName(@"E:\efolder1", ES);
            SetVolumeNameForVolumeMountPoint(@"E:\efolder1", 0x7B);
            SetCreateFileFromDeviceError(@"E:\efolder1", 0x05);

            SetFileAttributes(@"E:\efolder1\dfolder1", FileAttributes.Directory | FileAttributes.ReparsePoint);
            SetVolumePathName(@"E:\efolder1\dfolder1", DS);
            SetVolumeNameForVolumeMountPoint(@"E:\efolder1\dfolder1", 0x7B);
            SetCreateFileFromDeviceError(@"E:\efolder1\dfolder1", 0x05);

            SetFileAttributes(@"E:\efolder1\dfolder1\winlink", FileAttributes.Directory | FileAttributes.ReparsePoint);
            SetVolumePathName(@"E:\efolder1\dfolder1\winlink", CS);
            SetVolumeNameForVolumeMountPoint(@"E:\efolder1\dfolder1\winlink", 0x7B);
            SetCreateFileFromDeviceError(@"E:\efolder1\dfolder1\winlink", 0x05);
        }
    }
}
