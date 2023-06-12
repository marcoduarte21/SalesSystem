using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Model
{
    public class VentaDetalles
    {
        public int Id { get; set; }
        public int Id_Venta { get; set; }
        public int Id_Inventario { get; set; }
        public int Cantidad { get; set; }
        public float Precio { get; set; }
        public float Monto { get; set; }
        public float MontoDescuento { get; set; }
    }
}
