namespace Popup;

public partial class MensajePage : ContentPage
{
	public MensajePage(string miDato)
	{
		InitializeComponent();

        lbNombre.Text = miDato;
	}

    async private void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}