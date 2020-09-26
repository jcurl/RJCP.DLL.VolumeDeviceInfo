# About VolumeInfo

This is a test application, intended for research and experimentation for
writing other applications I need. None-the-less, it might contain enough useful
code for others to read, and possibly use.

It is written in C# and targets Windows XP with .NET 4.0, which works quite well
up to and including the current Windows 10 v2004.

Under the hood, it uses a lot of P/Invoke calls to the Kernel, and IOCTL calls
to get the information required.

## Using VolumeInfo

Run it from the command line, and provide a list paths, drive letters, or
devices to gather information about, such as:

* The Volume which is referred to by the path. Volumes are Windows way of
  describing logical disks for a range of devices, like floppies, CDRoms, RAM
  disks, etc.
* Information about the partition for the volume
* Information about the file system
* Information about the disk

### Example: Drive Letter

```cmd
VolumeInfo C:\
```
```
Device Path: C:\
  Volume
    Volume Path     : C:\
    Volume Device   : \\?\Volume{4a2248f3-ec20-4c41-b781-ff19fa7913e6}
    Volume Drive    : C:
    NT DOS Device   : \Device\HarddiskVolume3
  Partition
    Partition Style : GuidPartitionTable
    Part Number     : 3
    Part Offset     : 18500000
    Part Length     : 475.6 GB
    GPT Attributes  : None
    GPT Name        : Basic data partition
    GPT Type        : ebd0a0a2-b9e5-4433-87c0-68b6b72699c7
    GPT Id          : 4a2248f3-ec20-4c41-b781-ff19fa7913e6
  File System
    Label           : Local Disk
    Serial Number   : D470-C5ED
    Flags           : CaseSensitiveSearch, CasePreservedNames, UnicodeOnDisk, PersistentAcls, FileCompression, VolumeQuotas, SupportsSparseFiles, SupportsReparsePoints, ReturnsCleanupResultInfo, SupportsPosixUnlinkRename, SupportsObjectIds, SupportsEncryption, NamedStreams, SupportsTransactions, SupportsHardLinks, SupportsExtendedAttributes, SupportsOpenByFileId, SupportsUsnJournal
    File System     : NTFS
  Device
    Vendor          :
    Product         : SAMSUNG MZFLV512HCJH-000MV; Revision BXV75M0Q
    SerialNumber    : 0025_3844_61B5_6586.
    Bus Type        : Non-Volatile Memory Express (NVMe)
    SCSI Device Type: Direct Access Device; SCSI Modifier: 0
    Command Queueing: True
    Removable Media : False
    Media Present   : True
    Disk Read Only  : False
    Device GUID:    : ba408eac-457b-e82e-f5ea-f5764f6a8c94 (Page83DeviceGuid)
    Device Number   : Disk #0
    Media Type      : FixedMedia
    Cyl/Trk/Sec/Byte: 62260/255/63/512 (476.9 GB)
    Bytes/Sector    : Physical 4096; Logical 512
    Seek Penalty    : False
```

Note, you should provide a trailing backslash (`\`) on the drive letter, so that
it refers to the root of the file system, and not the current directory for that
drive letter.

### Example: Windows Device Path

You can give it a Win32 Device path also, and it will look up the associated
volume and if it is mounted (and if so, which drive letter it is mounted as).

```cmd
VolumeInfo \\.\HarddiskVolume3
```

### Example: Physical Device

Use the SysInternals WinObj tool to determine what Physical Drives you have, and then query them.

```cmd
VolumeInfo \\.\PhysicalDrive0
```
```
Device Path: \\.\PhysicalDrive0
  Volume
    Volume Path     : \\.\PhysicalDrive0\
    Volume Device   : \\.\PhysicalDrive0
    Volume Drive    :
    NT DOS Device   :
  Partition
    Partition Style : GuidPartitionTable
    Part Number     : 0
    Part Offset     : 00000000
    Part Length     : 476.9 GB
    GPT Attributes  : None
    GPT Name        :
    GPT Type        : 00000000-0000-0000-0000-000000000000
    GPT Id          : 1180bb72-6dd1-4481-b0f3-9e0e1f1b61d0
  Device
    Vendor          :
    Product         : SAMSUNG MZFLV512HCJH-000MV; Revision BXV75M0Q
    SerialNumber    : 0025_3844_61B5_6586.
    Bus Type        : Non-Volatile Memory Express (NVMe)
    SCSI Device Type: Direct Access Device; SCSI Modifier: 0
    Command Queueing: True
    Removable Media : False
    Media Present   : True
    Disk Read Only  : False
    Device GUID:    : ba408eac-457b-e82e-f5ea-f5764f6a8c94 (Page83DeviceGuid)
    Device Number   : Disk #0
    Media Type      : FixedMedia
    Cyl/Trk/Sec/Byte: 62260/255/63/512 (476.9 GB)
    Bytes/Sector    : Physical 4096; Logical 512
    Seek Penalty    : False
```

### SUBST drives

It works with drives that have been SUBST'd and will resolve using the
`QueryDosDevice()` API to which path the drive is substituted to.

### Junction Points

If your code is iterating over the attributes of folders, you can get the volume
information of that folder, which might be different to the current drive
letter.

e.g.

`E:\efolder\ffolder` points to `F:\ffolder`

Then getting information about `E:\efolder\ffolder` will return details about
drive `F:`, as this is the path that is being pointed to.