using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Text.Json.Serialization;

namespace Proyecto.Model
{
    public class AjusteDeInventarioParaAgregar
    {
        [HiddenInput]
        [Key]
        [Required]
        public int Id { get; set; }
        public int Id_Inventario { get; set; }
        [Required(ErrorMessage = "El campo Ajuste es requerido")]
        public int Ajuste { get; set; }
        [Required(ErrorMessage = "El campo Tipo es requerido")]
        public TipoDeAjuste Tipo { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "El campo Observaciones es requerido")]
        public string Observaciones { get; set; }

        public string UserId { get; set; }


    }
}
