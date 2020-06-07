using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RyonaVibration
{
    public class SpeedTime
    {
        public SpeedTime(double speedInPercent, int timeInMs, bool force = false)
        {
            SpeedInPercent = speedInPercent;
            TimeInMs = timeInMs;

            if(SpeedInPercent > 1d)
            {
                SpeedInPercent = 1d;
            }
            else if(SpeedInPercent < 0.1d)
            {
                SpeedInPercent = 0.1d;
            }

            if(TimeInMs < 100)
            {
                TimeInMs = 100;
            }
        }

        //Between 0 and 1, 1 = 100%, 0.5 = 50%
        public double SpeedInPercent { get; set; } = 0d;

        public int TimeInMs { get; set; } = 0;

        public bool Force { get; set; } = false;
    }
}
