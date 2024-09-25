using MauiCameraResize.Utilities;

namespace MauiCameraResize
{
    public partial class MainPage : ContentPage
    {
        DeviceHelper _device;

        public MainPage()
        {
            InitializeComponent();
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
    }
}
