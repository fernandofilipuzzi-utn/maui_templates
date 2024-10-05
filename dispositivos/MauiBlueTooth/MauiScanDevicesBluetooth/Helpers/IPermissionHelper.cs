namespace MauiScanDevicesBluetooth.Helpers
{
    public interface IPermissionHelper
    {
        Task<PermissionStatus> RequestAllPermissionsAsync();
    }
}
