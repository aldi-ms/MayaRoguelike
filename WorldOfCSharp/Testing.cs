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

            GameEngine.GameField[unit.X, unit.Y].Item = knife;
            GameEngine.GameField[unit.X+1, unit.Y].Item = chest;
            GameEngine.GameField[unit.X-1, unit.Y].Item = helm;
        }
    }
}
