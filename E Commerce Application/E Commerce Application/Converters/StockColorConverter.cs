using E_Commerce_Application.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.Converters
{
    public class StockColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool IsAvailable)
            {
                bool available = (bool)value;
                //bool isInStock = cartResponse.IsAvailable;
                return available ? Color.FromHex("#00C569") : Color.FromHex("#ff2036");
            }
            return Color.FromHex("#ff2036");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
