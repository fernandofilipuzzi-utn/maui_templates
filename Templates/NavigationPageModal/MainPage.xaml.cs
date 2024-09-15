namespace NavigationPageModal
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private  async void btnPage1_Clicked(object sender, EventArgs e)
        {
            var tcs = new TaskCompletionSource<string>();
            var page1 = new Page1(tcs);

            await Navigation.PushAsync(page1);
            string Valor = await tcs.Task;

            await DisplayAlert("Valor devuelto", $"Valor devuelto: {Valor}", "OK");
        }
    }

}
