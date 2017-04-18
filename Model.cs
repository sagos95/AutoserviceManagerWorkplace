using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoserviceManagerWorkplace
{
    public class Order
    {
        public int OrderId { get; set; }       
        public string OrderContent { get; set; }
        public DateTime StartDateOfWork { get; set; }
        public DateTime EndDateOfWork { get; set; }
        public int Price { get; set; }

        public CarOwner CarOwner { get; set; }
        public Car Car { get; set; }
    }

    public class CarOwner
    {
        public int CarOwnerId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public int BirthYear { get; set; }
        public string PhoneNumber { get; set; }

        public virtual List<Car> Cars { get; set; }
        public virtual List<Order> Orders { get; set; }
    }

    public class Car
    {
        public int CarId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public bool AutomaticTransmission { get; set; }
        public int EnginePower { get; set; }

        public CarOwner CarOwner { get; set; }
        public virtual List<Order> Orders { get; set; }
    }
}
