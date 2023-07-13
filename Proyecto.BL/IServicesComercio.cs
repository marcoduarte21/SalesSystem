using Microsoft.AspNetCore.Routing.Constraints;
using Proyecto.Model;

namespace Proyecto.BL
{
    public interface IServicesComercio
    {
        public List<Model.Inventarios> ObtengaLaListaDeInventarios();
        public List<Model.Inventarios> ObtengaLaListaDeInventariosPorElNombre(string nombre);
        public void AgregueElItemAlInventario(Model.InventariosParaAgregar item);
        public void EditeElItemDelInventario(int idItem, string nombre, Categoria categoria, decimal precio);
        public Model.Inventarios ObtengaElItemDelInventario(int id);
        public void AgregueLaVenta(Model.Ventas venta); 
        public void AgregueElItemALaVenta(Model.VentaDetalles detalle);
        public void ElimineElItemDeLaVenta(int id);
        public Model.Ventas ObtengaLaVentaPorElId(int id);
        public void ApliqueElDescuento(Model.Ventas ventas);
        public void ProceseLaVenta(Model.Ventas ventas);
        public void AbrirCaja();
        
    }
}