using MathNet.Numerics.Distributions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

                var respuesta = await clientehttp.GetAsync("https://localhost:7273/api/AperturaDeCaja/ObtengaLaListaDeCajas");
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
                await httpClient.PostAsync("https://localhost:7273/api/AperturaDeCaja/CreateCaja", byteContent);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }



        public async Task<ActionResult> CerrarCaja(int id)
        {

            Model.AperturasDeCaja item = new Model.AperturasDeCaja();
            try
            {
                item.Id = id;
               
                var httpClient = new HttpClient();
                string json = JsonConvert.SerializeObject(item);

                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);

                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await httpClient.PutAsync("https://localhost:7273/api/AperturaDeCaja/CerrarCaja", byteContent);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }

           
        }

        public IActionResult AbrirCaja(int id)
        {
            if (ModelState.IsValid)
            {
                AperturasDeCaja abrir = _servicesComercio.ObtenerIdCaja(id);
                _servicesComercio.AbrirCaja(abrir);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest(ModelState);
            }

        }




    }
}
