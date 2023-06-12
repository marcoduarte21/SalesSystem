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
        public int Id { get; set; }
        public int Id_Inventario { get; set; }
        [ForeignKey("Id_Inventario")]
        public virtual Inventarios Inventarios { get; set; }
        [Display(Name = "Cantidad Actual")]
        [Required(ErrorMessage = "El campo Cantidad Actual es requerido")]
        public int CantidadActual { get; set; }
        public int Ajuste { get; set; }
        public TipoDeAjuste Tipo { get; set; }
        [DataType(DataType.MultilineText)]
        public string Observaciones { get; set; }
        public string UserId { get; set; }
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }
    }
}
