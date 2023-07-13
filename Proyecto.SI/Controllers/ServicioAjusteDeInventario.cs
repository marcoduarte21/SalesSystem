using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Proyecto.SI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioAjusteDeInventario : ControllerBase
    {
        DA.DBContexto DBContexto;
        BL.ServicesComercio ServiciosDelComercio;
       

        public ServicioAjusteDeInventario(DA.DBContexto connection)
        {

            DBContexto = connection;
            ServiciosDelComercio = new BL.ServicesComercio(connection);
           
        }

        [HttpGet("ObtengaLaListaDeAjusteDeInventarios")]
        public List<Model.Inventarios>ObtengaLaListaDeInventarios()
        {
            return ServiciosDelComercio.ObtengaLaListaDeInventarios();
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
        public IActionResult RegristreUnAjusteDeInventario([FromBody] Model.AjusteDeInventarioParaAgregar ajusteDeInventario)
        {
            ServiciosDelComercio.AgregueElNuevoAjusteDeInventario(ajusteDeInventario);
            return Ok(ajusteDeInventario);
        }



    }
}
