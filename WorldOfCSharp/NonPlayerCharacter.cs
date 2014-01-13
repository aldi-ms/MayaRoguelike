using System;
namespace WorldOfCSharp
{
    public class NonPlayerCharacter : Unit
    {
        private string name;
        private char visCh;
        private ConsoleColor color;
        private int unitSpeed;
        private int flags;

        public NonPlayerCharacter(int x, int y, int flags, int unitSpeed, char visCh, ConsoleColor color, string name)
            : base(x, y, flags, unitSpeed, visCh, color, name)
        {
            this.name = name;
            if (visCh != '@')
            {
                this.visCh = visCh;
            }
            else
            {
                throw new ArgumentException("Unit visualChar must not have value '@'.");
            }
            this.color = color;
            this.unitSpeed = unitSpeed;
            this.flags = flags;
        }
        
        public NonPlayerCharacter(Unit unit)
            : this(unit.X, unit.Y, unit.Flags, unit.Speed, unit.VisualChar, unit.Color, unit.Name)
        { }
    }
}
