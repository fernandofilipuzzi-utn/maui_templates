

namespace MauiScanDevicesBluetooth.Helpers
{
    public class CustomPermissionsHelper//: IPermissionHelper
    {
        /*
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
        */

        public async Task<PermissionStatus> RequestAllPermissionsAsync()
        {
            var statusBluetoothScan = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            var statusBluetoothConnect = await Permissions.RequestAsync<BluetoothPermissions>();

            if (statusBluetoothScan != PermissionStatus.Granted || statusBluetoothConnect != PermissionStatus.Granted)
            {
                return PermissionStatus.Denied;
            }

            return PermissionStatus.Granted;
        }

        public class BluetoothPermissions : Permissions.BasePlatformPermission
        {
#if ANDROID
            public override (string androidPermission, bool isRuntime)[] RequiredPermissions => new[]
            {
        (Android.Manifest.Permission.BluetoothScan, true),
        (Android.Manifest.Permission.BluetoothConnect, true)
    };
#endif
        }
    }
}
