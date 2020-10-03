# 0. Development Information

This document covers some information on how to use this library, and what you
can expect when running on Windows systems.

Table of Contents

* 1.0 Introduction
  * 1.1 Basic Volume Information
  * 1.2 Disk Information, Geometry
  * 1.3 Partition Information
  * 1.4 Filesystem Information
* 2.0 Implementation Details
* 3.0 Some Configurations and the Results
  * 3.1 Basic Disk
  * 3.2 Physical Drive
  * 3.3 Floppy Disks
  * 3.4 CD-ROM Drive
  * 3.5 USB Card Reader
  * 3.6 Laptop Card Reader
  * 3.7 A SUBST drive
  * 3.8 ImDisk Virtual RAM Disk
  * 3.9 Network Drive
  * 3.10 USB Thumb Drives
  * 3.11 Windows Server 2003 R2 SP2 Spanning Volume
  * 3.12 Windows Server 2003 R2 SP2 with Mirrored Volume
  * 3.13 Windows ReFS File System with Windows 10 v2004
  * 3.14 Experimental: Windows Btrfs File System with Windows 10 v2004

## 1.0 Introduction

The simplest use case, which is valid for most users, is to get information
based on a drive letter. That volume is on a Basic Disk and is usually formatted
as NTFS on a single partition.

```csharp
using RJCP.IO.Storage;

VolumeDeviceInfo info = VolumeDeviceInfo.Create(@"C:\");
```

### 1.1 Basic Volume Information

There are groups of properties providing information for this disk.

* `info.Path` - This is the path that was given when the object was created, and
  in this example will be `C:\`.
* `info.DriveType` - The type of the drive. This is similar to the standard
  value by Windows (Fixed, Removable, CDRom, Remote, etc.) but also adds other
  drive types as necessary (notably, Floppy).
* `info.Volume` - Contains information related to the Volume (which is a Windows
  term). It contains:
  * `Path` - the windows path to the root of the volume. For the `C:` drive,
    this is usually `C:\`. This path is interesting for junctions that point to
    other volumes, or substituted drives, you'll get the path to that which
    might not be on the same disk being queried.
  * `DevicePath` - This is the name of the volume itself, as the Windows driver
    creates it. It's usually of the form
    `\\.\Volume{XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX}`.
  * `DosDevicePath` - The path that maps the drive letter to the Windows NT path
    name (not the Win32 path name), usually interesting for drivers. Modern
    versions of Windows allow access to the DOS device path through the prefix
    `\\.\GLOBALROOT`.
  * `DriveLetter` - This is the drive letter of the volume, if the volume is
    mounted with one. Not all volumes are visible via drive letters.

For example, a typical drive might return

```text
  Volume
    Drive Type      : Fixed
    Volume Path     : C:\
    Volume Device   : \\?\Volume{8e292c38-0000-0000-0000-500600000000}
    Volume Drive    : C:
    NT DOS Device   : \Device\HarddiskVolume2
```

### 1.2 Disk Information, Geometry

Because a volume usually resides on a single disk, there is information that can
be obtained about the physical disk where the volume is kept. This is contained
in the property `info.Disk`. This property is `null` if the volume doesn't exist
on a disk, e.g. such as a remote network drive.

* `info.Disk.Device` - Provides device specific information
  * `VendorId` - The vendor of the disk.
  * `ProductId` - The name of the product.
  * `ProductRevision` - Usually the version of the product.
  * `SerialNumber` - The serial number of the physical device.
  * `HasCommandQueueing` - Indicates if the drive supports command queuing (a
    performance feature).
  * `ScsiDeviceType` - The device type, as specified by the SCSI standard.
  * `ScsiDeviceModifier` - A device specific modifier as specified by the SCSI
    standard.
  * `BusType` - The technology used to connect the device to the system.
  * `GuidFlags` and `Guid` - A unique identifier (if available) for the device.
    The flags indicate if the identifier is available and if it's persistent.
* `info.Disk.Extents` An array of disk extents. This can be used to identify
  where the volume is physically mounted. On a basic disk, there is usually only
  one extent. For floppy drives, there are no extents and this field may be
  `null`. For mirrored drives, or spanned volumes, there may be multiple
  extents.
  * `Extents[n].Device` - The device name of the drive. You can instantiate a
    new instance of `VolumeDeviceInfo` with this string to get information about
    that physical drive.
  * `Extents[n].StartingOffset` - The offset, in bytes, to where this extent
    starts. For basic disks, with only one extent, this is usually the same as
    the start of the partition.
  * `Extents[n].ExtentLength` - The length of the extent, in bytes. For basic
    disks, with only one extent, this is usually the same as the length of the
    partition.
* `info.Disk.IsRemovableMedia` - Indicates if the media can be removed from the
  system. This is typical for USB thumbdrives, floppy disks and CDROMs, but may
  also be the case for some SSDs and HDDs.
* `info.Disk.IsMediaPresent` - Indicates if there is media present in the drive.
* `info.Disk.HasSeekPenalty` - Usually indicates if there is an access time
  associated with reading/writing from the device. This parameter appears to be
  unreliable.
* `info.Disk.IsReadOnly` - Indicates if the media is read only.
* `info.Disk.Geometry` - Provides geometric (logical and physical geometry) for
  the drive.
  * `Cylinders` - The number of logical cylinders for the disk, a legacy of old
    HDDs of 30 years ago.
  * `TracksPerCylinder` - The number of logical tracks per cylinder for the
    disk, a legacy of old HDDs of 30 years ago.
  * `SectorsPerTrack` - The number of logical sectors per track for the disk, a
    legacy of old HDDs of 30 years ago.
  * `BytesPerSector` - For HDDs, this is the logical number of bytes per sector.
    By using this parameter and the `Cylinders`, `TracksPerCylinder`,
    `SectorsPerTrack`, one can calculate the capacity of the drive. Older HDDs
    have physically 512 bytes per sector, newer drives have 4096 bytes per
    sector. This value is typically reported as 512.
  * `BytesPerPhysicalSector` - In about 2010-2012, drives were readily available
    that had physically 4096 bytes per sector, and mechanical hard drives
    emulated the sector to be 512 bytes. This indicates to the Operating System
    drivers the alignment of sectors (reads and writes should be read/written in
    this size and aligned as such).

For example, the `C:\` drive might return:

```text
  Device
    Extent: \\.\PhysicalDrive0
      Offset        : 0x06500000
      Length        : 237.9 GB
    Vendor          :
    Product         : Samsung SSD 840 PRO Series; Revision DXM05B0Q
    SerialNumber    : S1ATNEAD500134R
    Bus Type        : Serial AT Attachment (SATA)
    SCSI Device Type: Direct Access Device; SCSI Modifier: 0
    Command Queueing: True
    Device GUID:    : 0ac65c80-25a5-773d-8415-78af23e6f7e0 (Page83DeviceGuid)
    Removable Media : False
    Media Present   : True
    Disk Read Only  : False
    Cyl/Trk/Sec/Byte: 31130/255/63/512 (238.5 GB)
    Bytes/Sector    : Physical 512; Logical 512
    Seek Penalty    : False
```

### 1.3 Partition Information

Similarly, partition information can be provided with the `info.Partition`
property.  This property is `null` if the volume doesn't exist on a disk, e.g.
such as a remote network drive, or if the volume spans multiple partitions.

* `info.Partition` - General information about the partition.
  * `Style` - The type of the partition, if it is a GUID Partition Type (GPT) or
    a Master Boot Record (MBR) type.
  * `Number` - The partition number.
  * `Offset` - the offset, in bytes, to where the partition starts.
  * `Length` - the length of the partition, in bytes.

More information about the partition can be obtained depending on the `Style`.

| Style              | Type to cast to for more information |
|--------------------|--------------------------------------|
| MasterBootRecord   | IMbrPartition                        |
| GuidPartitionTable | IGptPartition                        |
| Raw                | no type cast                         |

A GPT entry contains the following extra information:

* `Type` - The GPT type, which is a GUID. This indicates if it is a Microsoft,
  EFT, or a Linux partition, among others.
* `Id` - An identifier for the partition
* `Name` - A readable name for the partition type.
* `Attributes` - Any attributes for the GPT entry.

An MBR partition entry contains the following extra information:

* `Type` - An 8-bit value for the partition type, e.g. 7 is used for NTFS and is
  common on Windows. 130, 131 are typical values for Linux.
* `Bootable` - A flag to tell the Operating System BIOS if this partition is
  bootable.
* `MbrSectorOffset` - This is the number sectors offset from the start of the
  partition to where the data for the partition begins.

An example of data that might be returned is:

```text
Partition
    Partition Style : MasterBootRecord
    Part Number     : 2
    Part Offset     : 0x06500000
    Part Length     : 237.9 GB
    MBR bootable    : False
    MBR Type        : 7
    MBR Offset      : 206848
```

### 1.4 Filesystem Information

The volume data is managed by a file system driver. The final piece of
information that can be obtained about the file system are:

* `info.FileSystem` - Contains information about the file system. This may be
  `null` if no file system is recognized, or if there is no media present.
  * `Label` - The label given to the file system.
  * `Serial` - A serial number. On Windows, this is a 32-bit value represented
    as a string `XXXX-XXXX`.
  * `Flags` - A set of flags indicating the features the file system supports.
    This might be Operating System dependent.
  * `Name` - The name of the file system format, e.g. `NTFS`.
  * `BytesPerSector` - This is the number of logical bytes per sector the file
    system uses internally.
  * `SectorsPerCluster` - This is the number of sectors per cluster, where a
    cluster is a single unit of storage for the file system.
  * `UserFreeBytes` - The number of free bytes available to the user to write to
    the file system. This may be the same, or less than the `TotalFreeBytes`.
  * `TotalFreeBytes` - The total number of free bytes available as reported by
    the file system.
  * `TotalBytes` - The capacity of the file system, in bytes.

  An example of some file system data that might be returned is:

```text
File System
    Label           :
    Serial Number   : 1EAB-A5B6
    Flags           : CaseSensitiveSearch, CasePreservedNames, UnicodeOnDisk,
                      PersistentAcls, FileCompression, VolumeQuotas,
                      SupportsSparseFiles, SupportsReparsePoints,
                      ReturnsCleanupResultInfo, SupportsPosixUnlinkRename,
                      SupportsObjectIds, SupportsEncryption, NamedStreams,
                      SupportsTransactions, SupportsHardLinks,
                      SupportsExtendedAttributes, SupportsOpenByFileId,
                      SupportsUsnJournal
    File System     : NTFS
    Bytes Per Sector: 512
    Sectors Per Clus: 8
    User Free       : 37.8 GB
    Total Free      : 37.8 GB
    Capacity        : 237.9 GB
```

## 2.0 Implementation Details

The library is implemented for .NET 4.0, using Kernel32.dll P/Invokes to query
the Operating System direct. There are no WMI calls (which are slow). Queries
are done using the File System API of Windows, or using DeviceIoControl commands
directly to the Windows drivers and capturing the results.

Unit test cases are written for various scenarios, captured into XML files
(recorded using the `VolumeInfo` tool), to help maintain high compatibility when
making changes without having to test repeatedly on hardware that might not be
available immediately for testing.

As such, it's reasonably fast (timeout operations are due to drivers refreshing
their cached values for the underlying file system).

It has been tested on Windows XP SP3 in a Virtual Machine, Windows 7 SP1,
Windows Server 2003 R2 SP2, and Windows 10 on real hardware with various
devices.

It works on both 32-bit and 64-bit Windows Operating Systems. Non-Windows
Operating Systems are not supported.

## 3.0 Some Configurations and the Results

This section tries to capture some scenarios tested, and the results provided by
this library. All results are available precisely in the form of unit tests in
this project, and should be referred to on *possible* behaviour. Behaviour may
change depending on the Operating System and the drivers of course.

Volume information is always made available, else an exception is raised when
creating a new `VolumeDeviceInfo` object.

Summary Table of Information Provided:

| Type            | Media? | DriveType       | Disk    | Disk.Device | Disk.Extents | Disk.Geometry | Partition | File System |
|-----------------|--------|-----------------|---------|-------------|--------------|---------------|-----------|-------------|
| Basic Disk      | TRUE   | Fixed/Removable | Present | Present     | 1 entry      | Present       | Present   | Present     |
| Physical Drive  | TRUE   | Fixed/Removable | Present | Present     | null         | Present       | Present   | null        |
| USB Floppy      | FALSE  | Floppy          | Present | Present     | null         | null          | null      | null        |
| USB Floppy      | TRUE   | Floppy          | Present | Present     | null         | Present       | null      | Present     |
| Floppy          | FALSE  | Floppy          | Present | null        | null         | null          | null      | null        |
| Floppy          | TRUE   | Floppy          | Present | null        | null         | Present       | null      | Present     |
| CDROM           | FALSE  | Cdrom           | Present | Present     | null         | null          | null      | null        |
| CDROM           | TRUE   | Cdrom           | Present | Present     | null         | Present       | Present   | Present     |
| USB Card Reader | FALSE  | Removable       | Present | Present     | 1 entry      | null          | null      | null        |
| USB Card Reader | TRUE   | Removable       | Present | Present     | 1 entry      | Present       | Present   | Present     |
| Laptop Card Rdr | TRUE   | Removable       | Present | Present     | 1 entry      | Present       | Present   | Present     |
| SUBST Drive     | -      | Fixed/Removable |
| ImDisk RAM Disk | TRUE   | Fixed           | Present | null        | null         | Present       | Present   | Present     |
| Network Drive   | -      | Remote          | null    | -           | -            | -             | null      | null        |
| USB Thumb       | TRUE   | Removable       | Present | Present     | 1 entry      | Present       | Present   | Present     |
| Spanning Volume | TRUE   | Fixed           | Present | null        | 2 entries    | Present       | null      | Present     |
| Mirrored Volume | TRUE   | Fixed           | Present | null        | 2 entries    | Present       | null      | Present     |

See the following sections for more specific information in addition.

### 3.1 Basic Disk

A basic disk is a file system on a single partition on a single hard disk. It is
the most common case provided.

* Because the volume resides on one disk, disk infrmation is present, device
  information can usually be obtained, along with the geometry information.
* There is only a single extent, the offset/legnth are the same as given in the
  partition table. One can use the `Device` to get the physical device the
  volume is mounted on, which upon query, should result in the same device and
  geometry information.

An example of a basic boot disk, with an MBR partition is:

```text
Device Path: C:\
  Volume
    Drive Type      : Fixed
    Volume Path     : C:\
    Volume Device   : \\?\Volume{8e292c38-0000-0000-0000-500600000000}
    Volume Drive    : C:
    NT DOS Device   : \Device\HarddiskVolume2
  Partition
    Partition Style : MasterBootRecord
    Part Number     : 2
    Part Offset     : 06500000
    Part Length     : 237.9 GB
    MBR bootable    : False
    MBR Type        : 7
    MBR Offset      : 206848
  File System
    Label           :
    Serial Number   : 1EAB-A5B6
    Flags           : CaseSensitiveSearch, CasePreservedNames, UnicodeOnDisk,
                      PersistentAcls, FileCompression, VolumeQuotas,
                      SupportsSparseFiles, SupportsReparsePoints,
                      ReturnsCleanupResultInfo, SupportsPosixUnlinkRename,
                      SupportsObjectIds, SupportsEncryption, NamedStreams,
                      SupportsTransactions, SupportsHardLinks,
                      SupportsExtendedAttributes, SupportsOpenByFileId,
                      SupportsUsnJournal
    File System     : NTFS
    Bytes Per Sector: 512
    Sectors Per Clus: 8
    User Free       : 37.8 GB
    Total Free      : 37.8 GB
    Capacity        : 237.9 GB
  Device
    Extent: \\.\PhysicalDrive0
      Offset        : 06500000
      Length        : 237.9 GB
    Vendor          :
    Product         : Samsung SSD 840 PRO Series; Revision DXM05B0Q
    SerialNumber    : S1ATNEAD500134R
    Bus Type        : Serial AT Attachment (SATA)
    SCSI Device Type: Direct Access Device; SCSI Modifier: 0
    Command Queueing: True
    Device GUID:    : 0ac65c80-25a5-773d-8415-78af23e6f7e0 (Page83DeviceGuid)
    Removable Media : False
    Media Present   : True
    Disk Read Only  : False
    Cyl/Trk/Sec/Byte: 31130/255/63/512 (238.5 GB)
    Bytes/Sector    : Physical 512; Logical 512
    Seek Penalty    : False
```

### 3.2 Physical Drive

When querying a disk, the extent device value returns the name of the physical
drive that can be queried. The string itself is generated based on documentation
from MSDN, which takes the device number and appends that to
`\\.\PhysicalDrive`.

When querying the physical drive, the disk device and geometry information is
usually the same as the basic disk. It might be surprising to see that partition
information is made available.

GPT disks have a specific type for the partition type that encompasses the
entire disk. The partition for the entire disk has the partition number of zero,
and generally describes the entire capacity of the disk.

An example of the physical drive for the drive `C:\` given above:

```text
Device Path: \\.\PhysicalDrive0
  Volume
    Drive Type      : Fixed
    Volume Path     : \\.\PhysicalDrive0\
    Volume Device   : \\.\PhysicalDrive0
    Volume Drive    :
    NT DOS Device   :
  Partition
    Partition Style : MasterBootRecord
    Part Number     : 0
    Part Offset     : 00000000
    Part Length     : 238.5 GB
    MBR bootable    : False
    MBR Type        : 0
    MBR Offset      : 0
  Device
    Vendor          :
    Product         : Samsung SSD 840 PRO Series; Revision DXM05B0Q
    SerialNumber    : S1ATNEAD500134R
    Bus Type        : Serial AT Attachment (SATA)
    SCSI Device Type: Direct Access Device; SCSI Modifier: 0
    Command Queueing: True
    Device GUID:    : 0ac65c80-25a5-773d-8415-78af23e6f7e0 (Page83DeviceGuid)
    Removable Media : False
    Media Present   : True
    Disk Read Only  : False
    Cyl/Trk/Sec/Byte: 31130/255/63/512 (238.5 GB)
    Bytes/Sector    : Physical 512; Logical 512
    Seek Penalty    : False
```

### 3.3 Floppy Disks

Floppy disks are the way of the Dodo, but they're supported by Windows, and
there's still enough USB floppy disk readers available, that it might still be
useful. The software implementation doesn't return the type of the floppy disk,
only that it is. You can obtain the floppy disk information through the
geometry.

There is no partition information associated with a floppy disk.

For no floppy disk present, the following information is returned:

```text
Device Path: A:\
  Volume
    Drive Type      : Floppy
    Volume Path     : A:\
    Volume Device   : \\?\Volume{f35b67a3-fda7-11ea-b203-7085c2221e14}
    Volume Drive    : A:
    NT DOS Device   : \Device\Floppy0
  Device
    Vendor          : TEAC
    Product         : FD-05PUB        ; Revision 3000
    SerialNumber    :
    Bus Type        : Universal Serial Bus (USB)
    SCSI Device Type: Direct Access Device; SCSI Modifier: 0
    Command Queueing: False
    Device GUID:    : 31332e98-04cd-11eb-b205-806e6f6e6963 (RandomDeviceGuidReasonNoHwId)
    Removable Media : True
    Media Present   : False
    Disk Read Only  : True
    Seek Penalty    : Unknown
```

Insert a floppy disk, and the following is shown:

```text
Device Path: A:\
  Volume
    Drive Type      : Floppy
    Volume Path     : A:\
    Volume Device   : \\?\Volume{f35b67a3-fda7-11ea-b203-7085c2221e14}
    Volume Drive    : A:
    NT DOS Device   : \Device\Floppy0
  File System
    Label           :
    Serial Number   : 0D18-1AEE
    Flags           : CasePreservedNames, UnicodeOnDisk, ReturnsCleanupResultInfo,
                      ReadOnlyVolume
    File System     : FAT
    Bytes Per Sector: 512
    Sectors Per Clus: 1
    User Free       : 0.0 GB
    Total Free      : 0.0 GB
    Capacity        : 0.0 GB
  Device
    Vendor          : TEAC
    Product         : FD-05PUB        ; Revision 3000
    SerialNumber    :
    Bus Type        : Universal Serial Bus (USB)
    SCSI Device Type: Direct Access Device; SCSI Modifier: 0
    Command Queueing: False
    Device GUID:    : 31332e98-04cd-11eb-b205-806e6f6e6963 (RandomDeviceGuidReasonNoHwId)
    Removable Media : True
    Media Present   : True
    Disk Read Only  : True
    Cyl/Trk/Sec/Byte: 80/2/18/512 (0.0 GB)
    Bytes/Sector    : Physical 512; Logical 512
    Seek Penalty    : Unknown
```

A physical Floppy drive emulated in VMWare for Windows XP. There is no Device
information present here, like for the USB implementation.

```text
Device Path: A:\
  Volume
    Drive Type      : Floppy
    Volume Path     : A:\
    Volume Device   : \\?\Volume{5619d5dc-ffd3-11ea-8e38-000c29553b8c}
    Volume Drive    : A:
    NT DOS Device   : \Device\Floppy0
  File System
    Label           : FLOPPY
    Serial Number   : 6C26-C2C2
    Flags           : CasePreservedNames, UnicodeOnDisk
    File System     : FAT
    Bytes Per Sector: 512
    Sectors Per Clus: 1
    User Free       : 0.0 GB
    Total Free      : 0.0 GB
    Capacity        : 0.0 GB
  Device
    Removable Media : True
    Media Present   : True
    Disk Read Only  : False
    Cyl/Trk/Sec/Byte: 80/2/18/512 (0.0 GB)
    Bytes/Sector    : Physical 512; Logical 512
    Seek Penalty    : Unknown
```

And a physical Floppy drive emulated in VMWare without a disk.

```text
Device Path: B:\
  Volume
    Drive Type      : Floppy
    Volume Path     : B:\
    Volume Device   : \\?\Volume{1833df12-ffe0-11ea-8e39-000c29553b8c}
    Volume Drive    : B:
    NT DOS Device   : \Device\Floppy1
  Device
    Removable Media : True
    Media Present   : False
    Disk Read Only  : True
    Seek Penalty    : Unknown
```

### 3.4 CD-ROM Drive

A CDROM drive can provide device information when no disk is inserted:

```text
Device Path: F:\
  Volume
    Drive Type      : CdRom
    Volume Path     : F:\
    Volume Device   : \\?\Volume{71045c0e-add5-11e7-b180-806e6f6e6963}
    Volume Drive    : F:
    NT DOS Device   : \Device\CdRom0
  Device
    Vendor          : TSSTcorp
    Product         : DVD+-RW TS-H653G; Revision DW10
    SerialNumber    :
    Bus Type        : Serial AT Attachment (SATA)
    SCSI Device Type: CD-ROM Device; SCSI Modifier: 0
    Command Queueing: False
    Device GUID:    : 00000000-0000-0000-0000-000000000000 (None)
    Removable Media : True
    Media Present   : False
    Disk Read Only  : True
    Seek Penalty    : Unknown
```

When a disk is inserted, more information becomes available:

```text
Device Path: F:\
  Volume
    Drive Type      : CdRom
    Volume Path     : F:\
    Volume Device   : \\?\Volume{71045c0e-add5-11e7-b180-806e6f6e6963}
    Volume Drive    : F:
    NT DOS Device   : \Device\CdRom0
  Partition
    Partition Style : MasterBootRecord
    Part Number     : 0
    Part Offset     : 00000000
    Part Length     : 1.5 GB
    MBR bootable    : False
    MBR Type        : 11
    MBR Offset      : 0
  File System
    Label           : Ubuntu-Budgie 18
    Serial Number   : 2F09-5357
    Flags           : CaseSensitiveSearch, UnicodeOnDisk, ReadOnlyVolume,
                      SupportsOpenByFileId
    File System     : CDFS
    Bytes Per Sector: 2048
    Sectors Per Clus: 1
    User Free       : 0.0 GB
    Total Free      : 0.0 GB
    Capacity        : 1.5 GB
  Device
    Vendor          : TSSTcorp
    Product         : DVD+-RW TS-H653G; Revision DW10
    SerialNumber    :
    Bus Type        : Serial AT Attachment (SATA)
    SCSI Device Type: CD-ROM Device; SCSI Modifier: 0
    Command Queueing: False
    Device GUID:    : 00000000-0000-0000-0000-000000000000 (None)
    Removable Media : True
    Media Present   : True
    Disk Read Only  : True
    Cyl/Trk/Sec/Byte: 380/64/32/2048 (1.5 GB)
    Bytes/Sector    : Physical 2048; Logical 2048
    Seek Penalty    : Unknown
```

When the disk is present, partition information is available, with partition 0
(the entire disk).

### 3.5 USB Card Reader

The example given here is a USB Card Reader from DELL. It provides information
with and without a card inserted.

```text
Device Path: H:\
  Volume
    Drive Type      : Removable
    Volume Path     : H:\
    Volume Device   : \\?\Volume{71045c21-add5-11e7-b180-806e6f6e6963}
    Volume Drive    : H:
    NT DOS Device   : \Device\HarddiskVolume6
  Device
    Extent: \\.\PhysicalDrive3
      Offset        : 00000000
      Length        : 0.0 GB
    Vendor          : DELL
    Product         : USB   HS-CF Card; Revision 7.12
    SerialNumber    : 00000208A3E0
    Bus Type        : Universal Serial Bus (USB)
    SCSI Device Type: Direct Access Device; SCSI Modifier: 0
    Command Queueing: False
    Device GUID:    : ab937ae9-e419-2f45-32f5-fa67c2e6c520 (None)
    Removable Media : True
    Media Present   : False
    Disk Read Only  : True
    Seek Penalty    : Unknown
```

It's interesting to see that disk extents are available, even when no card is
present, although the offset and length are zero, which would be expected.

Inserting a card provides the following information:

```text
Device Path: H:\
  Volume
    Drive Type      : Removable
    Volume Path     : H:\
    Volume Device   : \\?\Volume{71045c21-add5-11e7-b180-806e6f6e6963}
    Volume Drive    : H:
    NT DOS Device   : \Device\HarddiskVolume6
  Partition
    Partition Style : MasterBootRecord
    Part Number     : 1
    Part Offset     : 00007E00
    Part Length     : 3.8 GB
    MBR bootable    : True
    MBR Type        : 12
    MBR Offset      : 63
  File System
    Label           : EOS_DIGITAL
    Serial Number   : 0000-0000
    Flags           : CasePreservedNames, UnicodeOnDisk, ReturnsCleanupResultInfo,
                      SupportsEncryption
    File System     : FAT32
    Bytes Per Sector: 512
    Sectors Per Clus: 64
    User Free       : 2.6 GB
    Total Free      : 2.6 GB
    Capacity        : 3.8 GB
  Device
    Extent: \\.\PhysicalDrive3
      Offset        : 00007E00
      Length        : 3.8 GB
    Vendor          : DELL
    Product         : USB   HS-CF Card; Revision 7.12
    SerialNumber    : 00000208A3E0
    Bus Type        : Universal Serial Bus (USB)
    SCSI Device Type: Direct Access Device; SCSI Modifier: 0
    Command Queueing: False
    Device GUID:    : ab937ae9-e419-2f45-32f5-fa67c2e6c520 (None)
    Removable Media : True
    Media Present   : True
    Disk Read Only  : False
    Cyl/Trk/Sec/Byte: 499/255/63/512 (3.8 GB)
    Bytes/Sector    : Physical 512; Logical 512
    Seek Penalty    : Unknown
```

The Device GUID may change between computer restarts.

### 3.6 Laptop Card Reader

Using the integrated card reader in a Surface Book Pro, where the card is
encrypted using BitLocker but the password is not yet entered:

```text
Device Path: D:\
  Volume
    Drive Type      : Removable
    Volume Path     : D:\
    Volume Device   : \\?\Volume{d5fcf8f3-1d12-11ea-913c-806e6f6e6963}
    Volume Drive    : D:
    NT DOS Device   : \Device\HarddiskVolume9
  Partition
    Partition Style : MasterBootRecord
    Part Number     : 1
    Part Offset     : 01000000
    Part Length     : 59.6 GB
    MBR bootable    : False
    MBR Type        : 7
    MBR Offset      : 32768
  Device
    Extent: \\.\PhysicalDrive1
      Offset        : 01000000
      Length        : 59.6 GB
    Vendor          : Generic-
    Product         : SD Card         ; Revision 1.00
    SerialNumber    :
    Bus Type        : Universal Serial Bus (USB)
    SCSI Device Type: Direct Access Device; SCSI Modifier: 13
    Command Queueing: False
    Device GUID:    : 537baabc-f7f9-11ea-9169-985fd3d32a6a (RandomDeviceGuidReasonNoHwId)
    Removable Media : True
    Media Present   : True
    Disk Read Only  : True
    Cyl/Trk/Sec/Byte: 7783/255/63/512 (59.6 GB)
    Bytes/Sector    : Physical 512; Logical 512
    Seek Penalty    : Unknown
```

Enter the BitLocker password:

```text
Device Path: D:\
  Volume
    Drive Type      : Removable
    Volume Path     : D:\
    Volume Device   : \\?\Volume{d5fcf8f3-1d12-11ea-913c-806e6f6e6963}
    Volume Drive    : D:
    NT DOS Device   : \Device\HarddiskVolume9
  Partition
    Partition Style : MasterBootRecord
    Part Number     : 1
    Part Offset     : 01000000
    Part Length     : 59.6 GB
    MBR bootable    : False
    MBR Type        : 7
    MBR Offset      : 32768
  File System
    Label           : Samsung64GB
    Serial Number   : 0000-0000
    Flags           : CasePreservedNames, UnicodeOnDisk, ReturnsCleanupResultInfo,
                      SupportsEncryption
    File System     : exFAT
    Bytes Per Sector: 512
    Sectors Per Clus: 256
    User Free       : 35.2 GB
    Total Free      : 35.2 GB
    Capacity        : 59.6 GB
  Device
    Extent: \\.\PhysicalDrive1
      Offset        : 01000000
      Length        : 59.6 GB
    Vendor          : Generic-
    Product         : SD Card         ; Revision 1.00
    SerialNumber    :
    Bus Type        : Universal Serial Bus (USB)
    SCSI Device Type: Direct Access Device; SCSI Modifier: 13
    Command Queueing: False
    Device GUID:    : 537baabc-f7f9-11ea-9169-985fd3d32a6a (RandomDeviceGuidReasonNoHwId)
    Removable Media : True
    Media Present   : True
    Disk Read Only  : False
    Cyl/Trk/Sec/Byte: 7783/255/63/512 (59.6 GB)
    Bytes/Sector    : Physical 512; Logical 512
    Seek Penalty    : Unknown
```

Remove the SD Card from the reader:

```text
Device Path: D:\
  Error: Path can't be resolved to a volume
```

This means that the Volume Manager sees the device no longer inserted and has
unmounted the volume, with details of the device no longer available.

### 3.7 A SUBST drive

A drive mapped with SUBST is handled specially. The drive that is being
substituted returns information about the actual device. But we can see through
the NT DosDevice what the mapping is.

Otherwise, all other information points to the actual drive that would otherwise
be accessed.

For example:

```cmd
> SUBST P: C:\
```

The following results would be shown:

```text
Device Path: P:\
  Volume
    Drive Type      : Fixed
    Volume Path     : C:\
    Volume Device   : \\?\Volume{8e292c38-0000-0000-0000-500600000000}
    Volume Drive    : P:
    NT DOS Device   : \??\C:
  Partition
    Partition Style : MasterBootRecord
    Part Number     : 2
    Part Offset     : 06500000
    Part Length     : 237.9 GB
    MBR bootable    : False
    MBR Type        : 7
    MBR Offset      : 206848
  File System
    Label           :
    Serial Number   : 1EAB-A5B6
    Flags           : CaseSensitiveSearch, CasePreservedNames, UnicodeOnDisk,
                      PersistentAcls, FileCompression, VolumeQuotas, SupportsSparseFiles,
                      SupportsReparsePoints, ReturnsCleanupResultInfo,
                      SupportsPosixUnlinkRename, SupportsObjectIds, SupportsEncryption,
                      NamedStreams, SupportsTransactions, SupportsHardLinks,
                      SupportsExtendedAttributes, SupportsOpenByFileId, SupportsUsnJournal
    File System     : NTFS
    Bytes Per Sector: 512
    Sectors Per Clus: 8
    User Free       : 37.8 GB
    Total Free      : 37.8 GB
    Capacity        : 237.9 GB
  Device
    Extent: \\.\PhysicalDrive0
      Offset        : 06500000
      Length        : 237.9 GB
    Vendor          :
    Product         : Samsung SSD 840 PRO Series; Revision DXM05B0Q
    SerialNumber    : S1ATNEAD500134R
    Bus Type        : Serial AT Attachment (SATA)
    SCSI Device Type: Direct Access Device; SCSI Modifier: 0
    Command Queueing: True
    Device GUID:    : 0ac65c80-25a5-773d-8415-78af23e6f7e0 (Page83DeviceGuid)
    Removable Media : False
    Media Present   : True
    Disk Read Only  : False
    Cyl/Trk/Sec/Byte: 31130/255/63/512 (238.5 GB)
    Bytes/Sector    : Physical 512; Logical 512
    Seek Penalty    : False
```

Note that everything is identical to that for the basic disk example prior, only
the Volume Drive Letter and the NT Dos Device indicate this is a different drive
letter. This library will take the prefix `\??\` and treat this as a SUBST
drive.

### 3.8 ImDisk Virtual RAM Disk

The free software ImDisk allows to create a virtual RAM disk in memory, to
simulate a real disk. There are some limitations of this driver though - it
doesn't register through the Volume Manager, and so some functionality is lost.

```text
Device Path: R:\
  Volume
    Drive Type      : Fixed
    Volume Path     : \\.\GLOBALROOT\Device\ImDisk0\
    Volume Device   : \\.\GLOBALROOT\Device\ImDisk0
    Volume Drive    : R:
    NT DOS Device   : \Device\ImDisk0
  Partition
    Partition Style : MasterBootRecord
    Part Number     : 1
    Part Offset     : 00007E00
    Part Length     : 0.5 GB
    MBR bootable    : False
    MBR Type        : 6
    MBR Offset      : 1
  File System
    Label           : RAMDISK
    Serial Number   : C858-F289
    Flags           : CaseSensitiveSearch, CasePreservedNames, UnicodeOnDisk,
                      PersistentAcls, FileCompression, VolumeQuotas,
                      SupportsSparseFiles, SupportsReparsePoints,
                      ReturnsCleanupResultInfo, SupportsPosixUnlinkRename,
                      SupportsObjectIds, SupportsEncryption, NamedStreams,
                      SupportsTransactions, SupportsHardLinks, SupportsExtendedAttributes,
                      SupportsOpenByFileId, SupportsUsnJournal
    File System     : NTFS
    Bytes Per Sector: 512
    Sectors Per Clus: 8
    User Free       : 0.5 GB
    Total Free      : 0.5 GB
    Capacity        : 0.5 GB
  Device
    Removable Media : False
    Media Present   : True
    Disk Read Only  : False
    Cyl/Trk/Sec/Byte: 130/128/63/512 (0.5 GB)
    Bytes/Sector    : Physical 512; Logical 512
    Seek Penalty    : Unknown
```

It's not possible to get a volume path (because the driver doesn't register with
the Volume Manager, the mapping of the drive to a volume returns `null`). Thus
the trick that modern Windows makes the device available via the prefix
``\\.\GLOBALROOT`, which can be used to gather information.

This driver would return via the Win32 API, that the device number is zero and
that the Device Type is Disk. Normally, one can use this to determine a physical
disk (`\\.\PhysicalDrive0`), but this is obviously wrong for the ImDisk driver.

We see that this driver has no backing store that can be used, as there are no
extents.

### 3.9 Network Drive

A network drive provides almost no information, only enough to indicate that it
is a remote file system.

```text
Device Path: M:\
  Volume
    Drive Type      : Remote
    Volume Path     :
    Volume Device   :
    Volume Drive    : M:
    NT DOS Device   : \Device\LanmanRedirector\;M:0000000006db30ab\localhost\share
```

## 3.10 USB Thumb Drives

This are pretty common and behave similar to basic disks.

```text
Device Path: E:\
  Volume
    Drive Type      : Removable
    Volume Path     : E:\
    Volume Device   : \\?\Volume{299b5220-f500-11ea-9168-985fd3d32a6a}
    Volume Drive    : E:
    NT DOS Device   : \Device\HarddiskVolume39
  Partition
    Partition Style : MasterBootRecord
    Part Number     : 1
    Part Offset     : 00100000
    Part Length     : 58.7 GB
    MBR bootable    : False
    MBR Type        : 7
    MBR Offset      : 2048
  File System
    Label           : USB
    Serial Number   : 36F8-F5F6
    Flags           : CaseSensitiveSearch, CasePreservedNames, UnicodeOnDisk,
                      PersistentAcls, FileCompression, VolumeQuotas,
                      SupportsSparseFiles, SupportsReparsePoints,
                      ReturnsCleanupResultInfo, SupportsPosixUnlinkRename,
                      SupportsObjectIds, SupportsEncryption, NamedStreams,
                      SupportsTransactions, SupportsHardLinks,
                      SupportsExtendedAttributes, SupportsOpenByFileId,
                      SupportsUsnJournal
    File System     : NTFS
    Bytes Per Sector: 512
    Sectors Per Clus: 8
    User Free       : 58.6 GB
    Total Free      : 58.6 GB
    Capacity        : 58.7 GB
  Device
    Extent: \\.\PhysicalDrive2
      Offset        : 00100000
      Length        : 58.7 GB
    Vendor          : SanDisk
    Product         : Cruzer Glide 3.0; Revision 1.00
    SerialNumber    : 4C530001120606116282
    Bus Type        : Universal Serial Bus (USB)
    SCSI Device Type: Direct Access Device; SCSI Modifier: 0
    Command Queueing: False
    Device GUID:    : a183a5b5-d50a-86a9-188b-739eab126a73 (None)
    Removable Media : True
    Media Present   : True
    Disk Read Only  : False
    Cyl/Trk/Sec/Byte: 7664/255/63/512 (58.7 GB)
    Bytes/Sector    : Physical 512; Logical 512
    Seek Penalty    : Unknown
```

Some drives behave badly, for example, this drive is inserted, but likely there
was a USB error and the disk was unmounted.

```text
Device Path: F:\
  Volume
    Drive Type      : Removable
    Volume Path     : F:\
    Volume Device   : \\?\Volume{58184c44-f6bc-11ea-9169-985fd3d32a6a}
    Volume Drive    : F:
    NT DOS Device   : \Device\HarddiskVolume40
  Device
    Extent: \\.\PhysicalDrive3
      Offset        : 00000000
      Length        : 0.0 GB
    Vendor          : SanDisk
    Product         : U3 Cruzer Micro ; Revision 8.02
    SerialNumber    : 43202207CA531669
    Bus Type        : Universal Serial Bus (USB)
    SCSI Device Type: Direct Access Device; SCSI Modifier: 0
    Command Queueing: False
    Device GUID:    : 8efcc1d1-b02a-05d7-56a6-06aaae42be6a (None)
    Removable Media : True
    Media Present   : False
    Disk Read Only  : True
    Seek Penalty    : Unknown
```

### 3.11 Windows Server 2003 R2 SP2 Spanning Volume

A disk was set up that has three partitions. A Volume was created on the first
partition, a different volume on the second. The first volume was extended to
the third partition:

```text
+------------------+------------------+------------------+
| SIMPLE VOLUME    | SIMPLE VOLUME    | SIMPLE VOLUME    |
+------------------+------------------+------------------+
| Simple (F:)      | Simple2          | Simple (F:)      |
| 4.00 GB NTFS     | 4.00 GB NTFS     | 7.00 GB NTFS     |
| Healthy          | Healthy          | Healthy          |
+------------------+------------------+------------------+
```

In the setup, the volume `Simple2` was not assigned a drive letter. This can be
found by enumerating through volumes however.

Running the output on drive `F:` above results in:

```text
Device Path: F:\
  Volume
    Drive Type      : Fixed
    Volume Path     : F:\
    Volume Device   : \\?\Volume{273d5d74-ead4-4847-8846-941c99522a66}
    Volume Drive    : F:
    NT DOS Device   : \Device\HarddiskDmVolumes\Xxx-d02058760efDg0\Volume2
  File System
    Label           : Simple
    Serial Number   : 54D5-9860
    Flags           : CaseSensitiveSearch, CasePreservedNames, UnicodeOnDisk,
                      PersistentAcls, FileCompression, VolumeQuotas,
                      SupportsSparseFiles, SupportsReparsePoints,
                      SupportsObjectIds, SupportsEncryption, NamedStreams
    File System     : NTFS
    Bytes Per Sector: 512
    Sectors Per Clus: 8
    User Free       : 11.0 GB
    Total Free      : 11.0 GB
    Capacity        : 11.0 GB
  Device
    Extent: \\.\PhysicalDrive3
      Offset        : 00007E00
      Length        : 4.0 GB
    Extent: \\.\PhysicalDrive3
      Offset        : 200007E00
      Length        : 7.0 GB
    Removable Media : False
    Media Present   : True
    Disk Read Only  : False
    Cyl/Trk/Sec/Byte: 1958/255/63/512 (15.0 GB)
    Bytes/Sector    : Physical 512; Logical 512
    Seek Penalty    : Unknown
```

You'll observe there is no partition information. This can actually be obtained
from the disk extents, of which there are now two.

To get information about the disk themselves, you need to take the drive given
by the extent and run the query using the `\\.\PhysicalDrive3`.

### 3.12 Windows Server 2003 R2 SP2 with Mirrored Volume

Under Windows, the following mirrored volume was set up:

```text
+--------------------------------------------------------+
| MIRRORED VOLUME                                        |
+--------------------------------------------------------+
| MIRROR (E:)                                            |
| 10.00 GB NTFS                                          |
| Healthy                                                |
+--------------------------------------------------------+

+--------------------------------------------------------+
| MIRRORED VOLUME                                        |
+--------------------------------------------------------+
| MIRROR (E:)                                            |
| 10.00 GB NTFS                                          |
| Healthy                                                |
+--------------------------------------------------------+
```

Then running a query on drive `E:\` provides

```text
Device Path: E:\
  Volume
    Drive Type      : Fixed
    Volume Path     : E:\
    Volume Device   : \\?\Volume{78d41c0e-348a-49e2-b7de-b92956381bd3}
    Volume Drive    : E:
    NT DOS Device   : \Device\HarddiskDmVolumes\Xxx-d02058760efDg0\Volume1
  File System
    Label           : MIRROR
    Serial Number   : 28E7-CE46
    Flags           : CaseSensitiveSearch, CasePreservedNames, UnicodeOnDisk,
                      PersistentAcls, FileCompression, VolumeQuotas,
                      SupportsSparseFiles, SupportsReparsePoints,
                      SupportsObjectIds, SupportsEncryption, NamedStreams
    File System     : NTFS
    Bytes Per Sector: 512
    Sectors Per Clus: 8
    User Free       : 9.9 GB
    Total Free      : 9.9 GB
    Capacity        : 10.0 GB
  Device
    Extent: \\.\PhysicalDrive2
      Offset        : 00007000
      Length        : 10.0 GB
    Extent: \\.\PhysicalDrive1
      Offset        : 00007000
      Length        : 10.0 GB
    Removable Media : False
    Media Present   : True
    Disk Read Only  : False
    Cyl/Trk/Sec/Byte: 1468/255/56/512 (10.0 GB)
    Bytes/Sector    : Physical 512; Logical 512
    Seek Penalty    : Unknown
```

We can now see that two physical drives are used, and they're both of the same
offset and length.

### 3.13 Windows ReFS File System with Windows 10 v2004

A quick overview of how the ReFS filesystem is reported on Windows 10:

```text
Device Path: E:\
  Volume
    Drive Type      : Fixed
    Volume Path     : E:\
    Volume Device   : \\?\Volume{6ba1874e-0072-4554-a054-9f83d01164e0}
    Volume Drive    : E:
    NT DOS Device   : \Device\HarddiskVolume6
  Partition
    Partition Style : GuidPartitionTable
    Part Number     : 2
    Part Offset     : 01000000
    Part Length     : 10,0 GB
    GPT Attributes  : None
    GPT Name        : Basic data partition
    GPT Type        : ebd0a0a2-b9e5-4433-87c0-68b6b72699c7
    GPT Id          : 6ba1874e-0072-4554-a054-9f83d01164e0
  File System
    Label           : ReFS
    Serial Number   : EC23-936A
    Flags           : CaseSensitiveSearch, CasePreservedNames, UnicodeOnDisk,
                      PersistentAcls, SupportsSparseFiles, SupportsReparsePoints,
                      ReturnsCleanupResultInfo, NamedStreams, SupportsOpenByFileId,
                      SupportsUsnJournal, SupportsIntegrityStreams,
                      SupportsBlockRefCounting, SupportsSparseVdl, SupportsGhosting
    File System     : ReFS
    Bytes Per Sector: 512
    Sectors Per Clus: 8
    User Free       : 8,9 GB
    Total Free      : 8,9 GB
    Capacity        : 9,9 GB
  Device
    Extent: \\.\PhysicalDrive1
      Offset        : 01000000
      Length        : 10,0 GB
    Vendor          :
    Product         : VMware Virtual SATA Hard Drive; Revision 00000001
    SerialNumber    : 02000000000000000001
    Bus Type        : Serial AT Attachment (SATA)
    SCSI Device Type: Direct Access Device; SCSI Modifier: 0
    Command Queueing: True
    Device GUID:    : dd792752-8065-f007-853d-4d68b2de9045 (Page83DeviceGuid)
    Removable Media : False
    Media Present   : True
    Disk Read Only  : False
    Cyl/Trk/Sec/Byte: 1305/255/63/512 (10,0 GB)
    Bytes/Sector    : Physical 512; Logical 512
    Seek Penalty    : Unknown
  ```

### 3.14 Experimental: Windows Btrfs File System with Windows 10 v2004

I tried installing BTRFS on a Windows 10 v2004 Enterprise (Secure Boot must be
disabled for it to install properly, and the driver is somewhat experimental),
but we can see how Windows responds with this tool. The version tested here is
v1.7.4.

```text
Device Path: F:
  Volume
    Drive Type      : Fixed
    Volume Path     : F:\
    Volume Device   : \\?\Volume{8e4e907a-7424-0d28-5c95-53777a124a6f}
    Volume Drive    : F:
    NT DOS Device   : \Device\Btrfs{7a904e8e-2474-280d-5c95-53777a124a6f}
  Partition
    Partition Style : GuidPartitionTable
    Part Number     : 2
    Part Offset     : 01000000
    Part Length     : 10.0 GB
    GPT Attributes  : GptBasicDataAttributeNoDriveLetter
    GPT Name        : Basic data partition
    GPT Type        : ebd0a0a2-b9e5-4433-87c0-68b6b72699c7
    GPT Id          : 649e42c4-caa5-4932-9271-5b9339019dec
  File System
    Label           :
    Serial Number   : 7A12-4A6F
    Flags           : CaseSensitiveSearch, CasePreservedNames, UnicodeOnDisk,
                      PersistentAcls, SupportsSparseFiles, SupportsReparsePoints,
                      SupportsPosixUnlinkRename, SupportsObjectIds, NamedStreams,
                      SupportsHardLinks, SupportsExtendedAttributes,
                      SupportsOpenByFileId, SupportsBlockRefCounting
    File System     : Btrfs
    Bytes Per Sector: 512
    Sectors Per Clus: 8
    User Free       : 10.0 GB
    Total Free      : 10.0 GB
    Capacity        : 10.0 GB
  Device
    Extent: \\.\PhysicalDrive1
      Offset        : 01000000
      Length        : 10.0 GB
    Vendor          :
    Product         : VMware Virtual SATA Hard Drive; Revision 00000001
    SerialNumber    : 02000000000000000001
    Bus Type        : Serial AT Attachment (SATA)
    SCSI Device Type: Direct Access Device; SCSI Modifier: 0
    Command Queueing: True
    Device GUID:    : dd792752-8065-f007-853d-4d68b2de9045 (Page83DeviceGuid)
    Removable Media : False
    Media Present   : True
    Disk Read Only  : False
    Cyl/Trk/Sec/Byte: 162/255/63/4096 (9,9 GB)
    Bytes/Sector    : Physical 512; Logical 4096
    Seek Penalty    : Unknown
  ```

So it appears that the logical block size is 4096, which is different to the
reports for all other software. I suspect this is because there might be
confusion between the logical disk geometry, and the cluster size for BTRFS.
