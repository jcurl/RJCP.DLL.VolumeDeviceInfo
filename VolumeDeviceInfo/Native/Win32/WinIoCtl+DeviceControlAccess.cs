namespace RJCP.Native.Win32
{
    internal partial class WinIoCtl
    {
        public enum DeviceControlAccess
        {
            Any = 0,
            Special = Any,
            Read = 1,
            Write = 2
        }
    }
}
