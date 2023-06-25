using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Proyecto.BL;
using Proyecto.Model;

namespace Proyecto.UI.Controllers
{
    
    public class CajaController : Controller
    {
        private readonly ServicesComercio servicesComercio;

        public CajaController(DA.DBContexto connection, UserManager<IdentityUser> userManager)
        {
            servicesComercio = new ServicesComercio(connection, userManager);
        }


        public IActionResult Index()
        {
            List<Model.AperturasDeCaja> lista;
            lista = servicesComercio.LaListaDeCaja();
            return View(lista);
        }

        public ActionResult Create()
        {
            return View();
        }
        // POST: InventariosController/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Model.AperturasDeCaja caja)
        {
            servicesComercio.AbraLaCaja(caja);
            return View();
        }
        public ActionResult Edit(int id)
        {
            Model.AperturasDeCaja item;
            item = servicesComercio.ObtengaLaCaja(id);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Model.AperturasDeCaja item)
        {
            try
            {
                servicesComercio.CerrarCaja(item.Id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


    }

    
}
