using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

/**
 * escala la imagen directamente mediante la libreria sixlabors
 * 
 * el tema es que si bien conserva el exif - demora un monton.
 */

namespace MauiCameraResize.Utilities
{
    public class SixLaborsImageDevice : IImageDevice
    {
        public int MaxWidthHeight { get; set; } = 1000;
        public int CompressionQuality { get; set; } = 75;
        public double CustomPhotoSize { get; set; } = 50;

        public async Task<byte[]?> TakePhoto(FileResult simagen)
        {
            using (var photoStream = await simagen.OpenReadAsync())
            {
                using (var originalImage = SixLabors.ImageSharp.Image.Load(photoStream))
                {
                    int newWidth = (int)(originalImage.Width * (CustomPhotoSize / 100));
                    int newHeight = (int)(originalImage.Height * (CustomPhotoSize / 100));

                    float ratio = 1;
                    if (originalImage.Width > MaxWidthHeight || originalImage.Height > MaxWidthHeight)
                    {
                        ratio = Math.Min(MaxWidthHeight * 1f / originalImage.Width, MaxWidthHeight * 1f / originalImage.Height);
                        newWidth = (int)(originalImage.Width * ratio);
                        newHeight = (int)(originalImage.Height * ratio);
                    }

                    originalImage.Mutate(x => x.Resize(newWidth, newHeight));

                    using (var memoryStream = new MemoryStream())
                    {
                        originalImage.Save(memoryStream, new JpegEncoder
                        {
                            Quality = CompressionQuality
                        });

                        return memoryStream.ToArray();
                    }
                }
            }
        }
    }
}
