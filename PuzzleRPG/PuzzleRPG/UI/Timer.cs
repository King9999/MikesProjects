/* This is a timer that is used to record the player's total play time.  This timer is hidden from the player. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PuzzleRPG.UI
{
    class Timer
    {
        ushort millisecs; //increases by 2 every frame (30 frames * 2 = 1 second)
        ushort seconds;
        ushort minutes;
        ushort hours;     //max hours is 999

        public Timer()
        {
            millisecs = 0;
            seconds = 0;
            minutes = 0;
            hours = 0;
        }

        public void Reset()
        {
            millisecs = 0;
            seconds = 0;
            minutes = 0;
            hours = 0;
        }

        public string Time()  //returns time
        {
            //append a 0 if hours or minutes is less than 10
            string time = "";

            if (hours < 10 && minutes < 10)
                time = "0" + hours + ":0" + minutes;
            else if (hours < 10 && minutes >= 10)
                time = "0" + hours + ":" + minutes;
            else if (hours >= 10 && minutes < 10)
                time = hours + ":0" + minutes;
            else
                time = hours + ":" + minutes;

            return time;
        }

        public ushort Hour() { return hours; }
        public ushort Minutes() { return minutes; }
        public ushort Seconds() { return seconds; }

        //used when loading record file. Gets current time
        public void SetTimer(ushort hours, ushort mins)
        {
            this.hours = hours;
            minutes = mins;
        }

        public void SetTimer(ushort hours, ushort mins, ushort secs)
        {
            this.hours = hours;
            minutes = mins;
            seconds = secs;
        }

        public bool TimeUp()
        {
            return (hours == 0 && minutes == 0 && seconds == 0);
        }

        //runs the timer.  Must go in update loop
        public void Tick()
        {
            millisecs += 2;     //2 * 30 frames = 60 frames = 1 second
            if (millisecs > 59)
            {
                millisecs = 0;
                seconds++;

                if (seconds > 59)
                {
                    seconds = 0;
                    minutes++;

                    if (minutes > 59)
                    {
                        minutes = 0;
                        hours++;

                        if (hours > 999)    //should not get here
                        {
                            hours = 999;
                        }
                    }
                }
            }
        }
    }
}
