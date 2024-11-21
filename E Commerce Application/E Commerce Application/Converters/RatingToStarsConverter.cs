using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_Application.Converters
{
    public class RatingToStarsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int rating && parameter is string parameterString && int.TryParse(parameterString, out int starIndex))
            {
                return rating >= starIndex ? Application.Current.Resources["FilledStarIcon"] : Application.Current.Resources["EmptyStarIcon"];
            }

            return Application.Current.Resources["EmptyStarIcon"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
