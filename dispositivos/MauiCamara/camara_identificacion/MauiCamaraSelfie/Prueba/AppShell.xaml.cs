namespace Prueba
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();


            Routing.RegisterRoute(nameof(FacePage), typeof(FacePage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
        }
    }
}
