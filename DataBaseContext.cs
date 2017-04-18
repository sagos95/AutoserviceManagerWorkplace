using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoserviceManagerWorkplace
{
    class DataBaseContext : DbContext
    {
        public DataBaseContext()
            : base("AMWDataBase")
        { 

        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<CarOwner> CarOwners { get; set; }
        public DbSet<Car> Cars { get; set; }
    }
}
