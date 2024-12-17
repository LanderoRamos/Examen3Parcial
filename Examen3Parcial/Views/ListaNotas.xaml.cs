using System;
using System.IO;
using Microsoft.Maui.Controls;
namespace Examen3Parcial.Views;

public partial class ListaNotas : ContentPage
{
    Helper.FirebaseHelper firebaseHelper = new Helper.FirebaseHelper();
    //string base64sting;
    public ListaNotas()
	{
		InitializeComponent();
        cargarproducto();

   

    }

    private async void cargarproducto()
    {
        var Nota = await firebaseHelper.GetAllProducto();
        NotasListView.ItemsSource = Nota;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender as Button;
        var producto = button?.BindingContext as Modelos.Notas;

        if (producto != null)
        {
            await Navigation.PushAsync(new Views.EditNota(producto));

        }
    }

    //boton de eliminar
    private async void Button_Clicked_1(object sender, EventArgs e)
    {
        var button = (Button)sender as Button;
        var nota = button?.BindingContext as Modelos.Notas;
        await firebaseHelper.DeleteProducto(nota.ID_NOTA);
        await DisplayAlert("Exito", "Eliminado con Extito", "OK");
        cargarproducto();
    }

    private async void Button_Clicked_2(object sender, EventArgs e)
    {
        var button = (Button)sender as Button;
        var nota = button?.BindingContext as Modelos.Notas;
       
        await DisplayAlert("Exito", "\nID_Nota: " + nota.ID_NOTA + "\nDescripcion: "+nota.DESCRIPCION+"\nFecha de Ingreso: "+nota.FECHAINGRESO+ "\nFecha de Seleccionada: " + nota.FECHA, "OK");
        cargarproducto();
    }
}