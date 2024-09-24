namespace MauiCamera
{
    public partial class MainPage : ContentPage
    {
        DeviceHelper _device;

        public MainPage()
        {
            InitializeComponent();
            _device = new DeviceHelper();
        }

        private async void btnPermiso_Clicked(object sender, EventArgs e)
        {
            lbnHola.Text = "Hola, Fernando!";

            //string imagen=await _device.TakePhoto(this);
            var imagen = await _device.TakePhoto(this);

            if (imagen != null)
            {
                myImage.Source = imagen;
                await Shell.Current.DisplayAlert("Captura foto Realizada", "Resultado", "ok");
            }
            else
                await Shell.Current.DisplayAlert("Error Captura de foto", "Error", "ok");
        }
    }

}
