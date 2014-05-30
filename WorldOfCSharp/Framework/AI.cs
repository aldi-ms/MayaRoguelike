using System;
using MT19937;

namespace Maya.AI
{
    public static class ArtificialIntelligence
    {
        public static int DrunkardWalk(Unit unit)
        {
            MersenneTwister mt = new MersenneTwister();
            unit.MakeAMove((CardinalDirection)mt.Next(0, 9));

            return 100;
        }
    }
}
