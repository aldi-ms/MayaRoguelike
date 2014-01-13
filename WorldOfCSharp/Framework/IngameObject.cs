using System;

namespace WorldOfCSharp
{
    public class IngameObject : GameObject
    {
        public IngameObject(int x, int y, int flags, char visualChar, ConsoleColor color, string name)
            : base(x, y, flags, visualChar, color, name)
        { }

        public IngameObject(string name, int flags, char visualChar, ConsoleColor color)
            : this(0, 0, flags, visualChar, color, name)
        { }
    }
}