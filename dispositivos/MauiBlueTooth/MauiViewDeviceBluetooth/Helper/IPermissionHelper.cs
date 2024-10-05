namespace MauiViewDeviceBluetooth.Helpers
{
    public interface IPermissionHelper
    {
        Task<PermissionStatus> RequestAllPermissionsAsync();
    }
}
