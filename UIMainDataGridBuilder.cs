using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoserviceManagerWorkplace
{
    class UIMainDataGridBuilder
    {
        public static List<UIMainDataGridRow> GetOrdersData()
        {
            DataBaseContext context = new DataBaseContext();
            var currentOrders = context.Orders.Include("CarOwner").Include("Car");
            var result = new List<UIMainDataGridRow>();
            foreach(var order in currentOrders)
            {
                var currentCar = context.Cars.Where(car => car.CarId == order.Car.CarId).Single();
                var columns = new UIMainDataGridRow()
                {
                    OrderId = order.OrderId,
                    AutomaticTransmission = currentCar.AutomaticTransmission,
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
            return result;
        }
        public static void RefreshDataGrid(System.Windows.Controls.DataGrid dataGrid, List<UIMainDataGridRow> orderData)
        {
            DataBaseContext context = new DataBaseContext();
            dataGrid.ItemsSource = orderData;
        }
    }
}
