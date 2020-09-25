﻿namespace VolumeInfo.IO.Storage
{
    using System;

    public class GptPartition : PartitionInformation
    {
        public GptPartition() : base(PartitionStyle.GuidPartitionTable) { }

        public Guid Type { get; set; }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public EFIPartitionAttributes Attributes { get; set; }
    }
}
