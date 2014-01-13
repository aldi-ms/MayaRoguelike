using System.Collections.Generic;

namespace WorldOfCSharp
{
    public class Equipment
    {
        private const int ITEM_SLOTS = 16;
        private bool hasChanged;
        private Item[] equipment;
        private bool[] isSlotUsed;

        public Equipment()
        {
            this.equipment = new Item[ITEM_SLOTS];
            for (int i = 0; i < equipment.Length; i++)
            {
                this.equipment[i] = new Item();
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
        public void EquipItem(Item item)
        {
            if (item.Slot != EquipSlot.NotEquippable && item != null)
            {
                this.equipment[(int)item.Slot] = item;
                this.isSlotUsed[(int)item.Slot] = true;
                this.hasChanged = true;
            }
        }

        public void Unequip(Item item)
        {
            this.equipment[(int)item.Slot] = new Item();
            this.isSlotUsed[(int)item.Slot] = false;
        }

        public List<string> ToStringListDEPRECATED()
        {
            List<string> strList = new List<string>();

            for (int i = 0; i < equipment.Length; i++)
            {
                if (equipment[i].Slot != EquipSlot.NotEquippable)
                {
                    strList.Add(string.Format("{0}", equipment[i].ToFullString()));
                }
            }

            return strList;
        }
    }
}
