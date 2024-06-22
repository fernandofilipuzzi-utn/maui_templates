namespace NavegationPage
{
    public partial class MainPage : ContentPage
    {       
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnNextPageButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("Page1");
        }
    }
}
