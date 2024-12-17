namespace Examen3Parcial.Views;

public partial class ABMenu : ContentPage
{
	public ABMenu()
	{
		InitializeComponent();
	}

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.Agregar());
    }

    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.ListaNotas());
    }
}