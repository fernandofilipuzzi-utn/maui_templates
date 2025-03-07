

using CommonMaui;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Threading.Tasks;

namespace MauiSimple4
{
    public partial class MainPage : ContentPage
    {


        public MainPage()
        {
            InitializeComponent();

            Navegador.Source = "https://mitresa.gobdigital.com.ar/CiudadanoApp/index?app=true&appVersion=1";
            //= "https://esperanza.gobdigital.com.ar/web/vecino/app/login?appVersion=1";

            PreInitializeCacheDirectory();
        }

        private void PreInitializeCacheDirectory()
        {
            // Obtén la carpeta de caché
            string cacheDirectory = FileSystem.CacheDirectory;

            // Define un subdirectorio, por ejemplo, "Fotos"
            string photosDirectory = Path.Combine(cacheDirectory, "Fotos");

            // Crear el directorio si no existe
            if (!Directory.Exists(photosDirectory))
            {
                Directory.CreateDirectory(photosDirectory);
                Console.WriteLine($"Directorio creado: {photosDirectory}");
            }
            else
            {
                Console.WriteLine($"Directorio ya existe: {photosDirectory}");
            }
        }

        async private void Navegador_Navigating(object sender, WebNavigatingEventArgs e)
        {
            try
            {
                if (e.Url.Contains("action=PHOTO"))
                {
                    string param1 = "PHOTO";
                    string param2 = "inputFoto";

                    e.Cancel = true;

                    //await TomarFoto(param1, param2);

                    // Task.Run(async() =>await TomarFoto(param1, param2));
                    await TomarFoto(param1, param2);

                    //if (param1 != null && param1.Equals("PHOTO"))
                    //{
                    //    // Validar almacenamiento
                    //    /*
                    //    var storageStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
                    //    if (storageStatus != PermissionStatus.Granted)
                    //    {
                    //        storageStatus = await Permissions.RequestAsync<Permissions.StorageRead>();
                    //    }

                    //    if (storageStatus != PermissionStatus.Granted)
                    //    {
                    //        await DisplayAlert("Permiso denegado", "No se pueden guardar fotos sin el permiso de almacenamiento.", "OK");
                    //        return;
                    //    }
                    //    */

                    //    // Validar cámara
                    //    var cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();
                    //    if (cameraStatus != PermissionStatus.Granted)
                    //    {
                    //        cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();
                    //    }

                    //    if (cameraStatus != PermissionStatus.Granted)
                    //    {
                    //        await DisplayAlert("Permiso denegado", "No se puede tomar fotos.", "OK");
                    //        return;
                    //    }

                    //    // Verificar soporte para cámara
                    //    if (!MediaPicker.Default.IsCaptureSupported)
                    //    {
                    //        await DisplayAlert("No Camera", ":( No camera available.", "OK");
                    //        return;
                    //    }

                    //    // Capturar foto
                    //    try
                    //    {
                    //        MainThread.BeginInvokeOnMainThread(async () =>
                    //        {
                    //            FileResult? photo = await MediaPicker.CapturePhotoAsync();//.ConfigureAwait(false); ;


                    //            if (photo != null)
                    //            {
                    //                //await DisplayAlert("Éxito", $"Foto capturada: {photo.FullPath}", "OK");

                    //                Console.WriteLine(photo.FullPath);


                    //                string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                    //                using Stream stream = await photo.OpenReadAsync();

                    //                byte[]? imageBytes = new ImageDeviceAutoRotate().TakePhoto(stream);

                    //                if (imageBytes == null) return;

                    //                string result = Convert.ToBase64String(imageBytes);

                    //                MainThread.BeginInvokeOnMainThread(async () =>
                    //                {
                    //                    //await Navegador.EvaluateJavaScriptAsync($"setresult_takephoto('{result}')");
                    //                    await Navegador.EvaluateJavaScriptAsync("document.getElementById('" + param2 + "').value='" + result + "';");
                    //                    await Navegador.EvaluateJavaScriptAsync("document.getElementById('" + param2 + "Img').src='data:image/png;base64," + result + "';");
                    //                });

                    //            }
                    //        });
                    //}
                    //catch (Exception ex)
                    //{
                    //    await DisplayAlert("Error", $"Ocurrió un problema al tomar la foto: {ex.Message}", "OK");
                    //}
                    //}


                }
            }
            catch { }
        }

        async private Task TomarFoto(string param1, string param2)
        {
          //  dir();

            if (param1 != null && param1.Equals("PHOTO"))
            {
                // Validar almacenamiento
                /*
                var storageStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
                if (storageStatus != PermissionStatus.Granted)
                {
                    storageStatus = await Permissions.RequestAsync<Permissions.StorageRead>();
                }

                if (storageStatus != PermissionStatus.Granted)
                {
                    await DisplayAlert("Permiso denegado", "No se pueden guardar fotos sin el permiso de almacenamiento.", "OK");
                    return;
                }
                */

                // Validar cámara
                var cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();
                if (cameraStatus != PermissionStatus.Granted)
                {
                    cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();
                }

                if (cameraStatus != PermissionStatus.Granted)
                {
                    await DisplayAlert("Permiso denegado", "No se puede tomar fotos.", "OK");
                    return;
                }

                // Verificar soporte para cámara
                if (!MediaPicker.Default.IsCaptureSupported)
                {
                    await DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }

                // Capturar foto
                try
                {
                    FileResult? photo = await MediaPicker.Default.CapturePhotoAsync();//ConfigureAwait(false); ;

                    await DisplayAlert("Éxito", $"Foto capturada: {photo?.FullPath}", "OK");

                    if (photo != null)
                    {
                        //await DisplayAlert("Éxito", $"Foto capturada: {photo.FullPath}", "OK");

                        Console.WriteLine(photo.FullPath);


                        string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                        using Stream stream = await photo.OpenReadAsync();

                        byte[]? imageBytes=new ImageDeviceAutoRotate().TakePhoto(stream);

                        if (imageBytes == null) return;

                        string result = Convert.ToBase64String(imageBytes);

                        MainThread.BeginInvokeOnMainThread(async () =>
                        {
                            //await Navegador.EvaluateJavaScriptAsync($"setresult_takephoto('{result}')");
                            await Navegador.EvaluateJavaScriptAsync("document.getElementById('" + param2 + "').value='" + result + "';");
                            await Navegador.EvaluateJavaScriptAsync("document.getElementById('" + param2 + "Img').src='data:image/png;base64," + result + "';");
                        });

                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Ocurrió un problema al tomar la foto: {ex.Message}", "OK");
                }
            }
        }

        async private void dir()
        {
            try
            {
                string directoryPath = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).AbsolutePath, "MiApp");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un problema al tomar la foto: {ex.Message}", "OK");
            }
        }

        async private void btnTakeFoto_Clicked(object sender, EventArgs e)
        {
            string param1 = "PHOTO";
            string param2 = "inputFoto";

            if (param1 != null && param1.Equals("PHOTO"))
            {
                // Validar almacenamiento
                /*
                var storageStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
                if (storageStatus != PermissionStatus.Granted)
                {
                    storageStatus = await Permissions.RequestAsync<Permissions.StorageRead>();
                }

                if (storageStatus != PermissionStatus.Granted)
                {
                    await DisplayAlert("Permiso denegado", "No se pueden guardar fotos sin el permiso de almacenamiento.", "OK");
                    return;
                }
                */

                //var sensorStatus = await Permissions.CheckStatusAsync<Permissions.Sensors>();
                //if (sensorStatus != PermissionStatus.Granted)
                //{
                //    sensorStatus = await Permissions.RequestAsync<Permissions.Sensors>();
                //}
                //if (sensorStatus != PermissionStatus.Granted)
                //{
                //    await DisplayAlert("Permiso denegado", "No se puede utilizar el giroscopio.", "OK");
                //    return;
                //}

                // Validar cámara
                var cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();
                if (cameraStatus != PermissionStatus.Granted)
                {
                    cameraStatus = await Permissions.RequestAsync<Permissions.Camera>();
                }

                if (cameraStatus != PermissionStatus.Granted)
                {
                    await DisplayAlert("Permiso denegado", "No se puede tomar fotos.", "OK");
                    return;
                }

                // Verificar soporte para cámara
                if (!MediaPicker.Default.IsCaptureSupported)
                {
                    await DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }

                // Capturar foto
                try
                {
                    await DisplayAlert("Éxito", $"Foto capturada:", "OK");

                    FileResult? photo = await MediaPicker.CapturePhotoAsync();//ConfigureAwait(false); ;

                     await DisplayAlert("Éxito", $"Foto capturada: {photo?.FullPath}", "OK");
                    if (photo != null)
                    {
                        //await DisplayAlert("Éxito", $"Foto capturada: {photo.FullPath}", "OK");

                        Console.WriteLine(photo.FullPath);


                        string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                        using Stream stream = await photo.OpenReadAsync();

                        byte[]? imageBytes = new ImageDeviceAutoRotate().TakePhoto(stream);

                        if (imageBytes == null) return;

                        string result = Convert.ToBase64String(imageBytes);

                        MainThread.BeginInvokeOnMainThread(async () =>
                        {
                            //await Navegador.EvaluateJavaScriptAsync($"setresult_takephoto('{result}')");
                            await Navegador.EvaluateJavaScriptAsync("document.getElementById('" + param2 + "').value='" + result + "';");
                            await Navegador.EvaluateJavaScriptAsync("document.getElementById('" + param2 + "Img').src='data:image/png;base64," + result + "';");
                        });

                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Ocurrió un problema al tomar la foto: {ex.Message}", "OK");
                }
            }
        }
    }
}
