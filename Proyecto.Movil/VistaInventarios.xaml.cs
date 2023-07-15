using Newtonsoft.Json;
using Proyecto.Model;

namespace Proyecto.Movil;

public partial class VistaInventarios : ContentPage
{
	public VistaInventarios()
	{
		InitializeComponent();
	}
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var inventarios = await ObtengaLaLista();

        inventarioListView.ItemsSource = inventarios;
    }

    private async Task<List<Inventarios>> ObtengaLaLista()
    {
        var httpClient = new HttpClient();

        var respuesta = await httpClient.GetAsync("https://api-project-lenguajes.azurewebsites.net/api/ServicioDeInventarios/ObtengaLaListaDeInventarios");
        string apiResponse = await respuesta.Content.ReadAsStringAsync();

        var inventarios = JsonConvert.DeserializeObject<List<Inventarios>>(apiResponse);

        return inventarios;
    }
}