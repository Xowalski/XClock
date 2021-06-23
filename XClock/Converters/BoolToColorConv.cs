using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace XClock
{
    class BoolToColorConv : IValueConverter
    {
        public bool IsDark { get; set; } = false;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool b = (bool)value;
            LinearGradientBrush linearGradientBrush = new LinearGradientBrush();

            Color lightorange = new Color();
            lightorange = Color.FromRgb(255, 155, 0);
            Color orange = new Color();
            orange = Color.FromRgb(255, 140, 0);
            Color darkorange = new Color();
            darkorange = Color.FromRgb(255, 120, 0);

            Color lightgreen = new Color();
            lightgreen = Color.FromRgb(155, 250, 0);
            Color green = new Color();
            green = Color.FromRgb(124, 252, 0);
            Color darkgreen = new Color();
            darkgreen = Color.FromRgb(100, 255, 20);

            linearGradientBrush.StartPoint = new Point(0, 0);
            linearGradientBrush.EndPoint = new Point(1, 1);
            if (b)
            {
                if (IsDark)
                {
                    return Brushes.OrangeRed;
                }
                else
                {
                    linearGradientBrush.GradientStops.Add(new GradientStop(lightorange, 0.0));
                    linearGradientBrush.GradientStops.Add(new GradientStop(orange, 0.1));
                    linearGradientBrush.GradientStops.Add(new GradientStop(darkorange, 0.4));
                    linearGradientBrush.GradientStops.Add(new GradientStop(orange, 0.6));
                    linearGradientBrush.GradientStops.Add(new GradientStop(lightorange, 0.9));
                    return linearGradientBrush;
                }
            }
            else
            {
                if (IsDark)
                {
                    return Brushes.SpringGreen;
                }
                else
                {
                    linearGradientBrush.GradientStops.Add(new GradientStop(lightgreen, 0.0));
                    linearGradientBrush.GradientStops.Add(new GradientStop(green, 0.1));
                    linearGradientBrush.GradientStops.Add(new GradientStop(darkgreen, 0.4));
                    linearGradientBrush.GradientStops.Add(new GradientStop(green, 0.6));
                    linearGradientBrush.GradientStops.Add(new GradientStop(lightgreen, 0.9));
                    return linearGradientBrush;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
