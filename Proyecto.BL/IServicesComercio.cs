using Microsoft.AspNetCore.Routing.Constraints;

namespace Proyecto.BL
{
    public interface IServicesComercio
    {
        public void AgregueLaVenta(Model.Ventas venta);
        public void ElimineElItemDeLaVenta(int id);
        public Model.Ventas ObtengaLaVentaPorElId(int id);
        public void ApliqueElDescuento(float porcentajeDeDescuento);
        public Model.Ventas ProceseLaVenta(Model.Ventas ventas);
    }
}