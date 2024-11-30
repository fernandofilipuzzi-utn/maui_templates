namespace Popup
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        async private void OnCounterClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new MensajePage("Mi dato"));
        }
    }

}
