using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Proyecto.DA;
using Proyecto.UI.Controllers;

namespace Proyecto.UI.Controllers
{
    [Authorize]
    public class AjusteInventariosController : Controller
    {

        DA.DBContexto DBContexto;
        BL.ServicesComercio ServiciosDelComercio;

        public AjusteInventariosController(DA.DBContexto connection, UserManager<IdentityUser> userManager)
        {
            
            DBContexto = connection;
            ServiciosDelComercio = new BL.ServicesComercio(connection, userManager);
        }


        // GET: AjusteInventariosController
        public async Task< ActionResult> Index(string nombre)
        {
            var clientehttp = new HttpClient();
            List<Model.Inventarios> lista;


            if (nombre == null)
            {
                var respuesta = await clientehttp.GetAsync("https://localhost:7273/api/ServicioAjusteDeInventario/ObtengaLaListaDeAjusteDeInventario");
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
        public ActionResult Create(int id)
        {

            Model.Inventarios itemSeleccionado;
            Model.AjusteDeInventarios itemAjuste;
            itemSeleccionado = ServiciosDelComercio.ObtengaElItemDelInventario(id);

            itemAjuste = new Model.AjusteDeInventarios
            {
                Id_Inventario = id,
                CantidadActual = itemSeleccionado.Cantidad,
            };
            return View(itemAjuste);
        }

        // POST: AjusteInventariosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Model.AjusteDeInventarios item)
        {
            try
            {
                ServiciosDelComercio.AgregueElNuevoAjusteDeInventario(item);
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

