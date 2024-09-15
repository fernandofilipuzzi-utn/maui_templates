namespace MauiQRDeviceTemplate
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }


        private async void btnModal_Clicked(object sender, EventArgs e)
        {
            var tcs = new TaskCompletionSource<string>();
            
            var barcodePage = new BarcodePage(tcs);
            await Navigation.PushAsync(barcodePage);
            string scannedValue = await tcs.Task;
            await DisplayAlert("Escaneo completado", $"Valor escaneado: {scannedValue}", "OK");
        }

        private async void btnGoTo_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("BarcodePage");
        }
    }
}
