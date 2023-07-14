using Microsoft.AspNetCore.Mvc;
using Proyecto.BL;
using Proyecto.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Proyecto.SI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AperturaDeCajaController : ControllerBase
    {

        DA.DBContexto DBContexto;
        BL.ServicesComercio ServiciosDelComercio;


        public AperturaDeCajaController(DA.DBContexto connection)
        {
            DBContexto = connection;
            ServiciosDelComercio = new BL.ServicesComercio(connection);
        }


        [HttpGet("ObtengaLaListaDeCajas")]
        public List<Model.AperturasDeCaja> ObtengaLaListaDeCajas()
        {
            return ServiciosDelComercio.ObtengaLaListaDeCajas();
        }

        [HttpPost("CreateCaja")]
        public IActionResult CreateVenta([FromBody] Model.AperturasDeCaja caja)
        {
            ServiciosDelComercio.AbraLaCaja(caja);
            return Ok(caja);
        }

        [HttpPut("CerrarCaja")]
        public IActionResult Edite([FromBody] Model.AperturasDeCaja item)
        {
            if (ModelState.IsValid)
            {
                //AperturasDeCaja cerrar = ServiciosDelComercio.ObtenerIdCaja(item.Id);
                ServiciosDelComercio.CerrarCaja(item.Id);
                return Ok(item);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}
