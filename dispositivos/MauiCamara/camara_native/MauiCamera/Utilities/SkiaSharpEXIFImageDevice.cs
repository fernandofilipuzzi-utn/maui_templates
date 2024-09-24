using MetadataExtractor.Formats.Exif;
using MetadataExtractor;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiCamera.Utilities
{
    internal class SkiaSharpEXIFImageDevice : IImageDevice
    {
        public int MaxWidthHeight { get; set; } = 1000;

        /// 0 (peor calidad) - 100 (mejor calidad)
        public int CompressionQuality { get; set; } = 75;

        /// Porcentaje del tamaño original (0-100)
        public double CustomPhotoSize { get; set; } = 50;

        public async Task<byte[]?> TakePhotoAndCopyExif(FileResult simagen, string originalImagePath, string newImagePath)
        {
            byte[]? imageData = null;

            // Redimensionar la imagen
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

                    if (imageData != null)
                    {
                        await File.WriteAllBytesAsync(newImagePath, imageData);
                    }
                }
            }

            CopyExifData(originalImagePath, newImagePath);

            return imageData;
        }

        public void CopyExifData(string originalImagePath, string newImagePath)
        {
            var directories = ImageMetadataReader.ReadMetadata(originalImagePath);
            var exifSubIfdDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();

            if (exifSubIfdDirectory == null)
            {
                Console.WriteLine("No se encontraron datos EXIF en la imagen original.");
                return;
            }

            var originalExifData = new Dictionary<string, object>();
            foreach (var tag in exifSubIfdDirectory.Tags)
            {
                originalExifData[tag.Name] = tag.GetValue();
            }

            // Aquí puedes escribir los metadatos en la nueva imagen.
            // Por ahora, MetadataExtractor no soporta escribir EXIF directamente,
            // se podría usar otra herramienta para realizar la escritura o hacer una integración externa.
            using (var exifWriter = new ExifWriter(newImagePath))
            {
                foreach (var item in originalExifData)
                {
                    // Escribe cada metadato que deseas conservar
                    exifWriter.SetTag(item.Key, item.Value);
                }
            }

            Console.WriteLine("Metadatos EXIF copiados a la nueva imagen.");
        }

        public Task<byte[]?> TakePhoto(FileResult simagen)
        {
            throw new NotImplementedException();
        }
    }
}