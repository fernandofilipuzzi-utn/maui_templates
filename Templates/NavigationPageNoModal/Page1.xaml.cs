namespace NavigationPageNoModal
{
    //https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/shell/navigation?view=net-maui-8.0
    
    [QueryProperty(nameof(Parametro), "Parametro")]
    public partial class Page1 : ContentPage
    {

        public Page1()
        {
            InitializeComponent();
           // BindingContext = this;
        }

        Parametro _parametro;
        public Parametro Parametro
        {
            get
            {
                return _parametro;
            }
            set
            {
                _parametro = value;
                ParametroValor = _parametro.Valor;
            }
        }

        string _parametroValor;
        public string ParametroValor
        {
            get => _parametroValor;
            set
            {
                _parametroValor = value;
                OnPropertyChanged();
            }
        }

        private void btnFinalizar_Clicked(object sender, EventArgs e)
        {
            ParametroValor = "valor";

            Dispatcher.Dispatch(async () =>
            {
                await DisplayAlert("Result", ParametroValor, "OK");
                await Shell.Current.GoToAsync(".."); //vuelve a la página anterior
            });
        }
    }
}