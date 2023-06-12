﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Model
{
    public class AjusteDeInventarios
    {
        public int Id { get; set; }
        public int Id_Inventario { get; set; }
        public int CantidadActual { get; set; }
        public int Ajuste { get; set; }
        public TipoDeAjuste Tipo { get; set; }
        public string Observaciones { get; set; }
        public string UserId { get; set; }
        public DateTime Fecha { get; set; }
    }
}
