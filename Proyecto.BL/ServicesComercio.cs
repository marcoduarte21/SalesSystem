using Proyecto.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.BL
{
    public class ServicesComercio : IServicesComercio
    {
        DA.DBContexto Connection;
        public ServicesComercio(DA.DBContexto connection)
        {
            Connection = connection;
        }

        public void AgregueElItemAlInventario(Inventarios item)
        {
            item.Cantidad = 0;
            Connection.Inventarios.Add(item);
            Connection.SaveChanges();
        }

        public void AgregueLaVenta(Ventas venta)
        {
            throw new NotImplementedException();
        }

        public void ApliqueElDescuento(float porcentajeDeDescuento)
        {
            throw new NotImplementedException();
        }

        public void EditeElItemDelInventario(Inventarios item)
        {
            Model.Inventarios itemAEditar;
            itemAEditar = ObtengaElItem(item.Id);

            itemAEditar.Nombre = item.Nombre;
            itemAEditar.Categoria = item.Categoria;
            itemAEditar.Precio = item.Precio;

            Connection.Inventarios.Update(itemAEditar);
            Connection.SaveChanges();
        }

        public void ElimineElItemDeLaVenta(int id)
        {
            throw new NotImplementedException();
        }

        public Inventarios ObtengaElItem(int id)
        {
            Model.Inventarios item;

            item = Connection.Inventarios.Find(id);

            return item;
        }

        public List<Inventarios> ObtengaLaListaDeInventarios()
        {
            return Connection.Inventarios.ToList();
        }

        public List<Model.Inventarios> ObtengaLaListaDeInventariosPorElNombre(string nombre)
        {
            List<Model.Inventarios> ListaFiltrada;

            ListaFiltrada = Connection.Inventarios.Where(x => x.Nombre.Contains(nombre)).ToList();

            return ListaFiltrada;
        }

        public Ventas ObtengaLaVentaPorElId(int id)
        {
            throw new NotImplementedException();
        }

        public Ventas ProceseLaVenta(Ventas ventas)
        {
            throw new NotImplementedException();
        }
    }
}
