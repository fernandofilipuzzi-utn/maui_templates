using static Microsoft.Maui.ApplicationModel.Permissions;

namespace MauiBlueTooth.CustomPermissions
{
    public class MyDevicesPermission : BasePlatformPermission
    {
#if ANDROID

        public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
            new List<(string permission, bool isRuntime)>
            {
                ("android.permission.BLUETOOTH", true),
                ("android.permission.BLUETOOTH_ADMIN", true),
                ("android.permission.BLUETOOTH_SCAN", true),
                ("android.permission.BLUETOOTH_CONNECT", true),
                ("android.permission.BLUETOOTH_ADVERTISE", true)
            }.ToArray();
#endif
    }
}