using Microsoft.Web.WebView2.Core;

namespace MauiBiometric
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
            var result = BiometricAuthenticationServer.Default.AuthenticateAsync(new CoreWebView2BasicAuthenticationRequestedEventArgs()
            {
                Title = "Please authenticate to increment",
                NegativeText = "Cancel authentication"
            }, CancellationToken.None);

            if (result.Status == BiometricResponseStatus.Success)
            {
                count++;

                if (count == 1)
                    CounterBtn.Text = $"Clicked {count} time";
                else
                    CounterBtn.Text = $"Clicked {count} times";

                SemanticScreenReader.Announce(CounterBtn.Text);
            }
            else
            {
                await DisplayAlert("Nope", "Could not authentication", "OK");
            }
        }
    }

}
