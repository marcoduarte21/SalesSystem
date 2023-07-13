using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Proyecto.Model
{
    public class VentaParaIniciar
    {
        [Required]
        [Key]
        [HiddenInput]
        public int Id { get; set; }
        [Display(Name = "Nombre del Cliente")]
        [Required(ErrorMessage = "El campo Nombre del Cliente es requerido")]
        public string NombreCliente { get; set; }
        
        public string UserId { get; set; }
        
    }
}
