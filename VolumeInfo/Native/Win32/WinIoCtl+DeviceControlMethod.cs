namespace VolumeInfo.Native.Win32
{
    internal partial class WinIoCtl
    {
        public enum DeviceControlMethod
        {
            Buffered = 0,
            InDirect = 1,
            OutDirect = 2,
            Neither = 3
        }
    }
}
