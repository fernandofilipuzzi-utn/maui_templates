namespace MauiGestureWeb2
{
    public partial class MainPage : ContentPage
    {
        public string url { get; set; }

        public MainPage()
        {
            InitializeComponent();

            Navegador.Source = "https://www.google.com";
            
        }

        private void refreshView_Loaded(object sender, EventArgs e)
        { }

        int estado = 1;
        private void refreshView_Refreshing(object sender, EventArgs e)
        {
            if (estado == 1)
            {
                //Navegador.Source = url;
                Navegador.Reload();
            }
        }

        private void Navegador_Navigating(object sender, WebNavigatingEventArgs e)
        {
            estado = 2;
            refreshView.IsRefreshing = true; //esperando respuesta

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                ResolverDesconexion("RECONECTAR", null);
            }
            else
            {
                ResolverDesconexion("DESCONEXION", null);
            }

        }

        private void Navegador_Navigated(object sender, WebNavigatedEventArgs e)
        {
            refreshView.IsRefreshing = false; 
            estado = 1;

            if (e.Result == WebNavigationResult.Failure)
            {
                ResolverDesconexion("DESCONEXION", e);

            }
            else if (e.Result == WebNavigationResult.Timeout)
            {
                ResolverDesconexion("TIMEOUT", e);
            }
            else
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    Navegador.IsVisible = true;
                    Mensage.IsVisible = false;
                    Reconexion.IsVisible = false;
                    Timeout.IsVisible = false;
                }
            }
        }

        private void ResolverDesconexion(string opcion, WebNavigatedEventArgs e)
        {
            switch (opcion)
            {
                case "RECONECTAR":
                {
                    refreshView.IsRefreshing = false;
                    estado = 1;
                } 
                break;
                case "GEOLOCALIZANDO":
                {
                    Navegador.IsVisible = false;
                    Mensage.IsVisible = true;
                }
                break;
                case "DESCONEXION":
                {
                    Navegador.IsVisible = false;
                    Reconexion.IsVisible = true;
                }
                break;
                case "TIMEOUT":
                {
                    Navegador.IsVisible = false;
                    Timeout.IsVisible = true;
                }
                break;
            }
        }
        
        private bool NoContainParametros(string url)
        {
            return true;
        }

        public async Task<string> GetHTMLPagina(string fileName)
        {
            using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
            using var reader = new StreamReader(stream);

            var contents = reader.ReadToEnd();
            return contents;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
        }
    }
}
