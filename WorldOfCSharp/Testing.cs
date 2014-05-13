using System;
using WorldOfCSharp;
using MT19937;

namespace WorldOfCSharp.Tests
{
    public static class Testing
    {
        public static MersenneTwister mt = new MersenneTwister();
        public static void ItemTest(Unit unit)
        {
            Item[] itemArr = new Item[]
            {
                new Item("Knife", new ItemType((int)BaseType.Weapon, 2, EquipSlot.MainHand), 2, 4, 50, 60, strength: 1, dexterity: 2),
                new Item("Armor", new ItemType((int)BaseType.Armor, 3, EquipSlot.Chest), new ItemStats(stamina: 5)),
                new Item("Helm", new ItemType((int)BaseType.Armor, 3, EquipSlot.Head), new ItemStats(intelligence: 3, stamina: 10)),
                new Item("Sword", new ItemType((int)BaseType.Weapon, 2, EquipSlot.MainHand), 3, 6, 40, 50, strength: 7),
            };

            foreach (Item item in itemArr)
                GameEngine.GameField[unit.X, unit.Y].ItemList.Add(item);
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
            string name = "TestUnit_" + (char)mt.Next(65, 91) + (char)mt.Next(65, 91) + (char)mt.Next(65, 91);
            Unit testUnit = new Unit(x, y, unitFlags, randChar, randColor, name);
            GameEngine.AddUnit(testUnit);
        }
    }
}
