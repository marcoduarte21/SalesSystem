using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Model
{
    public class VentaParaTerminar
    {
        [Required]
        [Key]
        [HiddenInput]
        public int Id { get; set; }

        [Display(Name = "Tipo de Pago")]
        [Required(ErrorMessage = "El campo Tipo de Pago es requerido")]
        public TipoDePago TipoDePago { get; set; }
    }
}
