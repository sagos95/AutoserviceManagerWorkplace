using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AutoserviceManagerWorkplace.UI
{
    public enum DiagramType
    {
        [Description("Круговая")]
        Pie,
        [Description("График")]
        Lines,
        [Description("Колонки")]
        Column
    }
    public enum DiagramDataType
    {
        [Description("Количество заказов по маркам авто")]
        OrderPerBrand,
        [Description("Количество заказов в месяц")]
        OrderPerMonth,
        [Description("Количество заказов по стоимости работ")]
        OrderPerPriceGroup
    }

    class DiagramViewModel : INotifyPropertyChanged
    {
        protected static string PriceGroup(int price)
        {
            if (price <= 1000) return "1. до 1000р.";
            if (price > 1000 && price <= 5000) return "2. от 1000 до 5000р.";
            return "3. более 5000р.";
        }

        private ObservableCollection<string> diagramDataVariants = new ObservableCollection<string>();
        public ObservableCollection<string> DiagramDataVariants
        {
            get { return diagramDataVariants; }
            set
            {
                diagramDataVariants = value;
                OnPropertyChanged("DiagramDataVariants");
            }
        }
        private DiagramDataType currentDiagramDataType = DiagramDataType.OrderPerBrand;
        public DiagramDataType CurrentDiagramDataType
        {
            get { return currentDiagramDataType; }
            set
            {
                currentDiagramDataType = value;
                var orderData = UIOrderRowBuilder.GetOrdersData();
                switch (value)
                {
                    case DiagramDataType.OrderPerBrand:                       
                        ChartData = orderData.GroupBy(row => row.Brand).ToDictionary(item => item.Key, item => item.Count());                        
                        break;
                    case DiagramDataType.OrderPerMonth: 
                        ChartData = orderData.GroupBy(row => row.StartDateOfWork.ToString("MMMM")).ToDictionary(item => item.Key, item => item.Count());  
                        break;
                    case DiagramDataType.OrderPerPriceGroup:
                        var query = orderData.GroupBy(row => PriceGroup(row.Price).ToString()).OrderBy(group => group.Key);
                        ChartData = query.ToDictionary(item => item.Key, item => item.Count());
                        break;     
                }
                OnPropertyChanged("CurrentDiagramType");
            }
        }
        private ObservableCollection<string> aviableDiagramTypes = new ObservableCollection<string>();
        public ObservableCollection<string> AviableDiagramTypes
        {
            get { return aviableDiagramTypes; }
            set
            {
                aviableDiagramTypes = value;
                OnPropertyChanged("AviableDiagramTypes");
            }
        }
        private DiagramType currentDiagramType = DiagramType.Pie;
        public DiagramType CurrentDiagramType
        {
            get { return currentDiagramType; }
            set
            {
                currentDiagramType = value;
                OnPropertyChanged("CurrentDiagramType");
            }
        }

        private Dictionary<string, int> chartData = new Dictionary<string, int>();
        public Dictionary<string, int> ChartData
        {
            get { return chartData; }
            set
            {
                chartData = value;
                OnPropertyChanged("ChartData");
            }
        }

        public DiagramViewModel()
        {
            foreach (Enum enumItem in Enum.GetValues(typeof(DiagramType)))
            {
                AviableDiagramTypes.Add(GetEnumDescription(enumItem));
            }

            foreach (Enum enumItem in Enum.GetValues(typeof(DiagramDataType)))
            {
                DiagramDataVariants.Add(GetEnumDescription(enumItem));
            }

            //chartData.Add("Audi", 12);
            //chartData.Add("Skoda", 7);
            //chartData.Add("WV", 19);
            //chartData.Add("Mazda", 10);
        }

        public static string GetEnumDescription(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attributes =
                (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static Enum GetEnumValue(string description, Type enumObject)
        {
            var t = enumObject.GetType();
            var enumValues = Enum.GetValues(enumObject);
            foreach(Enum value in enumValues)
            {
                if (GetEnumDescription(value).Equals(description))
                {
                    return value;
                }
            }
            return null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
