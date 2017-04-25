using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AutoserviceManagerWorkplace.DataAccess
{
    public class CompareBy<T> : IComparer<T>
    {
        private PropertyInfo propertyInfo;
        private bool raiseSort;
        public CompareBy(string sortingPropertyName, bool raiseSort)
        {
            propertyInfo = typeof(T).GetProperty(sortingPropertyName);
            var methodInfo = typeof(T).GetMethod(sortingPropertyName);

            this.raiseSort = raiseSort;
        }
        public int Compare(T row1, T row2)
        {
            int result;
            var parameter1 = (IComparable)propertyInfo.GetValue(row1, null);
            var parameter2 = (IComparable)propertyInfo.GetValue(row2, null);
            if (parameter1 == null)
            {
                result = 1;
            }
            else
            {
                if (parameter2 == null)
                {
                    result = -1;
                }
                else
                {
                    result = parameter1.CompareTo(parameter2);
                }
            }            
            return raiseSort ? result : -result;
        }
    }
}
