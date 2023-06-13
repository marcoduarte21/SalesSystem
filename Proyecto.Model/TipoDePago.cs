using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Proyecto.Model
{
    public enum TipoDePago
    {
        Efectivo = 1,
        Tarjeta = 2,
        [Display(Name = "SINPEMóvil")]
        SINPEMovil = 3,
    }
}