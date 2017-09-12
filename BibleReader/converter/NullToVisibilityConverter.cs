using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace wits.converter {
    public class NullToVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameterObj, CultureInfo culture) {
            return BooleanToVisibilityConverter.ConvertToVisibility(value != null, parameterObj);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

}
