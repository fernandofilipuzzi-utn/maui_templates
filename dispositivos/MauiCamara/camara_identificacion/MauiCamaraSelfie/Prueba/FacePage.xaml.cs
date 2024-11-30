
using CommunityToolkit.Maui.Core.Primitives;

namespace Prueba;

[QueryProperty(nameof(OnImageCapturedCallback), "OnImageCapturedCallback")]
public partial class FacePage : ContentPage
{
    public Action<ImageSource> OnImageCapturedCallback { get; set; }

    public FacePage()
	{
		InitializeComponent();

    }

    async private void MyCamera_MediaCaptured(object? sender, CommunityToolkit.Maui.Views.MediaCapturedEventArgs e)
    {
        if (MyCamera.IsAvailable == true)
        {
            var availableCameras = await MyCamera.GetAvailableCameras(CancellationToken.None);
            var frontCamera = availableCameras.FirstOrDefault(c => c.Position == CameraPosition.Front);

            if (frontCamera != null)
            {

                MyCamera.SelectedCamera = frontCamera;

            }

            if(e.Media!=null)
            { 
                var capturedImageSource = ImageSource.FromStream(() => e.Media);

                OnImageCapturedCallback?.Invoke(capturedImageSource);
            }
        }
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await MyCamera.CaptureImage(CancellationToken.None);

        await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);

    }
}