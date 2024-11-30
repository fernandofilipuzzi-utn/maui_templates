using AppEjemploLogin.Views;

namespace AppEjemploLogin;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute("LoginView", typeof(LoginView));
        Routing.RegisterRoute("MainPage", typeof(MainPage));
    }
}
