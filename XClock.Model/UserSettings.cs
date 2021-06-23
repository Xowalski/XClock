using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XClock.Model
{
    public static class UserSettings
    {
        public static Alarm ReadAlarm()
        {
            Properties.Settings settings = Properties.Settings.Default;
            if (settings.IsSet)
            {
                return new Alarm(settings.IsSet, settings.DateTime);
            }
            else
            {
                return new Alarm();
            }
        }

        public static void SaveAlarm(Alarm alarm)
        {
            Properties.Settings settings = Properties.Settings.Default;
            settings.IsSet = alarm.IsSet;
            settings.DateTime = alarm.DateTime;
            settings.Save();
        }

        public static Pomodoro ReadPomodoro()
        {
            Properties.Settings settings = Properties.Settings.Default;
            return new Pomodoro(settings.WorkTime, settings.ShortBreakTime, settings.LongBreakTime, settings.Reps);
        }

        public static void SavePomodoro(Pomodoro pomodoro)
        {
            Properties.Settings settings = Properties.Settings.Default;
            settings.WorkTime = pomodoro.WorkTime;
            settings.ShortBreakTime = pomodoro.ShortBreakTime;
            settings.LongBreakTime = pomodoro.LongBreakTime;
            settings.Reps = pomodoro.Reps;
            settings.Save();
        }
    }
}
