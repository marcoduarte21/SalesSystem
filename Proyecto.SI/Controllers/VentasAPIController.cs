using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto.BL;
using System.Text.RegularExpressions;

namespace Proyecto.SI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasAPIController : ControllerBase
    {
        BL.ServicesComercio servicesComercio;

        public VentasAPIController(DA.DBContexto connection)
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
        public IActionResult CreateVenta([FromBody] Model.VentaParaIniciar ventas)
        {
            servicesComercio.AgregueLaVenta(ventas);
            return Ok(ventas);
        }

        [HttpPost("CreateItemVenta")]
        public IActionResult CreateItemVenta([FromBody] Model.DetallesVentaParaAgregar ventas)
        {

            Model.Inventarios inventarios;
            inventarios = servicesComercio.ObtengaElItemDelInventario(ventas.Id_Inventario);
            servicesComercio.AgregueElItemALaVenta(ventas);
            return Ok(ventas);
        }

        [HttpPut("DeleteItemVenta")]
        public IActionResult DeleteItemVenta(int id)
        {

            if(ModelState.IsValid)
            {
                Model.VentaDetalles ventaDetalles = servicesComercio.ObtengaElItemDelDetalle(id);

                servicesComercio.ElimineElItemDelDetalle(ventaDetalles.Id, ventaDetalles.Id_Inventario, ventaDetalles.Id_Venta);
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }

        }

        [HttpPut("ProceseLaVenta")]
        public IActionResult ProceseLaVenta([FromBody] Model.VentaParaTerminar venta)
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
        public IActionResult ApliqueDescuento([FromBody] Model.VentaParaAplicarDescuento venta)
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
