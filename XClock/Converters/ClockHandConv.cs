using System;
using System.Globalization;
using System.Windows.Data;

namespace XClock
{
    class ClockHandConv : IValueConverter
    {
        public ClockHand ClockHand { get; set; } = ClockHand.HourlyHand;
        public double Offset { get; set; } = 0;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dt = (DateTime)value;
            double num = 0;
            switch (ClockHand)
            {
                case ClockHand.HourlyHand:
                    num = dt.Hour;
                    if (num>12)
                    {
                        num -= 12;
                    }
                    num += dt.Minute / 60.0;
                    num /= 12;
                    break;
                case ClockHand.MinuteHand:
                    num = dt.Minute;
                    num += dt.Second / 60.0;
                    num /= 60.0;
                    break;
                case ClockHand.SecondHand:
                    num = dt.Second;
                    num /= 60.0;
                    break;
            }
            num *= 360.0;
            return num + Offset;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
