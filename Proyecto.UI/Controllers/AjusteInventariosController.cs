using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Proyecto.DA;
using Proyecto.UI.Controllers;

namespace Proyecto.UI.Controllers
{
    [Authorize]
    public class AjusteInventariosController : Controller
    {

        DA.DBContexto DBContexto;
        BL.ServicesComercio ServiciosDelComercio;
        private readonly UserManager<IdentityUser> _userManager;


        public AjusteInventariosController(DA.DBContexto connection, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            DBContexto = connection;
            ServiciosDelComercio = new BL.ServicesComercio(connection);
        }


        // GET: AjusteInventariosController
        public ActionResult Index(string nombre)
        {
            if (nombre == null)
            {
                List<Model.Inventarios> lista;
                lista = ServiciosDelComercio.ObtengaLaListaDeInventarios();
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

        // GET: AjusteInventariosController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AjusteInventariosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Model.AjusteDeInventarios item, int id)
        {
            try
            {
                ViewBag.IdInventario = id;
                item.UserId = _userManager.GetUserId(HttpContext.User);

                ServiciosDelComercio.AgregueElNuevoAjusteDeInventario(item, id);
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

