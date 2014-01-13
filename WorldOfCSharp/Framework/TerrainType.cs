using System;

namespace WorldOfCSharp
{
    public class TerrainType : GameObject
    {
        public TerrainType(int x, int y, int flags, char visualChar, ConsoleColor color, string name)
            : base(x, y, flags, visualChar, color, name)
        { }

        public TerrainType(string name, int flags, char visualChar, ConsoleColor color) //constructor for db load
            : this(0, 0, flags, visualChar, color, name)
        { }

        public TerrainType(TerrainType tt)
            : this(tt.X, tt.Y, tt.Flags, tt.VisualChar, tt.Color, tt.Name)
        { }
    }
}
