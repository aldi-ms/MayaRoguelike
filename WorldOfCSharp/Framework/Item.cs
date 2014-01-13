using System;

namespace WorldOfCSharp
{
    public class Item
    {
        private string name;
        private ItemType itemType = new ItemType();
        private ItemStats itemStats;
        private Unit owner;
        private bool isEquipped = false;
        private bool isStored = false;
        private int inventorySlot;
        //weapon fields

        public Item(string name, ItemType itemType, int strength = 0, int dexterity = 0, int stamina = 0, int intelligence = 0, int spirit = 0)
        {
            this.name = name;
            if (itemType.Slot == EquipSlot.MainHand || itemType.Slot == EquipSlot.OffHand)
                throw new ArgumentException("Use the weapon constructor for items of type Weapon!");
            this.itemType = itemType;
            this.itemStats = new ItemStats(strength: strength, dexterity: dexterity, stamina: stamina, intelligence: intelligence, spirit: spirit);
        }

        public Item(string name, ItemType itemType, ItemStats itemStats)
        {
            if (itemType.Slot == EquipSlot.MainHand || itemType.Slot == EquipSlot.OffHand)
                throw new ArgumentException("Use the weapon constructor for items of type Weapon!");
            this.name = name;
            this.itemType = itemType;
            this.itemStats = itemStats;
        }

        //weapon constructor
        public Item(string name, ItemType itemType, int numberOfDies, int sidesPerDie, int speed, int accuracy, 
            int strength = 0, int dexterity = 0, int stamina = 0, int intelligence = 0, int spirit = 0)
        {
            this.name = name; 
            this.itemType = itemType;
            if (itemType.Slot != EquipSlot.MainHand && itemType.Slot != EquipSlot.OffHand)
                throw new ArgumentException("Use the normal constructor for items which are not of type Weapon!");

            this.itemStats = new ItemStats(numberOfDies, sidesPerDie, speed, accuracy, strength: strength, 
                dexterity: dexterity, stamina: stamina, intelligence: intelligence, spirit: spirit);
        }

        public Item()
            : this("unknown item", new ItemType())
        { }

        public ItemType ItemType
        {
            get { return this.itemType; }
        }

        public ItemStats ItemStats
        {
            get { return this.itemStats; }
            set { this.itemStats = value; }
        }

        public EquipSlot Slot
        {
            get { return this.itemType.Slot; }
        }

        public int InventorySlot
        {
            get { return this.inventorySlot; }
        }

        //public void Actions()
        //{
        //    if (isEquipped)
        //    {
        //        GameEngine.MessageLog.SendMessage(string.Format("{0} -- [T] Take off, [D] Drop", this.ToFullString()));

        //        ConsoleKeyInfo key = Console.ReadKey(true);
        //        switch (key.Key)
        //        {
        //            case ConsoleKey.D:
        //                //this.TakeOff();
        //                this.Drop();
        //                break;
        //            case ConsoleKey.T:
        //                this.TakeOff();
        //                break;
        //            default:
        //                GameEngine.MessageLog.SendMessage("No action taken.");
        //                break;
        //        }
        //    }
        //    else
        //    {
        //        GameEngine.MessageLog.SendMessage(string.Format("{0} -- [E] Equip, [D] Drop", this.ToFullString()));

        //        ConsoleKeyInfo key = Console.ReadKey(true);
        //        switch (key.Key)
        //        {
        //            case ConsoleKey.D:
        //                this.Drop();
        //                break;
        //            case ConsoleKey.E:
        //                this.Equip(this.owner);
        //                break;
        //            default:
        //                GameEngine.MessageLog.SendMessage("No action taken.");
        //                break;
        //        }
        //    }
        //}
        
        public override string ToString()
        {
            return string.Format("[{0}]", this.name);
        }
         
        public string ToFullString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("{0}: [{1}", this.Slot, this.name);

            if (this.ItemStats.ActiveStats.Count > 0)
                sb.Append(" (");
            foreach (var stat in this.ItemStats.ActiveStats)
            {
                sb.AppendFormat("+{0}{1};", stat.Stat, stat.StatShortName);
            }
            if (this.ItemStats.ActiveStats.Count > 0)
                sb.Append(")");

            sb.Append("]");

            return sb.ToString();
        }
    }
}
