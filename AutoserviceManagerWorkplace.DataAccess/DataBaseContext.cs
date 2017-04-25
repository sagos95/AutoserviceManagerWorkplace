using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoserviceManagerWorkplace.DataAccess
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext()
            : base("AMWDataBase")
        { 

        }

        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<CarOwnerModel> CarOwners { get; set; }
        public DbSet<CarModel> Cars { get; set; }
    }
}
