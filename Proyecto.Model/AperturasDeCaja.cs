using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace Proyecto.Model
{
    public class AperturasDeCaja
    {
        [Key]
        [HiddenInput]
        public int Id { get; set; }
        public int UserId { get; set; }
        [Display(Name = "Fecha de Inicio")]
        [Required(ErrorMessage = "El campo Fecha de Inicio es requerido")]
        public DateTime FechaDeInicio { get; set; }
        [Display(Name = "Fecha de Cierre")]
        public DateTime? FechaDeCierre { get; set; }
        public string? Observaciones { get; set; }
        public EstadoDeLaCaja Estado { get; set; }

       
    }
}