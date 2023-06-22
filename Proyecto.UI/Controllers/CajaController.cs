using Microsoft.AspNetCore.Mvc;
using Proyecto.BL;

namespace Proyecto.UI.Controllers
{
    
    public class CajaController : Controller
    {
        private readonly ServicesComercio _servicesComercio;

        public CajaController(ServicesComercio servicesComercio)
        {
            _servicesComercio = servicesComercio;
        }


        public IActionResult AbrirCaja()
        {
            try
            {
                _servicesComercio.AbrirCaja();
                return RedirectToAction("Index", "Home"); // Redirigir a la página de inicio u otra página deseada
            }
            catch (Exception ex)
            {
                // Manejar el error, mostrar un mensaje de error, redirigir a una página de error, etc.
                return View("Error", ex.Message);
            }
        }
        public IActionResult Index()
        {
            return View();
        }
       

    }
}
