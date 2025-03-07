using MetadataExtractor.Formats.Exif;
using MetadataExtractor;
using SkiaSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using ExifTag = SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag;

/*
    conserva  exif -solo la orientacion

    pero demora mucho la escritura del exif
 */


namespace MauiCameraView.Utilities
{
    public class SkiaSharpEXIFImageDevice : IImageDevice
    {
        public int MaxWidthHeight { get; set; } = 1000;

        public int CompressionQuality { get; set; } = 75;

        public double CustomPhotoSize { get; set; } = 50;

        public async Task<byte[]?> ProcesarPhoto(FileResult simagen)
        {
            byte[]? imageData = null;
            int? originalOrientation = null;

            #region extrae la orientación desde el exif
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


            #region escala la imagen
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
            #endregion

            #region escribe solo orientacion de la imagen original
            if (imageData != null && originalOrientation.HasValue)
            {
                using (var imageSharpImage = SixLabors.ImageSharp.Image.Load(imageData))
                {
                    if (imageSharpImage.Metadata.ExifProfile == null)
                    {
                        imageSharpImage.Metadata.ExifProfile = new SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifProfile();
                        imageSharpImage.Metadata.ExifProfile.SetValue(ExifTag.Orientation, (ushort)originalOrientation.Value);
                         
                        using (var ms = new MemoryStream())
                        {
                            imageSharpImage.Save(ms, new JpegEncoder
                            {
                                Quality = CompressionQuality
                            });
                            imageData = ms.ToArray();
                        }
                    }
                }
                return imageData;
            }
            #endregion

            return imageData;
        }

        public async Task<byte[]?> ProcesarPhoto(Stream sphoto)
        {
            byte[]? imageData = null;
            int? originalOrientation = null;

            #region extrae la orientación desde el exif
            
                var directories = ImageMetadataReader.ReadMetadata(sphoto);
                var exifDirectory = directories.OfType<ExifIfd0Directory>().FirstOrDefault();
                if (exifDirectory != null && exifDirectory.TryGetInt32(ExifDirectoryBase.TagOrientation, out var orientation))
                {
                    originalOrientation = orientation;
                }
            
            #endregion

            #region escala la imagen
            
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
           
            #endregion

            #region escribe solo orientacion de la imagen original
            if (imageData != null && originalOrientation.HasValue)
            {
                using (var imageSharpImage = SixLabors.ImageSharp.Image.Load(imageData))
                {
                    if (imageSharpImage.Metadata.ExifProfile == null)
                    {
                        imageSharpImage.Metadata.ExifProfile = new SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifProfile();
                        imageSharpImage.Metadata.ExifProfile.SetValue(ExifTag.Orientation, (ushort)originalOrientation.Value);

                        using (var ms = new MemoryStream())
                        {
                            imageSharpImage.Save(ms, new JpegEncoder
                            {
                                Quality = CompressionQuality
                            });
                            imageData = ms.ToArray();
                        }
                    }
                }
                return imageData;
            }
            #endregion

            return imageData;
        }
    }
}