using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiCameraView.Platforms.Android.Utilities
{
    public interface IPhotoPicker
    {
        Task<Stream?> CapturePhotoAsync();
    }
}
