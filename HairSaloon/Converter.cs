using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;
using HairSaloon;

namespace HairSaloon
{
    public class Converter : IValueConverter
    {
        // change color of specific whole rows of Data Grid
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Appointment)
            {
                if (((Appointment)value).Customer.Age > 50)
                {
                    return Brushes.Red;
                }
                else
                {
                    return Brushes.White;
                }
            }
            else
            {
                return Brushes.White;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
