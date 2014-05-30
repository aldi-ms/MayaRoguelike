using System.Collections.Generic;

namespace Maya
{
    public class Inventory
    {
        private const int BASE_BAG_SLOTS = 20;
        private readonly Equipment equipmentConnected;
        private int count = 0;
        private bool[] isSlotUsed = new bool[BASE_BAG_SLOTS];
        private Item[] inventory = new Item[BASE_BAG_SLOTS];
        private Unit owner;
        //other bag slots to be implemented

        public Inventory(Equipment equipmentToConnect)
        {
            this.equipmentConnected = equipmentToConnect;
            for (int i = 0; i < BASE_BAG_SLOTS; i++)
            {
                this.inventory[i] = null;
                this.isSlotUsed[i] = false;
            }
            this.owner = equipmentConnected.Owner;
        }

        public Item this[int index]
        {
            get { return this.inventory[index]; }
        }

        public int Count
        {
            get { return this.count; }
        }

        public bool[] IsSlotUsed
        {
            get { return this.isSlotUsed; }
        }

        public void StoreItem(Item item)
        {
            for (int i = 0; i < this.inventory.Length; i++)
            {
                if (!this.isSlotUsed[i])
                {
                    this.inventory[i] = item;
                    this[i].InventorySlot = i;
                    this.isSlotUsed[i] = true;
                    this.count++;

                    break;
                }
            }

            //Sort();
        }

        public Item DropItem(Item item)
        {
            if (this.isSlotUsed[item.InventorySlot])
            {
                this.inventory[item.InventorySlot] = null;
                this.isSlotUsed[item.InventorySlot] = false;
                this.count--;

                item.InventorySlot = -1;

                return item;
            }
            //else throw ex.
            return null;
        }

        //private void Sort()
        //{
        //    int[] intArr = new int[this.inventory.Length];

        //    for (int i = 0; i < this.inventory.Length; i++)
        //    {
        //        intArr[i] = this[i].ItemType.ItemCodeToInt;
        //    }

        //    for (int j = 0; j < intArr.Length; j++)
        //    {
        //        int key = intArr[j];
        //        Item swapItem = inventory[j];

        //        int i = j - 1;

        //        while (i >= 0 && intArr[i] > key)
        //        {
        //            intArr[i + 1] = intArr[i];
        //            inventory[i + 1] = inventory[i];

        //            i = i - 1;
        //        }

        //        intArr[i + 1] = key;
        //        inventory[i + 1] = swapItem;
        //    }
        //}

        //public List<string> ToStringListDEPRECATED()
        //{
        //    List<string> strList = new List<string>();
        //    for (int i = 0; i < this.inventory.Length; i++)
        //    {
        //        if (this.inventory[i].Slot != EquipSlot.NotEquippable)
        //            strList.Add(string.Format("{0}", this.inventory[i].ToString()));
        //    }

        //    return strList;
        //}
    }
}
