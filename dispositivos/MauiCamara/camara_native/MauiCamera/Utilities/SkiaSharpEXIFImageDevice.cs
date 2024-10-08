﻿using MetadataExtractor.Formats.Exif;
using MetadataExtractor;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp.Formats.Jpeg;
using ImageMagick;
using ExifTag = SixLabors.ImageSharp.Metadata.Profiles.Exif.ExifTag;

namespace MauiCamera.Utilities
{
    public class SkiaSharpEXIFImageDevice : IImageDevice
    {
        public int MaxWidthHeight { get; set; } = 1000;

        public int CompressionQuality { get; set; } = 75;

        public double CustomPhotoSize { get; set; } = 50;

        public async Task<byte[]?> TakePhoto(FileResult simagen)
        {
            byte[]? imageData = null;
            int? originalOrientation = null;

            using (var sphoto = await simagen.OpenReadAsync())
            {
                var directories = ImageMetadataReader.ReadMetadata(sphoto);

                var exifDirectory = directories.OfType<ExifIfd0Directory>().FirstOrDefault();
                if (exifDirectory != null && exifDirectory.TryGetInt32(ExifDirectoryBase.TagOrientation, out var orientation))
                {
                    originalOrientation = orientation;
                }
            }

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

            return imageData;
        }
    }
}