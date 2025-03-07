

using Android.App;

namespace MauiSimple3
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            Navegador.Source = "https://esperanza.gobdigital.com.ar/web/vecino/app/login?appVersion=1";
        }

        async private void Navegador_Navigating(object sender, WebNavigatingEventArgs e)
        {
            try
            {
                if (e.Url.Contains("action=PHOTO"))
                {
                    e.Cancel = true; //cancela la navegación

                    //string param2 = "inputFoto";
                    //string result =  "iVBORw0KGgoAAAANSUhEUgAAAFgAAABuCAIAAAAs1EPTAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAALsSURBVHhe7dBRkuIwDEXR3v+mGZV06QoMoe1Ysg31zs+MhZOO7s9NnEJAIaAQUAgoBPYK8fMKvxXbJQRLn+NemS1CsOsd0/N5hfUh2PLtntxwjLItDsFyDetx7ytDsFnzbtyuabE+BIc28YjhnGdZiMv7XH7wvTUhYhnDuQdPZrdYGYJDv8HHX1IIfGQIM/6GJwoBhcCCECk7pLzkSCGgEFAILAhhxtcYf8MThYBC4CNDDD7+kkJgZQjDuQdPfkcIc3mZyw++tziE4dyGZ74phGGnnq1677fbIoRh9BZXvy+EYTPH6ASXaiqYxSECK94xdYwcoxpbhDDseo57ZXYJEVj6Eb8V2ytEmLn/L4WAQkAhoBBQCCgE9g1hOE+xVwgCPOK3YruEYOm7p0ncKbVFCNY94IfDT5zLLA7Blu44if+HmBjONVaGYD/H6FUIE0PDucCaEKx1x9T9PwkxN5yzLQjBQo7RwdncxE+Gc6rZIVjFMXr05icTvxrOeeaFYAPH6JXGC4Zzkkkh+HbH6ET7HcM5w4wQfHXbdzfejGuG87DaEHysY/SX9stx03AeUxiCz3SMGnTdj8uG84CSEHydY9Ss96m4HxhdkhyCL3KMOl17Np4ynPtlhuBbHKN+1x6PpwKjTiUhOF8y8oahZ/l3GyPLGIUYpRBQCCgEFAIKAYWAQkAhoBBQCCgEFAKbhjCcZ9krBA3umE6xUQi2P+CHKbYIwd4Hv8O4MMH6ELFwYOQYzWqxOAS7OkYHZ/MKy0LEkoHRK9yoz7EgBJs5Rm+13xwxO0RsFRj9hdvFLaaGYKH+lXisssWkEOzhGHUaebbFjBCxQ2DUj+fLWtSG4NvvmF7FW2paFIbgqx2jYblvO6oKEV8cGCWpeKcpeOMBo1S8Or0v/ybhGx2jAhXvz3xdfJ/hXIm/lPe3kkPwv3pRwXAeNu/T01EiqcUHhzCUyGjx2SGMQiRTCCgEFAIKAYWAQkAhoBBQCCiEu93+AULsKLiMx2Y9AAAAAElFTkSuQmCC";

                    //MainThread.BeginInvokeOnMainThread(async () =>
                    //{
                    //    //await Navegador.EvaluateJavaScriptAsync($"setresult_takephoto('{result}')");
                    //    await Navegador.EvaluateJavaScriptAsync("document.getElementById('" + param2 + "').value='" + result + "';");
                    //    await Navegador.EvaluateJavaScriptAsync("document.getElementById('" + param2 + "Img').src='data:image/png;base64," + result + "';");
                    //});

                    //btnTakeFoto_Clicked(this, EventArgs.Empty);

                    string param1 = "PHOTO";
                    string param2 = "inputFoto";
                    // Task.Run(() => Tomar(param1, param2)).Wait();
                    await Tomar(param1, param2);

                }
            }
            catch (Exception ex) { }
        }

        private void Navegador_Navigated(object sender, WebNavigatedEventArgs e)
        {
        }

        async private Task<string?> TomarFoto()
        {
            string? base64 = null;

            if (MediaPicker.Default.IsCaptureSupported == false)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return base64;
            }

            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Camera>();
            }

            if (status != PermissionStatus.Granted) return base64;

            FileResult? photo = await MediaPicker.Default.CapturePhotoAsync();

            if (photo != null)
            {
                using var s = await photo.OpenReadAsync();
                byte[] imageBytes = new byte[s.Length];
                await s.ReadAsync(imageBytes);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    myImage.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                });
            }

            return "iVBORw0KGgoAAAANSUhEUgAAAFgAAABuCAIAAAAs1EPTAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAALsSURBVHhe7dBRkuIwDEXR3v+mGZV06QoMoe1Ysg31zs+MhZOO7s9NnEJAIaAQUAgoBPYK8fMKvxXbJQRLn+NemS1CsOsd0/N5hfUh2PLtntxwjLItDsFyDetx7ytDsFnzbtyuabE+BIc28YjhnGdZiMv7XH7wvTUhYhnDuQdPZrdYGYJDv8HHX1IIfGQIM/6GJwoBhcCCECk7pLzkSCGgEFAILAhhxtcYf8MThYBC4CNDDD7+kkJgZQjDuQdPfkcIc3mZyw++tziE4dyGZ74phGGnnq1677fbIoRh9BZXvy+EYTPH6ASXaiqYxSECK94xdYwcoxpbhDDseo57ZXYJEVj6Eb8V2ytEmLn/L4WAQkAhoBBQCCgE9g1hOE+xVwgCPOK3YruEYOm7p0ncKbVFCNY94IfDT5zLLA7Blu44if+HmBjONVaGYD/H6FUIE0PDucCaEKx1x9T9PwkxN5yzLQjBQo7RwdncxE+Gc6rZIVjFMXr05icTvxrOeeaFYAPH6JXGC4Zzkkkh+HbH6ET7HcM5w4wQfHXbdzfejGuG87DaEHysY/SX9stx03AeUxiCz3SMGnTdj8uG84CSEHydY9Ss96m4HxhdkhyCL3KMOl17Np4ynPtlhuBbHKN+1x6PpwKjTiUhOF8y8oahZ/l3GyPLGIUYpRBQCCgEFAIKAYWAQkAhoBBQCCgEFAKbhjCcZ9krBA3umE6xUQi2P+CHKbYIwd4Hv8O4MMH6ELFwYOQYzWqxOAS7OkYHZ/MKy0LEkoHRK9yoz7EgBJs5Rm+13xwxO0RsFRj9hdvFLaaGYKH+lXisssWkEOzhGHUaebbFjBCxQ2DUj+fLWtSG4NvvmF7FW2paFIbgqx2jYblvO6oKEV8cGCWpeKcpeOMBo1S8Or0v/ybhGx2jAhXvz3xdfJ/hXIm/lPe3kkPwv3pRwXAeNu/T01EiqcUHhzCUyGjx2SGMQiRTCCgEFAIKAYWAQkAhoBBQCCiEu93+AULsKLiMx2Y9AAAAAElFTkSuQmCC";
        }

        async private void btnTakeFoto_Clicked(object sender, EventArgs e)
        {

            ExecuteActionFromJavascript("PHOTO", "inputFoto");
        }

        private async Task ExecuteActionFromJavascript(string param1, string param2)
        {
            if (param1 != null && param1.Equals("PHOTO"))
            {
                var result = await TomarFoto();

                if (result != null)
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        //await Navegador.EvaluateJavaScriptAsync($"setresult_takephoto('{result}')");
                        await Navegador.EvaluateJavaScriptAsync("document.getElementById('" + param2 + "').value='" + result + "';");
                        await Navegador.EvaluateJavaScriptAsync("document.getElementById('" + param2 + "Img').src='data:image/png;base64," + result + "';");
                    });
                }
            }
        }

        async private Task Tomar(string param1, string param2)
        {

            if (param1 != null && param1.Equals("PHOTO"))
            {
                //var result = await TomarFoto();
                             
                string? base64 = null;

                if (MediaPicker.Default.IsCaptureSupported == false)
                {
                    await DisplayAlert("No Camera", ":( No camera available.", "OK");                    
                }

                var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.Camera>();
                }

                //if (status != PermissionStatus.Granted) return base64;

                FileResult? photo = await MediaPicker.Default.CapturePhotoAsync() ;

                if (photo != null)
                {
                    using var s = await photo.OpenReadAsync();
                    byte[] imageBytes = new byte[s.Length];
                    await s.ReadAsync(imageBytes);

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        myImage.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
                    });
                }

                string result = "iVBORw0KGgoAAAANSUhEUgAAAFgAAABuCAIAAAAs1EPTAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAALsSURBVHhe7dBRkuIwDEXR3v+mGZV06QoMoe1Ysg31zs+MhZOO7s9NnEJAIaAQUAgoBPYK8fMKvxXbJQRLn+NemS1CsOsd0/N5hfUh2PLtntxwjLItDsFyDetx7ytDsFnzbtyuabE+BIc28YjhnGdZiMv7XH7wvTUhYhnDuQdPZrdYGYJDv8HHX1IIfGQIM/6GJwoBhcCCECk7pLzkSCGgEFAILAhhxtcYf8MThYBC4CNDDD7+kkJgZQjDuQdPfkcIc3mZyw++tziE4dyGZ74phGGnnq1677fbIoRh9BZXvy+EYTPH6ASXaiqYxSECK94xdYwcoxpbhDDseo57ZXYJEVj6Eb8V2ytEmLn/L4WAQkAhoBBQCCgE9g1hOE+xVwgCPOK3YruEYOm7p0ncKbVFCNY94IfDT5zLLA7Blu44if+HmBjONVaGYD/H6FUIE0PDucCaEKx1x9T9PwkxN5yzLQjBQo7RwdncxE+Gc6rZIVjFMXr05icTvxrOeeaFYAPH6JXGC4Zzkkkh+HbH6ET7HcM5w4wQfHXbdzfejGuG87DaEHysY/SX9stx03AeUxiCz3SMGnTdj8uG84CSEHydY9Ss96m4HxhdkhyCL3KMOl17Np4ynPtlhuBbHKN+1x6PpwKjTiUhOF8y8oahZ/l3GyPLGIUYpRBQCCgEFAIKAYWAQkAhoBBQCCgEFAKbhjCcZ9krBA3umE6xUQi2P+CHKbYIwd4Hv8O4MMH6ELFwYOQYzWqxOAS7OkYHZ/MKy0LEkoHRK9yoz7EgBJs5Rm+13xwxO0RsFRj9hdvFLaaGYKH+lXisssWkEOzhGHUaebbFjBCxQ2DUj+fLWtSG4NvvmF7FW2paFIbgqx2jYblvO6oKEV8cGCWpeKcpeOMBo1S8Or0v/ybhGx2jAhXvz3xdfJ/hXIm/lPe3kkPwv3pRwXAeNu/T01EiqcUHhzCUyGjx2SGMQiRTCCgEFAIKAYWAQkAhoBBQCCiEu93+AULsKLiMx2Y9AAAAAElFTkSuQmCC"; 

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    //await Navegador.EvaluateJavaScriptAsync($"setresult_takephoto('{result}')");
                    await Navegador.EvaluateJavaScriptAsync("document.getElementById('" + param2 + "').value='" + result + "';");
                    await Navegador.EvaluateJavaScriptAsync("document.getElementById('" + param2 + "Img').src='data:image/png;base64," + result + "';");
                });
                
            }
        }


    }
}
