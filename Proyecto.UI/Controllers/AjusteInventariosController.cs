using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Proyecto.DA;
using Proyecto.UI.Controllers;
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
                var respuesta = await clientehttp.GetAsync("https://localhost:7273/api/ServicioAjusteDeInventario/ObtengaLaListaDeAjusteDeInventarios");
                string respuestaDelApi = await respuesta.Content.ReadAsStringAsync();
                lista = JsonConvert.DeserializeObject<List<Model.Inventarios>>(respuestaDelApi);
                
                return View(lista);
            }
            else
            {
                List<Model.Inventarios> listaFiltradaPorElNombre;
                listaFiltradaPorElNombre = ServiciosDelComercio.ObtengaLaListaDeInventariosPorElNombre(nombre);
                return View(listaFiltradaPorElNombre);
            }
        }

        // GET: AjusteInventariosController/Details/5
        public ActionResult Details(int id)
        {
            List<Model.AjusteDeInventarios> lista;
            lista = ServiciosDelComercio.ObtengaLaListaDeAjustesDeInventario(id);
            return View(lista);
        }


        public ActionResult DetallesDelAjuste(int id)
        {
            Model.AjusteDeInventarios item;
            item = ServiciosDelComercio.ObtengaLosDetallesDelAjusteDeInventario(id);
            return View(item);
        }

        // GET: AjusteInventariosController/Create
        public async Task<ActionResult> CreateAsync(int id)
        {

            Model.Inventarios itemSeleccionado;
            Model.AjusteDeInventarios itemAjuste;

            //itemSeleccionado = ServiciosDelComercio.ObtengaElItemDelInventario(id);

            //itemAjuste = new Model.AjusteDeInventarios
            //{
            //    Id_Inventario = id,
            //    CantidadActual = itemSeleccionado.Cantidad,
            //};

            var clientehttp = new HttpClient();

            var query = new Dictionary<string, string>()
            {

                ["id"] = id.ToString()
            };
                                 
            var uri = QueryHelpers.AddQueryString("https://localhost:7273/api/ServicioAjusteDeInventario/ObtengaElInventarioPorId", query);

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

                string json = JsonConvert.SerializeObject(item);
                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);

                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");  //Revisar la linea " System.Net.Http.Headers"

               var response = await clienteHttp.PostAsync("https://localhost:7273/api/ServicioAjusteDeInventario/RegistreUnAjusteDeInventario", byteContent);


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AjusteInventariosController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AjusteInventariosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AjusteInventariosController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AjusteInventariosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

