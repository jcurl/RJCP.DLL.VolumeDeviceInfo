namespace RJCP.IO.Storage.Win32
{
    internal class DiskGeometry
    {
        public MediaType MediaType { get; set; }

        public long Cylinders { get; set; }

        public int TracksPerCylinder { get; set; }

        public int SectorsPerTrack { get; set; }

        public int BytesPerSector { get; set; }
    }
}
