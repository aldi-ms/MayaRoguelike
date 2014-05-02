using System;

/* 
 * 
Used Flags:
# - description
0 – is movable
1 – is collidable
 * 
 */

namespace WorldOfCSharp
{
    [Flags]
    public enum Flags : uint
    {
        IsMovable = 0x1,
        IsCollidable = 0x2,
        HasEffect = 0x4,
        IsTransparent = 0x8,
        IsPlayerControl = 0x10
    }

    public class GameObject
    {
        private Flags flags;
        private Coordinate coord;
        private char visualChar = '\0';
        private ConsoleColor color;
        private string name;
                
        public GameObject(Coordinate coordinates, Flags flags, char visualChar, ConsoleColor color, string name)
        {
            this.coord = coordinates;
            this.flags = flags;
            this.visualChar = visualChar;
            this.color = color;
            this.name = name;            
        }

        public GameObject(int x, int y, Flags flags, char visualChar, ConsoleColor color, string name)
            : this(new Coordinate(x, y), flags, visualChar, color, name)
        { }

        public int X
        {
            get { return this.coord.X; }
            set 
            {
                int temp = value;
                if (temp >= 0 && temp <= Globals.GAME_FIELD_BOTTOM_RIGHT.X)
                {
                    this.coord.X = temp;
                }
            }
        }
        
        public int Y
        {
            get { return this.coord.Y; }
            set 
            {
                int temp = value;
                if (temp >= 0 && temp <= Globals.GAME_FIELD_BOTTOM_RIGHT.Y)
                {
                    this.coord.Y = temp;
                }
            }
        }

        public Flags Flags
        {
            get { return this.flags; }
            set { this.flags = value; }
        }

        public char VisualChar
        {
            get { return this.visualChar; }
        }

        public ConsoleColor Color
        {
            get { return this.color; }
        }
        
        public string Name
        {
            get { return this.name; }
        }
        /*
        public bool GetFlag(int position)
        {
            if (position >= 0 && position < 32) //32 flags - from 0 to 31
            {
                int bitValue = (flagsDEPRECATED & (1 << position)) >> position;

                if (bitValue == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                string exceptionStr = string.Format("Invalid flag position. Flag position value is {0}. Flag position should be in range 0 - 31", position);
                throw new ArgumentOutOfRangeException("Flag position", exceptionStr);
            }
        }
        
        public void SetFlag(int position, bool state)
        {
            if (position >= 0 && position < 32)
            {
                if (state)
                {
                    flagsDEPRECATED = flagsDEPRECATED | (1 << position);
                }
                else
                {
                    flagsDEPRECATED = flagsDEPRECATED & ~(1 << position);
                }
            }
            else
            {
                string exceptionStr = string.Format("Invalid flag position. Flag position value is {0}. Flag position should be in range 0 - 31.", position);
                throw new ArgumentOutOfRangeException("Flag position", exceptionStr);
            }
        }
        */
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("Game object: <{0}>\nChar: <{1}>\nCoordinates: <{2}>\nFlags: <{3}>", this.Name, this.VisualChar, this.coord, Convert.ToString(this.Flags));
            return sb.ToString();
        }
    }
}
