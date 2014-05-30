using MT19937;

namespace Maya
{
    public static class RNG
    {
        private static MersenneTwister mt = new MersenneTwister();

        public static int Roll(int numberOfDies, int sidesOfDie)
        {
            int result = 0;
            for (int i = 0; i < numberOfDies; i++)
                result += mt.Next(1, sidesOfDie + 1);
            return result;
        }

        public static int RollDice(string str)
        {
            string[] split = str.Split('d');
            return Roll(int.Parse(split[0]), int.Parse(split[1]));
        }
    }
}
