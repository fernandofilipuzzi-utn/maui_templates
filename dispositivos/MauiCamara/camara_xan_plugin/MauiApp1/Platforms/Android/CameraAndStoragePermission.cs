using Android;
using Android.Content.PM;
using Android.OS;
#if __ANDROID_29__
using AndroidX.Core.App;
using AndroidX.Core.Content;
#else
using Android.Support.V4.App;
using Android.Support.V4.Content;
#endif

namespace MauiApp1
{
    public partial class CameraAndStoragePermission : Permissions.BasePlatformPermission
    {
        //https://learn.microsoft.com/en-us/xamarin/essentials/permissions?context=xamarin%2Fandroid&tabs=android
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions
        {
            get
            {
                return new (string, bool)[]
                {
                    (Manifest.Permission.Camera, true),
                    (Manifest.Permission.Internet, true)
                };
            }
        }
    }
}
