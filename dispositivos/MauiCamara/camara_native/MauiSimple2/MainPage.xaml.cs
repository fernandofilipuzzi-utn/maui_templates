namespace MauiSimple2
{
    public partial class MainPage : ContentPage
    {
     

        public MainPage()
        {
            InitializeComponent();
        }

        async private void OnBtnTakeFoto(object sender, EventArgs e)
        {
            if (MediaPicker.Default.IsCaptureSupported == false)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
                return;
            }

            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Camera>();
            }

            if (status != PermissionStatus.Granted) return;

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

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (!string.IsNullOrEmpty(btnTakeFoto?.Text))
                    {
                        SemanticScreenReader.Announce(btnTakeFoto.Text);
                    }
                });
            }
        }
    }

}
