using MetadataExtractor.Formats.Exif;
using MetadataExtractor;
using SkiaSharp;

/*
 * usar esto.
 * 
 * Toma la orientacion del exif
 * Escala
 * y luego rota la imagen o refleja segun el exif
 * 
 * Este metodo es más rapido que grabar el exif
*/

namespace MauiCameraResize.Utilities
{    
    public class SkiaSharpEXIFRotateImageDevice : IImageDevice
    {
        public int MaxWidthHeight { get; set; } = 1000;
        public int CompressionQuality { get; set; } = 75;
        public double CustomPhotoSize { get; set; } = 50;

        public async Task<byte[]?> TakePhoto(FileResult simagen)
        {
            byte[]? imageData = null;
            int? originalOrientation = null;

            #region extracción del exif
            using (var sphoto = await simagen.OpenReadAsync())
            {
                var directories = ImageMetadataReader.ReadMetadata(sphoto);
                var exifDirectory = directories.OfType<ExifIfd0Directory>().FirstOrDefault();
                if (exifDirectory != null && exifDirectory.TryGetInt32(ExifDirectoryBase.TagOrientation, out var orientation))
                {
                    originalOrientation = orientation;
                }
            }
            #endregion

            #region escalado imagen
            using (var sphoto = await simagen.OpenReadAsync())
            {
                using (SKBitmap originalBitmap = SKBitmap.Decode(sphoto))
                {
                    SKBitmap transformedBitmap = AplicarOrientation(originalBitmap, originalOrientation);

                    # region redimensionar la imagen
                    int newWidth = (int)(transformedBitmap.Width * (CustomPhotoSize / 100));
                    int newHeight = (int)(transformedBitmap.Height * (CustomPhotoSize / 100));

                    float ratio = 1;
                    if (transformedBitmap.Width > MaxWidthHeight || transformedBitmap.Height > MaxWidthHeight)
                    {
                        ratio = Math.Min(MaxWidthHeight * 1f / transformedBitmap.Width, MaxWidthHeight * 1f / transformedBitmap.Height);
                        newWidth = (int)(transformedBitmap.Width * ratio);
                        newHeight = (int)(transformedBitmap.Height * ratio);
                    }
                    #endregion

                    #region crear el bitmap redimensionado
                    using (SKBitmap resizedBitmap = transformedBitmap.Resize(new SKImageInfo(newWidth, newHeight), SKFilterQuality.Medium))
                    using (SKImage image = SKImage.FromBitmap(resizedBitmap))
                    using (SKData encodedData = image.Encode(SKEncodedImageFormat.Jpeg, CompressionQuality))
                    {
                        imageData = encodedData.ToArray();
                    }
                    #endregion
                }
            }
            #endregion

            return imageData;
        }

        private SKBitmap AplicarOrientation(SKBitmap originalBitmap, int? orientation)
        {
            if (!orientation.HasValue) return originalBitmap;

            SKBitmap resultBitmap;

            switch (orientation.Value)
            {
                case 1: //normal
                    return originalBitmap;

                case 2: //voltear horizontalmente
                    resultBitmap = Rotate(originalBitmap, 0, -1, 1);
                    break;

                case 3: //girar 180 grados
                    resultBitmap = Rotate(originalBitmap, 180);
                    break;

                case 4: //voltear verticalmente
                    resultBitmap = Rotate(originalBitmap,0, 1, -1);
                    break;

                case 5: //voltear verticalmente y rotar 90 grados a la derecha
                    resultBitmap = Rotate(originalBitmap, 90,1,-1);
                    break;

                case 6: //girar 90 grados a la derecha

                    resultBitmap = Rotate(originalBitmap, 90);
                    break;

                case 7: //girar 90 grados a la izquierda y voltear verticalmente
                    resultBitmap = Rotate(originalBitmap, -90, -1, 1);
                    break;

                case 8: //girar 90 grados a la izquierda
                    resultBitmap = Rotate(originalBitmap, -90);
                    break;

                default: //orientación no es válida
                    return originalBitmap;
            }

            return resultBitmap;
        }

        //https://github.com/djdd87/SynoAI
        private SKBitmap Rotate(SKBitmap bitmap, double angle)
        {
            double radians = Math.PI * angle / 180;
            float sine = (float)Math.Abs(Math.Sin(radians));
            float cosine = (float)Math.Abs(Math.Cos(radians));
            int originalWidth = bitmap.Width;
            int originalHeight = bitmap.Height;
            int rotatedWidth = (int)(cosine * originalWidth + sine * originalHeight);
            int rotatedHeight = (int)(cosine * originalHeight + sine * originalWidth);

            SKBitmap rotatedBitmap = new(rotatedWidth, rotatedHeight);
            using (SKCanvas canvas = new(rotatedBitmap))
            {
                canvas.Clear();
                canvas.Translate(rotatedWidth / 2, rotatedHeight / 2);
                canvas.RotateDegrees((float)angle);
                canvas.Translate(-originalWidth / 2, -originalHeight / 2);
                canvas.DrawBitmap(bitmap, new SKPoint());
            }

            return rotatedBitmap;
        }

        private SKBitmap Rotate(SKBitmap bitmap, double angle, int ex, int ey)
        {
            double radians = Math.PI * angle / 180;
            float sine = (float)Math.Abs(Math.Sin(radians));
            float cosine = (float)Math.Abs(Math.Cos(radians));
            int originalWidth = bitmap.Width;
            int originalHeight = bitmap.Height;
            int rotatedWidth = (int)(cosine * originalWidth + sine * originalHeight);
            int rotatedHeight = (int)(cosine * originalHeight + sine * originalWidth);

            SKBitmap rotatedBitmap = new(rotatedWidth, rotatedHeight);
            using (SKCanvas canvas = new(rotatedBitmap))
            {
                canvas.Clear();
                canvas.Translate(rotatedWidth / 2, rotatedHeight / 2);
                canvas.RotateDegrees((float)angle);
                canvas.Translate(-originalWidth / 2, -originalHeight / 2);
                canvas.DrawBitmap(bitmap, new SKPoint());

                canvas.Scale(ex, ey);
            }

            return rotatedBitmap;
        }
    }
}