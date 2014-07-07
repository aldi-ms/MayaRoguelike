using System.Collections.Generic;

namespace Maya
{
    public class Equipment
    {
        public const int ITEM_SLOTS = 16;
        private int count = 0;
        private bool inventoryReWriteProt = true;
        private bool hasChanged;
        private bool[] isSlotUsed;
        private Item[] equipment;
        private Inventory inventoryConnected;
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

        public void EquipItem(Item item)
        {
            if (item != null && item.Slot != EquipSlot.NotEquippable)
            {
                this.inventoryConnected.DropItem(item);

                //if there is already equipped item, put it in the inventory
                if (this.equipment[(int)item.Slot] != null)
                {
                    //amulets, rings and one-handed weapons have 2 slots in the equipment, 
                    //so check if the item slot is full and switch to the other if it's empty
                    if (item.ItemType.BaseType == BaseType.Jewellery || item.ItemType.BaseType == BaseType.Weapon)
                    {
                        if ((JewelleryType)item.ItemType.SubType == JewelleryType.Amulet)
                            if (this.equipment[(int)EquipSlot.AmuletA] == null || this.equipment[(int)EquipSlot.AmuletB] == null)
                                item.ItemType.SwitchSlot();
                            else
                                this.Unequip(this.equipment[(int)item.Slot]);

                        if ((JewelleryType)item.ItemType.SubType == JewelleryType.Ring)
                            if (this.equipment[(int)EquipSlot.FingerA] == null || this.equipment[(int)EquipSlot.FingerB] == null)
                                item.ItemType.SwitchSlot();
                            else
                                this.Unequip(this.equipment[(int)item.Slot]);

                        if ((WeaponType)item.ItemType.SubType == WeaponType.OneHandedAxes ||
                            (WeaponType)item.ItemType.SubType == WeaponType.OneHandedMaces ||
                            (WeaponType)item.ItemType.SubType == WeaponType.OneHandedSwords)
                            if (this.equipment[(int)EquipSlot.MainHand] == null || this.equipment[(int)EquipSlot.OffHand] == null)
                                item.ItemType.SwitchSlot();
                            else
                                this.Unequip(this.equipment[(int)item.Slot]);
                    }
                    else
                        this.Unequip(this.equipment[(int)item.Slot]);
                }

                this.equipment[(int)item.Slot] = item;
                this.equipment[(int)item.Slot].isEquipped = true;
                this.isSlotUsed[(int)item.Slot] = true;
                this.Owner.AddAttributes(item);
                this.count++;
                this.hasChanged = true;
            }
        }

        /// <summary>
        /// Removes an item from the equipment and puts it in the inventory.
        /// </summary>
        /// <param name="item">The item specified.</param>
        public void Unequip(Item item)
        {
            this.equipment[(int)item.Slot] = null;
            this.isSlotUsed[(int)item.Slot] = false;
            this.count--;
            this.hasChanged = true;
            this.Owner.RemoveAttributes(item);

            item.isEquipped = false;
            this.inventoryConnected.StoreItem(item);
        }
    }
}
