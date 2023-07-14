using MathNet.Numerics.Distributions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Proyecto.Model;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Proyecto.UI.Controllers
{
    [Authorize]
    public class VentasController : Controller
    {

        BL.ServicesComercio ServicesComercio;
        private readonly UserManager<IdentityUser> _userManager;

        public VentasController(DA.DBContexto connection, UserManager<IdentityUser> userManager)
        {
            ServicesComercio = new BL.ServicesComercio(connection);

            List<Model.Inventarios> inventarios = ServicesComercio.ObtengaLaListaDeInventarios();
            ViewBag.ListaInventarios = inventarios;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {

            var httpClient = new HttpClient();
            List<Model.Ventas> lista;


            var respuesta = await httpClient.GetAsync("https://ventas-proyecto-ucr.azurewebsites.net/api/VentasAPI/GetVentas");
            string apiRespuesta = await respuesta.Content.ReadAsStringAsync();
            lista = JsonConvert.DeserializeObject<List<Model.Ventas>>(apiRespuesta);

            return View(lista);
        }


        public async Task <IActionResult> ListaInventarios()
        {
            var httpClient = new HttpClient();
            List<Model.Inventarios> lista;

            var respuesta = await httpClient.GetAsync("https://ventas-proyecto-ucr.azurewebsites.net/api/VentasAPI/ObtengaLaListaDeInventarios");
            string apiRespuesta = await respuesta.Content.ReadAsStringAsync();
            lista = JsonConvert.DeserializeObject<List<Model.Inventarios>>(apiRespuesta);


            return PartialView("ListaInventarios", lista);
        }


        public async Task <IActionResult> DetallesVenta(int id)
        {
            List<Model.VentaDetalles> lista;
            var httpCliente = new HttpClient();

            var query = new Dictionary<string, string>()
            {

                ["id"] = id.ToString(),
            };

            var uri = QueryHelpers.AddQueryString("https://ventas-proyecto-ucr.azurewebsites.net/api/VentasAPI/GetDetallesVenta", query);

            var response = await httpCliente.GetAsync(uri);
            string apiResponse = await response.Content.ReadAsStringAsync();
            lista = JsonConvert.DeserializeObject<List<Model.VentaDetalles>>(apiResponse);

            return View(lista);
        }

        public ActionResult RegistreLaVenta()
        {
            
            return View();
        }

        // POST: InventariosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistreLaVenta(Model.VentaParaIniciar venta)
        {
            try
            {
                var httpClient = new HttpClient();
                
                venta.UserId = _userManager.GetUserName(User);
                string json = JsonConvert.SerializeObject(venta);
                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);

                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                await httpClient.PostAsync("https://ventas-proyecto-ucr.azurewebsites.net/api/VentasAPI/CreateVenta", byteContent);
                
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> RegistreElItemALaVenta(int id)
        {
            Model.VentaDetalles detalles = new VentaDetalles();
            Model.Ventas venta;

            var httpClient = new HttpClient();

            var query = new Dictionary<string, string>()
            {

                ["id"] = id.ToString(),
            };

            var uri = QueryHelpers.AddQueryString("https://ventas-proyecto-ucr.azurewebsites.net/api/VentasAPI/GetVentaById", query);

            var response = await httpClient.GetAsync(uri);
            string apiResponse = await response.Content.ReadAsStringAsync();
            venta = JsonConvert.DeserializeObject<Model.Ventas>(apiResponse);


            detalles.Id_Venta = venta.Id;


            List<Model.Inventarios> lista;

            var respuesta = await httpClient.GetAsync("https://ventas-proyecto-ucr.azurewebsites.net/api/VentasAPI/ObtengaLaListaDeInventarios");
            string apiRespuesta = await respuesta.Content.ReadAsStringAsync();
            lista = JsonConvert.DeserializeObject<List<Model.Inventarios>>(apiRespuesta);

            ViewBag.ListaInventarios = lista;

            return View(detalles);
        }

        // POST: InventariosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> RegistreElItemALaVenta(Model.DetallesVentaParaAgregar detalles)
        {
            try
            {

                var httpClient = new HttpClient();


                List<Model.Inventarios> lista;

                var respuesta = await httpClient.GetAsync("https://ventas-proyecto-ucr.azurewebsites.net/api/VentasAPI/ObtengaLaListaDeInventarios");
                string apiRespuesta = await respuesta.Content.ReadAsStringAsync();
                lista = JsonConvert.DeserializeObject<List<Model.Inventarios>>(apiRespuesta);

                ViewBag.ListaInventarios = lista;

                string json = JsonConvert.SerializeObject(detalles);
                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);

                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                await httpClient.PostAsync("https://ventas-proyecto-ucr.azurewebsites.net/api/VentasAPI/CreateItemVenta", byteContent);


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> ElimineItemDeLaVenta(int id)
        {
            var httpClient = new HttpClient();

            var query = new Dictionary<string, string>()
            {

                ["id"] = id.ToString(),
            };

            var uri = QueryHelpers.AddQueryString("https://ventas-proyecto-ucr.azurewebsites.net/api/VentasAPI/DeleteItemVenta", query);

            var response = await httpClient.PutAsync(uri, null);

            return RedirectToAction(nameof(Index));
        }

        public ActionResult ProceseLaVenta(int id)
        {

            return View();
        }

        // POST: InventariosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <ActionResult> ProceseLaVenta(Model.VentaParaTerminar ventas)
        {
            try
            {


                var httpClient = new HttpClient();
                string json = JsonConvert.SerializeObject(ventas);

                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);

                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await httpClient.PutAsync("https://ventas-proyecto-ucr.azurewebsites.net/api/VentasAPI/ProceseLaVenta", byteContent);
               
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        public ActionResult ApliqueDescuento(int id)
        {
            
            return View();
        }

        // POST: InventariosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ApliqueDescuento(Model.VentaParaAplicarDescuento ventas)
        {
            try
            {

                var httpClient = new HttpClient();
                string json = JsonConvert.SerializeObject(ventas);

                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);

                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await httpClient.PutAsync("https://ventas-proyecto-ucr.azurewebsites.net/api/VentasAPI/ApliqueDescuento", byteContent);
             
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<List<Model.Inventarios>> ListaInventariosParaVentas()
        {
            var httpClient = new HttpClient();
            List<Model.Inventarios> lista;

            var respuesta = await httpClient.GetAsync("https://ventas-proyecto-ucr.azurewebsites.net/api/VentasAPI/ObtengaLaListaDeInventarios");
            string apiRespuesta = await respuesta.Content.ReadAsStringAsync();
            lista = JsonConvert.DeserializeObject<List<Model.Inventarios>>(apiRespuesta);


            return lista.ToList();
        }

    }
}
