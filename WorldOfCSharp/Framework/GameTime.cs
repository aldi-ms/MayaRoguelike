using System;

namespace Maya
{
    public class GameTime
    {
        //do not access these directly. use properties instead!
        MT19937.MersenneTwister mt = new MT19937.MersenneTwister();
        private int seconds;
        private int minutes;
        private int hour;
        private int day;
        private int month;
        private int year;
        private int ticks;

        public GameTime(int seconds, int minutes, int hour, int day, int month, int year)
        {
            this.seconds = seconds;
            this.minutes = minutes;
            this.hour = hour;
            this.day = day;
            this.month = month;
            this.year = year;
        }

        public GameTime()
            : this(0, 0, 0, 1, 1, 1)
        { }

        public GameTime Now
        {
            get 
            {
                return new GameTime(this.seconds, this.minutes, this.hour,
                    this.day, this.month, this.year);
            }
        }

        #region Properties. Always use these!
        public int Seconds
        {
            get { return this.seconds; }

            set 
            {
                if (value >= 60)
                {
                    this.seconds = 0;
                    this.Minutes++;
                }
                else
                {
                    this.seconds = value; 
                }
            }
        }

        public int Minutes
        {
            get { return this.minutes; }

            set
            {
                if (value >= 60)
                {
                    this.minutes = 0;
                    this.Hour++;
                }
                else
                {
                    this.minutes = value;
                }
            }
        }

        public int Hour
        {
            get { return this.hour; }

            set
            {
                if (value >= 24)
                {
                    this.hour = 0;
                    this.Day++;
                }
                else
                {
                    this.hour = value;
                }
            }
        }

        public int Day
        {
            get { return this.day; }

            set
            {
                if (value > 20)
                {
                    this.day = 1;
                    this.Month++;
                }
                else
                {
                    this.day = value;
                }
            }
        }

        public int Month
        {
            get { return this.month; }

            set
            {
                if (value > 12)
                {
                    this.month = 0;
                    this.Year++;
                }
                else
                {
                    this.month = value;
                }
            }
        }

        public int Year
        {
            get { return this.year; }
            set { this.year = value; }
        }

        public int Ticks
        { 
            get { return this.ticks; }
        }
        #endregion

        public void Tick()
        {
            if (ticks % 5 != 0)
                this.Seconds += mt.Next(2, 6);
            else
                this.Seconds += mt.Next(4, 9);
            this.ticks++;
        }

        public void Tick(int ticks)
        {
            for (int i = 0; i < ticks; i++)
            {
                this.Tick();
                this.ticks++;
            }
        }

        public string TimeToString()
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2}", this.Hour, this.Minutes, this.Seconds);
        }

        public string ToSaveString()
        {
            return string.Format("{0}.{1}.{2}.{3}.{4}.{5}", this.Seconds, this.Minutes, this.Hour, this.Day, this.Month, this.Year);
        }

        public string ShortDateToString()
        {
            /*string str = string.Format("day of {0}, month of {1}", ((GameDay)Day).GetName(), ((GameMonth)Month).GetName());
            System.Text.StringBuilder sb = new System.Text.StringBuilder(str.Length);
            sb.Append(' ', (Globals.CONSOLE_WIDTH - Globals.GAME_FIELD_BOTTOM_RIGHT.X) - str.Length);
            sb.Append(str);
            return sb.ToString();*/

            return string.Format("{0}/{0}/{0}", this.Day, this.Month, this.Year);
        }

        public override string ToString()
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2}, day of {3}, month of {4}", this.Hour, this.Minutes, this.Seconds,
                ((GameDay)Day).GetName(), ((GameMonth)Month).GetName());
        }
    }
}
