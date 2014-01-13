using System;
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


        public static void CheckForEffect(Unit unit, int objX, int objY)
        {
            if (gameField[objX, objY].IngameObject != null)
            {
                if (gameField[objX, objY].IngameObject.GetFlag(2))
                {
                    switch (gameField[objX, objY].IngameObject.Name)
                    {
                        case "savepoint":
                            SaveLoadTools.SaveGame(unit);
                            ConsoleTools.PrintDebugInfo("Game saved.");
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        
        public static void New()
        {
            string pcName = GameUI.NewCharacterName();

            GameUI.InGameUI();

            messageLog = new MessageLog(0, Globals.CONSOLE_HEIGHT - 1, Globals.GAME_FIELD_BOTTOM_RIGHT.X,
                (Globals.CONSOLE_HEIGHT - (Globals.GAME_FIELD_BOTTOM_RIGHT.Y + 1)), true);
            MessageLog.SendMessage("Message log initialized.");
            MessageLog.DeleteLog();

            gameField = MapTools.LoadMap(SaveLoadTools.LoadSavedMapName());       //load map<<<<<<<<
            PlayerCharacter pc = new PlayerCharacter(10, 10, pcName);
            
            Initialize(pc);
        }
        
        public static void Load()
        {
            GameUI.InGameUI();

            messageLog = new MessageLog(0, Globals.CONSOLE_HEIGHT - 1, Globals.GAME_FIELD_BOTTOM_RIGHT.X,
                (Globals.CONSOLE_HEIGHT - (Globals.GAME_FIELD_BOTTOM_RIGHT.Y + 1)), true);
            MessageLog.SendMessage("Message log initialized.");

            gameField = MapTools.LoadMap(SaveLoadTools.LoadSavedMapName());       //load map<<<<<<<<
            PlayerCharacter pc = SaveLoadTools.LoadSavedPlayerCharacter();
            
            Initialize(pc);
        }

        public static void AddUnit(Unit unit)
        {
            queuedUnits.Add(unit);
        }

        //!!!POTENTIAL BUG!!! REMOVES THE UNIT AFTER TimeTick() FOREACH HAS FINISHED
        //do NOT use for a "kill" method
        public static void RemoveUnit(Unit unit)
        {
            removedUnits.Add(unit);
        }

        private static void Initialize(PlayerCharacter pc)
        {
            rightInfoPane = new RightInfoPane(pc);

            VEngine = new VisualEngine(GameField);
            VEngine.PrintUnit(pc);

            TimeTick(pc);
        }

        private static void TimeTick(PlayerCharacter pc)
        {
            Units.Clear();
            AddUnit(pc);

            bool loop = true;
            int ticks = 0;

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
                    unit.Energy += unit.Speed;
                    if (unit.Energy >= 100)
                    {
                        int energyCost = 0;
                        if (unit.UniqueID != pc.UniqueID)
                        {
                            energyCost = AI.ArtificialIntelligence.DrunkardWalk(unit);
                        }
                        else if (unit.UniqueID == pc.UniqueID)
                        {
                            energyCost = PlayerControl(pc);
                        }
                        unit.Energy = unit.Energy - (unit.Speed + energyCost);
                    }
                }
            }
        }
        
        private static int PlayerControl(PlayerCharacter pc)
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
                            ConsoleTools.PrintDebugInfo(pc.ItemInSlot(EquipSlot.Chest));
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
                                pc.Inventory.AddItem(GameField[pc.X, pc.Y].Item);
                            }
                            return 100;

                        case ConsoleKey.I:
                            ShowInventoryWindow(pc);
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
                            GameUI.MainMenu();
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

        private static void ShowInventoryWindow(PlayerCharacter pc)
        {
            //show equipment/inventory window
            Window invWindow = new Window(pc, "inventory");
            invWindow.Show();

            invWindow.Write("\tEquipment:", ConsoleColor.Cyan);
            for (int i = 0; i < pc.Equipment.IsSlotUsed.Length; i++)
            {
                if (pc.Equipment.IsSlotUsed[i] == true)
                {
                    invWindow.Write(string.Format("{0} - {1}", (char)(i + 65), pc.Equipment[i].ToFullString()));
                }
            }

            invWindow.Write("\tInventory:", ConsoleColor.Cyan);
            
            for (int i = 0; i < pc.Inventory.IsSlotUsed.Length; i++)
            {
                if (pc.Inventory.IsSlotUsed[i] == true)
                {
                    invWindow.Write(string.Format("{0} - {1}", (char)(i + 65), pc.Inventory[i].ToFullString()));
                }
            }

            ConsoleKeyInfo key;
            bool loop = true;
            selectionChar = (char)(selectionChar + 32); //to convert the char to lower case
            do
            {
                key = Console.ReadKey(true);
                if (key.KeyChar >= 'a' && key.KeyChar < selectionChar)
                {
                    int selectedItem = key.KeyChar - 'a';
                    if (equipment.Count != 0 && selectedItem < equipment.Count)
                    {
                        int counter = -1;
                        for (int i = 0; i < Globals.ITEM_SLOTS; i++)
                        {
                            if (pc.Equipment[i].Slot != EquipSlot.NotEquippable)
                            {
                                counter++;
                                if (counter == selectedItem)
                                {
                                    pc.Equipment[i].Actions();
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        pc.Inventory[selectedItem - equipment.Count].Actions();
                    }
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    loop = false;
                    invWindow.CloseWindow();
                }
            } while(loop);
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
