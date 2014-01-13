using MT19937;

namespace WorldOfCSharp
{
    public static class RNG
    {
        private static MersenneTwister mt = new MersenneTwister();

        public static int Roll(int numberOfDies, int sidesOfDie)
        {
            int result = 0;
            for (int i = 0; i < numberOfDies; i++)
            {
                result += mt.Next(1, sidesOfDie + 1);
            }
            return result;
        }
    }
}
