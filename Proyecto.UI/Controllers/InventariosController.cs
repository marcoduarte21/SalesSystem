using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using Proyecto.UI.Models;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace Proyecto.UI.Controllers
{
    [Authorize]
    public class InventariosController : Controller
    {
        DA.DBContexto DBContexto;
        BL.ServicesComercio ServiciosDelComercio;

        public InventariosController(DA.DBContexto connection)
        {
            DBContexto = connection;
            ServiciosDelComercio = new BL.ServicesComercio(connection);
        }


        // GET: InventariosController
        public async Task<IActionResult> Index(string nombre)
        {
            var clientehttp = new HttpClient();
            List<Model.Inventarios> lista;


            if (nombre == null)
            {
                var respuesta = await clientehttp.GetAsync("https://ventas-proyecto-ucr.azurewebsites.net/api/ServicioDeInventarios/ObtengaLaListaDeInventarios");
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

                var uri = QueryHelpers.AddQueryString("https://ventas-proyecto-ucr.azurewebsites.net/api/ServicioDeInventarios/ObtengaLaListaPorNombre", query);

                var response = await clientehttp.GetAsync(uri);
                string apiResponse = await response.Content.ReadAsStringAsync();
                lista = JsonConvert.DeserializeObject<List<Model.Inventarios>>(apiResponse);

                return View(lista);
            }
        }

        // GET: InventariosController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            Model.Inventarios item;
            var clientehttp = new HttpClient();

            var query = new Dictionary<string, string>()
            {

                ["id"] = id.ToString()
            };

            var uri = QueryHelpers.AddQueryString("https://ventas-proyecto-ucr.azurewebsites.net/api/ServicioDeInventarios/ObtengaElInventarioPorId", query);

            var respuesta = await clientehttp.GetAsync(uri);
            string respuestaDelApi = await respuesta.Content.ReadAsStringAsync();

            item = JsonConvert.DeserializeObject<Proyecto.Model.Inventarios>(respuestaDelApi);



            return View(item);


        }

        // GET: InventariosController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InventariosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Model.Inventarios item)
        {
            try
            {
                var httpClient = new HttpClient();

                string json = JsonConvert.SerializeObject(item);
                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);

                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                await httpClient.PostAsync("https://ventas-proyecto-ucr.azurewebsites.net/api/ServicioDeInventarios/AgregueElItemAlInventario", byteContent);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }


        }

        // GET: InventariosController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            Model.Inventarios item;
            var clientehttp = new HttpClient();

            var query = new Dictionary<string, string>()
            {

                ["id"] = id.ToString()
            };

            var uri = QueryHelpers.AddQueryString("https://ventas-proyecto-ucr.azurewebsites.net/api/ServicioDeInventarios/ObtengaElInventarioPorId", query);

            var respuesta = await clientehttp.GetAsync(uri);
            string respuestaDelApi = await respuesta.Content.ReadAsStringAsync();

            item = JsonConvert.DeserializeObject<Proyecto.Model.Inventarios>(respuestaDelApi);



            return View(item);
        }

        // POST: InventariosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Model.Inventarios item)
        {
            try
            {
                Model.InventariosParaAgregar itemParaEditar = new Model.InventariosParaAgregar();
                itemParaEditar.Id = item.Id;
                itemParaEditar.Nombre = item.Nombre;
                itemParaEditar.Categoria = item.Categoria;
                itemParaEditar.Precio = item.Precio;


                var httpClient = new HttpClient();
                string json = JsonConvert.SerializeObject(itemParaEditar);

                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);

                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await httpClient.PutAsync("https://ventas-proyecto-ucr.azurewebsites.net/api/ServicioDeInventarios/EditarItemDelInventario", byteContent);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }

        }


    }
}