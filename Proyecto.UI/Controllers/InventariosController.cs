using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Proyecto.UI.Controllers
{
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
        public ActionResult Index(string nombre)
        {
            if(nombre == null)
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

        // GET: InventariosController/Details/5
        public ActionResult Details(int id)
        {
            Model.Inventarios item;
            item = ServiciosDelComercio.ObtengaElItemDelInventario(id);
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
        public ActionResult Create(Model.Inventarios item)
        {
            try
            {
                ServiciosDelComercio.AgregueElItemAlInventario(item);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InventariosController/Edit/5
        public ActionResult Edit(int id)
        {
            Model.Inventarios item;
            item = ServiciosDelComercio.ObtengaElItemDelInventario(id);
            return View(item);
        }

        // POST: InventariosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Model.Inventarios item)
        {
            try
            {
                ServiciosDelComercio.EditeElItemDelInventario(item);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        
    }
}
