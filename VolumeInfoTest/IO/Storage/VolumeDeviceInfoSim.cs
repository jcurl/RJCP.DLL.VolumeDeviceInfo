namespace VolumeInfo.IO.Storage
{
    public class VolumeDeviceInfoSim : VolumeDeviceInfo
    {
        public VolumeDeviceInfoSim(string simFile, string pathName) :
            base(new Win32.OSVolumeDeviceInfoSim(simFile), pathName)
        { }
    }
}
