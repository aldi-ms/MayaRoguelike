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

        /// <summary>
        /// Returns a random integer between [low, high], both inclusive.
        /// </summary>
        /// <param name="low"></param>
        /// <param name="high"></param>
        /// <returns></returns>
        public static int Random(int low, int high)
        {
            return mt.Next(low, high + 1);
        }
    }
}
