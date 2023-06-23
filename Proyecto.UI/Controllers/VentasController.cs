using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Proyecto.UI.Controllers
{
    public class VentasController : Controller
    {

        BL.ServicesComercio ServicesComercio;

        public VentasController(DA.DBContexto connection, UserManager<IdentityUser> userManager)
        {
            ServicesComercio = new BL.ServicesComercio(connection, userManager);
        }
        

        public ActionResult ModuloVenta(Model.Ventas ventas) {

            ViewBag.ListaDeInventarios = ServicesComercio.ObtengaLaListaDeInventarios();
            return View();
        }


        public ActionResult CrearVenta(Model.Ventas ventas)
        {

            ServicesComercio.AgregueLaVenta(ventas);
            return View();
        }

    }
}
