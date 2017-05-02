using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoserviceManagerWorkplace.UI
{
    public class UIOrderRowModel
    {
        public int OrderId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string AutomaticTransmission { get; set; }
        public int EnginePower { get; set; }
        public string OrderContent { get; set; }
        public DateTime StartDateOfWork { get; set; }
        public string StartDateOfWorkWithDots
        {
            get
            {
                return StartDateOfWork.ToString("dd MMMM yyyy");
            }
            private set { }
        }
        public DateTime? EndDateOfWork { get; set; }
        public string EndDateOfWorkWithDots
        {
            get
            {
                return EndDateOfWork.HasValue ? EndDateOfWork.Value.ToString("dd MMMM yyyy") : "Заказ выполняется";
            }
            private set { }
        }
        public int Price { get; set; }
        public string CarOwnerInfo { get; set; }
        public static string ConvertToUIPropertyName(OrderProperty orderProperty)
        {
            var result = orderProperty.ToString();
            if (orderProperty == OrderProperty.EndDateOfWork || orderProperty == OrderProperty.StartDateOfWork)
            {
                return result + "WithDots";
            }
            return result;
        }
    }
}
