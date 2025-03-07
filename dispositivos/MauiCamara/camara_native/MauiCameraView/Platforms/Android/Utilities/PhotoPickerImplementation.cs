using Android.App;
using Android.Content;
using Android.Provider;

[assembly: Dependency(typeof(MauiCameraView.Platforms.Android.Utilities.PhotoPickerImplementation))]
namespace MauiCameraView.Platforms.Android.Utilities
{
    public class PhotoPickerImplementation : IPhotoPicker
    {
        public async Task<Stream?> CapturePhotoAsync()
        {
            // Crear el Intent para abrir la cámara
            var intent = new Intent(MediaStore.ActionImageCapture);

            // Verificar si hay una actividad de cámara disponible
            var packageManager = global::Android.App.Application.Context;
            //var activities = packageManager.QueryIntentActivities(intent, 0);
            //if (activities.Count == 0)
            //    return null; // Si no hay cámara disponible, retornar null

            // Crear archivo temporal para guardar la foto
            var photoFile = new Java.IO.File(global::Android.App.Application.Context.GetExternalFilesDir(null), "photo.jpg");
            //var photoUri = global::Android.Net.Uri.FromFile(photoFile);
            var photoUri = FileProvider.GetUriForFile(
    global::Android.App.Application.Context,
    "MauiCameraView", // Debe coincidir con la autoridad en el manifest
    photoFile);

            // Establecer el archivo donde la foto será guardada
            intent.PutExtra(MediaStore.ExtraOutput, photoUri);

            // Lanzar la actividad de cámara
            var activity = (Activity)Microsoft.Maui.ApplicationModel.Platform.CurrentActivity;
            activity.StartActivityForResult(intent, 100); // Código de solicitud arbitrario 100

            // Esperar la respuesta (esto debería hacerse en un método más asíncrono en un contexto real)
            await Task.Delay(1000); // Solo para simular el proceso asincrónico

            // Devolver el archivo como un stream
            return new FileStream(photoFile.AbsolutePath, FileMode.Open);
        }
    }
}