using MauiPermisos.Helpers;

namespace MauiPermisos
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnBtnPermisoIndividualClicked(object sender, EventArgs e)
        {
            var status=await new CustomPermissionsHelper().RequestAllPermissionsAsync();

            if (status == PermissionStatus.Granted)
            {
                await Shell.Current.DisplayAlert("OK! ", "Resultado", "OK");
            }
        }

        private async void OnBtnPermisosPersonalizadosClicked(object sender, EventArgs e)
        {
            if (DeviceInfo.Platform != DevicePlatform.Android)
                return;

            var status = await new CustomPermissionsHelper().RequestAllPermissionsAsync();

            if (status == PermissionStatus.Granted)
            {
                await Shell.Current.DisplayAlert("OK! ", "Resultado", "OK");
            }
        }
    }
}
