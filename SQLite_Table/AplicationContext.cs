using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace SQLite_Table
{
    public class AplicationContext : DbContext
    {
        public AplicationContext(): base("DefaultConnection")
        {
        }
        public DbSet<GraficCard> GraficCards { get; set; }

    }

}
