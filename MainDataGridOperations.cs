using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;

namespace AutoserviceManagerWorkplace
{     
    /// <summary>
    /// Будущий класс Model. Позже отсюда перекочует бОльшая часть кода в класс ViewModel
    /// </summary>
    public static class MainDataGridOperations
    {
        private static CurrentMode currentMode;
        private static string currentColumnName = "Year";
        private static bool raiseSort = true;
        private enum CurrentMode { Sort, Filter, Find }; 
        private enum ComboBoxState { Freezed, Unfreezed }

        public static void ChangeOperationParameter(object parameter)
        {
            object[] parameters = (object[])parameter;
            var paramNum = 0;
            var dataGrid = (DataGrid)parameters[paramNum++];
            var columnNameWasChanged = !((string)parameters[paramNum]).Equals("") && !currentColumnName.Equals((string)parameters[paramNum]);
            if (columnNameWasChanged)
            {
                currentColumnName = (string)parameters[paramNum];
            }
            paramNum++;
            switch(currentMode)
            {
                case CurrentMode.Sort:
                    SortMainDataGrid(dataGrid);
                    break;
                case CurrentMode.Filter:
                    var comboBox = (ComboBox)parameters[paramNum++];
                    if (columnNameWasChanged)
                    {
                        ReloadComboBoxValues(comboBox);
                    }
                    FilterMainDataGrid(dataGrid, comboBox);
                    break;
                case CurrentMode.Find:
                    FindInMainDataGrid(dataGrid);
                    break;
            }
        }

        private static void SortMainDataGrid(System.Windows.Controls.DataGrid dataGrid)
        {
            var customComparer = new CompareBy(currentColumnName, raiseSort);

            var data = UIMainDataGridBuilder.GetOrdersData();
            data.Sort(customComparer);

            UIMainDataGridBuilder.RefreshDataGrid(dataGrid, data);
        }

        private static void FilterMainDataGrid(DataGrid dataGrid, ComboBox comboBox)
        {
            if ((ComboBoxState)comboBox.Tag != ComboBoxState.Freezed)
            {
                var data = UIMainDataGridBuilder.GetOrdersData();
                var filteringColumn = typeof(UIMainDataGridRow).GetProperty(currentColumnName);
                data = data.Where(order => filteringColumn.GetValue(order, null).ToString() == comboBox.SelectedItem.ToString()).ToList();            
                UIMainDataGridBuilder.RefreshDataGrid(dataGrid, data);
            }
        }

        private static void FindInMainDataGrid(System.Windows.Controls.DataGrid dataGrid)
        {

        }

        public static void ChangeMode(object parameter)
        {
            var parameters = (object[])parameter;
            var paramNum = 0;
            var showingPanel = (Grid)parameters[paramNum++];
            var dataGrid = (DataGrid)parameters[paramNum++];
            showingPanel.Width = Double.NaN;
            currentMode = (CurrentMode)Enum.Parse(typeof(CurrentMode), (string)parameters[paramNum++]);
            
            switch (currentMode)
            {
                case CurrentMode.Sort:
                    for (var i = paramNum++; i < parameters.Length; ++i)
                {
                    ((Grid)parameters[i]).Width = 0;
                }
                SortMainDataGrid(dataGrid);
                    break;
                case CurrentMode.Filter:
                    var comboBox = (ComboBox)parameters[paramNum++];
                    ReloadComboBoxValues(comboBox);
                    for (var i = paramNum++; i < parameters.Length; ++i)
                    {
                        ((Grid)parameters[i]).Width = 0;
                    }
                    FilterMainDataGrid(dataGrid, comboBox);
                    break;
                case CurrentMode.Find:
                    FindInMainDataGrid(dataGrid);
                    break;
            }
        }

        private static void ReloadComboBoxValues(ComboBox comboBox)
        {
            DataBaseContext context = new DataBaseContext();
            var orderData = UIMainDataGridBuilder.GetOrdersData();
            var columnPropertyInfo = typeof(UIMainDataGridRow).GetProperty(currentColumnName);
            comboBox.Tag = ComboBoxState.Freezed;
            comboBox.Items.Clear();            
            foreach (var order in orderData)
            {
                comboBox.Items.Add(columnPropertyInfo.GetValue(order, null).ToString());                
            }
            comboBox.Tag = ComboBoxState.Unfreezed;
            comboBox.SelectedItem = comboBox.Items[0];
        }

        public static void ChangeSortFlow(object parameter)
        {
            var parameters = (object[])parameter;
            raiseSort = Convert.ToBoolean(parameters[1]);
            SortMainDataGrid((System.Windows.Controls.DataGrid)parameters[0]);
        }
    }

    class CompareBy : IComparer<UIMainDataGridRow>
    {
        private PropertyInfo propertyInfo;
        private bool raiseSort;
        public CompareBy(string sortingPropertyName, bool raiseSort)
        {
            propertyInfo = typeof(UIMainDataGridRow).GetProperty(sortingPropertyName);
            this.raiseSort = raiseSort;
        }
        public int Compare(UIMainDataGridRow row1, UIMainDataGridRow row2)
        {
            var parameter1 = (IComparable)propertyInfo.GetValue(row1, null);
            var parameter2 = (IComparable)propertyInfo.GetValue(row2, null);
            var result = parameter1.CompareTo(parameter2);
            return raiseSort ? result : -result;
        }
    }
}
