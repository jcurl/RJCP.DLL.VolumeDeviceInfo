namespace VolumeInfo.IO.Storage.Win32
{
    public class VolumeDeviceInfoWin32Sim : VolumeDeviceInfoWin32
    {
        public VolumeDeviceInfoWin32Sim(string simFile, string pathName) :
            base(new OSVolumeDeviceInfoSim(simFile), pathName)
        { }
    }
}
