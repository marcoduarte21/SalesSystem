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
    public class AjusteDeInventarios
    {
        [HiddenInput]
        [Key]
        [Required]
        public int Id { get; set; }
        public int Id_Inventario { get; set; }
        [ForeignKey("Id_Inventario")]
        public virtual Inventarios Inventarios { get; set; }
        [Display(Name = "Cantidad Actual")]
        [Required(ErrorMessage = "El campo Cantidad Actual es requerido")]
        public int CantidadActual { get; set; }
        [Required(ErrorMessage = "El campo Ajuste es requerido")]
        public int Ajuste { get; set; }
        [Required(ErrorMessage = "El campo Tipo es requerido")]
        public TipoDeAjuste Tipo { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "El campo Observaciones es requerido")]
        public string Observaciones { get; set; }
        public string UserId { get; set; }
        [Required(ErrorMessage = "El campo Fecha es requerido")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }
    }
}
