using System.Threading.Tasks;

namespace NavigationPageModal;

public partial class Page1 : ContentPage
{
    private TaskCompletionSource<string> _taskCompletionSource;

    public Page1(TaskCompletionSource<string> taskCompletionSource)
    {
		InitializeComponent();
        _taskCompletionSource = taskCompletionSource;
    }

    private void btnFinalizar_Clicked(object sender, EventArgs e)
    {
        string resultado = "valor";
        _taskCompletionSource.TrySetResult(resultado);

        Dispatcher.Dispatch(async () =>
        {
            await DisplayAlert("Result", resultado, "OK");
            await Navigation.PopAsync(); 
        });
    }
}