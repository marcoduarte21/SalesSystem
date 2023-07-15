namespace Proyecto.Movil;

public partial class VistaVentas : ContentPage
{
	public VistaVentas()
	{
		InitializeComponent();
	}

    private void OnButton1Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new Ventas());
    }

    private void OnButton2Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new VistaInventarios());
    }
}