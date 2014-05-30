using System;

namespace Maya
{
    public class Item
    {
        private string name;
        //private ItemType itemType;
        //private ItemStats_DEPRECATED itemStats;
        private ItemAttributes itemStats;
        private int inventorySlot;
        public bool isEquipped = false;

        public Item(string name, ItemAttributes itemStats)
        {
            this.name = name;
            this.itemStats = itemStats;
            this.inventorySlot = -1;    //default value -1 for not in inventory.
        }
        
        public ItemType ItemType
        {
            get { return this.itemStats.ItemType; }
        }

        public ItemAttributes ItemStats
        {
            get { return this.itemStats; }
            set { this.itemStats = value; }
        }

        public EquipSlot Slot
        {
            get { return this.ItemType.Slot; }
        }

        public int InventorySlot
        {
            get { return this.inventorySlot; }
            set { this.inventorySlot = value; }
        }
                 
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("[{0}", this.name, this.Slot.ToString()); //???????????!!!

            if (ItemStats.ItemType.BaseType == BaseType.Weapon)
                sb.AppendFormat(", {0} dmg(\x00b1{1})", this.ItemStats.BaseDamage, this.ItemStats.RandomElement);

            for (int i = 0; i < BaseAttributes.Count; i++)
                if (ItemStats[i] > 0)
                    sb.AppendFormat(", +{0} {1}", ItemStats[i], ItemStats[i, i]);

            sb.Append("]");
            return sb.ToString();
        }
    }
}
