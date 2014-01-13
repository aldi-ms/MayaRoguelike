using System.Collections.Generic;

namespace WorldOfCSharp
{
    public class Inventory
    {
        private const int BASE_BAG_SLOTS = 20;
        //private int otherBagSlots = 0; //to be implemented...
        private Item[] inventory = new Item[BASE_BAG_SLOTS];
        private bool[] isSlotUsed = new bool[BASE_BAG_SLOTS];
        private Unit invOwner;

        public Inventory(Unit invOwner)
        {
            this.invOwner = invOwner;
            for (int i = 0; i < BASE_BAG_SLOTS; i++)
            {
                this.inventory[i] = new Item();
                this.isSlotUsed[i] = false;
            }
        }

        public Item this[int index]
        {
            get { return this.inventory[index]; }
        }

        public bool[] IsSlotUsed
        {
            get { return this.isSlotUsed; }
        }

        public void AddItem(Item item)
        {
            for (int i = 0; i < this.inventory.Length; i++)
            {
                if (this.isSlotUsed[i] == false)
                {
                    this.inventory[i] = item;
                    this.isSlotUsed[i] = true;
                }
            }

            Sort();
        }

        public void RemoveItem(Item item)
        {
            if (this.isSlotUsed[item.InventorySlot] == true)
            {
                this.inventory[item.InventorySlot] = new Item();
                this.isSlotUsed[item.InventorySlot] = false;
            }
        }
        
        private void Sort()
        {
            int[] intArr = new int[this.inventory.Length];

            for (int i = 0; i < this.inventory.Length; i++)
            {
                intArr[i] = this[i].ItemType.ItemCodeToInt;
            }

            for (int j = 0; j < intArr.Length; j++)
            {
                int key = intArr[j];
                Item swapItem = inventory[j];

                int i = j - 1;

                while (i >= 0 && intArr[i] > key)
                {
                    intArr[i + 1] = intArr[i];
                    inventory[i + 1] = inventory[i];

                    i = i - 1;
                }

                intArr[i + 1] = key;
                inventory[i + 1] = swapItem;
            }
        }

        public List<string> ToStringListDEPRECATED()
        {
            List<string> strList = new List<string>();
            for (int i = 0; i < this.inventory.Length; i++)
            {
                if (this.inventory[i].Slot != EquipSlot.NotEquippable)
                    strList.Add(string.Format("{0}", this.inventory[i].ToFullString()));
            }

            return strList;
        }
    }
}
