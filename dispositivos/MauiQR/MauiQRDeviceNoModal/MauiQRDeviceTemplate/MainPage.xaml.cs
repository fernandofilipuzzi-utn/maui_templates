using System.Reflection.Metadata;

namespace MauiQRDeviceTemplate
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }


        private async void btnGoTo_Clicked(object sender, EventArgs e)
        {
            var tcs = new TaskCompletionSource<string>();

            var pageParams = new Dictionary<string, object> 
            {
                { "Parametro", tcs}
            };

            await Shell.Current.GoToAsync("BarcodePage", pageParams);


            string scannedValue = await tcs.Task;
            await DisplayAlert("Escaneo completado", $"Valor escaneado: {scannedValue}", "OK");
        }
    }
}
