﻿using System;
using System.Collections.Generic;
using System.Text;
using WorldOfCSharp.FieldOfView;

namespace WorldOfCSharp
{
    public static class GameEngine
    {
        private static GameCell[,] gameField;
        private static MessageLog messageLog;
        private static List<Unit> units = new List<Unit>();
        private static List<Unit> queuedUnits = new List<Unit>();
        private static List<Unit> removedUnits = new List<Unit>();
        private static VisualEngine VEngine;
        private static GameTime gameTime = new GameTime();
        private static RightInfoPane rightInfoPane;
        private static int ticks = 0;       //use bigger integer type!

        public static VisualEngine VisualEngine
        {
            get { return VEngine; }
        }

        public static GameCell[,] GameField
        {
            get { return gameField; }
            set { gameField = value; }
        }

        public static MessageLog MessageLog
        {
            get { return messageLog; }
        }

        public static List<Unit> Units
        {
            get { return units; }
            set { units = value; }
        }

        public static GameTime GameTime
        {
            get { return gameTime; }
        }

        public static RightInfoPane RightInfoPane
        {
            get { return rightInfoPane; }
        }

        public static int Ticks
        {
            get { return ticks; }
            set { ticks = value; }
        }

        public static void CheckForEffect(Unit unit, int objX, int objY)
        {
            if (gameField[objX, objY].IngameObject != null)
            {
                if (gameField[objX, objY].IngameObject.GetFlag(2))
                {
                    switch (gameField[objX, objY].IngameObject.Name)
                    {
                            //add hit/walk related objects+events here
                        case "savepoint":
                            if (unit.GetFlag(4))
                            {
                                SaveLoadTools.SaveGame();
                                MessageLog.SendMessage("Game Saved.");
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        
        public static void New()
        {
            string pcName = UIElements.PromptForName();

            UIElements.InGameUI();

            messageLog = new MessageLog(0, Globals.CONSOLE_HEIGHT - 1, Globals.GAME_FIELD_BOTTOM_RIGHT.X,
                (Globals.CONSOLE_HEIGHT - (Globals.GAME_FIELD_BOTTOM_RIGHT.Y + 1)), true);
            MessageLog.SendMessage("Message log initialized.");
            MessageLog.DeleteLog();

            gameField = MapTools.LoadMap(SaveLoadTools.LoadSavedMapName());       //load map<<<<<<<<
            Unit pc = new Unit(10, 10, 3, '@', ConsoleColor.White, pcName);
            pc.SetFlag(4, true);
            
            Initialize(pc);
        }
        
        public static void Load()
        {
            UIElements.InGameUI();

            messageLog = new MessageLog(0, Globals.CONSOLE_HEIGHT - 1, Globals.GAME_FIELD_BOTTOM_RIGHT.X,
                (Globals.CONSOLE_HEIGHT - (Globals.GAME_FIELD_BOTTOM_RIGHT.Y + 1)), true);
            MessageLog.SendMessage("Message log initialized.");

            gameField = MapTools.LoadMap(SaveLoadTools.LoadSavedMapName());       //load map<<<<<<<<
            Unit pc = SaveLoadTools.LoadUnits();
            
            Initialize(pc);
        }

        public static void AddUnit(Unit unit)
        {
            queuedUnits.Add(unit);
        }

        //!!!POTENTIAL BUG!!! REMOVES THE UNIT AFTER TimeTick() FOREACH HAS FINISHED
        //do NOT use for killing a unit!
        public static void RemoveUnit(Unit unit)
        {
            removedUnits.Add(unit);
        }

        private static void Initialize(Unit pc)
        {
            rightInfoPane = new RightInfoPane(pc);

            VEngine = new VisualEngine(GameField);
            VEngine.PrintUnit(pc);

            TimeTick(pc);
        }

        private static void TimeTick(Unit pc)
        {
            Units.Clear();
            AddUnit(pc);

            bool loop = true;
            int energyCost = 0;

            while (loop)
            {
                ticks++;
                if (ticks % 2 == 0)
                    gameTime.Tick();
                if (ticks % 50 == 0)
                    pc.EffectsPerFive();

                rightInfoPane.Update(pc);

                //add queued units
                foreach (Unit queuedUnit in queuedUnits)
                {
                    Units.Add(queuedUnit);
                }
                queuedUnits.Clear();

                //remove units
                foreach (Unit unitToRemove in removedUnits)
                {
                    Units.Remove(unitToRemove);
                }
                removedUnits.Clear();

                foreach (Unit unit in Units)
                {
                    unit.Stats.Energy += unit.Stats.ActionSpeed;
                    if (unit.Stats.Energy >= 100)
                    {
                        if (unit.GetFlag(4))
                        {
                            energyCost = PlayerControl(pc);
                        }
                        else 
                        {
                            energyCost = AI.ArtificialIntelligence.DrunkardWalk(unit);
                        }
                        unit.Stats.Energy = unit.Stats.Energy - (unit.Stats.ActionSpeed + energyCost);
                    }
                }
            }
        }
        
        private static int PlayerControl(Unit pc)
        {
            bool loop = true;
            while (loop)
            {
                if (Console.KeyAvailable)
                {
                    loop = false;
                    ConsoleKeyInfo key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.T:      //Tests
                            Tests.Testing.ItemTest(pc);
                            return 0;

                        case ConsoleKey.Y:
                            return 0;

                        case ConsoleKey.E:
                            return 0;
                            
                        case ConsoleKey.H:
                            MessageLog.ShowLogFile(pc);
                            return 0;

                        case ConsoleKey.G:
                        case ConsoleKey.OemComma:
                            //pick up item
                            if (GameField[pc.X, pc.Y].Item != null)
                            {
                                pc.Inventory.PickUpItem(GameField[pc.X, pc.Y].Item);
                                MessageLog.SendMessage(string.Format("Picked up {0}.", GameField[pc.X, pc.Y].Item.ToString()));
                                GameField[pc.X, pc.Y].Item = null;
                            }
                            return 100;

                        case ConsoleKey.I:
                            OpenInventory(pc);
                            return 100;

                        case ConsoleKey.UpArrow:
                        case ConsoleKey.NumPad8:
                            pc.MakeAMove(Direction.North);
                            break;

                        case ConsoleKey.DownArrow:
                        case ConsoleKey.NumPad2:
                            pc.MakeAMove(Direction.South);
                            break;

                        case ConsoleKey.LeftArrow:
                        case ConsoleKey.NumPad4:
                            pc.MakeAMove(Direction.West);
                            break;

                        case ConsoleKey.RightArrow:
                        case ConsoleKey.NumPad6:
                            pc.MakeAMove(Direction.East);
                            break;

                        case ConsoleKey.NumPad7:
                            pc.MakeAMove(Direction.NorthWest);
                            break;

                        case ConsoleKey.NumPad9:
                            pc.MakeAMove(Direction.NorthEast);
                            break;

                        case ConsoleKey.NumPad1:
                            pc.MakeAMove(Direction.SouthWest);
                            break;

                        case ConsoleKey.NumPad3:
                            pc.MakeAMove(Direction.SouthEast);
                            break;

                        case ConsoleKey.Escape:
                            loop = false;
                            UIElements.MainMenu();
                            return 0;

                        default:
                            loop = true;
                            break;
                    }
                    if (GameField[pc.X, pc.Y].Item != null)
                    {
                        MessageLog.SendMessage(string.Format("You see a {0} here.", GameField[pc.X, pc.Y].Item.ToString()));
                    }
                }
            }
            return 100;
        }

        private static void OpenInventory(Unit pc)
        {
            Window invWindow = new Window(pc, "inventory");
            ShowWindow(pc, invWindow);

            ConsoleKeyInfo key;
            bool loop = true;
            do
            {
                key = Console.ReadKey(true);
                int letterCode = key.KeyChar;
                int itemPosition;

                if (char.IsLower(key.KeyChar))
                {
                    if (!key.Modifiers.HasFlag(ConsoleModifiers.Control))
                    {
                        itemPosition = letterCode - 97;
                        if (itemPosition < pc.Inventory.IsSlotUsed.Length && pc.Inventory[itemPosition] != null)
                        {
                            ItemActions(pc, pc.Inventory[itemPosition]);
                        }
                    }
                    else
                    {
                        itemPosition = letterCode + 25 - 97;
                        if (itemPosition < pc.Inventory.IsSlotUsed.Length && pc.Inventory[itemPosition] != null)
                        {
                            ItemActions(pc, pc.Inventory[itemPosition]);
                        }
                    }
                }
                else if (char.IsUpper(key.KeyChar))
                {
                    itemPosition = letterCode - 65;
                    if (itemPosition < pc.Equipment.IsSlotUsed.Length && pc.Equipment[itemPosition] != null)
                    {
                        ItemActions(pc, pc.Equipment[itemPosition]);
                    }
                }
                if (key.Key == ConsoleKey.Escape)
                {
                    loop = false;
                    invWindow.CloseWindow();
                }
            } while(loop);
        }

        private static void ShowWindow(Unit unit, Window invWindow)
        {
            int letterCount = 0;
            bool CTRLMod = false;
            int letterSize = 65;
            //show equipment/inventory window
            Item[] itemsOwned = new Item[unit.Inventory.Count + unit.Equipment.Count];
            int itemsCount = 0;

            invWindow.Show();

            invWindow.WriteLine("Equipment:", ConsoleColor.Red);
            for (int i = 0; i < unit.Equipment.IsSlotUsed.Length; i++)
            {
                if (unit.Equipment.IsSlotUsed[i] == true)
                {
                    itemsOwned[itemsCount++] = unit.Equipment[i];
                    invWindow.Write(string.Format("{0} - {1}: {2}", (char)(unit.Equipment[i].Slot + letterSize), unit.Equipment[i].Slot, unit.Equipment[i].ToString()), ConsoleColor.Yellow);
                }
            }

            letterCount = 0;
            letterSize = 97;
            invWindow.WriteLine("Inventory:", ConsoleColor.Red);
            invWindow.Write("");
            for (int i = 0; i < unit.Inventory.IsSlotUsed.Length; i++)
            {
                if (unit.Inventory.IsSlotUsed[i] == true)
                {
                    itemsOwned[itemsCount++] = unit.Inventory[i];
                    if (!CTRLMod)
                        invWindow.Write(string.Format("{0} - {1}", (char)(unit.Inventory[i].InventorySlot + letterSize), unit.Inventory[i].ToString()), ConsoleColor.White);
                    else
                        invWindow.Write(string.Format("^{0} - {1}", (char)(unit.Inventory[i].InventorySlot + letterSize), unit.Inventory[i].ToString()), ConsoleColor.White);

                    if (letterCount > 25 && !CTRLMod)
                    {
                        letterCount = 0;
                        CTRLMod = true;
                    }
                }
            }
        }

        private static void ItemActions(Unit unit, Item item)
        {
            if (item != null)
                if (item.isEquipped)
                {
                    GameEngine.MessageLog.SendMessage(string.Format("{0} -- [T] Take off, [D] Drop", item.ToString()));

                    ConsoleKeyInfo key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.D:
                            unit.Equipment.Unequip(item);
                            GameField[unit.X, unit.Y].Item = unit.Inventory.DropItem(item);
                            RefreshInventoryScreen(unit);
                            MessageLog.SendMessage(string.Format("{0} ({1}) dropped.", item.ToString(), item.Slot));
                            break;
                        case ConsoleKey.T:
                            unit.Equipment.Unequip(item);
                            RefreshInventoryScreen(unit);
                            MessageLog.SendMessage(string.Format("You are taking off {0} ({1}).", item.ToString(), item.Slot));
                            break;
                        default:
                        case ConsoleKey.Escape:
                            GameEngine.MessageLog.SendMessage("No action taken.");
                            break;
                    }
                }
                else
                {
                    GameEngine.MessageLog.SendMessage(string.Format("{0} -- [E] Equip, [D] Drop", item.ToString()));

                    ConsoleKeyInfo key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.D:
                            GameField[unit.X, unit.Y].Item = unit.Inventory.DropItem(item);
                            RefreshInventoryScreen(unit);
                            MessageLog.SendMessage(string.Format("{0} dropped.", item.ToString()));
                            break;
                        case ConsoleKey.E:
                            unit.Equipment.EquipItem(item);
                            RefreshInventoryScreen(unit);
                            MessageLog.SendMessage(string.Format("Equipping {0} to {1}.", item.ToString(), item.Slot));
                            break;
                        default:
                            GameEngine.MessageLog.SendMessage("No action taken.");
                            break;
                    }
                }
        }

        private static void RefreshInventoryScreen(Unit unit)
        {
            for (int i = 0; i < Window.ActiveWindows.Count; i++)
            {
                if (Window.ActiveWindows[i].Title == "INVENTORY")
                {
                    Window.ActiveWindows[i].ResetLinePosition();
                    ShowWindow(unit, Window.ActiveWindows[i]);
                }
            }
        }


        //Method to fill the whole map with a single type of terrain.
        //public static void FillGameField(TerrainType terrain)
        //{
        //    for (int x = 0; x < gameField.GetLength(0); x++)
        //    {
        //        for (int y = 0; y < gameField.GetLength(1); y++)
        //        {
        //            gameField[x, y] = new GameCell();
        //            gameField[x, y].Terrain = new TerrainType(x, y, terrain.Flags, terrain.VisualChar, terrain.Color, terrain.Name);
        //        }
        //    }
        //}


        //Generate new map file (put in New())
        //gameField = MapTools.LoadMap(SaveLoadTools.LoadSavedMapName());
        //FillGameField(new TerrainType("floor", 0, '.', ConsoleColor.Gray));
        //MapTools.GenerateMapFile(gameField);
    }
}
