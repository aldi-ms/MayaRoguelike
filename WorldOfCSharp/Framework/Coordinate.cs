using System;

namespace WorldOfCSharp
{
    public struct Coordinate
    {
        private int x;
        private int y;

        public Coordinate(int x, int y)
        {
            if (x >= 0 && x <= Globals.CONSOLE_WIDTH)
            {
                this.x = x;
            }
            else
            {
                string exceptionStr = string.Format("Invalid width(X) coordinates. X -axis has a value of {0}. X -axis value should be in range 0 to {1}.", 
                    x, Globals.CONSOLE_WIDTH);
                throw new ArgumentOutOfRangeException("Constructor check.", exceptionStr);
            }
            
            if (y >= 0 && y <= Globals.CONSOLE_HEIGHT)
            {
                this.y = y;
            }
            else
            {
                string exceptionStr = string.Format("Invalid height(Y) coordinates. Y -axis has a value of {0}. Y -axis value should be in range 0 to {1}.", 
                    y, Globals.CONSOLE_HEIGHT);
                throw new ArgumentOutOfRangeException("Constructor check.", exceptionStr);
            }
        }

        public int X
        {
            get { return this.x; }

            set
            {
                if (value >= 0 && value <= Globals.CONSOLE_WIDTH)
                {
                    this.x = value;
                }
                else
                {
                    string exceptionStr = string.Format("Invalid width(X) coordinates. X -axis has a value of {0}. X -axis value should be in range 0 to {1}.",
                        value, Globals.CONSOLE_WIDTH);
                    throw new ArgumentOutOfRangeException("X -axis check.", exceptionStr);
                }
            }
        }

        public int Y
        {
            get { return this.y; }

            set
            {
                if (value >= 0 && value <= Globals.CONSOLE_HEIGHT)
                {
                    this.y = value;
                }
                else
                {
                    string exceptionStr = string.Format("Invalid height(Y) coordinates. Y -axis has a value of {0}. Y -axis value should be in range 0 to {1}.",
                        value, Globals.CONSOLE_HEIGHT);
                    throw new ArgumentOutOfRangeException("Y -axis check.", exceptionStr);
                }
            }
        }

        public override string ToString()
        {
            return string.Format("[{0};{1}]", this.X, this.Y);
        }
    }
}
