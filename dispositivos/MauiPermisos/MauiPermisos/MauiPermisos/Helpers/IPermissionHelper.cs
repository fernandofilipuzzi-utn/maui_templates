
namespace MauiPermisos.Helpers
{
    public interface IPermissionHelper
    {
       Task<PermissionStatus> RequestAllPermissionsAsync();
    }
}
