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

    public class DiagramViewModel : INotifyPropertyChanged
    {
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
        private DiagramDataType currentDiagramDataType;
        public DiagramDataType CurrentDiagramDataType
        {
            get { return currentDiagramDataType; }
            set
            {
                currentDiagramDataType = value;
                var currentOrderData = OrderCollection;
                switch (value)
                {
                    case DiagramDataType.OrderPerBrand:                       
                        ChartData = currentOrderData.GroupBy(row => row.Brand).ToDictionary(item => item.Key, item => item.Count());                        
                        break;
                    case DiagramDataType.OrderPerMonth:
                        currentOrderData = new ObservableCollection<UIOrderRowModel>(currentOrderData.Where(order => order.StartDateOfWork.Year == DateTime.Now.Year));
                        var query1 = currentOrderData.GroupBy(row => row.StartDateOfWork.Month).OrderBy(group => group.Key);
                        ChartData = query1.ToDictionary(item => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(item.Key), item => item.Count());  
                        break;
                    case DiagramDataType.OrderPerPriceGroup:
                        var query2 = currentOrderData.GroupBy(row => PriceGroup(row.Price).ToString()).OrderBy(group => group.Key);
                        ChartData = query2.ToDictionary(item => item.Key, item => item.Count());
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
        private ObservableCollection<UIOrderRowModel> orderCollection = null;
        public ObservableCollection<UIOrderRowModel> OrderCollection
        {
            get
            {
                return orderCollection;
            }
            set
            {
                orderCollection = value;
                foreach (Enum enumItem in Enum.GetValues(typeof(DiagramType)))
                {
                    AviableDiagramTypes.Add(GetEnumDescription(enumItem));
                }
                foreach (Enum enumItem in Enum.GetValues(typeof(DiagramDataType)))
                {
                    DiagramDataVariants.Add(GetEnumDescription(enumItem));
                }
                CurrentDiagramDataType = DiagramDataType.OrderPerBrand;
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
        private string PriceGroup(int price)
        {
            if (price <= 1000) return "1. до 1 000р.";
            if (price > 1000 && price <= 5000) return "2. от 1 000 до 5 000р.";
            if (price > 5000 && price <= 10000) return "2. от 5 000 до 10 000р.";
            return "3. более 10 000р.";
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
