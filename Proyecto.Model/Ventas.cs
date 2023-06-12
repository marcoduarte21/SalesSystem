using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Model
{
    public class Ventas
    {
        public int Id { get; set; }
        public string NombreCliente { get; set; }
        public DateTime Fecha { get; set; }
        public TipoDePago TipoDePago { get; set; }
        public float Total { get; set; }
        public float SubTotal { get; set; }
        public int PorcentajeDesCuento { get; set; }
        public float MontoDescuento { get; set; }
        public int UserId { get; set; }
        public EstadoDeLaVenta Estado { get; set; }
        public int IdAperturaDeCaja { get; set; }

    }
}
