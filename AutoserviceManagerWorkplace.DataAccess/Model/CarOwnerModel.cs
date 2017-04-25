using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoserviceManagerWorkplace.DataAccess
{
    public class CarOwnerModel
    {
        [Key]
        public int CarOwnerId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public int BirthYear { get; set; }
        public string PhoneNumber { get; set; }

        public virtual List<CarModel> Cars { get; set; }
    }
}
