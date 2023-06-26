using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Proyecto.Model;

namespace Proyecto.UI.Controllers
{
    [Authorize]
    public class VentasController : Controller
    {

        BL.ServicesComercio ServicesComercio;

        public VentasController(DA.DBContexto connection, UserManager<IdentityUser> userManager)
        {
            ServicesComercio = new BL.ServicesComercio(connection, userManager);

            List<Model.Inventarios> inventarios = ServicesComercio.ObtengaLaListaDeInventarios();
            ViewBag.ListaInventarios = inventarios;
        }


        public ActionResult Registro()
        {

            ViewBag.ListaDeInventarios = ServicesComercio.ObtengaLaListaDeInventarios();
            return View();
        }

        public ActionResult Index()
        {

            List<Model.Ventas> lista;
            lista = ServicesComercio.ObtengaLaListaDeVentas();

            return View(lista);
        }


        public IActionResult ListaInventarios()
        {
            List<Model.Inventarios> lista;
            lista = ServicesComercio.ObtengaLaListaDeInventarios();
            return PartialView("ListaInventarios", lista);
        }


        public ActionResult DetallesVenta(int id)
        {
            List<Model.VentaDetalles> lista;
            lista = ServicesComercio.ObtengaLaListaDelDetalleLaVenta(id);
            return View(lista);
        }

        public ActionResult RegistreLaVenta()
        {
            return View();
        }

        // POST: InventariosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistreLaVenta(Model.Ventas venta)
        {
            try
            {
                ServicesComercio.AgregueLaVenta(venta);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult RegistreElItemALaVenta(int id)
        {
            Model.VentaDetalles detalles = new VentaDetalles();
            Model.Ventas venta;
            venta = ServicesComercio.ObtengaLaVentaPorElId(id);
            detalles.Id_Venta = venta.Id;

            ViewBag.ListaInventarios = ServicesComercio.ObtengaLaListaDeInventarios();

            return View(detalles);
        }

        // POST: InventariosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistreElItemALaVenta(Model.VentaDetalles detalles)
        {
            try
            {
                ViewBag.ListaInventarios = ServicesComercio.ObtengaLaListaDeInventarios();

                Model.Inventarios inventarios;
                inventarios = ServicesComercio.ObtengaElItemDelInventario(detalles.Id_Inventario);
                detalles.Precio = inventarios.Precio;

                
                ServicesComercio.AgregueElItemALaVenta(detalles, inventarios);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ElimineItemDelInventario(int id)
        {
            Model.VentaDetalles ventaDetalles;
            ventaDetalles = ServicesComercio.ObtengaElItemDelDetalle(id);

            ServicesComercio.ElimineElItemDelDetalle(id, ventaDetalles.Id_Inventario, ventaDetalles.Id_Venta);
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Edit(int id)
        {
            
            return View();
        }

        // POST: InventariosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Model.Ventas ventas)
        {
            try
            {
                ServicesComercio.ProceseLaVenta(ventas);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        public ActionResult ApliqueDescuento(int id)
        {
            Model.Ventas ventas;
            ventas = ServicesComercio.ObtengaLaVentaPorElId(id);
            return View(ventas);
        }

        // POST: InventariosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApliqueDescuento(Model.Ventas ventas)
        {
            try
            {
                ServicesComercio.ApliqueElDescuento(ventas);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        
    }
}
