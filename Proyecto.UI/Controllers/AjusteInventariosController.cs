using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Proyecto.DA;
using Proyecto.UI.Controllers;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Proyecto.UI.Controllers
{
    [Authorize]
    public class AjusteInventariosController : Controller
    {

        DA.DBContexto DBContexto;
        BL.ServicesComercio ServiciosDelComercio;
        private readonly UserManager<IdentityUser> _UserManager;


        public AjusteInventariosController(DA.DBContexto connection, UserManager<IdentityUser> _userManager)
        {
            
            DBContexto = connection;
            ServiciosDelComercio = new BL.ServicesComercio(connection);
            _UserManager = _userManager;
        }


        // GET: AjusteInventariosController
        public async Task<IActionResult> Index(string nombre)
        {
            var clientehttp = new HttpClient();
            List<Model.Inventarios> lista;


            if (nombre == null)
            {
                var respuesta = await clientehttp.GetAsync("https://api-project-lenguajes.azurewebsites.net/api/ServicioAjusteDeInventario/ObtengaLaListaDeAjusteDeInventarios");
                string respuestaDelApi = await respuesta.Content.ReadAsStringAsync();
                lista = JsonConvert.DeserializeObject<List<Model.Inventarios>>(respuestaDelApi);
                
                return View(lista);
            }
            else
            {
                var query = new Dictionary<string, string>()
                {

                    ["nombre"] = nombre
                };

                var uri = QueryHelpers.AddQueryString("https://api-project-lenguajes.azurewebsites.net/api/ServicioAjusteDeInventario/ObtengaLaListaPorNombre", query);

                var response = await clientehttp.GetAsync(uri);
                string apiResponse = await response.Content.ReadAsStringAsync();
                lista = JsonConvert.DeserializeObject<List<Model.Inventarios>>(apiResponse);

                return View(lista);
            }
        }

        // GET: AjusteInventariosController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var httpClient = new HttpClient();
            List<Model.AjusteDeInventarios> lista;


            var httpCliente = new HttpClient();

            var query = new Dictionary<string, string>()
            {

                ["id"] = id.ToString(),
            };

            var uri = QueryHelpers.AddQueryString("https://api-project-lenguajes.azurewebsites.net/api/ServicioAjusteDeInventario/ObtengaLaListaDeAjustes", query);

            var response = await httpCliente.GetAsync(uri);
            string apiResponse = await response.Content.ReadAsStringAsync();
            lista = JsonConvert.DeserializeObject<List<Model.AjusteDeInventarios>>(apiResponse);
            return View(lista);
        }


        public async Task< ActionResult> DetallesDelAjuste(int id)
        {
            Model.AjusteDeInventarios ajuste;
            var httpClient = new HttpClient();

            var query = new Dictionary<string, string>()
            {

                ["id"] = id.ToString(),
            };

            var uri = QueryHelpers.AddQueryString("https://api-project-lenguajes.azurewebsites.net/api/ServicioAjusteDeInventario/GetDetalleDelAjuste", query);

            var response = await httpClient.GetAsync(uri);
            string apiResponse = await response.Content.ReadAsStringAsync();
            ajuste = JsonConvert.DeserializeObject<Model.AjusteDeInventarios>(apiResponse);
            return View(ajuste);
        }

        // GET: AjusteInventariosController/Create
        public async Task<ActionResult> CreateAsync(int id)
        {

            Model.Inventarios itemSeleccionado;
            Model.AjusteDeInventarios itemAjuste;

          
            var clientehttp = new HttpClient();

            var query = new Dictionary<string, string>()
            {

                ["id"] = id.ToString()
            };
                                 
            var uri = QueryHelpers.AddQueryString("https://api-project-lenguajes.azurewebsites.net/api/ServicioAjusteDeInventario/ObtengaElInventarioPorId", query);

            var respuesta = await clientehttp.GetAsync(uri);
            string respuestaDelApi = await respuesta.Content.ReadAsStringAsync();

            itemSeleccionado = JsonConvert.DeserializeObject<Proyecto.Model.Inventarios>(respuestaDelApi);

            itemAjuste = new Model.AjusteDeInventarios
            {
                Id_Inventario = id,
                
            };

            return View(itemAjuste);
        }

        // POST: AjusteInventariosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Model.AjusteDeInventarios item)
        {
            try
            {

                //ServiciosDelComercio.AgregueElNuevoAjusteDeInventario(item);
                var clienteHttp = new HttpClient();
                item.UserId = _UserManager.GetUserName(User);
                string json = JsonConvert.SerializeObject(item);
                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);

                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");  //Revisar la linea " System.Net.Http.Headers"

                await clienteHttp.PostAsync("https://api-project-lenguajes.azurewebsites.net/api/ServicioAjusteDeInventario/RegistreUnAjusteDeInventario", byteContent);


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

    }
}

