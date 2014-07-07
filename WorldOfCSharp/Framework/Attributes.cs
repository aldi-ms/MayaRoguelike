using System;
using System.Collections.Generic;

namespace Maya
{
    /// <remarks>
    /// To be fixed values according 
    /// to calculations of fitness level.
    /// </remarks>
    public enum Fitness
    {
        Feeble = 0,
        Weak = 2,
        Sound = 4,
        Striking = 6,
        Rugged = 8,
        Brawny = 10
    }

    public enum StatNumber
    { 
        Strength = 0,
        Dexterity,
        Constitution,
        Wisdom,
        Spirit,
        Luck
    }

    public class BaseAttributes
    {
        protected int strength;
        protected int dexterity;
        protected int constitution;
        protected int wisdom;
        protected int spirit;
        protected int luck;

        public BaseAttributes(int str = 1, int dex = 1, int con = 1, int wis = 1, int spi = 1, int luck = 1)
        {
            this.strength = str;
            this.dexterity = dex;
            this.constitution = con;
            this.wisdom = wis;
            this.spirit = spi;
            this.luck = luck;
        }

        public static int Count
        {
            //number of stats, for the indexer (0 - 5);
            get { return 6; }
        }

        /// <summary>
        /// Indexer for the base attributes.
        /// </summary>
        /// <param name="index">str: 0, dex: 1, con: 2, wis: 3, spi: 4, luck: 5</param>
        /// <returns></returns>
        public int this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return this.strength;
                    case 1:
                        return this.dexterity;
                    case 2:
                        return this.constitution;
                    case 3:
                        return this.wisdom;
                    case 4:
                        return this.spirit;
                    case 5:
                        return this.luck;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            set
            {
                switch (index)
                {
                    case 0:
                        this.strength = value;
                        break;
                    case 1:
                        this.dexterity = value;
                        break;
                    case 2:
                        this.constitution = value;
                        break;
                    case 3:
                        this.wisdom = value;
                        break;
                    case 4:
                        this.spirit = value;
                        break;
                    case 5:
                        this.luck = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Get shortened stat names.
        /// </summary>
        /// <param name="index">Index of stat.</param>
        /// <param name="idx">Used just to distinguish the two indexers.</param>
        /// <returns>The name of the stat.</returns>
        public string this[int index, int idx]
        {
            get 
            {
                switch (index)
                {
                    case 0: return "str";
                    case 1: return "dex";
                    case 2: return "con";
                    case 3: return "wis";
                    case 4: return "spi";
                    case 5: return "luck";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /* UNUSED ENUMERATOR
        public IEnumerator<int> GetEnumerator()
        {
            return StatValues();
        }

        public IEnumerator<int> StatValues()
        {
            yield return strength;
            yield return dexterity;
            yield return constitution;
            yield return wisdom;
            yield return spirit;
            yield return luck;
        }
        */

        public override string ToString()
        {
            return string.Format("str = {0} \ndex = {1} \ncon = {2} \nwis = {3} \nspi = {4} \nluck = {5}",
                this.strength, this.dexterity, this.constitution, this.wisdom, this.spirit, this.luck);
        }
    }

    public class UnitAttributes : BaseAttributes
    {
        //Defines body build
        private double height;
        private int weight;
        private int age;
        private Fitness fitness;
        //race goes here?

        /// <summary>
        /// Body Mass Index.
        /// </summary>
        private double BMI;

        private int currentHealth;
        private int energy;

        public UnitAttributes(int age, int str = 1, int dex = 1, int con = 1, int wis = 1, int spi = 1, int luck = 1)
            : base(str, dex, con, wis, spi, luck)
        {
            this.age = age;
            this.height = 1.85;      //later calc based on age/race/class etc.
            this.weight = 72;       //- - - - same as above - - - - -
            this.fitness = Fitness.Striking;        //- - - - same as above - - - - -
            this.BMI = weight / (height * height);
            currentHealth = MaxHealth;
        }

        public int MaxHealth
        {
            get
            {
                double a = (strength + wisdom) * 0.9 + (dexterity + spirit) * 0.75 + luck * 0.25;
                return (int)(3 * constitution + 2 * Math.Log(a * Math.Pow(constitution, 3), 2));
            }
        }

        public int ActionSpeed
        {
            get 
            {
                if (BMI > 18.5 && BMI < 30)
                    return (int)((dexterity + (int)fitness) / Math.Sqrt(BMI) + 
                        (2 * dexterity * (int)fitness) / ((BMI * BMI) / Math.Sqrt((double)fitness)) + 5 * Math.Sqrt(dexterity));
                else 
                {
                    int BMIPenalty = 45;
                    return (int)((dexterity + (int)fitness) / Math.Sqrt(BMIPenalty) + 
                        (2 * dexterity * (int)fitness) / ((BMIPenalty * BMIPenalty) / Math.Sqrt((double)fitness)) + 5 * Math.Sqrt(dexterity));
                }
            }
        }

        public int HealthRegen
        {
            get { return (int)(constitution / 2 + spirit); }
        }

        public int CurrentHealth
        {
            get
            {
                return this.currentHealth <= this.MaxHealth ?
                    this.currentHealth : this.MaxHealth;
            }
            set
            {
                if (value <= this.MaxHealth)
                    this.currentHealth = value;
                else
                    this.currentHealth = this.MaxHealth;
            }
        }

        public int Energy
        {
            get { return this.energy; }
            set { this.energy = value; }
        }

        public int Age
        {
            get { return this.age; }
        }
    }

    public class ItemAttributes : BaseAttributes
    {
        private ItemType itemType;

        //item specific fields
        private float itemWeight;
        //for weapons
        private int baseDamage;
        private int speed;
        private int accuracy;
        private string randomElement;   //in format 2d5, 1d3, 3d12, etc.

        /// <summary>
        /// ItemAttribute constructor for armor and jewellery items.
        /// </summary>
        public ItemAttributes(ItemType itemType, float weight, int str = 0, int dex = 0, int con = 0, int wis = 0, int spi = 0, int luck = 0)
            : base(str, dex, con, wis, spi, luck)
        {
            if (itemType.BaseType == BaseType.Armor ||
                itemType.BaseType == BaseType.Jewellery)
            {
                this.itemType = itemType;
                this.itemWeight = weight;
            }
            else
                throw new ArgumentException("Used constructor is for items of BaseType Armor and Jewellery only!");
        }

        /// <summary>
        /// ItemAttribute constructor for weapons.
        /// </summary>
        public ItemAttributes(ItemType itemType, float weight, int baseDamage, string randomElement, int speed, int accuracy, 
            int str = 0, int dex = 0, int con = 0, int wis = 0, int spi = 0, int luck = 0)
            : base(str, dex, con, wis, spi, luck)
        {
            if (itemType.BaseType == BaseType.Weapon)
            {
                this.itemType = itemType;
                this.itemWeight = weight;
                this.baseDamage = baseDamage;
                this.randomElement = randomElement;
                this.speed = speed;
                this.accuracy = accuracy;
            }
            else
                throw new ArgumentException("Used constructor is for items of BaseType Weapon only!");
        }

        public ItemType ItemType
        {
            get { return this.itemType; }
        }

        public int BaseDamage
        {
            get { return this.baseDamage; }
        }

        public string RandomElement
        {
            get { return this.randomElement; }
        }

        public float Weight
        {
            get { return this.itemWeight; }
        }

        public int Speed
        {
            get { return this.speed; }
        }

        public int Accuracy
        {
            get { return this.accuracy; }
        }
    }
}
