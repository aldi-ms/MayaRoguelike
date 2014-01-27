using System;
using System.Collections.Generic;

namespace WorldOfCSharp
{
    public struct StatStruct
    {
        private int stat;
        private string statName;

        public StatStruct(int stat, string statName)
        {
            this.stat = stat;
            this.statName = statName;
        }

        public int Stat
        {
            get { return this.stat; }
            set { this.stat = value; }
        }

        public string StatShortName
        {
            get { return this.statName; }
            set { this.statName = value; }
        }
    }

    public class UnitStats
    {
        public static readonly UnitStats MIN_STATS = new UnitStats(1, 1, 1, 1, 1);
        //RPG stats
        private int strength;
        private int dexterity;
        private int stamina;
        private int intelligence;
        private int spirit;
        private int currentHP;
        private int actionSpeed = 10;
        //derived rpg stats
        private int maxHP;
        private int HP5;
        private int accuracy;
        private int energy = 50;
        
        public UnitStats(int strength = 1, int dexterity = 1, int stamina = 1, int intelligence = 1, int spirit = 1)
        {
            this.strength = strength;
            this.dexterity = dexterity;
            this.stamina = stamina;
            this.intelligence = intelligence;
            this.spirit = spirit;
            this.CalcDerivedStats();
            this.currentHP = this.HitPoints;
        }

        #region StatProperties
        //put limitations to who can touch the stats!
        public int Strength
        {
            get { return this.strength; }
            set
            {
                this.strength = value;
                this.CalcDerivedStats();
            }
        }

        public int Dexterity
        {
            get { return this.dexterity; }
            set
            {
                this.dexterity = value;
                this.CalcDerivedStats();
            }
        }

        public int Stamina
        {
            get { return this.stamina; }
            set
            {
                this.stamina = value;
                this.CalcDerivedStats();
            }
        }

        public int Intelligence
        {
            get { return this.intelligence; }
            set
            {
                this.intelligence = value;
                this.CalcDerivedStats();
            }
        }

        public int Spirit
        {
            get { return this.spirit; }
            set
            {
                this.spirit = value;
                this.CalcDerivedStats();
            }
        }

        public int HitPoints
        {
            get { return this.maxHP; }
            set { this.maxHP = value; }
        }

        public int Accuracy
        {
            get { return this.accuracy; }
            set { this.accuracy = value; }
        }

        public int HPPerFive
        {
            get { return this.HP5; }
        }

        public int CurrentHP
        {
            get { return this.currentHP; }
            set
            {
                if (value <= this.HitPoints)
                    this.currentHP = value;
                else
                    this.currentHP = this.HitPoints;
            }
        }

        public int ActionSpeed
        {
            get { return this.actionSpeed; }
            set { this.actionSpeed = value; }
        }

        public int Energy
        {
            get { return this.energy; }
            set { this.energy = value; }
        }
        #endregion

        private void CalcDerivedStats()
        {
            this.maxHP = (int)(stamina * 10 + (strength + dexterity + intelligence + spirit) * 0.2);
            this.HP5 = (int)((stamina * 10 + (strength + dexterity + intelligence)) * 0.05 + spirit * 2);
        }

        public override string ToString()
        {
            return string.Format("{0};{1};{2};{3};{4};{5};{6}", strength, dexterity, stamina, intelligence, spirit, currentHP, actionSpeed);
        }
    }

    public class ItemStats
    {
        //core rpg stats
        private int strength;
        private int dexterity;
        private int stamina;
        private int intelligence;
        private int spirit;
        private int hitPoints;
        private List<StatStruct> activeStats = new List<StatStruct>();
        //weapon fields
        private int numberOfDies;
        private int sidesPerDie;
        private int speed;
        private int accuracy;

        public ItemStats(int strength = 0, int dexterity = 0, int stamina = 0, int intelligence = 0, int spirit = 0, int hitPoints = 0)
        {
            this.strength = strength;
            if (this.strength != 0)
                activeStats.Add(new StatStruct(this.strength, "str"));

            this.dexterity = dexterity;
            if (this.dexterity != 0)
                activeStats.Add(new StatStruct(this.dexterity, "dex"));

            this.stamina = stamina;
            if (this.stamina != 0)
                activeStats.Add(new StatStruct(this.stamina, "sta"));

            this.intelligence = intelligence;
            if (this.intelligence != 0)
                activeStats.Add(new StatStruct(this.intelligence, "int"));

            this.spirit = spirit;
            if (this.spirit != 0)
                activeStats.Add(new StatStruct(this.spirit, "spi"));
        }

        public ItemStats(int numberOfDies, int sidesPerDie, int speed, int accuracy, 
            int strength = 0, int dexterity = 0, int stamina = 0, int intelligence = 0, int spirit = 0, int hitPoints = 0)
        {
            this.numberOfDies = numberOfDies;
            this.sidesPerDie = sidesPerDie;
            this.speed = speed;
            this.accuracy = accuracy;

            this.strength = strength;
            if (this.strength != 0)
                activeStats.Add(new StatStruct(this.strength, "str"));

            this.dexterity = dexterity;
            if (this.dexterity != 0)
                activeStats.Add(new StatStruct(this.dexterity, "dex"));

            this.stamina = stamina;
            if (this.stamina != 0)
                activeStats.Add(new StatStruct(this.stamina, "sta"));

            this.intelligence = intelligence;
            if (this.intelligence != 0)
                activeStats.Add(new StatStruct(this.intelligence, "int"));

            this.spirit = spirit;
            if (this.spirit != 0)
                activeStats.Add(new StatStruct(this.spirit, "spi"));

            this.hitPoints = hitPoints;
            if (this.hitPoints != 0)
                activeStats.Add(new StatStruct(this.hitPoints, "hp"));
        }

        public int Strength
        {
            get { return (int)this.strength; }
            private set { this.strength = value; }
        }

        public int Dexterity
        {
            get { return (int)this.dexterity; }
            private set { this.dexterity = value; }
        }

        public int Stamina
        {
            get { return (int)this.stamina; }
            private set { this.stamina = value; }
        }

        public int Intelligence
        {
            get { return (int)this.intelligence; }
            private set{ this.intelligence = value; }
        }

        public int Spirit
        {
            get { return (int)this.spirit; }
            private set { this.spirit = value; }
        }

        public int HitPoints
        {
            get { return (int)this.hitPoints; }
            private set { this.hitPoints = value; }
        }

        public int NumberOfDies
        {
            get { return this.numberOfDies; }
            private set { this.numberOfDies = value; }
        }

        public int SidesPerDie
        {
            get { return this.sidesPerDie; }
            private set { this.sidesPerDie = value; }
        }

        public int Speed
        {
            get { return this.speed; }
            private set { this.speed = value;  }
        }

        public int Accuracy
        {
            get { return this.accuracy; }
            private set { this.accuracy = value; }
        }

        public List<StatStruct> ActiveStats
        {
            get { return activeStats; }
        }
    }
}
