namespace Prueba
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async private void Button_Clicked(object sender, EventArgs e)
        {
            Action<ImageSource> imageCapturedCallback = (capturedImage) =>
            {
                if (capturedImage != null)
                {
                    Dispatcher.Dispatch(() => MyImage.Source = capturedImage);
                }
            };

            var pageParams = new Dictionary<string, object>
            {
                { "OnImageCapturedCallback", imageCapturedCallback }
            };

            await Shell.Current.GoToAsync($"{nameof(FacePage)}", pageParams);
        }
    }
}
