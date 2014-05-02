using System;
using WorldOfCSharp;
using MT19937;

namespace WorldOfCSharp.Tests
{
    public static class Testing
    {
        public static void ItemTest(Unit unit)
        {
            Item knife = new Item("Knife", new ItemType((int)BaseType.Weapon, 2, EquipSlot.MainHand), 2, 4, 50, 60, strength: 1, dexterity: 2);
            Item chest = new Item("Armor", new ItemType((int)BaseType.Armor, 3, EquipSlot.Chest), new ItemStats(stamina: 5));
            Item helm = new Item("Helm", new ItemType((int)BaseType.Armor, 3, EquipSlot.Head), new ItemStats(intelligence: 3, stamina: 10));

            unit.Inventory.StoreItem(knife);
            unit.Inventory.StoreItem(chest);
            unit.Inventory.StoreItem(helm);
        }

        public static void EnumToConsole(Flags flOpt)
        {
            Console.WriteLine(flOpt);
            Console.WriteLine((int)flOpt);
        }

        public static void IntToEnum(int flag)
        {
            Flags flOpt = (Flags)flag;
            EnumToConsole(flOpt);
        }

        public static void FlatArrayTest()
        {
            Framework.FlatArray<int> flatArray = new Framework.FlatArray<int>(5, 20);

            for (int i = 0; i < flatArray.Width; i++)
            {
                for (int j = 0; j < flatArray.Height; j++)
                {
                    Console.Write(flatArray[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();

            flatArray[0, 0] = 1;
            flatArray[4, 19] = 1;
            flatArray[3, 14] = 1;
            flatArray[1, 1] = 1;
            flatArray[0, 19] = 2;

            for (int i = 0; i < flatArray.Width; i++)
            {
                for (int j = 0; j < flatArray.Height; j++)
                {
                    Console.Write(flatArray[i, j]);
                }
                Console.WriteLine();
            }
        }
    }
}
