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

        [HttpGet("ObtengaLaListaDeAjusteDeInventario")]
        public List<Model.Inventarios>ObtengaLaListaDeInventario()
        {



            List<Model.Inventarios> lista;
            lista = ServiciosDelComercio.ObtengaLaListaDeInventarios();
            return lista;
        }


    }
}
