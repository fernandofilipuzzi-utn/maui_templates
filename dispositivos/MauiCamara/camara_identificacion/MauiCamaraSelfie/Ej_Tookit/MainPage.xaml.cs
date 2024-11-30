namespace Ej_Tookit
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void MyCamera_MediaCaptured(object? sender, CommunityToolkit.Maui.Views.MediaCapturedEventArgs e)
        {
            if (Dispatcher.IsDispatchRequired)
            {
                Dispatcher.Dispatch(() => MyImage.Source = ImageSource.FromStream(() => e.Media));
                return;
            }

            MyImage.Source = ImageSource.FromStream(() => e.Media);
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await MyCamera.CaptureImage(CancellationToken.None);
        }
    }

}
