﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Model
{
    public class Ventas
    {
        [Required]
        [Key]
        [HiddenInput]
        public int Id { get; set; }
        [Display(Name = "Nombre del Cliente")]
        [Required(ErrorMessage = "El campo Nombre del Cliente es requerido")]
        public string NombreCliente { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "El campo Fecha es requerido")]
        public DateTime Fecha { get; set; }
        [Display(Name = "Tipo de Pago")]
        [Required(ErrorMessage = "El campo Tipo de Pago es requerido")]
        public TipoDePago TipoDePago { get; set; }
        [Required(ErrorMessage = "El campo Total es requerido")]
        public float Total { get; set; }
        [Required(ErrorMessage = "El campo SubTotal es requerido")]
        public float SubTotal { get; set; }
        [Display(Name = "Porcentaje de Descuento")]
        [Required(ErrorMessage = "El campo Porcentaje de Descuento es requerido")]
        public int PorcentajeDesCuento { get; set; }
        [Display(Name = "Monto de Descuento")]
        [Required(ErrorMessage = "El campo Monto de Descuento es requerido")]
        public float MontoDescuento { get; set; }
        public int UserId { get; set; }
        public EstadoDeLaVenta Estado { get; set; }
        [Required]
        public int IdAperturaDeCaja { get; set; }

    }
}
