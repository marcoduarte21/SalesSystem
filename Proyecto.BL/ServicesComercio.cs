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
using System.Runtime.CompilerServices;

namespace Proyecto.BL
{
    public class ServicesComercio : IServicesComercio
    {

        public DA.DBContexto Connection;

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

        public List<Model.Ventas> ObtengaLaListaDeVentas()
        {
            return Connection.Ventas.ToList();
        }

        public void AgregueElItemAlInventario(InventariosParaAgregar item)
        {
            Model.Inventarios itemDelInventario = new Inventarios();
            itemDelInventario.Id = item.Id;
            itemDelInventario.Nombre = item.Nombre;
            itemDelInventario.Categoria = item.Categoria;
            itemDelInventario.Precio = item.Precio;
            itemDelInventario.Cantidad = 0;
            item.Cantidad = 0;
            Connection.Inventarios.Add(itemDelInventario);
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
        

        public void AgregueLaVenta(Ventas venta)
        {
            venta.Fecha = ObtenerFechaActual();
            venta.IdAperturaDeCaja = 1;
            venta.Estado = EstadoDeLaVenta.EnProceso;

            Connection.Ventas.Add(venta);
            Connection.SaveChanges();

        }

      
        public void AgregueElItemALaVenta(Model.VentaDetalles detalle)
        {
            Model.VentaDetalles ventaDetalles = new VentaDetalles();
            Model.Ventas venta;
            Model.Inventarios inventario;

            venta = ObtengaLaVentaPorElId(detalle.Id_Venta);
            venta.VentaDetalles = new List<Model.VentaDetalles>();

            ventaDetalles.Id_Venta = detalle.Id_Venta;
            ventaDetalles.Id_Inventario = detalle.Id_Inventario;
            ventaDetalles.Precio = detalle.Precio;
            ventaDetalles.Cantidad = detalle.Cantidad;
            ventaDetalles.Monto = ObtengaElTotalDelItemDeLaVenta(ventaDetalles);

            venta.SubTotal += ventaDetalles.Monto;
            venta.Total += ventaDetalles.Monto;

            inventario = ObtengaElItemDelInventario(detalle.Id_Inventario);
            inventario.Cantidad = inventario.Cantidad - ventaDetalles.Cantidad;

            venta.VentaDetalles.Add(ventaDetalles);
            Connection.Inventarios.Update(inventario);
            Connection.Ventas.Update(venta);
            Connection.SaveChanges();

        }

        private decimal ObtengaElTotalDelItemDeLaVenta(VentaDetalles detalle)
        {
            decimal subtotal;
            subtotal = detalle.Precio * detalle.Cantidad;

            return subtotal;
        }

        public void ApliqueElDescuento(Model.Ventas ventas)
        {

            int subtotal = (int)ventas.SubTotal;
            Model.Ventas venta;
            venta = ObtengaLaVentaPorElId(ventas.Id);
           
            venta.PorcentajeDesCuento = ventas.PorcentajeDesCuento;
            venta.SubTotal = subtotal;

            venta.MontoDescuento = venta.SubTotal * (venta.PorcentajeDesCuento / 100);
            venta.Total = venta.SubTotal - venta.MontoDescuento;

            foreach(var item in Connection.VentaDetalles)
            {
                if(item.Id_Venta == venta.Id)
                {
                    item.MontoDescuento = item.Monto * (venta.PorcentajeDesCuento / 100);
                    item.Monto -= item.MontoDescuento;
                    Connection.VentaDetalles.Update(item);
                }
            }
          
            Connection.Ventas.Update(venta);
            Connection.SaveChanges();
        }

        public void ElimineElItemDelDetalle(int id, int IdInventario, int idVenta)
        {
            Model.Inventarios inventarios;
            Model.VentaDetalles ventaDetalles;
            Model.Ventas venta;
            venta = ObtengaLaVentaPorElId(idVenta);
            ventaDetalles = ObtengaElItemDelDetalle(id);
            inventarios = ObtengaElItemDelInventario(IdInventario);

            foreach(var item in Connection.VentaDetalles)
            {
                if(item.Id == id)
                {
                    inventarios.Cantidad = inventarios.Cantidad + ventaDetalles.Cantidad;
                    venta.SubTotal -= ventaDetalles.Monto;
                    venta.MontoDescuento -= ventaDetalles.MontoDescuento;
                    venta.Total -= (ventaDetalles.Monto - ventaDetalles.MontoDescuento);

                    Connection.Ventas.Update(venta);
                    Connection.Inventarios.Update(inventarios);
                    Connection.VentaDetalles.Remove(item);
                }
            }
            Connection.SaveChanges();
        }


        public VentaDetalles ObtengaElItemDelDetalle(int id)
        {
            foreach (var item in Connection.VentaDetalles)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }
            return null;
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
        public void CerrarCaja(int IdCaja)
        {
            Model.AperturasDeCaja caja;
            caja = ObtengaLaCaja(IdCaja);
            caja.FechaDeCierre = ObtenerFechaActual();
            caja.Estado = EstadoDeLaCaja.CERRADA;

            Connection.AperturasDeCaja.Update(caja);
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
                string userName = user.Identity?.Name;
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
                string userName = user?.Identity?.Name;
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


        public List<Model.VentaDetalles> ObtengaLaListaDelDetalleLaVenta(int idVenta)
        {

            var lista = from detalle in Connection.VentaDetalles
                        join venta in Connection.Ventas on detalle.Id_Venta equals venta.Id
                        where detalle.Id_Venta == idVenta
                        select detalle;

            return (List < Model.VentaDetalles >) lista.ToList();
        }


        public List<Model.Inventarios> ObtengaLaListaDeInventarios()
        {
            var lista = from inventario in Connection.Inventarios
                        select inventario;

            if (lista.Count() > 0)
            {

                return (List<Model.Inventarios>)lista.ToList();
            }
            else
            {
                return null;
            }
        }



        public List<Model.Inventarios> ObtengaLaListaDeInventariosPorElNombre(string nombre)
        {
            List<Model.Inventarios> ListaFiltrada;

            ListaFiltrada = Connection.Inventarios.Where(x => x.Nombre.Contains(nombre)).ToList();

            return ListaFiltrada;
        }

        public void AgregueElNuevoAjusteDeInventario(AjusteDeInventarioParaAgregar NuevoAjuste)
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

        public int RetorneLaCantidadFinal(Model.AjusteDeInventarioParaAgregar NuevoAjuste, Model.Inventarios itemDelInventario)
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
            foreach (var item in Connection.Ventas)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }
            return null;
        }

        public void ProceseLaVenta(Ventas ventas)
        {
            Model.Ventas venta;
            venta = ObtengaLaVentaPorElId(ventas.Id);

            venta.Estado = EstadoDeLaVenta.Terminada;
            venta.TipoDePago = ventas.TipoDePago;

            Connection.Ventas.Update(venta);
            Connection.SaveChanges();
            
        }

        public List<AperturasDeCaja> ObtengaLaListaDeCajas()
        {
            return Connection.AperturasDeCaja.ToList();
        }

        public void CerrarCaja(AperturasDeCaja aperturasDeCaja)
        {
            AperturasDeCaja aperturaParaEditar = ObtenerIdCaja(aperturasDeCaja.Id);

            aperturaParaEditar.FechaDeCierre = DateTime.Now;
            aperturaParaEditar.Estado = EstadoDeLaCaja.CERRADA;

            Connection.AperturasDeCaja.Update(aperturasDeCaja);
            Connection.SaveChanges();
        }

        public AperturasDeCaja ObtenerIdCaja(int id)
        {
            AperturasDeCaja elResultado;

            elResultado = Connection.AperturasDeCaja.Find(id);

            return elResultado;
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
