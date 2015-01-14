using System;

namespace Maya.Framework
{
    public class Experience
    {
        public const int MAX_LEVEL = 50;
        private int level;
        private double experiencePoints;     //eXperience Points; used to buy level/level spells/skills
        private float experienceModifier;
        private int[] XPArray;

        public Experience(float experienceModifier)
        {
            this.level = 0;
            this.experiencePoints = 0.0d;
            this.experienceModifier = experienceModifier;
            this.XPArray = new int[MAX_LEVEL];
            this.ExperienceTable(ref this.XPArray);
        }

        public int[] ExpPointsArray
        {
            get { return this.XPArray; }
        }

        public double XP
        {
            get { return this.experiencePoints; }
        }

        public int Level
        {
            get { return this.level; }
        }

        public void GainXP(double experienceGain)
        {
            this.experiencePoints += experienceGain * this.experienceModifier;
            do
            {
                if (this.experiencePoints >= this.ExpPointsArray[this.level])
                {
                    GainLevel();
                    this.experiencePoints -= this.ExpPointsArray[this.level - 1];
                }
            } while (this.experiencePoints >= this.ExpPointsArray[this.level]);
        }

        public void GainLevel()
        {
            this.level++;
        }

        private void ExperienceTable(ref int[] array)
        {
            int levels = array.Length;
            int xpForFirstLevel = 90;
            int xpForLastLevel = 115900;

            double B = Math.Log(xpForLastLevel / xpForFirstLevel) / (levels - 1);
            //double B = Math.Log(1 + 0.16);
            double A = xpForFirstLevel / (Math.Exp(B) - 1.0);

            for (int i = 0; i < levels; i++)
            {
                int old_xp = (int)Math.Round(A * Math.Exp(B * (i - 1)));
                int new_xp = (int)Math.Round(A * Math.Exp(B * i));
                array[i] = new_xp - old_xp;
            }
        }
    }
}
