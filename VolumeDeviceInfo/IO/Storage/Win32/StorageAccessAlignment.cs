namespace RJCP.IO.Storage.Win32
{
    internal class StorageAccessAlignment
    {
        public int BytesPerCacheLine { get; set; }

        public int BytesOffsetForCacheAlignment { get; set; }

        public int BytesPerLogicalSector { get; set; }

        public int BytesPerPhysicalSector { get; set; }

        public int BytesOffsetForSectorAlignment { get; set; }
    }
}
