using MauiApp3.Utils;
using SkiaSharp;

namespace MauiApp3
{
    public partial class MainPage : ContentPage
    {


        public MainPage()
        {
            InitializeComponent();
        }
        /*
        private async void OnCounterClicked(object sender, EventArgs e)
        {
            FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

            if (photo != null)
            {
                var stream = await photo.OpenReadAsync();
                myImage.Source = ImageSource.FromStream(() => stream);
            }
        }
        */

        /*
        private async void OnCounterClicked(object sender, EventArgs e)
        {
            try
            {
                FileResult? photo = await MediaPicker.Default.CapturePhotoAsync();

                byte[] buffer = new byte[0];

                if (photo != null)
                {
               
                    using (Stream sphoto = await photo.OpenReadAsync())
                    {
                        buffer=await new PhotoDevice { maxWidthHeight =1000,
                                                       compressionQuality =75}.TakePhoto(sphoto);
                    }

                     myImage.Source = ImageSource.FromStream(() => new MemoryStream(buffer));
                 
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
        */

        /*
        private async void OnCounterClicked(object sender, EventArgs e)
        {
            try
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    FileResult? photo = await MediaPicker.Default.CapturePhotoAsync();

                    if (photo != null)
                    {
                        var stream = await photo.OpenReadAsync();
                        myImage.Source = ImageSource.FromStream(() => stream);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
        */

        public int maxWidthHeight { get; set; } = 1000;        
        public int compressionQuality { get; set; } = 75; 

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            try
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    FileResult? photo = await MediaPicker.Default.CapturePhotoAsync();

                    if (photo != null)
                    {
                        using (var stream = await photo.OpenReadAsync())
                        {
                            //using (MemoryStream msBuffer = new MemoryStream())//se cierra antes
                            MemoryStream msBuffer = new MemoryStream();
                            //{
                            using (SKBitmap originalBitmap = SKBitmap.Decode(stream))
                            {
                                int newWidth = originalBitmap.Width;
                                int newHeight = originalBitmap.Height;

                                if (originalBitmap.Width > maxWidthHeight || originalBitmap.Height > maxWidthHeight)
                                {
                                    float ratio = Math.Min(maxWidthHeight * 1f / originalBitmap.Width, maxWidthHeight * 1f / originalBitmap.Height);
                                    newWidth = (int)(originalBitmap.Width * ratio);
                                    newHeight = (int)(originalBitmap.Height * ratio);
                                }

                                using SKBitmap resizedBitmap = originalBitmap.Resize(new SKImageInfo(newWidth, newHeight), new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear));
                                using SKImage image = SKImage.FromBitmap(resizedBitmap);
                                using SKData encodedData = image.Encode(SKEncodedImageFormat.Jpeg, compressionQuality);


                                //encodedData.AsStream();
                                encodedData.SaveTo(msBuffer);
                                msBuffer.Seek(0, SeekOrigin.Begin);
                                myImage.Source = ImageSource.FromStream(() => msBuffer);//el buffer no se tiene que cerrar
                            }

                            //var reader = new StreamReader(msBuffer);
                            //myImage.Source = ImageSource.FromStream(() => stream);
                            // }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }

}