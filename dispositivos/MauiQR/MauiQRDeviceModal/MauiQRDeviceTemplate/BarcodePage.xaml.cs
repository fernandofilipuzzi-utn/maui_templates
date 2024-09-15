using BarcodeScanner.Mobile;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace MauiQRDeviceTemplate;

public partial class BarcodePage : ContentPage
{
    private TaskCompletionSource<string> _taskCompletionSource;

    public BarcodePage(TaskCompletionSource<string> taskCompletionSource)
    {
        InitializeComponent();

        _taskCompletionSource = taskCompletionSource;

#if ANDROID
        BarcodeScanner.Mobile.Methods.SetSupportBarcodeFormat(BarcodeScanner.Mobile.BarcodeFormats.QRCode|BarcodeScanner.Mobile.BarcodeFormats.Code39);
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

        _taskCompletionSource.TrySetResult(result);
        Dispatcher.Dispatch(async () =>
        {
            await DisplayAlert("Result", result, "OK");
            Camera.IsScanning = true;

            await Navigation.PopAsync();
        });
    }
}