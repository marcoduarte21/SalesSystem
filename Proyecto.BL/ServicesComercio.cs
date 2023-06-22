using Microsoft.AspNetCore.Mvc;
using Proyecto.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Http;

namespace Proyecto.BL
{
    public class ServicesComercio : IServicesComercio
    {

        DA.DBContexto Connection;
        
        public ServicesComercio(DA.DBContexto connection)
        {
            Connection = connection;
            
        }

        public void AbrirCaja()
        {
            // Crear nueva apertura de caja
            AperturasDeCaja nuevaApertura = new AperturasDeCaja
            {
                FechaDeInicio = DateTime.Now,
                UserId = int.Parse(ObtenerUsuarioLogueado()),
                Estado = EstadoDeLaCaja.ABIERTA // Asignar el estado de la caja a "Abierta"
            };

            Connection.AperturasDeCaja.Add(nuevaApertura);
            Connection.SaveChanges();
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

        public void CerrarCaja()
        {
            throw new NotImplementedException();
        }

        public void EditeElItemDelInventario(int idItem, string nombre, Categoria categoria, decimal precio)
        {
            Model.Inventarios itemAEditar;
            itemAEditar = ObtengaElItemDelInventario(idItem);

            itemAEditar.Nombre = nombre;
            itemAEditar.Categoria = categoria;
            itemAEditar.Precio = precio;

            Connection.Inventarios.Update(itemAEditar);
            Connection.SaveChanges();
        }

        public void ElimineElItemDeLaVenta(int id)
        {
            throw new NotImplementedException();
        }

        public string ObtenerUsuarioLogueado()
        {
            var httpContextAccessor = new HttpContextAccessor();
            var user = httpContextAccessor.HttpContext?.User;

            if (user?.Identity?.IsAuthenticated == true)
            {
                string usuario = user.Identity.Name;
                return usuario;
            }
            else
            {
                // Manejar el caso cuando no hay usuario logueado
                throw new Exception("No hay un usuario logueado.");
            }
        }

        public Inventarios ObtengaElItemDelInventario(int id)
        {
            foreach(var item in Connection.Inventarios)
            {
                if(item.Id == id)
                {
                    return item;
                }
            }
            return null;
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

        public void AgregueElNuevoAjusteDeInventario(AjusteDeInventarios NuevoAjuste)
        {

            Model.Inventarios itemDelInventario;
            Model.AjusteDeInventarios ajuste = new AjusteDeInventarios();
            itemDelInventario = ObtengaElItemDelInventario(NuevoAjuste.Id_Inventario);
            itemDelInventario.AjusteDeInventarios = new List<Model.AjusteDeInventarios>();
            if(NuevoAjuste.Tipo == TipoDeAjuste.Aumento)
            {
                itemDelInventario.Cantidad = itemDelInventario.Cantidad + NuevoAjuste.Ajuste;
            }
            else
            {
                itemDelInventario.Cantidad = itemDelInventario.Cantidad - NuevoAjuste.Ajuste;
            }
            ajuste.CantidadActual = NuevoAjuste.CantidadActual;
            ajuste.Ajuste = NuevoAjuste.Ajuste;
            ajuste.Tipo = NuevoAjuste.Tipo;
            ajuste.Observaciones = NuevoAjuste.Observaciones;
            ajuste.Fecha = ObtenerFechaActual();
            ajuste.UserId = ObtenerUsuarioLogueado();
            itemDelInventario.AjusteDeInventarios.Add(ajuste);
            Connection.Inventarios.Update(itemDelInventario);
            Connection.SaveChanges();
        }

        public List<AjusteDeInventarios> ObtengaLaListaDeAjustesDeInventario(int id)
        {
            return Connection.AjusteDeInventarios.Where(a => a.Id_Inventario == id).ToList();
        }


        public DateTime ObtenerFechaActual()
        {
            DateTime fechaActual = DateTime.Now;
            return fechaActual;
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
