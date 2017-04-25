using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoserviceManagerWorkplace.DataAccess;

namespace AutoserviceManagerWorkplace.UI
{
    public class UIOrderRowBuilder
    {
        public static ObservableCollection<UIOrderRow> GetOrdersData()
        {
            var result = new ObservableCollection<UIOrderRow>();
            using (DataBaseContext context = new DataBaseContext())
            {
                var currentOrders = context.Orders.Include("Car").ToList(); 
                foreach (var order in currentOrders)
                {
                    var currentCar = context.Cars.Include("CarOwner").FirstOrDefault(car => car.CarId == order.Car.CarId);
                    var currenCarOwner = context.CarOwners.FirstOrDefault(owner => owner.CarOwnerId == currentCar.CarOwner.CarOwnerId);
                    var columns = new UIOrderRow()
                    {
                        OrderId = order.OrderId,
                        AutomaticTransmission = currentCar.AutomaticTransmission?
                                                Properties.Resources.AutomaticTransmissionType : Properties.Resources.ManualTransmissionType,
                        Brand = currentCar.Brand,
                        EnginePower = currentCar.EnginePower,
                        Model = currentCar.Model,
                        Year = currentCar.Year,
                        OrderContent = order.OrderContent,
                        StartDateOfWork = order.StartDateOfWork,
                        EndDateOfWork = order.EndDateOfWork,
                        Price = order.Price
                    };
                    result.Add(columns);
                }
            }
            return result;
        }
    }
}
