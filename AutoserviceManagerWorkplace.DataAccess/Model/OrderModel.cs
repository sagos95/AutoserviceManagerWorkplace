using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoserviceManagerWorkplace.DataAccess
{
    public class OrderModel
    {
        [Key]
        public int OrderId { get; set; }
        public string OrderContent { get; set; }
        public DateTime StartDateOfWork { get; set; }
        public DateTime? EndDateOfWork { get; set; }
        public int Price { get; set; }
        public CarModel Car { get; set; }
    }
}
