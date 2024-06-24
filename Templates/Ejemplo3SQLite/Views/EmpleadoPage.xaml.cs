using Ejemplo3SQLite.ViewModels;

namespace Ejemplo3SQLite.Views;

public partial class EmpleadoPage : ContentPage
{
	public EmpleadoPage(EmpleadoViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}