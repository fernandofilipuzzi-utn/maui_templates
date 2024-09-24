using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using Plugin.CurrentActivity;

namespace MauiApp1
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        /*  protected override void OnCreate(Bundle bundle)
          {

              base.OnCreate(bundle);
              CrossCurrentActivity.Current.Init(this, bundle);
              global::Xamarin.Forms.Forms.Init(this, bundle);
              LoadApplication(new App());

          }

          public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
          {
              PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
          }
          */

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Platform.Init(this, bundle);
            Platform.ActivityStateChanged += Platform_ActivityStateChanged;
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        void Platform_ActivityStateChanged(object sender, ActivityStateChangedEventArgs e) =>
            Toast.MakeText(this, e.State.ToString(), ToastLength.Short).Show();
    }
}
