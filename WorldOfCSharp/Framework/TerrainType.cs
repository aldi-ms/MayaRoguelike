using System;

namespace Maya
{
    public class Terrain : GameObject
    {
        private int positionInDB = -1;

        public int PositionInDB
        {
            get { return this.positionInDB; }
        }
        public Terrain(int x, int y, Flags flags, char visualChar, ConsoleColor color, string name)
            : base(x, y, flags, visualChar, color, name)
        { }

        public Terrain(int positionInDB, string name, Flags flags, char visualChar, ConsoleColor color) //constructor for db load
            : this(0, 0, flags, visualChar, color, name)
        {
            this.positionInDB = positionInDB;
        }

        public Terrain(Terrain tt)
            : this(tt.PositionInDB, tt.Name, tt.Flags, tt.VisualChar, tt.Color)
        { }
    }
}
