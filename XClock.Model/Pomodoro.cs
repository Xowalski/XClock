using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XClock.Model
{
    public class Pomodoro
    {
        //pomodoro parameters
        public int WorkTime { get; set; }
        public int ShortBreakTime { get; set; }
        public int LongBreakTime { get; set; }
        public int Reps { get; set; }

        public Pomodoro(int workTime = 25, int shortBreakTime = 5, int longBreakTime = 30, int reps = 4)
        {
            WorkTime = workTime;
            ShortBreakTime = shortBreakTime;
            LongBreakTime = longBreakTime;
            Reps = reps;
        }

        //flag showing whether pomodoro is working or not
        public bool IsPomodoroOn { get; set; } = false;

        private int repsCount = 0;
        public int RepsCount
        {
            get
            {
                return repsCount;
            }
        }

        private bool isCurrentIntervalWork = false;
        public bool IsCurrentIntervalWork
        {
            get
            {
                return isCurrentIntervalWork;
            }
        }

        private int nextPomodoroIntervalChangeTime;

        public int NextPomodoroIntervalChangeTime
        {
            get
            {
                return nextPomodoroIntervalChangeTime;
            }
        }

        public void PomodoroIntervalChange()
        {
            if (isCurrentIntervalWork)
            {
                repsCount++;
                if (repsCount < Reps)
                {
                    isCurrentIntervalWork = false;
                    nextPomodoroIntervalChangeTime = ShortBreakTime;
                }
                else
                {
                    repsCount = 0;
                    isCurrentIntervalWork = false;
                    nextPomodoroIntervalChangeTime = LongBreakTime;
                }
            }
            else
            {
                isCurrentIntervalWork = true;
                nextPomodoroIntervalChangeTime = WorkTime;
            }
        }
    }
}
