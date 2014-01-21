using System.Collections.Generic;

namespace WorldOfCSharp
{
    public class Equipment
    {
        public const int ITEM_SLOTS = 16;
        private bool hasChanged;
        private Item[] equipment;
        private bool[] isSlotUsed;
        private int count = 0;
        private Inventory inventoryConnected;
        private bool inventoryReWriteProt = true;
        private Unit unitOwner;

        public Equipment(Unit owner)
        {
            this.equipment = new Item[ITEM_SLOTS];
            this.isSlotUsed = new bool[ITEM_SLOTS];
            this.unitOwner = owner;
            for (int i = 0; i < equipment.Length; i++)
            {
                this.equipment[i] = null;
                this.isSlotUsed[i] = false;
            }
        }

        public Item this[int index]
        {
            get { return this.equipment[index]; }
        }

        public bool[] IsSlotUsed
        {
            get { return this.isSlotUsed; }
        }

        public int Count
        {
            get { return this.count; }
        }

        public Unit Owner
        {
            get { return this.unitOwner; }
        }

        public bool HasChanged
        {
            get
            {
                if (this.hasChanged)
                {
                    this.hasChanged = false;
                    return true;
                }
                return false;
            }
        }

        public Inventory InventoryConnected
        {
            set 
            {
                if (inventoryReWriteProt)
                {
                    this.inventoryConnected = value;
                    inventoryReWriteProt = false;
                }
            }
        }
        //to be fixed to return item if there was one before equipping.
        public void EquipItem(Item item)
        {
            if (item.Slot != EquipSlot.NotEquippable && item != null)
            {
                this.inventoryConnected.DropItem(item);
                
                this.equipment[(int)item.Slot] = item;
                this.equipment[(int)item.Slot].isEquipped = true;
                this.isSlotUsed[(int)item.Slot] = true;
                this.Owner.AddStats(item);
                this.count++;
                this.hasChanged = true;
            }
        }

        public void Unequip(Item item)
        {
            this.equipment[(int)item.Slot] = null;
            this.isSlotUsed[(int)item.Slot] = false;
            this.count--;
            this.hasChanged = true;
            this.Owner.RemoveStats(item);

            item.isEquipped = false;
            this.inventoryConnected.PickUpItem(item);
        }

        //public List<string> ToStringListDEPRECATED()
        //{
        //    List<string> strList = new List<string>();

        //    for (int i = 0; i < equipment.Length; i++)
        //    {
        //        if (equipment[i].Slot != EquipSlot.NotEquippable)
        //        {
        //            strList.Add(string.Format("{0}", equipment[i].ToString()));
        //        }
        //    }

        //    return strList;
        //}        
    }
}
