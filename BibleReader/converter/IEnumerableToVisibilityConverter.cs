using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace wits.converter {
    public class IEnumerableToVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            bool isNothing = !(value is IEnumerable) || ((IEnumerable)value).Cast<object>().Count() == 0;
            if (parameter != null && parameter.ToString().ToLower() == "reverse")
                isNothing = !isNothing;

            return isNothing ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
