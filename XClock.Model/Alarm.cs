using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XClock.Model
{
    public class Alarm
    {
        public bool IsSet { get; set; }
        public DateTime DateTime { get; set; }
        public Alarm(bool isSet, DateTime dateTime)
        {
            IsSet = isSet;
            DateTime = dateTime;
        }

        public Alarm()
        {
            IsSet = false;
            DateTime = DateTime.Now;
        }
    }
}