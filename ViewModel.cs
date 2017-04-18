using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AutoserviceManagerWorkplace
{
    class ViewModel
    {
        public ICommand ChangeOperationParameter { get; set; }
        public ICommand ChangeMode { get; set; }
        public ICommand ChangeSortFlow { get; set; }

        public ViewModel()
        {
            ChangeOperationParameter = new Command(MainDataGridOperations.ChangeOperationParameter, alwaysCanExecute);
            ChangeMode = new Command(MainDataGridOperations.ChangeMode, alwaysCanExecute);
            ChangeSortFlow = new Command(MainDataGridOperations.ChangeSortFlow, alwaysCanExecute);
        }
        private bool alwaysCanExecute(object parameter)
        {
            return true;
        }
    }
}
