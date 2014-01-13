using System;
using MT19937;

namespace WorldOfCSharp.AI
{
    public static class ArtificialIntelligence
    {
        public static int DrunkardWalk(Unit unit)
        {
            if (unit.VisualChar != '@')
            {
                MersenneTwister mt = new MersenneTwister();
                int randDir = mt.Next(0, 9);
                unit.MakeAMove((Direction)randDir);
            }
            return 100;
        }
    }
}
