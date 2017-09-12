using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace wits.converter {

    // Default behavior: Only show a component if an enum property has the specific value
    // Parameters: <enum-value>[;reverse][;hidden]
    //  reverse: reverse the boolean logic
    //  hidden: use Visibility.Hidden when false (as opposed to default Collapsed)
    public class EnumToVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value == null)
                return Visibility.Collapsed;

            string[] pieces = parameter.ToString().Split(';');
            bool isVisible = value.ToString() == pieces[0];
            string remainingParams = string.Join(";", pieces.Skip(1).ToArray());
            return BooleanToVisibilityConverter.ConvertToVisibility(isVisible, remainingParams);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
