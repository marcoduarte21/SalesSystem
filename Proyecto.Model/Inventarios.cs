using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Model
{
   public class Inventarios
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public Categoria Categoria { get; set;}
        public int Cantidad { get; set; }
        public float Precio { get; set; }
    }
}
