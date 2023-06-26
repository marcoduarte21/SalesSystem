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
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Proyecto.BL
{
    public class ServicesComercio : IServicesComercio
    {

        DA.DBContexto Connection;
        UserManager<IdentityUser> _userManager;

        public ServicesComercio(DA.DBContexto connection, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            Connection = connection;

        }

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
                UserId = ObtenerElIdUsuarioLogueado(),
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
        public void AbraLaCaja(Model.AperturasDeCaja caja)
        {

            bool cajaAbierta = Connection.AperturasDeCaja.Any(a => a.UserId == ObtenerElIdUsuarioLogueado() && a.Estado == EstadoDeLaCaja.ABIERTA);

            if (cajaAbierta)
            {
                // El usuario ya tiene una caja abierta, puedes mostrar un mensaje o realizar alguna acción
                // Aquí puedes agregar tu lógica para manejar el caso cuando el usuario ya tiene una caja abierta
                // Por ejemplo, mostrar un mensaje de error, lanzar una excepción, etc.
                Console.WriteLine("Ya tienes una caja abierta.");
                return;
            }


            caja.FechaDeInicio = ObtenerFechaActual();
            caja.UserId = ObtenerElIdUsuarioLogueado();
            caja.Estado = EstadoDeLaCaja.ABIERTA;

            Connection.AperturasDeCaja.Add(caja);
            Connection.SaveChanges();

        }
        

        public Ventas AgregueLaVenta(Ventas venta)
        {
            venta.Fecha = ObtenerFechaActual();
            venta.UserId = ObtenerElIdUsuarioLogueado();
            venta.IdAperturaDeCaja = 1;
            venta.Estado = EstadoDeLaVenta.EnProceso;

            Connection.Ventas.Add(venta);
            Connection.SaveChanges();

            return venta;
        }

        public VentaDetalles AgregueElInventarioAlDetalle(Inventarios item)
        {
            Model.VentaDetalles detalle = new VentaDetalles();
            Model.Inventarios inventarios;
            inventarios = ObtengaElItemDelInventario(item.Id);
            inventarios.VentaDetalles = new List<Model.VentaDetalles>();

            detalle.Inventarios.Nombre = item.Nombre;
            detalle.Id_Inventario = item.Id;
            detalle.Precio = item.Precio;

            inventarios.VentaDetalles.Add(detalle);
            Connection.Inventarios.Update(inventarios);
            Connection.SaveChanges();

            return detalle;
            
        }


        public VentaDetalles AgregueElItemALaVenta(Model.VentaDetalles detalle)
        {
            Model.VentaDetalles ventaDetalles = new VentaDetalles();
            Model.Ventas venta;

            venta = ObtengaLaVentaPorElId(detalle.Id_Venta);
            venta.VentaDetalles = new List<Model.VentaDetalles>();

            detalle.SubTotal = ObtengaElSubTotalDelItemDeLaVenta(detalle);
            detalle.Monto = ObtengaElTotalDelItemDeLaVenta(detalle);

            ventaDetalles = detalle;

            venta.VentaDetalles.Add(ventaDetalles);
            Connection.Ventas.Update(venta);
            Connection.SaveChanges();

            return ventaDetalles;

        }

        private decimal ObtengaElSubTotalDelItemDeLaVenta(VentaDetalles detalle)
        {
            decimal subtotal;
            subtotal = detalle.Precio * detalle.Cantidad;

            return subtotal;
        }

        private decimal ObtengaElTotalDelItemDeLaVenta(VentaDetalles detalle)
        {
            decimal total;
            total = detalle.SubTotal - detalle.MontoDescuento;

            return total;
        }

        private decimal ObtengaElTotalDeLaVenta(Model.Ventas venta)
        {
            venta.Total = venta.SubTotal - ApliqueElDescuento(venta);
            return venta.Total;
        }

        public decimal ApliqueElDescuento(Model.Ventas venta)
        {

            venta.MontoDescuento = venta.SubTotal * (venta.PorcentajeDesCuento / 100);
            return venta.MontoDescuento;
        }

        public void ElimineElItemDelDetalle(int id)
        {

            foreach(var item in Connection.VentaDetalles)
            {
                if(item.Id == id)
                {
                    Connection.VentaDetalles.Remove(item);
                }
            }
        }

        public AperturasDeCaja ObtenerIdCaja(int id)
        {
            AperturasDeCaja elResultado;

            elResultado = Connection.AperturasDeCaja.Find(id);

            return elResultado;
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
        public void AbrirCaja(AperturasDeCaja aperturasDeCaja)
        {
            AperturasDeCaja aperturaParaEditar = ObtenerIdCaja(aperturasDeCaja.Id);

            aperturaParaEditar.FechaDeInicio = DateTime.Now;
            aperturaParaEditar.Estado = EstadoDeLaCaja.ABIERTA;
            aperturaParaEditar.FechaDeCierre = null;

            Connection.AperturasDeCaja.Update(aperturasDeCaja);
            Connection.SaveChanges();
        }

        public void CerrarCaja(AperturasDeCaja aperturasDeCaja)
        {
            AperturasDeCaja aperturaParaEditar = ObtenerIdCaja(aperturasDeCaja.Id);

            aperturaParaEditar.FechaDeCierre = DateTime.Now;
            aperturaParaEditar.Estado = EstadoDeLaCaja.CERRADA;

            Connection.AperturasDeCaja.Update(aperturasDeCaja);
            Connection.SaveChanges();
        }

        public void ElimineElItemDeLaVenta(int id)
        {
            throw new NotImplementedException();
        }

        public string ObtenerElIdUsuarioLogueado()
        {
            var httpContextAccessor = new HttpContextAccessor();
            var user = httpContextAccessor.HttpContext?.User;
           
            if (user?.Identity?.IsAuthenticated == true)
            {
                string userName = _userManager.GetUserId(user);
                return userName;
            }
            else
            {
                // Manejar el caso cuando no hay usuario logueado
                throw new Exception("No hay un usuario logueado.");
            }
        }

        public string ObtenerElUserNameLogueado()
        {
            var httpContextAccessor = new HttpContextAccessor();
            var user = httpContextAccessor.HttpContext?.User;

            if (user?.Identity?.IsAuthenticated == true)
            {
                string userName = _userManager.GetUserName(user);
                return userName;
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
        public AperturasDeCaja ObtengaLaCaja(int id)
        {
            foreach (var item in Connection.AperturasDeCaja)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }
            return null;
        }
        public List<Model.AperturasDeCaja> LaListaDeCaja()
        {
            string idUsuarioLogueado = ObtenerElIdUsuarioLogueado(); // Obtener el ID del usuario logueado

            var cajas = Connection.AperturasDeCaja
                .Where(caja => caja.UserId == idUsuarioLogueado && caja.Estado == EstadoDeLaCaja.ABIERTA)
                .ToList();

            return cajas;
        }


        public List<Model.VentaDetalles> ObtengaLaListaDelDetalleLaVenta()
        {

            var lista = from detalle in Connection.VentaDetalles
                        join inventario in Connection.Inventarios 
                        on detalle.Id_Inventario equals inventario.Id
                        join venta in Connection.Ventas on detalle.Id_Venta equals venta.Id
                        select detalle;

            return (List < Model.VentaDetalles >) lista.ToList();
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

            RetorneLaCantidadFinal(NuevoAjuste, itemDelInventario);

            ajuste.CantidadActual = itemDelInventario.Cantidad;
            ajuste.Ajuste = NuevoAjuste.Ajuste;
            ajuste.Tipo = NuevoAjuste.Tipo;
            ajuste.Observaciones = NuevoAjuste.Observaciones;
            ajuste.Fecha = ObtenerFechaActual();
            ajuste.UserId = ObtenerElUserNameLogueado();

            itemDelInventario.AjusteDeInventarios.Add(ajuste);
            Connection.Inventarios.Update(itemDelInventario);
            Connection.SaveChanges();
        }

        public int RetorneLaCantidadFinal(Model.AjusteDeInventarios NuevoAjuste, Model.Inventarios itemDelInventario)
        {

            if (NuevoAjuste.Tipo == TipoDeAjuste.Aumento)
            {
                itemDelInventario.Cantidad = itemDelInventario.Cantidad + NuevoAjuste.Ajuste;
                return itemDelInventario.Cantidad;
            }
            else
            {
                itemDelInventario.Cantidad = itemDelInventario.Cantidad - NuevoAjuste.Ajuste;
                return itemDelInventario.Cantidad;
            }
        }

        public AjusteDeInventarios ObtengaLosDetallesDelAjusteDeInventario(int id)
        {
            foreach (var ajuste in Connection.AjusteDeInventarios)
            {
                if (ajuste.Id == id)
                {
                    return ajuste;
                }
            }
            return null;
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

        public List<AperturasDeCaja> ObtengaLaListaDeCajas()
        {
            return Connection.AperturasDeCaja.ToList();
        }

        public void AgregarCaja(AperturasDeCaja aperturasDeCaja)
        {
            aperturasDeCaja.FechaDeInicio = DateTime.Now;
            aperturasDeCaja.UserId = ObtenerElIdUsuarioLogueado();
            aperturasDeCaja.Estado = EstadoDeLaCaja.ABIERTA;

            Connection.AperturasDeCaja.Add(aperturasDeCaja);
            Connection.SaveChanges();
        }




    }



}
