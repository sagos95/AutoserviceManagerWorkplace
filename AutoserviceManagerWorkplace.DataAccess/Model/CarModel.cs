using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoserviceManagerWorkplace.DataAccess
{
    public class CarModel
    {
        [Key]
        public int CarId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public bool AutomaticTransmission { get; set; }
        public int EnginePower { get; set; }

        public CarOwnerModel CarOwner { get; set; }
        public virtual List<OrderModel> Orders { get; set; }
    }
}
