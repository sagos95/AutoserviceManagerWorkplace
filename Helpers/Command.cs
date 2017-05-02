using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace AutoserviceManagerWorkplace.UI
{
    public class Command : ICommand
    {
        private Action<object> executeMethod;
        private Func<object,bool> canExecuteMethod;
        public Command(Action<object> executeMethod, Func<object, bool> canExecuteMethod)
        {
            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }

        public bool CanExecute(object parameter)
        {           
            return canExecuteMethod(parameter);
        }

        public event EventHandler CanExecuteChanged;
        public void OnCanExecuteChanged(object o, EventArgs e)
        {
            if (CanExecuteChanged != null)
            {
                EventHandler a = CanExecuteChanged;
                a(null, new EventArgs());
            }
        }

        public void Execute(object parameter)
        {
            executeMethod(parameter);
        }
    }
}
