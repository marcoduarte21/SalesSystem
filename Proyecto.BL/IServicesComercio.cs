using Microsoft.AspNetCore.Routing.Constraints;

namespace Proyecto.BL
{
    public interface IServicesComercio
    {
        public List<Model.Inventarios> ObtengaLaListaDeInventarios();
        public List<Model.Inventarios> ObtengaLaListaDeInventariosPorElNombre(string nombre);
        public void AgregueElItemAlInventario(Model.Inventarios item);
        public void EditeElItemDelInventario(Model.Inventarios item);
        public Model.Inventarios ObtengaElItemDelInventario(int id);
        public void AgregueLaVenta(Model.Ventas venta);
        public void ElimineElItemDeLaVenta(int id);
        public Model.Ventas ObtengaLaVentaPorElId(int id);
        public void ApliqueElDescuento(float porcentajeDeDescuento);
        public Model.Ventas ProceseLaVenta(Model.Ventas ventas);
    }
}