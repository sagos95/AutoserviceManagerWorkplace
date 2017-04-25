using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using AutoserviceManagerWorkplace.DataAccess;

namespace AutoserviceManagerWorkplace.UI
{
    public enum OrderProperty { OrderId, Brand, Model, Year, AutomaticTransmission, EnginePower, OrderContent, StartDateOfWork, EndDateOfWork, Price }
    public enum OperationType { Sort, Filter, Search }
    public enum SortDirection { Raise, Decrease }

    class MainViewModel : INotifyPropertyChanged
    {
        public ICommand ShowDiagramView { get; set; }

        private bool stopRefreshingFilterListBox = false;

        private OperationType currentOperationType = OperationType.Sort;
        public OperationType CurrentOperationType
        {
            get { return currentOperationType; }
            set
            {
                currentOperationType = value;
                switch (currentOperationType)
                {
                    case OperationType.Sort:
                        OrderCollection = UIOrderRowBuilder.GetOrdersData();
                        OrderCollection = SortOrders();
                        break;
                    case OperationType.Filter:
                        GetFilterValues();
                        break;
                    case OperationType.Search:
                        FindOrders();
                        break;
                }
                OnPropertyChanged("CurrentOperationType");
            }
        }
        private OrderProperty currentOrderProperty = OrderProperty.Model;
        public OrderProperty CurrentOrderProperty
        {
            get { return currentOrderProperty; }
            set
            {
                currentOrderProperty = value;
                switch (currentOperationType)
                {
                    case OperationType.Sort:                        
                        OrderCollection = SortOrders();  
                        break;
                    case OperationType.Filter:
                        GetFilterValues();
                        break;
                    case OperationType.Search:
                        FindOrders();
                        break;
                }                
                OnPropertyChanged("CurrentOrderProperty");
            }
        }
        private SortDirection sortDirection;
        public SortDirection SortDirection
        {
            get { return sortDirection; }
            set
            {
                sortDirection = value;
                OrderCollection = SortOrders();
            }
        }
        private ObservableCollection<string> filterValues = new ObservableCollection<string>();
        public ObservableCollection<string> FilterValues
        {
            get { return filterValues; }
            set
            {
                filterValues = value;
                OnPropertyChanged("FilterValues");
            }
        }
        private string selectedFilterValue;
        public string SelectedFilterValue
        {
            get { return selectedFilterValue; }
            set
            {
                if (value != null && CurrentOperationType == OperationType.Filter && !stopRefreshingFilterListBox)
                {
                    selectedFilterValue = value;
                    FilterOrders();
                    OnPropertyChanged("SelectedFilterValue");
                }
            }
        }
        private string searchValue = "";
        public string SearchValue
        {
            get { return searchValue; }
            set
            {                
                if (CurrentOperationType == OperationType.Search)
                {
                    searchValue = value;
                    FindOrders();
                    OnPropertyChanged("SearchValue");
                }
            }
        }

        ObservableCollection<UIOrderRow> orderCollection = UIOrderRowBuilder.GetOrdersData();
        public ObservableCollection<UIOrderRow> OrderCollection
        {
            get { return orderCollection; }
            set
            {
                orderCollection = value;
                OnPropertyChanged("OrderCollection");
            }
        }

        public MainViewModel()
        {
            ShowDiagramView = new Command(showDiagramView, alwaysCanExecute);           

        }
        private bool alwaysCanExecute(object parameter)
        {
            return true;
        }

        private ObservableCollection<UIOrderRow> SortOrders()
        {
            var currentColumnName = CurrentOrderProperty.ToString();
            var customComparer = new CompareBy<UIOrderRow>(currentColumnName, sortDirection == SortDirection.Raise);

            var list = new List<UIOrderRow>(orderCollection);
            list.Sort(customComparer);
            orderCollection = new ObservableCollection<UIOrderRow>(list);
            
            return orderCollection;
        }
        private void GetFilterValues()
        {
            string correctedPropertyName = UIOrderRow.ConvertToUIPropertyName(CurrentOrderProperty);                
            stopRefreshingFilterListBox = true;
            FilterValues.Clear();
            if (CurrentOrderProperty == OrderProperty.AutomaticTransmission)
            {
                FilterValues.Add(Properties.Resources.AutomaticTransmissionType);
                stopRefreshingFilterListBox = false;
                FilterValues.Add(Properties.Resources.ManualTransmissionType);
                SelectedFilterValue = FilterValues.First();
            }
            else
            {
                var columnPropertyInfo = typeof(UIOrderRow).GetProperty(correctedPropertyName);
                orderCollection = UIOrderRowBuilder.GetOrdersData();
                foreach (var order in orderCollection)
                {
                    if (columnPropertyInfo.GetValue(order, null) != null)
                    {
                        FilterValues.Add(columnPropertyInfo.GetValue(order, null).ToString());
                    }
                }
                stopRefreshingFilterListBox = false;
                FilterValues = new ObservableCollection<string>(FilterValues.GroupBy(x => x).Select(y => y.First()));
                SelectedFilterValue = FilterValues.First();
            }
            
        }
        private void FilterOrders()
        {
            OrderCollection = UIOrderRowBuilder.GetOrdersData();
            var filteringColumn = typeof(UIOrderRow).GetProperty(UIOrderRow.ConvertToUIPropertyName(CurrentOrderProperty));
            OrderCollection = new ObservableCollection<UIOrderRow>(
                    OrderCollection.Where(order => filteringColumn.GetValue(order, null).ToString() == SelectedFilterValue));
        }
        private void FindOrders()
        {
            OrderCollection = UIOrderRowBuilder.GetOrdersData();
            var findingColumn = typeof(UIOrderRow).GetProperty(CurrentOrderProperty.ToString());
            OrderCollection = new ObservableCollection<UIOrderRow>(
                   OrderCollection.Where(order => findingColumn.GetValue(order, null).ToString().StartsWith(SearchValue)));
        }

        private void showDiagramView(object obj)
        {
            var newView = new DiagramView();
            newView.Show();
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
