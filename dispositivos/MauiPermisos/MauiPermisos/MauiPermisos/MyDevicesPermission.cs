
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace MauiPermisos
{
    public class MyDevicesPermission : BasePlatformPermission
    {
#if ANDROID

        public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
            new List<(string permission, bool isRuntime)>
            {
                ("android.permission.CAMERA", true),
                ("android.permission.CALL_PHONE", true),
                ("android.permission.ACCESS_COARSE_LOCATION", true),
                ("android.permission.ACCESS_FINE_LOCATION", true)
            }.ToArray();
#endif
    }
}
