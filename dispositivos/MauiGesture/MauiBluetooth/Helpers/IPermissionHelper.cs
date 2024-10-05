namespace MauiBluetooth.Helpers
{
    public interface IPermissionHelper
    {
        Task<PermissionStatus> RequestAllPermissionsAsync();
    }
}
