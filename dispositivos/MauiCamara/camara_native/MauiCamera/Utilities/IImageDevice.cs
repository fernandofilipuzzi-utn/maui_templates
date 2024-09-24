using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiCamera.Utilities
{
    public interface IImageDevice
    {
        public int MaxWidthHeight { get; set; }

        public int CompressionQuality { get; set; }

        public double CustomPhotoSize { get; set; }

        Task<byte[]?> TakePhoto(FileResult simagen);
    }
}
