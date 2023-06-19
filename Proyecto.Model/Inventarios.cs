using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Model
{
   public class Inventarios
    {
        [HiddenInput]
        [Key]
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo Categoria es requerido")]
        public Categoria Categoria { get; set;}
        [Required(ErrorMessage = "El campo Cantidad es requerido")]
        public int Cantidad { get; set; }
        [Required(ErrorMessage = "El campo Precio es requerido")]
        public decimal Precio { get; set; }
        public ICollection<VentaDetalles> VentaDetalles { get; set; }
        public ICollection<AjusteDeInventarios> AjusteDeInventarios { get; set; }
    }
}
