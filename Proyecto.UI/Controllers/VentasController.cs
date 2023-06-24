using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query;
using Proyecto.Model;

namespace Proyecto.UI.Controllers
{
    public class VentasController : Controller
    {

        BL.ServicesComercio ServicesComercio;

        public VentasController(DA.DBContexto connection, UserManager<IdentityUser> userManager)
        {
            ServicesComercio = new BL.ServicesComercio(connection, userManager);
        }
        

        public ActionResult Registro() {

            ViewBag.ListaDeInventarios = ServicesComercio.ObtengaLaListaDeInventarios();
            return View();
        }


        public ActionResult Crear(Model.Ventas ventas)
        {
            ServicesComercio.AgregueLaVenta(ventas);
            return View("Registro");
        }

        public IActionResult ListaInventarios()
        {
            List<Model.Inventarios> lista;
            lista = ServicesComercio.ObtengaLaListaDeInventarios();
            return PartialView("ListaInventarios", lista);
        }

        public IActionResult InsertarItem(int id)
        {
            Model.Inventarios inventario;
            inventario = ServicesComercio.ObtengaElItemDelInventario(id);
            Model.VentaDetalles detalles;

            detalles = ServicesComercio.AgregueElInventarioAlDetalle(inventario);

            return PartialView("DetallesVenta", detalles);
        }

        public JsonResult Inserte(Ventas venta)
        {

            ServicesComercio.AgregueLaVenta(venta);

            return Json(venta);
        }

    }
}
