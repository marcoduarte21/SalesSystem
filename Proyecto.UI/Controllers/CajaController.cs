using MathNet.Numerics.Distributions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Proyecto.BL;
using Proyecto.Model;
using System.Net.Http.Headers;

namespace Proyecto.UI.Controllers
{

    [Authorize]
    public class CajaController : Controller
    {
        static string estado;
        private readonly ServicesComercio _servicesComercio;

        DA.DBContexto DBContexto;
        BL.ServicesComercio ServiciosDelComercio;
        private readonly UserManager<IdentityUser> _UserManager;

        public CajaController(DA.DBContexto connection, UserManager<IdentityUser> _userManager)
        {
            _servicesComercio = new ServicesComercio(connection);
            _UserManager = _userManager;
        }

        public async Task<IActionResult> Index()
        {
                var clientehttp = new HttpClient();
                List<Model.AperturasDeCaja> lista;

                var respuesta = await clientehttp.GetAsync("https://api-project-lenguajes.azurewebsites.net/api/AperturaDeCaja/ObtengaLaListaDeCajas");
                string respuestaDelApi = await respuesta.Content.ReadAsStringAsync();
                lista = JsonConvert.DeserializeObject<List<Model.AperturasDeCaja>>(respuestaDelApi);

                return View(lista);
            
            
            // List<AperturasDeCaja> aperturasDeCajas;
            //aperturasDeCajas = _servicesComercio.ObtengaLaListaDeCajas();
            //return View(aperturasDeCajas);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AperturasDeCaja aperturasDeCaja)
        {
            try
            {
                var httpClient = new HttpClient();

                aperturasDeCaja.UserId = _UserManager.GetUserName(User);
                string json = JsonConvert.SerializeObject(aperturasDeCaja);
                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);

                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                await httpClient.PostAsync("https://api-project-lenguajes.azurewebsites.net/api/AperturaDeCaja/CreateCaja", byteContent);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }



        public async Task<ActionResult> CerrarCaja(int id)
        {

            var httpClient = new HttpClient();

            var query = new Dictionary<string, string>()
            {

                ["id"] = id.ToString(),
            };

            var uri = QueryHelpers.AddQueryString("https://api-project-lenguajes.azurewebsites.net/api/AperturaDeCaja/CerrarCaja", query);

            var response = await httpClient.PutAsync(uri, null);

            return RedirectToAction(nameof(Index));



        }



        public async Task<ActionResult> AbrirCaja(int id)
        {

            var httpClient = new HttpClient();

            var query = new Dictionary<string, string>()
            {

                ["id"] = id.ToString(),
            };

            var uri = QueryHelpers.AddQueryString("https://api-project-lenguajes.azurewebsites.net/api/AperturaDeCaja/AbrirCaja", query);

            var response = await httpClient.PutAsync(uri, null);

            return RedirectToAction(nameof(Index));



        }


    }
}
