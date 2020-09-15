namespace VolumeInfo.IO.Storage.Win10
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "Correct case in the circumstances")]
    public class VolumeDeviceInfoWin10v2004 : VolumeDeviceInfo
    {
        public VolumeDeviceInfoWin10v2004(string pathName) : base(new OSVolumeDeviceInfoWin10v2004(), pathName) { }
    }
}
