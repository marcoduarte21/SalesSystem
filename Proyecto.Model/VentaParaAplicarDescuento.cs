using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Model
{
    public class VentaParaAplicarDescuento
    {

        [Required]
        [Key]
        [HiddenInput]
        public int Id { get; set; }

        [Display(Name = "Porcentaje de Descuento")]
        [Required(ErrorMessage = "El campo Porcentaje de Descuento es requerido")]
        public int PorcentajeDesCuento { get; set; }


    }
}
