using Microsoft.AspNetCore.Mvc;


using Microsoft.AspNetCore.Mvc;
using Proyecto.DA;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Proyecto.SI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioDeInventariosController : ControllerBase
    {
        DA.DBContexto DBContexto;
        BL.ServicesComercio ServiciosDelComercio;
        public ServicioDeInventariosController(DA.DBContexto connection)
        {

            DBContexto = connection;
            ServiciosDelComercio = new BL.ServicesComercio(connection);

        }

        [HttpGet("ObtengaLaListaDeInventarios")]
        public List<Model.Inventarios> ObtengaLaListaDeInventarios()
        {
            return ServiciosDelComercio.ObtengaLaListaDeInventarios();
        }

        [HttpGet("ObtengaLaListaPorNombre")]
        public List<Model.Inventarios> ObtengaLaListaPorNombre(string nombre)
        {
            return ServiciosDelComercio.ObtengaLaListaDeInventariosPorElNombre(nombre);
        }


        [HttpGet("ObtengaElInventarioPorId")]
        public Proyecto.Model.Inventarios ObtengaElInventarioPorId(int id)
        {
            Model.Inventarios elResultado;
            elResultado = ServiciosDelComercio.ObtengaElItemDelInventario(id);
            elResultado.Precio = (int)elResultado.Precio;
            return elResultado;
        }


        [HttpPost("AgregueElItemAlInventario")]
        public IActionResult RegistreUnItemEnElInventario([FromBody] Model.InventariosParaAgregar item)
        {
            ServiciosDelComercio.AgregueElItemAlInventario(item);
            return Ok(item);
        }


        [HttpPut("EditarItemDelInventario")]
        public IActionResult Edite([FromBody] Model.InventariosParaAgregar item)
        {
            if (ModelState.IsValid)
            {
                ServiciosDelComercio.EditeElItemDelInventario(item.Id, item.Nombre, item.Categoria, item.Precio);
                return Ok(item);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


    }
}
