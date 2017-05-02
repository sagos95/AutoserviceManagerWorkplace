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
using System.Text.RegularExpressions;

namespace AutoserviceManagerWorkplace.UI
{
    public enum OrderProperty { OrderId, Brand, Model, Year, AutomaticTransmission, EnginePower, OrderContent, StartDateOfWork, EndDateOfWork, Price }
    public enum OperationType { Sort, Filter, Search }
    public enum SortDirection { Raise, Decrease }

    class MainViewModel : INotifyPropertyChanged
    {
        public Command ShowDiagramView { get; set; }
        public Command NextPage { get; set; }
        public Command PreviousPage { get; set; }
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
                        OrderCollection = GetOrdersCollection();
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
        private OrderProperty currentOrderProperty;
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
        private int currentPageNum = 1;
        public object CurrentPageNum
        {
            get { return currentPageNum; }
            set
            {
                if (value.GetType().Equals(typeof(int)))
                {
                    currentPageNum = (int)value;
                }
                else
                {
                    var sourceString = value.ToString();
                    Regex regex = new Regex("[^0-9]+");
                    if (!regex.IsMatch(sourceString))
                    {
                        currentPageNum = sourceString.Equals("") ? 0 : System.Convert.ToInt32(sourceString);
                    }
                }
                NextPage.OnCanExecuteChanged(null, null);
                PreviousPage.OnCanExecuteChanged(null, null);
                OnPropertyChanged("CurrentPageNum");
                OnPropertyChanged("VisibleOrderCollection");
            }
        }
        private int ordersPageCount = orderCollection.Count;
        public int OrdersPagesCount
        {
            get { return ordersPageCount; }
            set
            {
                ordersPageCount = Convert.ToInt32(value);
                OnPropertyChanged("OrdersPagesCount");
            }
        }
        private int rowsPerPage = 10;
        public object RowsPerPage
        {
            get { return rowsPerPage; }
            set
            {
                if (value.GetType().Equals(typeof(int)))
                {
                    rowsPerPage = (int)value;
                }
                else
                {
                    var sourceString = value.ToString();
                    Regex regex = new Regex("[^0-9]+");                  
                    if (!regex.IsMatch(sourceString))
                    {
                        rowsPerPage = sourceString.Equals("") ? 0 : System.Convert.ToInt32(sourceString);
                    }
                }
                if (rowsPerPage <= 0)
                {
                    rowsPerPage = 1;
                }   
                CurrentPageNum = 1;
                NextPage.OnCanExecuteChanged(null, null);
                PreviousPage.OnCanExecuteChanged(null, null);
                OnPropertyChanged("RowsPerPage");
                OnPropertyChanged("VisibleOrderCollection");
            }
        }

        private static ObservableCollection<UIOrderRowModel> orderCollection = GetOrdersCollection();
        public ObservableCollection<UIOrderRowModel> OrderCollection
        {
            get { return orderCollection; }
            set
            {
                orderCollection = value;
                OnPropertyChanged("VisibleOrderCollection");
            }
        }
        public ObservableCollection<UIOrderRowModel> VisibleOrderCollection
        {
            get
            {
                var pagesStrictCount = orderCollection.Count / (double)rowsPerPage;
                OrdersPagesCount = (int)pagesStrictCount;
                if (pagesStrictCount - OrdersPagesCount > 0)
                {
                    OrdersPagesCount++;
                }
                var lowIndex = (currentPageNum - 1) * rowsPerPage;
                var highIndex = (currentPageNum) * rowsPerPage - 1;
                NextPage.OnCanExecuteChanged(null, null);
                PreviousPage.OnCanExecuteChanged(null, null);
                return new ObservableCollection<UIOrderRowModel>(
                    orderCollection.Where(row => orderCollection.IndexOf(row) >= lowIndex && orderCollection.IndexOf(row) <= highIndex));
            }
            set
            {
                OnPropertyChanged("VisibleOrderCollection");
            }
        }

        public MainViewModel()
        {
            ShowDiagramView = new Command(showDiagramView, x => true);
            NextPage = new Command(nextPage, canGoToNextPage);
            PreviousPage = new Command(previousPage, canGoToPreviousPage);
        }

        public static ObservableCollection<UIOrderRowModel> GetOrdersCollection()
        {
            var result = new ObservableCollection<UIOrderRowModel>();
            using (DataBaseContext context = new DataBaseContext())
            {
                var currentOrders = context.Orders.Include("Car").ToList();
                foreach (var order in currentOrders)
                {
                    var currentCar = context.Cars.Include("CarOwner").FirstOrDefault(car => car.CarId == order.Car.CarId);
                    var currenCarOwner = context.CarOwners.FirstOrDefault(owner => owner.CarOwnerId == currentCar.CarOwner.CarOwnerId);
                    var currenCarOwnerInfo =
                        String.Format("Информация о владельце\nФамилия: {0}\nИмя: {1}\nОтчество: {2}\nГод рожения: {3}\nТелефон: {4}",
                        currenCarOwner.Surname, currenCarOwner.Name, currenCarOwner.Patronymic, currenCarOwner.BirthYear, currenCarOwner.PhoneNumber);
                    var columns = new UIOrderRowModel()
                    {
                        OrderId = order.OrderId,
                        AutomaticTransmission = currentCar.AutomaticTransmission ?
                                                Properties.Resources.AutomaticTransmissionType : Properties.Resources.ManualTransmissionType,
                        Brand = currentCar.Brand,
                        EnginePower = currentCar.EnginePower,
                        Model = currentCar.Model,
                        Year = currentCar.Year,
                        OrderContent = order.OrderContent,
                        StartDateOfWork = order.StartDateOfWork,
                        EndDateOfWork = order.EndDateOfWork,
                        Price = order.Price,
                        CarOwnerInfo = currenCarOwnerInfo
                    };
                    result.Add(columns);
                }
            }
            return result;
        }
        private ObservableCollection<UIOrderRowModel> SortOrders()
        {
            var currentColumnName = CurrentOrderProperty.ToString();
            var customComparer = new CompareBy<UIOrderRowModel>(currentColumnName, sortDirection == SortDirection.Raise);

            var list = new List<UIOrderRowModel>(orderCollection);
            list.Sort(customComparer);
            orderCollection = new ObservableCollection<UIOrderRowModel>(list);
            
            return orderCollection;
        }
        private void GetFilterValues()
        {
            string correctedPropertyName = UIOrderRowModel.ConvertToUIPropertyName(CurrentOrderProperty);                
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
                var columnPropertyInfo = typeof(UIOrderRowModel).GetProperty(correctedPropertyName);
                orderCollection = GetOrdersCollection();
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
            OrderCollection = GetOrdersCollection();
            var filteringColumn = typeof(UIOrderRowModel).GetProperty(UIOrderRowModel.ConvertToUIPropertyName(CurrentOrderProperty));
            OrderCollection = new ObservableCollection<UIOrderRowModel>(
                    OrderCollection.Where(order => filteringColumn.GetValue(order, null).ToString() == SelectedFilterValue));
        }
        private void FindOrders()
        {
            OrderCollection = GetOrdersCollection();
            var findingColumn = typeof(UIOrderRowModel).GetProperty(UIOrderRowModel.ConvertToUIPropertyName(CurrentOrderProperty));
            OrderCollection = new ObservableCollection<UIOrderRowModel>(
                   OrderCollection.Where(order => findingColumn.GetValue(order, null).ToString().StartsWith(SearchValue)));
        }

        private void showDiagramView(object obj)
        {
            var newView = new DiagramView();
            newView.Show();
        }
        private void nextPage(object obj)
        {            
            CurrentPageNum = (int)CurrentPageNum + 1;
        }
        private void previousPage(object obj)
        {
            CurrentPageNum = (int)CurrentPageNum - 1;
        }
        private bool canGoToNextPage(object obj)
        {
            return currentPageNum < OrdersPagesCount;
        }
        private bool canGoToPreviousPage(object obj)
        {
            return currentPageNum > 1;
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
