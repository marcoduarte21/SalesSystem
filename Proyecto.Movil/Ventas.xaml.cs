namespace Proyecto.Movil;

public partial class Ventas : ContentPage
{
	public Ventas()
	{
		InitializeComponent();
	}
    private void OnButton1Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new VistaEfectivo());
    }

    private void OnButton2Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new VistaTarjeta());
    }

    private void OnButton3Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new VistaSinpeMovil());
    }
}