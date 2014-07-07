using System;
using System.Collections.Generic;

namespace Maya
{
    public class Unit : GameObject
    {
        private static List<int> listOfUnitIDs = new List<int>();
        private static MT19937.MersenneTwister mt = new MT19937.MersenneTwister();
        private UnitAttributes unitAttr;
        private Equipment equipment;
        private Inventory inventory;
        private int uniqueID = 0;

        public Unit(int x, int y, Flags flags, char visualChar, ConsoleColor color, string name)
            : base(x, y, flags, visualChar, color, name)
        {
            if (uniqueID == 0)
                this.uniqueID = UniqueIDGenerator();
            this.unitAttr = new UnitAttributes(18);
            this.inventory = new Inventory(this.equipment = new Equipment(this));
            this.equipment.InventoryConnected = this.inventory;
        }

        public Unit(int x, int y, Flags flags, char visualChar, ConsoleColor color, string name, int ID, UnitAttributes unitAttr)
            : this(x, y, flags, visualChar, color, name)
        {
            this.uniqueID = ID;
            this.unitAttr = unitAttr;
        }

        public Unit(Unit unit)
            : this(unit.X, unit.Y, unit.Flags, unit.VisualChar, unit.Color, unit.Name)
        { }

        public int UniqueID
        {
            get { return this.uniqueID; }
        }

        public Equipment Equipment
        {
            get { return this.equipment; }
            set { this.equipment = value; }
        }

        public Inventory Inventory
        {
            get { return this.inventory; }
            set { this.inventory = value; }
        }

        public UnitAttributes UnitAttr
        {
            get { return unitAttr; }
            set { this.unitAttr = value; }
        }
        
        public string ItemInSlot(EquipSlot slot)
        {
            return string.Format("Slot: {0}, equipped {1}.", Enum.GetName(typeof(EquipSlot), slot), this.equipment[(int)slot].ToString());
        }

        public void EffectsPerFive()
        {
            //HP regeneration
            if (this.UnitAttr.CurrentHealth + this.UnitAttr.HealthRegen < this.UnitAttr.MaxHealth)
                this.UnitAttr.CurrentHealth += this.UnitAttr.HealthRegen;
            else
                this.UnitAttr.CurrentHealth = this.UnitAttr.MaxHealth;
        }

        internal protected void MakeAMove(CardinalDirection direction)
        {
            if (this.Flags.HasFlag(Flags.IsMovable))
            {
                int deltaX = 0;
                int deltaY = 0;

                switch (direction)
                {
                    case CardinalDirection.North:
                        deltaY = -1;
                        break;

                    case CardinalDirection.South:
                        deltaY = 1;
                        break;

                    case CardinalDirection.West:
                        deltaX = -1;
                        break;

                    case CardinalDirection.East:
                        deltaX = 1;
                        break;

                    case CardinalDirection.NorthWest:
                        deltaX = -1;
                        deltaY = -1;
                        break;

                    case CardinalDirection.NorthEast:
                        deltaX = 1;
                        deltaY = -1;
                        break;

                    case CardinalDirection.SouthEast:
                        deltaX = 1;
                        deltaY = 1;
                        break;

                    case CardinalDirection.SouthWest:
                        deltaX = -1;
                        deltaY = 1;
                        break;

                    default:
                        break;
                }

                if (IsALegalMove(this.X + deltaX, this.Y + deltaY))
                {
                    GameEngine.VisualEngine.ClearGameObject(this);
                    this.X += deltaX;
                    this.Y += deltaY;
                    GameEngine.VisualEngine.PrintUnit(this);
                }
                else
                    if (!(this.X + deltaX < 0 || this.X + deltaX >= Globals.GAME_FIELD_BOTTOM_RIGHT.X ||
                        this.Y + deltaY < 0 || this.Y + deltaY >= Globals.GAME_FIELD_BOTTOM_RIGHT.Y))
                        GameEngine.CheckForEffect(this, this.X + deltaX, this.Y + deltaY);
            }
        }

        private int UniqueIDGenerator()
        {
            do
            {
                int id = mt.Next(10000000, int.MaxValue);
                ConsoleTools.QuickSort(listOfUnitIDs, 0, listOfUnitIDs.Count - 1);
                if (listOfUnitIDs.BinarySearch(id) < 0)
                {
                    listOfUnitIDs.Add(id);
                    return id;
                }
            } while (true);
        }

        private bool IsALegalMove(int x, int y)
        {
            if (x < 0 || x >= Globals.GAME_FIELD_BOTTOM_RIGHT.X 
                || y < 0 || y >= Globals.GAME_FIELD_BOTTOM_RIGHT.Y)
                return false;

            bool unit = false;
            if (!GameEngine.GameField[x, y].Terrain.Flags.HasFlag(Flags.IsCollidable))
            {
                if (GameEngine.GameField[x, y].Unit != null)
                {
                    if (!GameEngine.GameField[x, y].Unit.Flags.HasFlag(Flags.IsCollidable))
                        unit = true;
                }
                else unit = true;

                bool ingObj = false;
                if (GameEngine.GameField[x, y].IngameObject != null)
                {
                    if (!GameEngine.GameField[x, y].IngameObject.Flags.HasFlag(Flags.IsCollidable))
                        ingObj = true;
                }
                else ingObj = true;

                return unit && ingObj;
            }

            return false;
        }

        public void AddAttributes(Item item)
        {
            for (int i = 0; i < BaseAttributes.Count; i++)
                UnitAttr[i] += item.ItemAttr[i];
        }

        public void RemoveAttributes(Item item)
        {
            for (int i = 0; i < BaseAttributes.Count; i++)
                UnitAttr[i] -= item.ItemAttr[i];
        }
    }
}