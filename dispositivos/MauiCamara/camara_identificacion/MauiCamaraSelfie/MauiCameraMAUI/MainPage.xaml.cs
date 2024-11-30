using Camera.MAUI;

namespace MauiCameraMAUI
{
    public partial class MainPage : ContentPage
    {
  

        public MainPage()
        {
            InitializeComponent();
        }

        private void cameraView_CamerasLoaded(object sender, EventArgs e)
        {
            var frontCamera = cameraView.Cameras.FirstOrDefault(c => c.Position == CameraPosition.Front);


            cameraView.Camera = frontCamera;

            MainThread.BeginInvokeOnMainThread(async () =>
            {

                await cameraView.StopCameraAsync();
                await cameraView.StartCameraAsync();
                cameraView.ForceAutoFocus();

            });
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            myImage.Source = cameraView.GetSnapShot(Camera.MAUI.ImageFormat.PNG);
        }
    }

}
