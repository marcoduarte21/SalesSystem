using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Proyecto.Model
{
    public enum EstadoDeLaVenta
    {
        [Display(Name = "En Proceso")]
        EnProceso = 1,
        Terminada = 2,

    }
}