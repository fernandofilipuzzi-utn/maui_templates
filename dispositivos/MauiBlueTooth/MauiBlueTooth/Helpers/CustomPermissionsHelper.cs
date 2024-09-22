using MauiBluetooth.CustomPermissions;

namespace MauiBluetooth.Helpers
{
    public class CustomPermissionsHelper : IPermissionHelper
    {
        public async Task<PermissionStatus> RequestAllPermissionsAsync()
        {
            var status = PermissionStatus.Unknown;
            if (DeviceInfo.Version.Major >= 12)
            {
                status = await Permissions.CheckStatusAsync<MyDevicesPermission>();

                if (Permissions.ShouldShowRationale<MyDevicesPermission>())
                {
                    await Shell.Current.DisplayAlert("Se necesitan los permisos ", "justificación", "OK");
                }

                status = await Permissions.RequestAsync<MyDevicesPermission>();

                if (status != PermissionStatus.Granted)
                {
                    await Shell.Current.DisplayAlert("Permisos requeridos", "permisos en general", "OK");
                }
            }
            return status;
        }
    }
}
