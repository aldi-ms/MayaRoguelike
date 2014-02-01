using System;

namespace WorldOfCSharp
{
    public class TerrainType : GameObject
    {
        private int positionInDB = -1;

        public int PositionInDB
        {
            get { return this.positionInDB; }
        }
        public TerrainType(int x, int y, int flags, char visualChar, ConsoleColor color, string name)
            : base(x, y, flags, visualChar, color, name)
        { }

        public TerrainType(int positionInDB, string name, int flags, char visualChar, ConsoleColor color) //constructor for db load
            : this(0, 0, flags, visualChar, color, name)
        {
            this.positionInDB = positionInDB;
        }

        public TerrainType(int positionInDB, TerrainType tt)
            : this(positionInDB, tt.Name, tt.Flags, tt.VisualChar, tt.Color)
        { }
    }
}
