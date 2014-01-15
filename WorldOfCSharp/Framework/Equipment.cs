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

        public Equipment()
        {
            this.equipment = new Item[ITEM_SLOTS];
            this.isSlotUsed = new bool[ITEM_SLOTS];
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

        public void EquipItem(Item item)
        {
            if (item.Slot != EquipSlot.NotEquippable && item != null)
            {
                this.equipment[(int)item.Slot] = item;
                this.isSlotUsed[(int)item.Slot] = true;
                this.count++;
                this.hasChanged = true;
            }
        }

        public Item Unequip(Item item)
        {
            this.equipment[(int)item.Slot] = null;
            this.isSlotUsed[(int)item.Slot] = false;
            this.count--;
            return item;
        }

        public List<string> ToStringListDEPRECATED()
        {
            List<string> strList = new List<string>();

            for (int i = 0; i < equipment.Length; i++)
            {
                if (equipment[i].Slot != EquipSlot.NotEquippable)
                {
                    strList.Add(string.Format("{0}", equipment[i].ToString()));
                }
            }

            return strList;
        }
    }
}
