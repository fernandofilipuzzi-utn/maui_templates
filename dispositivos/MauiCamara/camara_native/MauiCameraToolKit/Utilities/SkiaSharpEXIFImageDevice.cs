﻿using SkiaSharp;

namespace MauiCameraToolKit.Utilities
{
    public class SkiaSharpEXIFImageDevice : IImageDevice
    {
        public int MaxWidthHeight { get; set; } = 1000;
        public int CompressionQuality { get; set; } = 75;
        public double CustomPhotoSize { get; set; } = 50;

        public async Task<byte[]?> TakePhoto(Microsoft.Maui.Storage.FileResult simagen)
        {
            byte[]? imageData = null;
            int? originalOrientation = null;

            // Tomamos la foto usando la API de Xamarin.Essentials para obtener el archivo
            using (var sphoto = await simagen.OpenReadAsync())
            {
                // Procesamiento y extracción de la imagen original usando SkiaSharp
                using (SKBitmap originalBitmap = SKBitmap.Decode(sphoto))
                {
                    // Aplicar redimensionado basado en el tamaño personalizado
                    int newWidth = (int)(originalBitmap.Width * (CustomPhotoSize / 100));
                    int newHeight = (int)(originalBitmap.Height * (CustomPhotoSize / 100));

                    // Mantener el aspecto de la imagen si excede el tamaño máximo permitido
                    float ratio = 1;
                    if (originalBitmap.Width > MaxWidthHeight || originalBitmap.Height > MaxWidthHeight)
                    {
                        ratio = Math.Min(MaxWidthHeight * 1f / originalBitmap.Width, MaxWidthHeight * 1f / originalBitmap.Height);
                        newWidth = (int)(originalBitmap.Width * ratio);
                        newHeight = (int)(originalBitmap.Height * ratio);
                    }

                    // Redimensionar la imagen usando SkiaSharp
                    using (SKBitmap resizedBitmap = originalBitmap.Resize(new SKImageInfo(newWidth, newHeight), SKFilterQuality.Medium))
                    using (SKImage image = SKImage.FromBitmap(resizedBitmap))
                    using (SKData encodedData = image.Encode(SKEncodedImageFormat.Jpeg, CompressionQuality))
                    {
                        imageData = encodedData.ToArray(); // Convertir la imagen redimensionada en un byte[]
                    }
                }
            }

            // Puedes agregar una lógica más avanzada aquí para manipular los metadatos, como la orientación
            // usando bibliotecas adicionales, si es necesario.

            return imageData; // Devolver los bytes de la imagen redimensionada
        }

    }
}