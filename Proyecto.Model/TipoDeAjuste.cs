using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Proyecto.Model
{
    public enum TipoDeAjuste
    {
        Aumento = 1,
        [Display(Name = "Disminución")]
        [Required(ErrorMessage = "El campo Disminución es requerido")]
        Disminucion = 2,
    }
}