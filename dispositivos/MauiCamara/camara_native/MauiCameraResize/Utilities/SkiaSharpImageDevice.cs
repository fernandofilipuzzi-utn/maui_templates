using SkiaSharp;

namespace MauiCameraResize.Utilities
{
    /*
     lee la orientación de la imagen original desde el exif y lueg
     genera una imagen nueva , con una nueva escala y rotada según el exif original

     nota: a diferencia de inyectar el exif en la nueva imagen , en este es mucho mas rapido
     */
    public class SkiaSharpImageDevice : IImageDevice
    {
        public int MaxWidthHeight { get; set; } = 1000;

        public int CompressionQuality { get; set; } = 75;

        public double CustomPhotoSize { get; set; } = 50;

        public async Task<byte[]?> TakePhoto(FileResult simagen)
        {
            byte[]? imageData = null;

            using (var sphoto = await simagen.OpenReadAsync())
            {
                using (SKBitmap originalBitmap = SKBitmap.Decode(sphoto))
                {
                    int newWidth = (int)(originalBitmap.Width * (CustomPhotoSize / 100));
                    int newHeight = (int)(originalBitmap.Height * (CustomPhotoSize / 100));

                    float ratio = 1;
                    if (originalBitmap.Width > MaxWidthHeight || originalBitmap.Height > MaxWidthHeight)
                    {
                        ratio = Math.Min(MaxWidthHeight * 1f / originalBitmap.Width, MaxWidthHeight * 1f / originalBitmap.Height);
                        newWidth = (int)(originalBitmap.Width * ratio);
                        newHeight = (int)(originalBitmap.Height * ratio);
                    }

                    using (SKBitmap resizedBitmap = originalBitmap.Resize(new SKImageInfo(newWidth, newHeight), SKFilterQuality.Medium))
                    using (SKImage image = SKImage.FromBitmap(resizedBitmap))
                    using (SKData encodedData = image.Encode(SKEncodedImageFormat.Jpeg, CompressionQuality))
                    {
                        imageData = encodedData.ToArray();
                    }
                }
            }

            return imageData;
        }
    }
}
