using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Proyecto.Model
{
    public enum Categoria
    {
        [Display(Name = "Clase A")]
        ClaseA = 1,
        [Display(Name = "Clase B")]
        ClaseB = 2,
        [Display(Name = "Clase C")]
        ClaseC = 3,
    }
}