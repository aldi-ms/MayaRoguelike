using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfCSharp
{
    public class PlayerCharacter : Unit
    {
        private string name;
        private const char visCh = '@';
        private static ConsoleColor color = ConsoleColor.White;
        private static int unitSpeed = 13;
        private static int flags = 3;  //00011 flags, movable and collidable == 3 ///11

        public PlayerCharacter(int x, int y, int flags, int unitSpeed, char visCh, ConsoleColor color, string name)
            : base(x, y, flags, unitSpeed, visCh, color, name)
        {
            this.name = name;
        }

        public PlayerCharacter(int x, int y, string name)
            : this(x, y, flags, unitSpeed, visCh, color, name)
        { }

        public PlayerCharacter(Unit unit)
            : base(unit)
        { }
    }
}
