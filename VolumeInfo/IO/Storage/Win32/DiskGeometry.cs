namespace VolumeInfo.IO.Storage.Win32
{
    public class DiskGeometry
    {
        public MediaType MediaType { get; set; }

        public long Cylinders { get; set; }

        public int TracksPerCylinder { get; set; }

        public int SectorsPerTrack { get; set; }

        public int BytesPerSector { get; set; }
    }
}
