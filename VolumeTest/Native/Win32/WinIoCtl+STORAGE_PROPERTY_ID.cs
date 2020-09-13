namespace VolumeTest.Native.Win32
{
    internal partial class WinIoCtl
    {
        public enum STORAGE_PROPERTY_ID : uint
        {
            StorageDeviceProperty = 0,
            StorageAdapterProperty = 1,
            StorageDeviceIdProperty = 2,
            StorageDeviceUniqueIdProperty = 3,
            StorageDeviceWriteCacheProperty = 4,
            StorageMiniportProperty = 5,
            StorageAccessAlignmentProperty = 6,
            StorageDeviceSeekPenaltyProperty = 7,
            StorageDeviceTrimProperty = 8,
            StorageDeviceWriteAggregationProperty = 9,
            StorageDeviceDeviceTelemetryProperty = 10,
            StorageDeviceLBProvisioningProperty = 11,
            StorageDevicePowerProperty = 12,
            StorageDeviceCopyOffloadProperty = 13,
            StorageDeviceResiliencyProperty = 14,
            StorageDeviceMediumProductType = 15,
            StorageAdapterRpmbProperty = 16,
            StorageAdapterCryptoProperty = 17,
            StorageDeviceTieringProperty = 18,
            StorageDeviceFaultDomainProperty = 19,
            StorageDeviceClusportProperty = 20,
            StorageDeviceDependantDevicesProperty = 21,
            StorageDeviceIoCapabilityProperty = 48,
            StorageAdapterProtocolSpecificProperty = 49,
            StorageDeviceProtocolSpecificProperty = 50,
            StorageAdapterTemperatureProperty = 51,
            StorageDeviceTemperatureProperty = 52,
            StorageAdapterPhysicalTopologyProperty = 53,
            StorageDevicePhysicalTopologyProperty = 54,
            StorageDeviceAttributesProperty = 55,
            StorageDeviceManagementStatus = 56,
            StorageAdapterSerialNumberProperty = 57,
            StorageDeviceLocationProperty = 58,
            StorageDeviceNumaProperty = 59,
            StorageDeviceZonedDeviceProperty = 60,
            StorageDeviceUnsafeShutdownCount = 61,
            StorageDeviceEnduranceProperty = 62
        }
    }
}
