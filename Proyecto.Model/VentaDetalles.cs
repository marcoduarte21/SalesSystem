using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Model
{
    public class VentaDetalles
    {
        [Key]
        [HiddenInput]
        [Required]
        public int Id { get; set; }
        [Required]
        public int Id_Venta { get; set; }
        [ForeignKey("Id_Venta")]
        public virtual Ventas Ventas { get; set; }
        [Required]
        public int Id_Inventario { get; set; }
        [ForeignKey("Id_Inventario")]
        public virtual Inventarios Inventarios { get; set; }
        [Required(ErrorMessage = "El campo Cantidad es requerido")]
        public int Cantidad { get; set; }
        [Required(ErrorMessage = "El campo Precio es requerido")]
        public decimal Precio { get; set; }
        [Required(ErrorMessage = "El campo Monto es requerido")]
        public decimal Monto { get; set; }
        [Display(Name = "Monto de Descuento")]
        [Required(ErrorMessage = "El campo Monto de Descuento es requerido")]
        public decimal MontoDescuento { get; set; }
    }
}
