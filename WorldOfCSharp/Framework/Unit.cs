using System;
using System.Collections.Generic;

namespace WorldOfCSharp
{
    public class Unit : GameObject
    {
        private int unitSpeed;
        private int energy = 50;
        private int uniqueID;
        private UnitStats unitStats;
        private static List<int> listOfUnitIDs = new List<int>();
        private static MT19937.MersenneTwister mt = new MT19937.MersenneTwister();
        private Equipment equipment;
        private Inventory inventory;
        private int currentHP = 0;

        public Unit(int x, int y, int flags, int unitSpeed, char visualChar, ConsoleColor color, string name)
            : base(x, y, flags, visualChar, color, name)
        {
            this.unitSpeed = unitSpeed;
            this.uniqueID = UniqueIDGenerator();
            this.unitStats = new UnitStats(1, 1, 1, 1, 1);
            this.equipment = new Equipment();
            this.inventory = new Inventory(this);
            this.currentHP = this.unitStats.HitPoints;
        }

        public Unit(int x, int y, int flags, int unitSpeed, char visualChar, ConsoleColor color, string name, UnitStats unitStats)
            : base(x, y, flags, visualChar, color, name)
        {
            this.unitStats = unitStats;
        }

        public Unit(Unit unit)
            : this(unit.X, unit.Y, unit.Flags, unit.unitSpeed, unit.VisualChar, unit.Color, unit.Name)
        { }

        #region Unit Properties

        public int UniqueID
        {
            get { return this.uniqueID; }
        }

        public int Energy
        {
            get { return this.energy; }
            set { this.energy = value; }
        }

        public int Speed
        {
            get { return this.unitSpeed; }
            set { this.unitSpeed = value; }
        }

        public Equipment Equipment
        {
            get { return this.equipment; }
            set { this.equipment = value; }
        }
                
        public int Strength
        {
            get { return this.unitStats.Strength; }
            set { this.unitStats.Strength = value; }
        }

        public int Dexterity
        {
            get { return this.unitStats.Dexterity; }
            set { this.unitStats.Dexterity = value; }
        }

        public int Stamina
        {
            get { return this.unitStats.Stamina; }
            set { this.unitStats.Stamina = value; }
        }

        public int Intelligence
        {
            get { return this.unitStats.Intelligence; }
            set { this.unitStats.Intelligence = value; }
        }

        public int Spirit
        {
            get { return this.unitStats.Spirit; }
            set { this.unitStats.Spirit = value; }
        }

        public int HitPoints
        {
            get { return this.unitStats.HitPoints; }
            set 
            {
                if (value < this.unitStats.HitPoints)
                {
                    this.currentHP = value;
                    GameEngine.MessageLog.SendMessage(string.Format("You are missing {0} HP.", this.unitStats.HitPoints - this.CurrentHP));
                }
                else if (value > this.unitStats.HitPoints)
                {
                    GameEngine.MessageLog.SendMessage(string.Format("You recovered {0} HP.", value - this.unitStats.HitPoints));
                }
                this.unitStats.HitPoints = value;
            }
        }

        public int CurrentHP
        {
            get 
            {
                if (this.currentHP > this.HitPoints)
                    this.currentHP = this.HitPoints;
                return this.currentHP;
            }
            private set { }
        }

        public int HPPerFive
        {
            get { return this.unitStats.HPPerFive; }
        }

        public int Accuracy
        {
            get { return this.unitStats.Accuracy; }
            set { this.unitStats.Accuracy = value; }
        }

        public Inventory Inventory
        {
            get { return this.inventory; }
            set { this.inventory = value; }
        }
        #endregion
        
        public void SumStats()
        {
        }

        public void DeductStats(Item item)
        {
        }
        
        public string ItemInSlot(EquipSlot slot)
        {
            return string.Format("Slot: {0}, equipped {1}.", Enum.GetName(typeof(EquipSlot), slot), this.equipment[(int)slot].ToString());
        }

        public void EffectsPerFive()
        {
            //HP regen
            if (this.currentHP + this.unitStats.HPPerFive < this.HitPoints)
                this.currentHP += this.unitStats.HPPerFive;
            else this.currentHP = this.HitPoints;
        }

        internal protected void MakeAMove(Direction direction)
        {
            if (this.GetFlag(0))
            {
                switch (direction)
                {
                    case Direction.North:
                        if ((this.Y - 1) >= 0)
                        {
                            if (CheckIfLegalMove(this.X, this.Y - 1))
                            {
                                GameEngine.VisualEngine.ClearGameObject(this);
                                this.Y--;
                                GameEngine.VisualEngine.PrintUnit(this);
                            }
                            else
                            {
                                if (this.VisualChar == '@')
                                {
                                    GameEngine.CheckForEffect(this, this.X, this.Y - 1);
                                }
                            }
                        }
                        break;

                    case Direction.South:
                        if (this.Y + 1 < Globals.GAME_FIELD_BOTTOM_RIGHT.Y)
                        {
                            if (CheckIfLegalMove(this.X, this.Y + 1))
                            {
                                GameEngine.VisualEngine.ClearGameObject(this);
                                this.Y++;
                                GameEngine.VisualEngine.PrintUnit(this);
                            }
                            else
                            {
                                if (this.VisualChar == '@')
                                {
                                    GameEngine.CheckForEffect(this, this.X, this.Y + 1);
                                }
                            }
                        }
                        break;

                    case Direction.West:
                        if (this.X - 1 >= 0)
                        {
                            if (CheckIfLegalMove(this.X - 1, this.Y))
                            {
                                GameEngine.VisualEngine.ClearGameObject(this);
                                this.X--;
                                GameEngine.VisualEngine.PrintUnit(this);
                            }
                            else
                            {
                                if (GameEngine.GameField[this.X - 1, this.Y].Unit != null)
                                {
                                    
                                }
                                if (this.VisualChar == '@') //keeping this just for savepoint & testing, make game save every X seconds, and remove this
                                {
                                    GameEngine.CheckForEffect(this, this.X - 1, this.Y);
                                }
                            }
                        }
                        break;

                    case Direction.East:
                        if (this.X + 1 < Globals.GAME_FIELD_BOTTOM_RIGHT.X)
                        {
                            if (CheckIfLegalMove(this.X + 1, this.Y))
                            {
                                GameEngine.VisualEngine.ClearGameObject(this);
                                this.X++;
                                GameEngine.VisualEngine.PrintUnit(this);
                            }
                            else
                            {
                                if (this.VisualChar == '@')
                                {
                                    GameEngine.CheckForEffect(this, this.X + 1, this.Y);
                                }
                            }
                        }
                        break;

                    case Direction.NorthWest:
                        if ((this.Y - 1) >= 0 && this.X - 1 >= 0)
                        {
                            if (CheckIfLegalMove(this.X - 1, this.Y - 1))
                            {
                                GameEngine.VisualEngine.ClearGameObject(this);
                                this.Y--;
                                this.X--;
                                GameEngine.VisualEngine.PrintUnit(this);
                            }
                            else
                            {
                                if (this.VisualChar == '@')
                                {
                                    GameEngine.CheckForEffect(this, this.X - 1, this.Y - 1);
                                }
                            }
                        }
                        break;

                    case Direction.NorthEast:
                        if ((this.Y - 1) >= 0 && this.X + 1 < Globals.GAME_FIELD_BOTTOM_RIGHT.X)
                        {
                            if (CheckIfLegalMove(this.X + 1, this.Y - 1))
                            {
                                GameEngine.VisualEngine.ClearGameObject(this);
                                this.Y--;
                                this.X++;
                                GameEngine.VisualEngine.PrintUnit(this);
                            }
                            else
                            {
                                if (this.VisualChar == '@')
                                {
                                    GameEngine.CheckForEffect(this, this.X + 1, this.Y - 1);
                                }
                            }
                        }
                        break;

                    case Direction.SouthEast:
                        if (this.Y + 1 < Globals.GAME_FIELD_BOTTOM_RIGHT.Y && this.X + 1 < Globals.GAME_FIELD_BOTTOM_RIGHT.X)
                        {
                            if (CheckIfLegalMove(this.X + 1, this.Y + 1))
                            {
                                GameEngine.VisualEngine.ClearGameObject(this);
                                this.Y++;
                                this.X++;
                                GameEngine.VisualEngine.PrintUnit(this);
                            }
                            else
                            {
                                if (this.VisualChar == '@')
                                {
                                    GameEngine.CheckForEffect(this, this.X + 1, this.Y + 1);
                                }
                            }
                        }
                        break;

                    case Direction.SouthWest:
                        if (this.Y + 1 < Globals.GAME_FIELD_BOTTOM_RIGHT.Y && this.X - 1 >= 0)
                        {
                            if (CheckIfLegalMove(this.X - 1, this.Y + 1))
                            {
                                GameEngine.VisualEngine.ClearGameObject(this);
                                this.Y++;
                                this.X--;
                                GameEngine.VisualEngine.PrintUnit(this);
                            }
                            else
                            {
                                if (this.VisualChar == '@')
                                {
                                    GameEngine.CheckForEffect(this, this.X - 1, this.Y + 1);
                                }
                            }
                        }
                        break;

                    default:
                        break;
                }
            }
        }
        
        private int Attack()
        {
            //make checks (accuracy, critical, etc.)
            return RNG.Roll(this.Equipment[(int)EquipSlot.MainHand].ItemStats.NumberOfDies, 
                this.Equipment[(int)EquipSlot.MainHand].ItemStats.SidesPerDie);
        }

        private int UniqueIDGenerator()
        {
            int id;
            do
            {
                id = mt.Next(10000000, int.MaxValue);
                ConsoleTools.QuickSort(listOfUnitIDs, 0, listOfUnitIDs.Count - 1);
                if (listOfUnitIDs.BinarySearch(id) < 0)
                {
                    listOfUnitIDs.Add(id);
                    return id;
                }
            } while (true);
        }

        private bool CheckIfLegalMove(int x, int y)
        {
            bool unit = false;

            if (!GameEngine.GameField[x, y].Terrain.GetFlag(1))
            {
                if (GameEngine.GameField[x, y].Unit != null)
                {
                    if (!GameEngine.GameField[x, y].Unit.GetFlag(1))
                    {
                        unit = true;
                    }
                }
                else unit = true;

                bool ingObj = false;

                if (GameEngine.GameField[x, y].IngameObject != null)
                {
                    if (!GameEngine.GameField[x, y].IngameObject.GetFlag(1))
                    {
                        ingObj = true;
                    }
                }
                else ingObj = true;

                return unit && ingObj;
            }
            return false;
        }
    }
}
