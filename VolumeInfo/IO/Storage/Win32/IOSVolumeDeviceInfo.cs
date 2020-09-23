namespace VolumeInfo.IO.Storage.Win32
{
    using System.IO;
    using System.Runtime.InteropServices;

    public interface IOSVolumeDeviceInfo
    {
        FileAttributes GetFileAttributes(string pathName);

        string GetVolumePathName(string pathName);

        string QueryDosDevice(string dosDevice);

        string GetVolumeNameForVolumeMountPoint(string pathName);

        SafeHandle CreateFileFromDevice(string pathName);

        bool RefreshVolume(SafeHandle hDevice);

        VolumeDeviceQuery GetStorageDeviceProperty(SafeHandle hDevice);

        VolumeInfo GetVolumeInformation(string devicePathName);

        StorageDeviceNumber GetDeviceNumber(SafeHandle hDevice);

        StorageDeviceNumber GetDeviceNumberEx(SafeHandle hDevice);

        DiskGeometry GetDiskGeometry(SafeHandle hDevice);

        bool GetMediaPresent(SafeHandle hDevice);

        BoolUnknown IncursSeekPenalty(SafeHandle hDevice);

        int GetLastWin32Error();
    }
}
