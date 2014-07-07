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

        public void SortInventory()
        {
            int lowerBound = 0; // First position to compare.
            int upperBound = BASE_BAG_SLOTS - 1; // First position NOT to compare.
            int n = upperBound;
            // Continue making passes while there is a potential exchange.
            while (lowerBound <= upperBound)
            {
                // assume impossibly high index for low end.
                int firstExchange = n;
                // assume impossibly low index for high end.
                int lastExchange = -1;
                // Make a pass over the appropriate range.
                for (int i = lowerBound; i < upperBound; i++)
                {
                    if (this.inventory[i].ItemType.BaseType > this.inventory[i + 1].ItemType.BaseType)
                    {
                        // Exchange elements
                        Item temp = this.inventory[i];
                        this.inventory[i] = this.inventory[i + 1];
                        this.inventory[i + 1] = temp;
                        // Remember first and last exchange indexes.
                        if (i < firstExchange)
                        { // True only for first exchange.
                            firstExchange = i;
                        }
                        lastExchange = i;
                    }
                }
                //--- Prepare limits for next pass.
                lowerBound = firstExchange - 1;
                if (lowerBound < 0)
                {
                    lowerBound = 0;
                }
                upperBound = lastExchange;
            }
        }
    }
}
