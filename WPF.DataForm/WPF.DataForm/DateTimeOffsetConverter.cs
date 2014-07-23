using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace WPF.DataForm
{
    public class DateTimeOffsetConverter:IValueConverter
    {
        public static Lazy<DateTimeOffsetConverter> Instance = new Lazy<DateTimeOffsetConverter>();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            return ((DateTimeOffset) value).LocalDateTime;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            return new DateTimeOffset((DateTime)value);
        }
    }
}
