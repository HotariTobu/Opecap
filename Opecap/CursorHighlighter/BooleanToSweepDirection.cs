using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Opecap
{
    public class BooleanToSweepDirection : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((value as bool?) ?? false) ? SweepDirection.Clockwise : SweepDirection.Counterclockwise;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
