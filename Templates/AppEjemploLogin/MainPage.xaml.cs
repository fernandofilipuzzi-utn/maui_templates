namespace AppEjemploLogin;

public partial class MainPage : ContentPage
{
   
    public MainPage()
    {
        InitializeComponent();
    }

    async protected override void OnAppearing()
    {
        base.OnAppearing();

        await Shell.Current.GoToAsync("LoginView");
    }
}
