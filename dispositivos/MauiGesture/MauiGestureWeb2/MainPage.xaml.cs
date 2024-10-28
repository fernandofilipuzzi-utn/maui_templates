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

        int estadoConexion = 1;
        private void refreshView_Refreshing(object sender, EventArgs e)
        {
            if (estadoConexion == 1)
            {
                //Navegador.Source = url;
                Navegador.Reload();
            }
        }

        private void Navegador_Navigating(object sender, WebNavigatingEventArgs e)
        {
            estadoConexion = 2;
            refreshView.IsRefreshing = true; //esperando respuesta

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                ResolverConexion("RECONECTAR", null);
            }
            else
            {
                ResolverConexion("DESCONEXION", null);
            }
        }

        private void Navegador_Navigated(object sender, WebNavigatedEventArgs e)
        {
            refreshView.IsRefreshing = false; 
            estadoConexion = 1;

            if (e.Result == WebNavigationResult.Failure)
            {
                ResolverConexion("DESCONEXION", e);
            }
            else if (e.Result == WebNavigationResult.Timeout)
            {
                ResolverConexion("TIMEOUT", e);
            }
            else
            {
                var current = Connectivity.NetworkAccess;
                if (current == NetworkAccess.Internet)
                {
                    //EsperandoGPS
                    Navegador.IsVisible = true;
                    EsperandoGPS.IsVisible = false;
                    MostrarReconexion.IsVisible = false;
                    MostrarTimeout.IsVisible = false;
                }
            }
        }

        private void ResolverConexion(string opcion, WebNavigatedEventArgs e)
        {
            switch (opcion)
            {
                case "RECONECTAR":
                {
                    refreshView.IsRefreshing = false;
                    estadoConexion = 1;
                } 
                break;
                case "GEOLOCALIZANDO":
                {
                    Navegador.IsVisible = false;
                    EsperandoGPS.IsVisible = true;
                    MostrarReconexion.IsVisible = false;
                }
                break;
                case "DESCONEXION":
                {
                    Navegador.IsVisible = false;
                    EsperandoGPS.IsVisible = false;
                    MostrarTimeout.IsVisible = false;
                    MostrarReconexion.IsVisible = true;
                }
                break;
                case "TIMEOUT":
                {
                    Navegador.IsVisible = false;
                    EsperandoGPS.IsVisible = false;
                    MostrarTimeout.IsVisible = false;
                    MostrarReconexion.IsVisible = false;
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
