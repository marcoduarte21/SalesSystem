using Newtonsoft.Json;
using Proyecto.Model;

namespace Proyecto.Movil;

public partial class VistaSinpeMovil : ContentPage
{
	public VistaSinpeMovil()
	{
		InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var ventasPorDia = await CalcularTotalVentasEnEfectivo();

        ventasListView.ItemsSource = ventasPorDia;
    }


    private async Task<List<Venta>> CalcularTotalVentasEnEfectivo()
    {
        List<Venta> laListaDeVentas;
        var httpClient = new HttpClient();

        var respuesta = await httpClient.GetAsync("https://api-project-lenguajes.azurewebsites.net/api/VentasAPI/GetVentas");
        string apiResponse = await respuesta.Content.ReadAsStringAsync();
        laListaDeVentas = JsonConvert.DeserializeObject<List<Venta>>(apiResponse);

        var ventasPorDia = laListaDeVentas
            .Where(venta => venta.TipoDePago.Equals(TipoDePago.SINPEMovil))
            .GroupBy(venta => venta.Fecha.Date)
            .Select(grupo => new Venta { Fecha = grupo.Key, Total = grupo.Sum(venta => venta.Total) })
            .ToList();

        return ventasPorDia;
    }
}