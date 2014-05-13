using System;

namespace WorldOfCSharp
{
    public class InGameObject : GameObject
    {
        private int positionInDB = -1;

        public int PositionInDB
        {
            get { return this.positionInDB; }
        }

        public InGameObject(int x, int y, Flags flags, char visualChar, ConsoleColor color, string name)
            : base(x, y, flags, visualChar, color, name)
        { }

        public InGameObject(int positionInDB, string name, Flags flags, char visualChar, ConsoleColor color)
            : this(0, 0, flags, visualChar, color, name)
        {
            this.positionInDB = positionInDB;
        }

        public InGameObject(InGameObject inGameObj)
            : this(inGameObj.PositionInDB, inGameObj.Name, inGameObj.Flags, inGameObj.VisualChar, inGameObj.Color)
        { }
    }
}