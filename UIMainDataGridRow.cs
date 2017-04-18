using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoserviceManagerWorkplace
{
    class UIMainDataGridRow
    {
        public int OrderId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public bool AutomaticTransmission { get; set; }
        public int EnginePower { get; set; }
        public string OrderContent { get; set; }
        public DateTime StartDateOfWork { get; set; }
        public DateTime EndDateOfWork { get; set; }
        public int Price { get; set; }
    }
}
