
using MauiCameraView.Utilities;

namespace MauiCameraView
{
    public partial class MainPage : ContentPage
    {
        DeviceHelper _device;

        public MainPage()
        {
            InitializeComponent();


            Navegador.Source = "https://esperanza.gobdigital.com.ar/web/vecino/app/login?appVersion=1";

            _device = new DeviceHelper();
        }

        private async void btnFoto_Clicked(object sender, EventArgs e)
        {
            lbnEstado.Text = "Tomando una foto";

            var imagen = await _device.TakePhoto(this);

            if (imagen != null)
            {
                Dispatcher.Dispatch(() =>
                {
                    myImage.Source = imagen;
                });
                await Shell.Current.DisplayAlert("Captura foto Realizada", "Resultado", "ok");
            }
            else
                await Shell.Current.DisplayAlert("Error Captura de foto", "Error", "ok");

            lbnEstado.Text = "Listo";
        }

        async private void Navegador_Navigating(object sender, WebNavigatingEventArgs e)
        {

            if (e.Url.Contains("action=PHOTO"))
            {
                e.Cancel = true;
                NavigationPage.SetHasNavigationBar(this, false);

                await TomarFoto();

                Navegador.Source = "about:blank";
            }
        }

        private void Navegador_Navigated(object sender, WebNavigatedEventArgs e)
        {

        }

        async private Task TomarFoto()
        {
            var imagen = await _device.TakePhoto(this);
        }
    }
}
