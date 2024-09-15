using BarcodeScanner.Mobile;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace MauiQRDevice
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

#if ANDROID
            BarcodeScanner.Mobile.Methods.SetSupportBarcodeFormat(BarcodeScanner.Mobile.BarcodeFormats.QRCode|
                BarcodeScanner.Mobile.BarcodeFormats.Code39
                );

            BarcodeScanner.Mobile.Methods.AskForRequiredPermission();
#endif
        }

        private void CameraView_OnDetected(object sender, BarcodeScanner.Mobile.OnDetectedEventArg e)
        {
            List<BarcodeResult> obj = e.BarcodeResults;

            string result = string.Empty;
            for (int i = 0; i < obj.Count; i++)
            {
                result += $"Type : {obj[i].BarcodeType}, Value : {obj[i].DisplayValue}{Environment.NewLine} ";
            }

            Dispatcher.Dispatch(async () =>
            {
                await DisplayAlert("Result", result, "OK");
                Camera.IsScanning = true;
            });
        }
    }

}
