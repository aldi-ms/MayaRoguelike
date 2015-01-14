using System;

namespace Maya.Framework
{
    internal enum Races
    {
        Human = 0
    }

    /// <summary>
    /// Saves the striking features of a race, to be used throughout the game.
    /// </summary>
    internal abstract class Race
    {
        public abstract int ExperienceModifier { get; }
        public abstract void SetAttributes(UnitAttributes attributes);
        public abstract int HeightGet(Unit unit);
        public abstract int WeightGet();
        public abstract int StrengthGet();
    }

    /// <summary>
    /// First implemented race for testing purposes.
    /// </summary>
    internal class Human : Race
    {
        private const double deltaPerCent = 0.2d;
        public UnitAttributes unitAttr;
        private int experienceModifier = 1;

        public override int ExperienceModifier
        {
            get
            {
                return this.experienceModifier;
            }
        }

        public override void SetAttributes(UnitAttributes attributes)
        {
            this.unitAttr = attributes;
        }

        public override int HeightGet(Unit unit)
        {
            int minHeight, maxHeight, delta;

            if (unitAttr.SexIsMale)
            {
                minHeight = 165;
                maxHeight = 185;
            }
            else
            {
                minHeight = 155;
                maxHeight = 175;
            }

            delta = (int)((maxHeight - minHeight) * deltaPerCent);
            return RNG.Random(minHeight, maxHeight) + RNG.Random(-delta, delta);
        }

        public override int WeightGet()
        {
            int minWeight, maxWeight, delta;

            if (unitAttr.SexIsMale)
            {
                minWeight = 75;
                maxWeight = 85;
            }
            else
            {
                minWeight = 50;
                maxWeight = 65;
            }

            delta = (int)((maxWeight - minWeight) * deltaPerCent);
            return RNG.Random(minWeight, maxWeight) + RNG.Random(-delta, delta);
        }

        public override int StrengthGet()
        {
            int maxStr, minStr, delta;

            if (unitAttr.SexIsMale)
            {
                minStr = 3;
                maxStr = 6;
            }
            else
            {
                minStr = 1;
                maxStr = 4;
            }

            delta = (int)((maxStr - minStr) * (deltaPerCent * 2));
            return RNG.Random(minStr, maxStr) + RNG.Random(-delta, delta);
        }
    }
}
