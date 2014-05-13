using System;

namespace WorldOfCSharp
{
    [Flags]
    public enum Flags
    {
        IsMovable = 1 << 0,
        IsCollidable = 1 << 1,
        HasEffect = 1 << 2,
        IsTransparent = 1 << 3,
        IsPlayerControl = 1 << 4
    }

    public class GameObject
    {
        private Flags flags;
        private ConsoleColor color;
        private Coordinate coord;
        private char visualChar;
        private string name;

        public GameObject(int x, int y, Flags flags, char visualChar, ConsoleColor color, string name)
        {
            this.coord = new Coordinate(x, y);
            this.flags = flags;
            this.visualChar = visualChar;
            this.color = color;
            this.name = name;
        }

        public int X
        {
            get { return this.coord.X; }
            set 
            {
                if (value >= 0 && value <= Globals.GAME_FIELD_BOTTOM_RIGHT.X)
                    this.coord.X = value;
            }
        }
        
        public int Y
        {
            get { return this.coord.Y; }
            set 
            {
                if (value >= 0 && value <= Globals.GAME_FIELD_BOTTOM_RIGHT.Y)
                    this.coord.Y = value;
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

        public override string ToString()
        {
            return string.Format("Game object: {0}\nChar: {1}\nCoordinates: {2}\nFlags: {3}",
                this.Name,
                this.VisualChar, 
                this.coord, 
                Convert.ToString(this.Flags));
        }
    }
}
