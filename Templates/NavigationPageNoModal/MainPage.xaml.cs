using System.Collections.Generic;

namespace NavigationPageNoModal
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void btnPage1_Clicked(object sender, EventArgs e)
        {
            var parametro = new Parametro
            {
                Valor = "MauiStockTake",
            };

            var pageParams = new Dictionary<string, object> {
                { "Parametro", parametro }
            };

            await Shell.Current.GoToAsync("Page1",pageParams);
        }
    }

}
