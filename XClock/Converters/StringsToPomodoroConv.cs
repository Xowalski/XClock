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
    class StringsToPomodoroConv : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int workTime;
            if (!(int.TryParse((string)values[0], out workTime)))
            {
                workTime = 25;
            }
            int shortBreak;
            if (!(int.TryParse((string)values[1], out shortBreak)))
            {
                shortBreak = 5;
            }
            int longBreak;
            if (!(int.TryParse((string)values[2], out longBreak)))
            {
                longBreak = 30;
            }
            int reps;
            if (!(int.TryParse((string)values[3], out reps)))
            {
                reps = 4;
            }
            return new Pomodoro(workTime, shortBreak, longBreak, reps);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
