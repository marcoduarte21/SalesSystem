using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Proyecto.DA
{
    public class DBContexto : DbContext
    {

        public DBContexto(DbContextOptions<DBContexto> dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<Model.Ventas> Ventas { get; set; }
        public DbSet<Model.AjusteDeInventarios> AjusteDeInventarios { get; set; }
        public DbSet<Model.AperturasDeCaja> AperturasDeCaja { get; set; }
        public DbSet<Model.VentaDetalles> VentaDetalles { get; set; }
        public DbSet<Model.Inventarios> Inventarios { get; set; }

    }
}