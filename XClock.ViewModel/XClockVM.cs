using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using XClock.Model;

namespace XClock.ViewModel
{
    public class XClockVM : INotifyPropertyChanged
    {
        private readonly bool isInDesignMode = DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject());

        #region Property Changed
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged()
        {
            if (CurrentTime - previousTime < TimeSpan.FromSeconds(1) && CurrentTime.Second == previousTime.Second)
            {
                return;
            }
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(nameof(CurrentTime)));
            }
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Clock
        private DateTime previousTime = DateTime.Now;
        public DateTime CurrentTime
        {
            get { return DateTime.Now; }
        }

        private const int refreshingTimeMs = 100;

        public XClockVM()
        {
            DispatcherTimer clockTimer = new DispatcherTimer();
            clockTimer.Tick += (sender, e) => { OnPropertyChanged(); };
            clockTimer.Tick += (sender, e) => { PlayAlarm(Alarm); };
            clockTimer.Tick += (sender, e) => { OnPropertyChanged(nameof(NextPomodoroBellTimeLeft)); };
            clockTimer.Interval = TimeSpan.FromMilliseconds(refreshingTimeMs);
            if (!isInDesignMode)
            {
                clockTimer.Start();
            }
        }
        #endregion

        #region Alarm

        private Alarm alarm = UserSettings.ReadAlarm();
        public Alarm Alarm
        {
            get
            {
                return alarm;
            }
            set
            {
                alarm = value;
                OnPropertyChanged(nameof(Alarm));
            }
        }

        private bool isAlarmPlaying = false;
        public bool IsAlarmPlaying
        {
            get
            {
                return isAlarmPlaying;
            }
            set
            {
                isAlarmPlaying = value;
                OnPropertyChanged(nameof(IsAlarmPlaying));
            }
        }

        private bool alarmFlag = false;

        private static MediaPlayer alarmPlayer;
        public MediaPlayer AlarmPlayer
        {
            get
            {
                return alarmPlayer;
            }
            set
            {
                alarmPlayer = value;
                OnPropertyChanged(nameof(AlarmPlayer));
            }
        }

        private ICommand setAlarmCommand;
        public ICommand SetAlarmCommand
        {
            get
            {
                if (setAlarmCommand == null)
                {
                    setAlarmCommand = new RelayCommand(o =>
                    {
                        UserSettings.SaveAlarm(new Alarm(true, (DateTime)o));
                        Alarm = UserSettings.ReadAlarm();
                    });
                }
                return setAlarmCommand;
            }
        }

        private ICommand turnOffAlarmCommand;

        public ICommand TurnOffAlarmCommand
        {
            get
            {
                if (turnOffAlarmCommand == null)
                {
                    turnOffAlarmCommand = new RelayCommand(o =>
                    {
                        UserSettings.SaveAlarm(new Alarm(false, (DateTime)o));
                        Alarm = new Alarm(false, (DateTime)o);
                    });
                }
                return turnOffAlarmCommand;
            }
        }

        public void PlayAlarm(Alarm alarm)
        {
            if (alarm.IsSet)
            {
                if (DateTime.Now.Hour == alarm.DateTime.Hour && DateTime.Now.Minute == alarm.DateTime.Minute &&
                    DateTime.Now.Second == alarm.DateTime.Second && alarmFlag == false)
                {
                    alarmFlag = true;
                    IsAlarmPlaying = true;

                    if (alarmPlayer == null)
                    {
                        alarmPlayer = new MediaPlayer();
                        alarmPlayer.Open(new Uri(System.IO.Path.GetFullPath("Sounds/AlarmPiano.mp3"), UriKind.RelativeOrAbsolute));
                        alarmPlayer.MediaEnded += (s, e) => { StopAlarm(); };
                        alarmPlayer.Play();
                    }
                }
            }
            if (DateTime.Now.Second != alarm.DateTime.Second && alarmFlag == true)
            {
                alarmFlag = false;
            }
        }

        private ICommand stopAlarmCommand;
        public ICommand StopAlarmCommand
        {
            get
            {
                if (stopAlarmCommand == null)
                {
                    stopAlarmCommand = new RelayCommand(o =>
                    {
                        StopAlarm();
                    });
                }
                return stopAlarmCommand;
            }
        }

        private void StopAlarm()
        {
            IsAlarmPlaying = false;
            alarmPlayer.MediaEnded -= (s, e) => { StopAlarm(); };
            alarmPlayer.Stop();
            alarmPlayer = null;
        }
        #endregion

        #region Pomodoro
        private Pomodoro pomodoro = UserSettings.ReadPomodoro();
        public Pomodoro Pomodoro
        {
            get
            {
                return pomodoro;
            }
            set
            {
                pomodoro = value;
                OnPropertyChanged(nameof(Pomodoro));
            }
        }

        private DispatcherTimer pomodoroTimer;
        private MediaPlayer pomodoroPlayer = new MediaPlayer();

        public DateTime NextPomodoroBell { get; set; } = DateTime.Now;
        public TimeSpan NextPomodoroBellTimeLeft
        {
            get
            {
                return NextPomodoroBell.Subtract(CurrentTime);
            }
        }

        private ICommand playPomodoroCommand;
        public ICommand PlayPomodoroCommand
        {
            get
            {
                if (playPomodoroCommand == null)
                {
                    playPomodoroCommand = new RelayCommand(o =>
                    {
                        PomodoroNextInterval();
                    },
                    o => Pomodoro.IsPomodoroOn == false);
                }
                return playPomodoroCommand;
            }
        }

        private void PomodoroNextInterval()
        {
            Pomodoro.PomodoroIntervalChange();
            pomodoroTimer = new DispatcherTimer();
            pomodoroTimer.Tick += (sender, e) => { PomodoroNotice(); };
            pomodoroTimer.Interval = TimeSpan.FromMinutes(Pomodoro.NextPomodoroIntervalChangeTime);
            pomodoroTimer.Start();
            NextPomodoroBell = CurrentTime.AddMinutes(Pomodoro.NextPomodoroIntervalChangeTime);
            OnPropertyChanged(nameof(NextPomodoroBell));
            OnPropertyChanged(nameof(NextPomodoroBell));
            Pomodoro.IsPomodoroOn = true;
            OnPropertyChanged(nameof(Pomodoro));
        }

        private void PomodoroNotice()
        {
            PlayPomodoroBell();
            pomodoroTimer.Stop();
            pomodoroTimer.Tick -= (sender, e) => { PomodoroNotice(); };
            pomodoroTimer = null;
            PomodoroNextInterval();
        }

        private void PlayPomodoroBell()
        {
            pomodoroPlayer = new MediaPlayer();
            pomodoroPlayer.Open(new Uri(System.IO.Path.GetFullPath("Sounds/PomodoroBellSound.mp3"), UriKind.RelativeOrAbsolute));
            pomodoroPlayer.Play();
        }

        private ICommand stopPomodoroCommand;
        public ICommand StopPomodoroCommand
        {
            get
            {
                if (stopPomodoroCommand == null)
                {
                    stopPomodoroCommand = new RelayCommand(o =>
                    {
                        pomodoroTimer.Stop();
                        pomodoroTimer.Tick -= (sender, e) => { PomodoroNotice(); };
                        pomodoroTimer = null;
                        Pomodoro = UserSettings.ReadPomodoro();
                    },
                    o => Pomodoro.IsPomodoroOn);
                }
                return stopPomodoroCommand;
            }
        }

        private ICommand skipPomodoroCommand;
        public ICommand SkipPomodoroCommand
        {
            get
            {
                if (skipPomodoroCommand == null)
                {
                    skipPomodoroCommand = new RelayCommand(o =>
                    {
                        PlayPomodoroBell();
                        PomodoroNextInterval();
                    });
                }
                return skipPomodoroCommand;
            }
        }

        private ICommand changePomodoroSettingsCommand;
        public ICommand ChangePomodoroSettingsCommand
        {
            get
            {
                if (changePomodoroSettingsCommand == null)
                {
                    changePomodoroSettingsCommand = new RelayCommand(o =>
                    {
                        Pomodoro pomodoroSettings = (Pomodoro)o;
                        UserSettings.SavePomodoro(pomodoroSettings);
                        Pomodoro = new Pomodoro(pomodoroSettings.WorkTime, pomodoroSettings.ShortBreakTime, pomodoroSettings.LongBreakTime, pomodoroSettings.Reps);
                    });
                }
                return changePomodoroSettingsCommand;
            }
        }
        #endregion
    }
}