using System;
using System.Collections.Generic;

namespace Maya
{
    public class Item
    {
        private static int lastItemID;
        private static bool isIDSet = false;
        private static List<Item> itemsGenerated = new List<Item>();
        private string name;
        private ItemAttributes itemAttr;
        private int inventorySlot;
        private int id;
        public bool isEquipped = false;

        /// <summary>
        /// Universal Item constructor.
        /// </summary>
        /// <param name="attributes">An object of type ItemAttributes.</param>
        public Item(string name, ItemAttributes attributes)
        {
            this.name = name;
            this.itemAttr = attributes;
            this.inventorySlot = -1;    //default value -1 for not in inventory.

            int id = Database.SearchItemDB(this.name);
            if (id == -1)
                this.id = ++lastItemID;
            else
                this.id = id;

            itemsGenerated.Add(this);
        }

        public Item(string name, ItemAttributes attributes, int id)
        {
            this.name = name;
            this.itemAttr = attributes;
            this.inventorySlot = -1;    //default value -1 for not in inventory.
            this.id = id;
        }

        public Item(Item itemFromDB)
            : this(itemFromDB.Name, itemFromDB.ItemAttr, itemFromDB.ID)
        { }

        public static int LastItemID
        {
            get { return lastItemID; }
            set
            {
                if (!isIDSet)
                {
                    isIDSet = true;
                    lastItemID = value;
                }
            }
        }

        public static List<Item> ItemsGenerated
        {
            get { return itemsGenerated; }
            private set { }
        }

        public int ID
        {
            get { return this.id; }
        }

        public ItemType ItemType
        {
            get { return this.itemAttr.ItemType; }
        }

        public ItemAttributes ItemAttr
        {
            get { return this.itemAttr; }
            set { this.itemAttr = value; }
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

        public string Name
        {
            get { return this.name; }
        }
        
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendFormat("[{0}", this.Name);

            if (ItemAttr.ItemType.BaseType == BaseType.Weapon)
                sb.AppendFormat(", {0} dmg(\u00b1{1})", this.ItemAttr.BaseDamage, this.ItemAttr.RandomElement);

            for (int i = 0; i < BaseAttributes.Count; i++)
                if (ItemAttr[i] != 0)
                    sb.AppendFormat(", {0}{1} {2}", ItemAttr[i] > 0 ? "+" : string.Empty, ItemAttr[i], ItemAttr[i, i]);

            sb.Append("]");
            return sb.ToString();
        }
    }
}
