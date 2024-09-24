

//using Android.Graphics;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        DeviceHelper _device;
        public MainPage()
        {
            InitializeComponent();

            _device = new DeviceHelper();
        }

        private async void btnTomarFoto_Clicked(object sender, EventArgs e)
        {
            lbnHola.Text = "Hola, Fernando!";

            var result = await _device.TakePhoto(this);

            if (result != null)
            {
                myImage.Source = result?.Path; //imagen;
                await Shell.Current.DisplayAlert("Foto realizada", "Visualizando foto!", "ok");
            }
            else
                await Shell.Current.DisplayAlert("No hay foto que mostrar", "Error", "ok");
        }

        private async void btnIniciarLLamada_Clicked(object sender, EventArgs e)
        {
            lbnHola.Text = "Iniciando LLamada!, jaja";
            /*
            var phoneDialer = await _device.TakePhone(this);

            if (phoneDialer.CanMakePhoneCall)
            {
                phoneDialer.MakePhoneCall("3434807427");
                await Shell.Current.DisplayAlert("LLamada", "LLamada en curso!", "ok");
            }
            else
                await Shell.Current.DisplayAlert("LLamada", "Error en el Intento de llamada", "ok");
            */

            /*
            if (PhoneDialer.Default.IsSupported)
                PhoneDialer.Default.Open("3434807427");
            else
                await Shell.Current.DisplayAlert("No es posible llamar", "Error", "ok");
            */

            await RequestPhonePermission();
        }

        async Task RequestPhonePermission()
        {
            if (DeviceInfo.Platform != DevicePlatform.Android)
                return;

            var status = PermissionStatus.Unknown;
            {
                status = await Permissions.CheckStatusAsync<Permissions.Phone>();

                if (status == PermissionStatus.Granted)
                {
                    var dialer = new PhoneDialer();
                    dialer.CallPhone("343155174484");
                    return;
                }

                if (Permissions.ShouldShowRationale<Permissions.Phone>())
                {
                    await Shell.Current.DisplayAlert("Needs permissions", "BECAUSE!!!", "OK");
                }

                status = await Permissions.RequestAsync<Permissions.Phone>();

            }


            if (status != PermissionStatus.Granted)
                await Shell.Current.DisplayAlert("Permission required",
                    "Phone permission is required for calling. " +
                    "We just want to do a test.", "OK");

            else if (status == PermissionStatus.Granted)
            {
                var dialer = new PhoneDialer();
                dialer.CallPhone("343155174484");
            }
        }
    }
}