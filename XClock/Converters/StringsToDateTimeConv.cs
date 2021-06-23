using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using XClock.Model;

namespace XClock
{
    class StringsToDateTimeConv : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int hours;
            if (!(int.TryParse((string)values[0], out hours)))
            {
                hours = DateTime.Now.Hour;
            }
            int minutes;
            if (!(int.TryParse((string)values[1], out minutes)))
            {
                minutes = DateTime.Now.Minute;
            }
            int seconds;
            if (!(int.TryParse((string)values[2], out seconds)))
            {
                seconds = DateTime.Now.Second;
            }
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hours, minutes, seconds);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
