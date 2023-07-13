using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto.BL;
using System.Text.RegularExpressions;

namespace Proyecto.SI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasAPI : ControllerBase
    {
        BL.ServicesComercio servicesComercio;

        public VentasAPI(DA.DBContexto connection)
        {
            servicesComercio = new BL.ServicesComercio(connection);
        }


        [HttpGet("GetVentas")]
        // GET: VentasAPI
        public List<Model.Ventas> GetVentas()
        {
            return servicesComercio.ObtengaLaListaDeVentas();
        }

        [HttpGet("GetDetallesVenta")]
        public List<Model.VentaDetalles> GetVentaDetalles(int id)
        {
            return servicesComercio.ObtengaLaListaDelDetalleLaVenta(id);
        }

        [HttpGet("GetVentaById")]
        public Model.Ventas GetVentasById(int id)
        {
            return servicesComercio.ObtengaLaVentaPorElId(id);

        }

        [HttpPost("CreateVenta")]
        public IActionResult CreateVenta([FromBody] Model.Ventas ventas)
        {
            servicesComercio.AgregueLaVenta(ventas);
            return Ok(ventas);
        }

        [HttpPost("CreateItemVenta")]
        public IActionResult CreateItemVenta([FromBody] Model.VentaDetalles ventas)
        {
            servicesComercio.AgregueElItemALaVenta(ventas);
            return Ok(ventas);
        }

        [HttpPut("DeleteItemVenta")]
        public IActionResult DeleteItemVenta([FromBody] Model.VentaDetalles ventaDetalles)
        {

            if(ModelState.IsValid)
            {
                servicesComercio.ElimineElItemDelDetalle(ventaDetalles.Id, ventaDetalles.Id_Inventario, ventaDetalles.Id_Venta);
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        [HttpPut("ProceseLaVenta")]
        public IActionResult ProceseLaVenta([FromBody] Model.Ventas venta)
        {

            if (ModelState.IsValid)
            {
                servicesComercio.ProceseLaVenta(venta);
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        [HttpPut("ApliqueDescuento")]
        public IActionResult ApliqueDescuento([FromBody] Model.Ventas venta)
        {

            if (ModelState.IsValid)
            {
                servicesComercio.ApliqueElDescuento(venta);
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

    }
}
