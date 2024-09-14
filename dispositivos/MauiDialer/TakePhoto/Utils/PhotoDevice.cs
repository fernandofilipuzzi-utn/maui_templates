using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp3.Utils
{
    internal class PhotoDevice
    {
        public int maxWidthHeight {get;set;}=1000;
        float photoSizeMultiplier = 0.5f; // Equivale a CustomPhotoSize = 50
        public int compressionQuality { get; set; } = 75; // 0 (peor calidad) - 100 (mejor calidad)

        public async Task<byte[]> TakePhoto(Stream simagen)
        {

            byte[] imageData = new byte[0];

            try
            {
                if (MediaPicker.Default.IsCaptureSupported)
                {
                    FileResult? photo = await MediaPicker.Default.CapturePhotoAsync();

                    if (photo != null)
                    {
                        using (Stream stream = await photo.OpenReadAsync())
                        using (MemoryStream msBuffer = new MemoryStream())
                        {
                            await stream.CopyToAsync(msBuffer);
                            imageData = msBuffer.ToArray();

                            using (SKBitmap originalBitmap = SKBitmap.Decode(imageData))
                            {
                                int newWidth = originalBitmap.Width;
                                int newHeight = originalBitmap.Height;

                                if (originalBitmap.Width > maxWidthHeight || originalBitmap.Height > maxWidthHeight)
                                {
                                    float ratio = Math.Min(maxWidthHeight * 1f / originalBitmap.Width, maxWidthHeight * 1f / originalBitmap.Height);
                                    newWidth = (int)(originalBitmap.Width * ratio);
                                    newHeight = (int)(originalBitmap.Height * ratio);
                                }

                                //filtro reducir aplico Nearest
                                //filtro para ampliar aplico linear

                                using SKBitmap resizedBitmap = originalBitmap.Resize(new SKImageInfo(newWidth, newHeight), new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear));

                                using SKImage image = SKImage.FromBitmap(resizedBitmap);
                                using SKData encodedData = image.Encode(SKEncodedImageFormat.Jpeg, compressionQuality);

                                return encodedData.ToArray();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return imageData;
        }

    }
}
