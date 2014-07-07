using System;
using Maya;
using MT19937;
using Maya.Framework;

namespace Maya.Tests
{
    public static class Testing
    {
        public static MersenneTwister mt = new MersenneTwister();

        public static void ItemTest(Unit unit)
        {
            var amuletType = new ItemType(JewelleryType.Amulet, EquipSlot.AmuletA);
            var anotherAmulet = new ItemType(JewelleryType.Amulet, EquipSlot.AmuletA);
            var chestArmorType = new ItemType(ArmorType.Leather, EquipSlot.Chest);
            var hoodArmorType = new ItemType(ArmorType.Leather, EquipSlot.Head);
            var swordType = new ItemType(WeaponType.OneHandedSwords, EquipSlot.MainHand);

            Item[] itemArr = new Item[]
            {
                new Item("Lucky Charm", new ItemAttributes(amuletType, 0.2f, 1, 1, 1, 1, 1, 5)),
                new Item("Undead's Claw", new ItemAttributes(anotherAmulet, 0.2f, str: 2, wis: 4, luck: -1)),
                new Item("Rugged L.Chest", new ItemAttributes(chestArmorType, 2.2f, str: 2, con: 3)),
                new Item("Assassins Hood", new ItemAttributes(hoodArmorType, 1f, dex:5, luck:1)),
                new Item("Dark Sword", new ItemAttributes(swordType, 3.5f, 30, "2d5", 30, 24, str: 5, con: 3))
            };

            GameEngine.GameField[unit.X, unit.Y].ItemList.AddRange(itemArr);
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

        public static void UnitSpawn(int x = 10, int y = 10)
        {
            while (GameEngine.GameField[x, y].Terrain.Flags.HasFlag(Flags.IsCollidable)
                || GameEngine.GameField[x, y].Unit != null 
                || GameEngine.GameField[x, y].IngameObject != null)
            {
                x += mt.Next(2);
                y += mt.Next(2);
            }
            Flags unitFlags = Flags.IsCollidable | Flags.IsMovable;
            char randChar = (char)mt.Next(97, 123);
            ConsoleColor randColor = (ConsoleColor)mt.Next(1, 16);
            string name = "_" + (char)mt.Next(65, 91) + (char)mt.Next(65, 91) + (char)mt.Next(65, 91);
            Unit testUnit = new Unit(x, y, unitFlags, randChar, randColor, name);
            GameEngine.AddUnit(testUnit);
        }
    }
}
