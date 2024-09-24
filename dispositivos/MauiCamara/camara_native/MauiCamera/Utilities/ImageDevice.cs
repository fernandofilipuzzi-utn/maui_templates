using ImageMagick;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using SkiaSharp;
using System.Text;

namespace MauiCamera.Utilities
{
    public class ImageDevice : IImageDevice
    {
        public int MaxWidthHeight { get; set; } = 1000;

        /// 0 (peor calidad) - 100 (mejor calidad)
        public int CompressionQuality { get; set; } = 75;

        /// Porcentaje del tamaño original (0-100)
        public double CustomPhotoSize { get; set; } = 50;

        public async Task<byte[]?> TakePhoto(FileResult simagen)
        {
            byte[]? imageData = null;
            byte[]? originalExifData = null;

            using (var sphoto = await simagen.OpenReadAsync())
            {
                originalExifData = ExtractExifData(sphoto);

                sphoto.Seek(0, SeekOrigin.Begin);
                using (SKBitmap originalBitmap = SKBitmap.Decode(sphoto))
                {
                    //reduce la imagen al porcentaje
                    int newWidth = (int)(originalBitmap.Width * (CustomPhotoSize / 100));
                    int newHeight = (int)(originalBitmap.Height * (CustomPhotoSize / 100));

                    //acota la imagen a un maximo
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

            if (imageData != null && originalExifData != null)
            {
                imageData = ReinjectExifData(imageData, originalExifData);
            }

            return imageData;
        }

        private SKBitmap AdjustOrientation(SKBitmap bitmap, int orientation)
        {
            SKBitmap rotatedBitmap;

            switch (orientation)
            {
                case 3: // Rotación 180 grados
                    rotatedBitmap = new SKBitmap(bitmap.Width, bitmap.Height);
                    using (var canvas = new SKCanvas(rotatedBitmap))
                    {
                        canvas.RotateDegrees(180, bitmap.Width / 2, bitmap.Height / 2);
                        canvas.DrawBitmap(bitmap, 0, 0);
                    }
                    break;

                case 6: // Rotación 90 grados hacia la derecha
                    rotatedBitmap = new SKBitmap(bitmap.Height, bitmap.Width); // Se invierten las dimensiones
                    using (var canvas = new SKCanvas(rotatedBitmap))
                    {
                        canvas.Translate(rotatedBitmap.Width, 0);
                        canvas.RotateDegrees(90);
                        canvas.DrawBitmap(bitmap, 0, 0);
                    }
                    break;

                case 8: // Rotación 90 grados hacia la izquierda
                    rotatedBitmap = new SKBitmap(bitmap.Height, bitmap.Width); // Se invierten las dimensiones
                    using (var canvas = new SKCanvas(rotatedBitmap))
                    {
                        canvas.Translate(0, rotatedBitmap.Height);
                        canvas.RotateDegrees(-90);
                        canvas.DrawBitmap(bitmap, 0, 0);
                    }
                    break;

                default: // No hay rotación
                    rotatedBitmap = bitmap;
                    break;
            }

            return rotatedBitmap;
        }

        private int GetImageOrientation(Stream imageStream)
        {
            try
            {
                // Reinicia el stream para leer EXIF desde el principio
                imageStream.Seek(0, SeekOrigin.Begin);

                var directories = ImageMetadataReader.ReadMetadata(imageStream);
                var exifIfd0Directory = directories.OfType<ExifIfd0Directory>().FirstOrDefault();

                if (exifIfd0Directory != null && exifIfd0Directory.TryGetInt32(ExifDirectoryBase.TagOrientation, out int orientation))
                {
                    return orientation;
                }
            }
            catch (Exception ex)
            {
                // Maneja errores al leer EXIF (por ejemplo, si no tiene EXIF)
            }

            return 1; // Si no se puede leer la orientación, asume la orientación normal
        }

        /*
        private byte[]? ExtractExifData(Stream imageStream)
        {
            try
            {
                imageStream.Seek(0, SeekOrigin.Begin);

                var directories = ImageMetadataReader.ReadMetadata(imageStream);
                var exifDirectory = directories.OfType<ExifIfd0Directory>().FirstOrDefault();

                if (exifDirectory != null)
                {
                    return Encoding.UTF8.GetBytes(exifDirectory.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al extraer EXIF: " + ex.Message);
            }

            return null;
        }*/

        private byte[]? ExtractExifData(Stream photoStream)
        {
            try
            {
                var directories = ImageMetadataReader.ReadMetadata(photoStream);

                using (var ms = new MemoryStream())
                {
                    foreach (var directory in directories)
                    {
                        foreach (var tag in directory.Tags)
                        {
                            var tagData = $"{directory.Name} - {tag.Name} = {tag.Description}";
                            Console.WriteLine(tagData);
                        }
                    }
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error extrayendo EXIF: {ex.Message}");
                return null;
            }
        }

        private byte[] ReinjectExifData(byte[] imageData, byte[] originalExifData)
        {
            try
            {
                // Combinar la imagen con los datos EXIF originales
                using (MemoryStream newImageStream = new MemoryStream(imageData))
                using (MemoryStream exifStream = new MemoryStream(originalExifData))
                using (SKBitmap bitmap = SKBitmap.Decode(newImageStream))
                {
                    // Aquí puedes reinyectar los metadatos EXIF (si tu biblioteca o herramientas lo soportan).
                    // Por ahora, este es solo un ejemplo conceptual.
                    // Se necesitaría una biblioteca más específica para reinyectar EXIF en JPEG.

                    // Retorna la imagen final con EXIF reinyectado
                    return newImageStream.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reinyectando EXIF: {ex.Message}");
                return imageData; // En caso de fallo, retornar la imagen sin los metadatos
            }
        }
    }
}

