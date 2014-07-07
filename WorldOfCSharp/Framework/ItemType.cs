using System;

namespace Maya
{
    #region ItemTypeEnums
    public enum ArmorType
    {
        Cloth,
        Leather,
        Mail,
        Plate,
        Shields,
        Librams,
        Idols,
        Totems,
        Sigils,
        Miscellaneous
    }

    public enum WeaponType
    {
        Bows,
        Crossbows,
        Daggers,
        Guns,
        FishingPoles,
        FistWeapons,
        OneHandedAxes,
        OneHandedMaces,
        OneHandedSwords,
        Polearms,
        Staves,
        Thrown,
        TwoHandedAxes,
        TwoHandedMaces,
        TwoHandedSwords,
        Wands,
        Miscellaneous
    }
    
    public enum ConsumableType
    {
        FoodDrink,
        Potion,
        Elixir,
        Flask,
        Bandage,
        ItemEnhancement,
        Scroll,
        Other,
        Consumable
    }

    public enum ProjectileType
    {
        Arrow,
        Bullet
    }

    public enum TradeGoodsType
    {
        ArmorEnchantment,
        Cloth,
        Devices,
        Elemental,
        Enchanting,
        Explosives,
        Herb,
        Jewelcrafting,
        Leather,
        Materials,
        Meat,
        MetalStone,
        Parts,
        WeaponEnchantment,
        TradeGoods,
        Other
    }

    public enum JewelleryType
    {
        Ring,
        Necklace,
        Amulet
    }

    public enum BaseType
    {
        Armor = 0,
        Weapon = 1,
        Consumable = 2,
        Container = 3,
        Gem = 4,
        Key = 5,
        Money = 6,
        Reagent = 7,
        Recipe = 8,
        Projectile = 9,
        QuestPlot = 10,
        Quiver = 11,
        TradeGoods = 12,
        Miscellaneous = 13,
        Jewellery = 14
    }

    public enum EquipSlot
    {
        //armor
        Head = 0,
        Shoulder = 1,
        Chest = 2,
        Back = 3,
        Wrist = 4,
        Hands = 5,
        Waist = 6,
        Legs = 7,
        Feet = 8,   //-->Feet used as pointer for last armor equip slot

        //accessories
        Neck = 9,
        AmuletA = 10,
        AmuletB = 11,
        FingerA = 12,
        FingerB = 13,

        //weapons/ammo (equips in hand)
        MainHand = 14,
        OffHand = 15,
        RangedOrRelic = 16,

        //projectiles
        Ammo = 17,

        NotEquippable = 18
    }
    #endregion

    public class ItemType
    {
        private EquipSlot itemSlot = EquipSlot.NotEquippable;
        private BaseType itemBaseType = BaseType.Miscellaneous;
        private int subItemType;

        public ItemType(BaseType baseItemType, int subItemType)
        {
            this.itemBaseType = baseItemType;
            this.subItemType = subItemType;
        }

        //for item db purposes
        public ItemType(BaseType baseItemType, int subItemType, EquipSlot equipSlot)
        {
            this.itemBaseType = baseItemType;
            this.subItemType = subItemType;
            this.itemSlot = equipSlot;
        }

        public ItemType(ArmorType armorType, EquipSlot itemSlot)
            : this(BaseType.Armor, (int)armorType)
        {
            if (itemSlot <= EquipSlot.Feet)
                this.itemSlot = itemSlot;
            else throw new ArgumentException("Item slot given is not armor!");
        }

        public ItemType(WeaponType weaponType, EquipSlot itemSlot)
            : this(BaseType.Weapon, (int)weaponType)
        {
            if (itemSlot >= EquipSlot.MainHand && itemSlot <= EquipSlot.RangedOrRelic)
                this.itemSlot = itemSlot;
            else throw new ArgumentException("Item slot given is not weapon!");
        }

        public ItemType(JewelleryType jewelleryType, EquipSlot itemSlot)
            : this(BaseType.Jewellery, (int)jewelleryType)
        {
            if (itemSlot >= EquipSlot.Neck && itemSlot <= EquipSlot.FingerB)
                this.itemSlot = itemSlot;
            else throw new ArgumentException("Item slot given is not jewellery!");
        }

        public ItemType(ProjectileType projectileType)
            : this(BaseType.Projectile, (int)projectileType)
        {
            this.itemSlot = EquipSlot.Ammo;
        }

        public ItemType(ConsumableType consumableType)
            : this(BaseType.Consumable, (int)consumableType)
        { }


        public ItemType(TradeGoodsType tradeGoodsType)
            : this(BaseType.TradeGoods, (int)tradeGoodsType)
        { }

        public EquipSlot Slot
        {
            get { return this.itemSlot; }
            //set { this.itemSlot = value; }
        }

        public BaseType BaseType
        {
            get { return this.itemBaseType; }
        }

        public int SubType
        {
            get { return this.subItemType; }
        }

        public void SwitchSlot()
        {
            switch (this.Slot)
            {
                case EquipSlot.AmuletA:
                    this.itemSlot = EquipSlot.AmuletB;
                    return;
                case EquipSlot.AmuletB:
                    this.itemSlot = EquipSlot.AmuletA;
                    return;
                case EquipSlot.FingerA:
                    this.itemSlot = EquipSlot.FingerB;
                    return;
                case EquipSlot.FingerB:
                    this.itemSlot = EquipSlot.FingerA;
                    return;
                case EquipSlot.MainHand:
                    this.itemSlot = EquipSlot.OffHand;
                    return;
                case EquipSlot.OffHand:
                    this.itemSlot = EquipSlot.MainHand;
                    return;
                default:
                    return;
            }
        }
    }
}
