using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Proyecto.BL;

namespace Proyecto.UI.Controllers
{
    
    public class CajaController : Controller
    {
        private readonly ServicesComercio _servicesComercio;

        public CajaController(DA.DBContexto connection, UserManager<IdentityUser> userManager)
        {
            _servicesComercio = new ServicesComercio(connection, userManager);
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
