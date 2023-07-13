using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Proyecto.Model
{
    public class DetallesVentaParaAgregar
    {

        [Key]
        [HiddenInput]
        [Required]
        public int Id { get; set; }
        [Required]
        public int Id_Venta { get; set; }

        public int Id_Inventario { get; set; }

        public int Cantidad { get; set; }
       

        
    }
}
