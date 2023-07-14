using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Proyecto.SI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioAjusteDeInventarioController : ControllerBase
    {
        DA.DBContexto DBContexto;
        BL.ServicesComercio ServiciosDelComercio;
       

        public ServicioAjusteDeInventarioController(DA.DBContexto connection)
        {

            DBContexto = connection;
            ServiciosDelComercio = new BL.ServicesComercio(connection);
           
        }

        [HttpGet("ObtengaLaListaDeAjusteDeInventarios")]
        public List<Model.Inventarios>ObtengaLaListaDeInventarios()
        {
            return ServiciosDelComercio.ObtengaLaListaDeInventarios();
        }

        [HttpGet("ObtengaLaListaPorNombre")]
        public List<Model.Inventarios> ObtengaLaListaPorNombre(string nombre)
        {
            return ServiciosDelComercio.ObtengaLaListaDeInventariosPorElNombre(nombre);
        }

        // GET api/<MenuController>/5
        [HttpGet("ObtengaElInventarioPorId")]
        public Proyecto.Model.Inventarios ObtengaElInventarioPorId(int id)
        {
            Model.Inventarios elResultado;
            elResultado = ServiciosDelComercio.ObtengaElItemDelInventario(id);
            return elResultado;
        }

        [HttpPost("RegistreUnAjusteDeInventario")]
        public IActionResult RegistreUnAjusteDeInventario([FromBody] Model.AjusteDeInventarioParaAgregar ajusteDeInventario)
        {
            ServiciosDelComercio.AgregueElNuevoAjusteDeInventario(ajusteDeInventario);
            return Ok(ajusteDeInventario);
        }

        [HttpGet("ObtengaLaListaDeAjustes")]
        public List<Model.AjusteDeInventarios> ObtengaLaListaDeAjustes(int id)
        {

            return ServiciosDelComercio.ObtengaLaListaDeAjustesDeInventario(id);
        }

        [HttpGet("GetDetalleDelAjuste")]
        public Model.AjusteDeInventarios GetDetalleDelAjuste(int id)
        {
            return ServiciosDelComercio.ObtengaLosDetallesDelAjusteDeInventario(id) ;
        }

    }
}
