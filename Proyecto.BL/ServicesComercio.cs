using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.BL
{
    public class ServicesComercio
    {
        DA.DBContexto Connection;
        public ServicesComercio(DA.DBContexto connection)
        {
            Connection = connection;
        }

    }
}
