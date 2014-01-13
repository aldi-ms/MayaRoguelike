namespace WorldOfCSharp
{
    public class ItemType
    {
        private static string[][] itemTypeArr = new string[System.Enum.GetNames(typeof(BaseType)).Length][];
        private ItemCode itemCode;
        private EquipSlot slot = EquipSlot.NotEquippable;

        public ItemType(int itemType, int subType, EquipSlot slot)
        {
            this.itemCode = new ItemCode(itemType, subType);
            this.slot = slot;
            Initialize();
        }

        public ItemType(int itemType, int subType)
            : this(itemType, subType, EquipSlot.NotEquippable)
        { }

        public ItemType()
            : this(13, 0)   //miscellaneous item
        { }

        public static string[][] ItemTypeArr
        {
            get { return itemTypeArr; }
        }

        public EquipSlot Slot
        {
            get { return this.slot; }
        }

        public ItemCode ItemCode
        {
            get { return this.itemCode; }
        }

        public int ItemCodeToInt
        {
            get 
            {
                string itemCode = string.Format("{0}{1}", this.itemCode.BaseTypeInt, this.itemCode.SubTypeInt);
                return int.Parse(itemCode);
            }
        }
        
        private static void Initialize()
        {
            itemTypeArr[(int)BaseType.Armor] = new string[]
            {
                "Cloth",
                "Leather",
                "Mail",
                "Plate",
                "Shields",
                "Librams",
                "Idols",
                "Totems",
                "Sigils",
                "Miscellaneous"
            };

            itemTypeArr[(int)BaseType.Weapon] = new string[]
            {
                "Bows",
                "Crossbows",
                "Daggers",
                "Guns",
                "Fishing Poles",
                "Fist Weapons",
                "One-Handed Axes",
                "One-Handed Maces",
                "One-Handed Swords",
                "Polearms",
                "Staves",
                "Thrown",
                "Two-Handed Axes",
                "Two-Handed Maces",
                "Two-Handed Swords",
                "Wands",
                "Miscellaneous"
            };

            itemTypeArr[(int)BaseType.Consumable] = new string[]
            {
                "Food & Drink",
                "Potion",
                "Elixir",
                "Flask",
                "Bandage",
                "Item Enhancement",
                "Scroll",
                "Other",
                "Consumable"
            };

            itemTypeArr[(int)BaseType.Container] = new string[]
            {
                "Bag",
                "Enchanting Bag",
                "Engineering Bag",
                "Gem Bag",
                "Herb Bag",
                "Mining Bag",
                "Soul Bag",
                "Leatherworking Bag" ,
            };

            itemTypeArr[(int)BaseType.Gem] = new string[] 
            {
                "Blue",
                "Green",
                "Orange",
                "Meta",
                "Prismatic",
                "Purple",
                "Red",
                "Simple",
                "Yellow" 
            };

            itemTypeArr[(int)BaseType.Key] = new string[] { "Key" };

            itemTypeArr[(int)BaseType.Money] = new string[] {  };

            itemTypeArr[(int)BaseType.Reagent] = new string[] { "Reagent" };

            itemTypeArr[(int)BaseType.Recipe] = new string[] 
            { 
                "Alchemy",
                "Blacksmithing",
                "Book",
                "Cooking",
                "Enchanting",
                "Engineering",
                "First Aid",
                "Leatherworking",
                "Tailoring" 
            };

            itemTypeArr[(int)BaseType.Projectile] = new string[] { "Arrow", "Bullet" };

            itemTypeArr[(int)BaseType.Quest] = new string[] { "Quest" };

            itemTypeArr[(int)BaseType.Quiver] = new string[] { "Ammo Pouch", "Quiver" };

            itemTypeArr[(int)BaseType.TradeGoods] = new string[] 
            {
                "Armor Enchantment",
                "Cloth",
                "Devices",
                "Elemental",
                "Enchanting",
                "Explosives",
                "Herb",
                "Jewelcrafting",
                "Leather",
                "Materials",
                "Meat",
                "Metal & Stone",
                "Parts",
                "Weapon Enchantment" ,
                "Trade Goods",
                "Other"
            };

            itemTypeArr[(int)BaseType.Miscellaneous] = new string[] { "Miscellaneous" };
        }
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
        Quest = 10,
        Quiver = 11,
        TradeGoods = 12,
        Miscellaneous = 13
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
        Feet = 8,
        //accessories
        Neck = 9,
        Trinket = 10,
        Finger = 11,
        //weapons
        MainHand = 12,
        OffHand = 13,
        RangedRelic = 14,
        Ammo = 15,
        //for items that are not equipped
        NotEquippable = 16
    }
}
