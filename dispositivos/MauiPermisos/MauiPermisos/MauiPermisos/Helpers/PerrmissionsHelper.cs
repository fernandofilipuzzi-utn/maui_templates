namespace MauiPermisos.Helpers
{
    public class PermissionsHelper:IPermissionHelper
    {
        public async Task<PermissionStatus> RequestAllPermissionsAsync()
        {
            var status = PermissionStatus.Unknown;
            status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status == PermissionStatus.Granted)
                return status;

            if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
            {
                await Shell.Current.DisplayAlert("Se necesitan los permisos ", "justificación", "OK");
            }

            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            if (status != PermissionStatus.Granted)
            {
                await Shell.Current.DisplayAlert("Permisos requeridos", "permisos para bluethooth", "OK");
            }
            return status;
        }
    }
}
