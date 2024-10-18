using System.Reflection;
using System.Text;

namespace MauiGestureWeb
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
        {}

        int estado =1;
        private void refreshView_Refreshing(object sender, EventArgs e)
        {

            //if (toRefresh == true)
            //{
            //    Navegador.Source = url;
            // //   refresh = true;
            //}
            //else
            //{ 
            //  //  refresh = false;
            //}

            if (estado == 1)
            {
                Navegador.Source = url;
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
                //
            }
            else
            {
                ResolverDesconexion(e.Url, e);
            }
        }

        private void Navegador_Navigated(object sender, WebNavigatedEventArgs e)
        {
            refreshView.IsRefreshing = false; //respuesta recibida
            estado = 1;

            if (e.Result == WebNavigationResult.Failure)
            {
                ResolverDesconexion(e.Url, null);

            }
            else if (e.Result == WebNavigationResult.Timeout)
            {
                ResolverTimeOut(e.Url, null);
            }
        }

        private void ResolverDesconexion(string url, WebNavigatingEventArgs e)
        {
            refreshView.IsRefreshing = false; //respuesta recibida
            estado = 1;

            if (NoContainParametros(url))
            {
                this.url = url;
            }

            var htmlSource = new HtmlWebViewSource()
            {
                Html = GetHTMLPagina("satelite.html").Result//GetHTMLSinConexion().Result
            };

            if (e != null)
                e.Cancel = true;

            Navegador.Source = htmlSource;
        }

        private void ResolverTimeOut(string url, WebNavigatingEventArgs e)
        {
            #region
            if (NoContainParametros(url))
            {
                this.url = url;
            }
            #endregion

            var htmlSource = new HtmlWebViewSource()
            {
                Html = GetHTMLPagina("timeout.html").Result
            };

            if (e != null)
                e.Cancel = true;

            Navegador.Source = htmlSource;
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
    }
}
