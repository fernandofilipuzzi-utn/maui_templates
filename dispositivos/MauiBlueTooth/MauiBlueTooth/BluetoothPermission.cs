using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices.Sensors;


namespace MauiBlueTooth
{
  
    public class BluetoothPermission : Permissions.BasePlatformPermission
    {
        // Para Android, necesitamos manejar específicamente los permisos de Bluetooth
#if ANDROID
        public override Task<PermissionStatus> CheckStatusAsync()
        {
            var status = PermissionStatus.Unknown;

            // Verifica si los permisos de ubicación están concedidos (necesario para BLE en Android)
            if (AndroidX.Core.Content.ContextCompat.CheckSelfPermission(Android.App.Application.Context, Android.Manifest.Permission.Bluetooth) == Android.Content.PM.Permission.Granted
                && AndroidX.Core.Content.ContextCompat.CheckSelfPermission(Android.App.Application.Context, Android.Manifest.Permission.BluetoothAdmin) == Android.Content.PM.Permission.Granted
                && AndroidX.Core.Content.ContextCompat.CheckSelfPermission(Android.App.Application.Context, Android.Manifest.Permission.AccessFineLocation) == Android.Content.PM.Permission.Granted)
            {
                status = PermissionStatus.Granted;
            }
            else
            {
                status = PermissionStatus.Denied;
            }

            return Task.FromResult(status);
        }

        public override async Task<PermissionStatus> RequestAsync()
        {
            var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            // Si la ubicación está concedida, también solicitamos permisos de Bluetooth
            if (status == PermissionStatus.Granted)
            {
                var bluetoothPermissionResult = new List<PermissionStatus>();

                var bluetoothPermission = await Permissions.RequestAsync<Permissions.Bluetooth>();
               // var bluetoothAdminPermission = await Permissions.RequestAsync<Permissions.BluetoothAdmin>();

                if (bluetoothPermission == PermissionStatus.Granted)// && bluetoothAdminPermission == PermissionStatus.Granted)
                {
                    return PermissionStatus.Granted;
                }
            }

            return PermissionStatus.Denied;
        }

#elif IOS
    // En iOS, no se requiere permiso específico de Bluetooth, pero es bueno verificar el estado del Bluetooth
    public override Task<PermissionStatus> CheckStatusAsync()
    {
        var bluetoothManager = Microsoft.Maui.Devices.Bluetooth.BluetoothManager.Default;

        return Task.FromResult(bluetoothManager.State == Microsoft.Maui.Devices.Bluetooth.BluetoothState.On ? PermissionStatus.Granted : PermissionStatus.Denied);
    }

    public override Task<PermissionStatus> RequestAsync()
    {
        // Para iOS no hay permiso específico de Bluetooth, pero podrías verificar si el Bluetooth está encendido.
        var bluetoothManager = Microsoft.Maui.Devices.Bluetooth.BluetoothManager.Default;

        return Task.FromResult(bluetoothManager.State == Microsoft.Maui.Devices.Bluetooth.BluetoothState.On ? PermissionStatus.Granted : PermissionStatus.Denied);
    }
#endif
    }

}
