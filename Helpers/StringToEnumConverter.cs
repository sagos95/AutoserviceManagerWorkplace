using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AutoserviceManagerWorkplace.UI
{
    class StringToEnumConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is Enum)
            {                
                return value.ToString();
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string)
            {
                var enumType = Type.GetType(parameter.ToString());
                return DiagramViewModel.GetEnumValue((string)value, enumType);
            }
            return Binding.DoNothing;
        }
    }
}
