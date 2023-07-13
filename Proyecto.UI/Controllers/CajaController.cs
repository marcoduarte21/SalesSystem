using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Proyecto.BL;
using Proyecto.Model;

namespace Proyecto.UI.Controllers
{

    [Authorize]
    public class CajaController : Controller
    {
        static string estado;
        private readonly ServicesComercio _servicesComercio;

        public CajaController(DA.DBContexto connection)
        {
            _servicesComercio = new ServicesComercio(connection);
        }

        public ActionResult Index()
        {
            List<AperturasDeCaja> aperturasDeCajas;
            aperturasDeCajas = _servicesComercio.ObtengaLaListaDeCajas();
            return View(aperturasDeCajas);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AperturasDeCaja aperturasDeCaja)
        {
            try
            {
                _servicesComercio.AgregarCaja(aperturasDeCaja);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public IActionResult CerrarCaja(int id)
        {
            if (ModelState.IsValid)
            {
                AperturasDeCaja cerrar = _servicesComercio.ObtenerIdCaja(id);
                _servicesComercio.CerrarCaja(cerrar);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        public IActionResult AbrirCaja(int id)
        {
            if (ModelState.IsValid)
            {
                AperturasDeCaja abrir = _servicesComercio.ObtenerIdCaja(id);
                _servicesComercio.AbrirCaja(abrir);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest(ModelState);
            }

        }




    }
}
