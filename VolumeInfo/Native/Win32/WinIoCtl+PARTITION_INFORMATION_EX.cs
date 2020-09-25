namespace VolumeInfo.Native.Win32
{
    using System;
    using System.Runtime.InteropServices;
    using IO.Storage;

    internal partial class WinIoCtl
    {
        // Must be set to explicit, else it won't overlap with PARTITION_INFORMATION_MBR.
        //  See these resources:
        //  - https://stackoverflow.com/questions/14128093/marshaling-error-in-x64
        //  - https://stackoverflow.com/questions/35786764/creating-multiple-partitions-on-usb-using-c-sharp/35792276
        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Auto)]
        public struct PARTITION_INFORMATION_GPT
        {
            [FieldOffset(0)]
            public Guid PartitionType;

            [FieldOffset(16)]
            public Guid PartitionId;

            [FieldOffset(32)]
            [MarshalAs(UnmanagedType.U8)]
            public EFIPartitionAttributes Attributes;

            [FieldOffset(40)]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 36)]
            public string Name;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PARTITION_INFORMATION_MBR
        {
            public byte PartitionType;

            // The BootIndicator and RecognizedPartition have to be of type 'byte', as 'bool' is not blittable. Using:
            //  [MarshalAs(UnmanagedType.U1)] bool BootIndicator
            // will actually overwrite the GUID PartitionType fields for bytes 2 and 3 to be the value 01 (which, as
            // this is a union type, those two bytes in the GUID have the same location as the next two 'bool' fields).
            public byte BootIndicator;
            public byte RecognizedPartition;

            public uint HiddenSectors;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct PARTITION_INFORMATION_UNION
        {
            [FieldOffset(0)]
            public PARTITION_INFORMATION_GPT Gpt;

            [FieldOffset(0)]
            public PARTITION_INFORMATION_MBR Mbr;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct PARTITION_INFORMATION_EX
        {
            [MarshalAs(UnmanagedType.U4)]
            public PartitionStyle PartitionStyle;
            public long StartingOffset;
            public long PartitionLength;
            public int PartitionNumber;
            public byte RewritePartition;
            public byte IsServicePartition;
            public short _reserved;   // Alignment
            public PARTITION_INFORMATION_UNION DriveLayoutInformaiton;
        }
    }
}
