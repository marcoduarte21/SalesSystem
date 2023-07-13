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
    public class InventariosParaAgregar
    {
        [HiddenInput]
        [Key]
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo Categoria es requerido")]
        public Categoria Categoria { get; set; }
        [Required(ErrorMessage = "El campo Cantidad es requerido")]
        public int Cantidad { get; set; }
        [Required(ErrorMessage = "El campo Precio es requerido")]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "El Precio no debe ser menor a 0")]
        public decimal Precio { get; set; }
    }
}