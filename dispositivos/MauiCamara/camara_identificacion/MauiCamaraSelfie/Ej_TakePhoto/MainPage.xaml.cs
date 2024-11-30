namespace Ej_TakePhoto
{
    public partial class MainPage : ContentPage
    {
   
        public MainPage()
        {
            InitializeComponent();
        }

        async private void Button_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    FileResult photo = await MediaPicker.Default.CapturePhotoAsync(new MediaPickerOptions
                    {
                        Title = "tome su foto",
                        
                    });

                    if (photo != null)
                    {
                        Stream stream = await photo.OpenReadAsync();

                        myImage.Source = ImageSource.FromStream(() => stream);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al capturar la foto: {ex.Message}");
            }
        }
    }

}
