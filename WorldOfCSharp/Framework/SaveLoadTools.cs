using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Xml.Linq;
using Maya.Framework;

namespace Maya
{
    public static class SaveLoadTools
    {
        internal const string UNITS_FILE = @"../../saves/units.xml";
        internal const string GAME_STATS_FILE = @"../../saves/gamestats.xml";
        internal const string ITEMS_ON_MAP_FILE = @"../../saves/mapitems.xml";
        internal const string ITEMS_SAVE_FILE = @"../../db/itemdb.xml";
        private static readonly Encoding ENCODING = Encoding.ASCII;

        public static void SaveGame(FlatArray<GameCell> gameField)
        {
            SaveUnits();
            SaveMap(gameField);
            SaveGameStats();
            SaveItemsToDB(Item.ItemsGenerated);
            GameEngine.MessageLog.SendMessage("Game saved.");
        }

        public static void LoadGame()
        {
            GameEngine.GameField = LoadMap(GameEngine.MapFileName = SaveLoadTools.SavedMapFileName());
            SaveLoadTools.LoadGameTime();
            Item.LastItemID = LastItemID();
            GameEngine.Units = LoadUnitsXML();
        }

        public static void SaveUnits()
        {
            XDocument units = new XDocument(new XElement("units"));

            XElement unitsElement = units.Element("units");
            foreach (var unit in GameEngine.Units)
            {
                XElement equipment;
                XElement inventory;

                unitsElement.Add(
                    new XElement("unit", 
                    new XAttribute("x", unit.X),
                    new XAttribute("y", unit.Y),
                    new XAttribute("z", unit.Z),
                    new XAttribute("flags", (int)unit.Flags),
                    new XAttribute("char", unit.VisualChar),
                    new XAttribute("color", unit.Color.ToString()),
                    new XAttribute("name", unit.Name),
                    new XAttribute("id", unit.UniqueID),
                    new XAttribute("age", unit.Attributes.Age),
                    new XElement("attributes",
                        new XAttribute("str", unit.Attributes[0]),
                        new XAttribute("dex", unit.Attributes[1]),
                        new XAttribute("con", unit.Attributes[2]),
                        new XAttribute("wis", unit.Attributes[3]),
                        new XAttribute("spi", unit.Attributes[4]),
                        new XAttribute("luck", unit.Attributes[5])),
                    equipment = new XElement("equipment"),
                    inventory = new XElement("inventory")
                    ));

                //add equipment items to unit save
                if (unit.Equipment.Count > 0)
                {
                    int count = unit.Equipment.Count;
                    int i = 0;
                    do
                    {
                        if (unit.Equipment[i] != null)
                        {
                            equipment.Add(
                                new XElement("item_id",
                                unit.Equipment[i].ID - 1));
                            count--;
                        }
                        i++;
                    } while(count > 0);
                }

                //add inventory items to unit save
                if (unit.Inventory.Count > 0)
                {
                    int count = unit.Inventory.Count;
                    int i = 0;
                    do
                    {
                        if (unit.Inventory[i] != null)
                        {
                            inventory.Add(new XElement("item_id", unit.Inventory[i].ID - 1));
                            count--;
                        }
                        i++;
                    } while (count > 0);
                }
            }
            units.Save(UNITS_FILE);
        }

        public static void SaveGameStats()
        {
            XDocument gameStats = new XDocument(new XElement("game_stats"));
            XElement statsElement = gameStats.Element("game_stats");

            statsElement.Add(
                new XElement("map_file", GameEngine.MapFileName),
                new XElement("game_time", GameEngine.GameTime.Now.ToSaveString())
                );
            gameStats.Save(@"../../saves/gamestats.xml");
        }

        public static string SavedMapFileName(string saveFileName = GAME_STATS_FILE)
        {
            XDocument gameStats = XDocument.Load(saveFileName);
            return gameStats.Element("game_stats").Element("map_file").Value;
        }

        public static void LoadGameTime(string saveFileName = GAME_STATS_FILE)
        {
            XDocument gameStats = XDocument.Load(saveFileName);
            string[] splitString = gameStats.Element("game_stats").Element("game_time").Value.Split('.');
            int[] gameTime = new int[splitString.Length];

            for (int i = 0; i < splitString.Length; i++)
                gameTime[i] = int.Parse(splitString[i]);

            GameEngine.GameTime = new GameTime(gameTime[0], gameTime[1], gameTime[2], 
                gameTime[3], gameTime[4], gameTime[5]);
        }

        public static int LastItemID(string saveFileName = ITEMS_SAVE_FILE)
        {
            XDocument items = XDocument.Load(saveFileName);
            return int.Parse(items.Element("items").Attribute("last_id").Value);
        }

        public static List<Unit> LoadUnitsXML(string file = UNITS_FILE)
        {
            List<Unit> unitList = new List<Unit>();
            XDocument unitsXML = XDocument.Load(UNITS_FILE);

            var units = unitsXML.Element("units").Elements();

            foreach (var unit in units)
            {
                //unit info
                int x = int.Parse(unit.Attribute("x").Value);
                int y = int.Parse(unit.Attribute("y").Value);
                int z = int.Parse(unit.Attribute("z").Value);
                int flags = int.Parse(unit.Attribute("flags").Value);
                char visualChar = char.Parse(unit.Attribute("char").Value);
                ConsoleColor color = unit.Attribute("color").Value.ToColor();
                string name = unit.Attribute("name").Value;
                int id = int.Parse(unit.Attribute("id").Value);
                int age = int.Parse(unit.Attribute("age").Value);

                //attributes info
                XElement unitAttr = unit.Element("attributes");
                int str = int.Parse(unitAttr.Attribute("str").Value);
                int dex = int.Parse(unitAttr.Attribute("dex").Value);
                int con = int.Parse(unitAttr.Attribute("con").Value);
                int wis = int.Parse(unitAttr.Attribute("wis").Value);
                int spi = int.Parse(unitAttr.Attribute("spi").Value);
                int luck = int.Parse(unitAttr.Attribute("luck").Value);

                Unit currentUnit;
                UnitAttributes unitAttributes = new UnitAttributes(age, str, dex, con, wis, spi, luck);
                unitList.Add(currentUnit = new Unit(x, y, (Flags)flags, visualChar, color, name, id, unitAttributes));

                //load equipment
                XElement equipment = unit.Element("equipment");
                var equipmentItems = equipment.Elements("item_id");

                foreach (var itemID in equipmentItems)
                {
                    Item equippedItem = new Item(Database.ItemDatabase[int.Parse(itemID.Value)]);
                    currentUnit.Equipment.EquipItem(equippedItem);
                }

                //load inventory
                XElement inventory = unit.Element("inventory");
                var inventoryItems = inventory.Elements("item_id");

                foreach (var itemID in inventoryItems)
                {
                    Item storedItem = new Item(Database.ItemDatabase[int.Parse(itemID.Value)]);
                    currentUnit.Inventory.StoreItem(storedItem);
                }                
            }

            return unitList;
        }

        public static void SpawnItemsOnMap(ref FlatArray<GameCell> gameField)
        {
            XDocument savedItems = XDocument.Load(ITEMS_ON_MAP_FILE);
            XElement root = savedItems.Element("item-cells");
            var itemCells = root.Elements("cell");

            foreach (var cell in itemCells)
            {
                int x = int.Parse(cell.Attribute("x").Value);
                int y = int.Parse(cell.Attribute("y").Value);
                var items = cell.Elements("item_id");

                foreach (var item in items)
                {
                    int itemID = int.Parse(item.Value);
                    Item itemOnMap = new Item(Database.ItemDatabase[itemID]);
                    gameField[x, y].ItemList.Add(itemOnMap);
                }
            }
        }
        
        public static FlatArray<GameCell> LoadMap(string mapFileName = null)
        {
            bool newMap = mapFileName == null;
            if (newMap)
                mapFileName = @"../../maps/0.wocm";
            Database.LoadDatabases();
            FlatArray<GameCell> gameGrid = new FlatArray<GameCell>(Globals.GAME_FIELD_BOTTOM_RIGHT.X, Globals.GAME_FIELD_BOTTOM_RIGHT.Y);

            using (var sReader = new StreamReader(mapFileName, ENCODING))
            {
                char procCh = (char)sReader.Read();

                StringBuilder mapName = new StringBuilder();        //--> map name
                do
                {
                    mapName.Append(procCh);
                    procCh = (char)sReader.Read();
                } while (procCh != '[');

                GameEngine.MapName = mapName.ToString();

                StringBuilder xSize = new StringBuilder(4);        //--> gameField.GetLength(0)
                procCh = (char)sReader.Read();
                do
                {
                    xSize.Append(procCh);
                    procCh = (char)sReader.Read();
                } while (procCh != ';');

                StringBuilder ySize = new StringBuilder(4);        //--> gameField.GetLength(1)
                procCh = (char)sReader.Read();
                do
                {
                    ySize.Append(procCh);
                    procCh = (char)sReader.Read();
                } while (procCh != ']');


                int charCode = sReader.Peek();
                for (int x = 0; x < int.Parse(xSize.ToString()); x++)
                {
                    for (int y = 0; y < int.Parse(ySize.ToString()); y++)
                    {
                        gameGrid[x, y] = new GameCell();
                        if (charCode != -1)
                        {
                            char readChar = (char)sReader.Read();

                            StringBuilder posInTerrainDB = new StringBuilder(4);        //--> position in DB
                            readChar = (char)sReader.Read();
                            do
                            {
                                posInTerrainDB.Append(readChar);
                                readChar = (char)sReader.Read();
                            } while (readChar != ']' && readChar != '<');

                            int index = int.Parse(posInTerrainDB.ToString());
                            gameGrid[x, y].Terrain = new Terrain(Database.TerrainDatabase[index]);
                            gameGrid[x, y].Terrain.X = x;
                            gameGrid[x, y].Terrain.Y = y;

                            if (readChar == '<')
                            {
                                StringBuilder posInInGameObjDB = new StringBuilder(4);        //--> position in DB
                                readChar = (char)sReader.Read();
                                do
                                {
                                    posInInGameObjDB.Append(readChar);
                                    readChar = (char)sReader.Read();
                                } while (readChar != '>');

                                readChar = (char)sReader.Read();

                                int objIndex = int.Parse(posInInGameObjDB.ToString());
                                gameGrid[x, y].IngameObject = new InGameObject(Database.ObjectDatabase[objIndex]);
                                gameGrid[x, y].IngameObject.X = x;
                                gameGrid[x, y].IngameObject.Y = y;
                            }
                        }
                    }
                }

                GameEngine.MessageLog.SendMessage("Map loaded.");
                if (!newMap)
                    SpawnItemsOnMap(ref gameGrid);

                return gameGrid;

            }
        }

        public static void SaveMap(FlatArray<GameCell> gameField)
        {
            StringBuilder parseMap = new StringBuilder();
            XDocument mapItems = new XDocument(new XElement("item-cells"));
            XElement itemCells = mapItems.Element("item-cells");

            for (int x = 0; x < gameField.Height; x++)
            {
                for (int y = 0; y < gameField.Width; y++)
                {
                    parseMap.AppendFormat("[{0}", gameField[x, y].Terrain.PositionInDB);

                    if (gameField[x, y].IngameObject == null)
                        parseMap.Append("]");
                    else
                        parseMap.AppendFormat("<{0}>]", gameField[x, y].IngameObject.PositionInDB);

                    //save items on map
                    if (gameField[x, y].ItemList.Count > 0)
                    {
                        XElement cell;
                        itemCells.Add(
                            cell = new XElement("cell",
                                new XAttribute("x", x),
                                new XAttribute("y", y)));

                        foreach (var item in gameField[x, y].ItemList)
                            cell.Add(new XElement("item_id", item.ID - 1));
                    }
                }
            }
            mapItems.Save(ITEMS_ON_MAP_FILE);

            //save map
            using (var sWriter = new StreamWriter(string.Format(@"../../maps/{0}.wocm", GameEngine.MapID), false, ENCODING))
            {
                sWriter.Write(GameEngine.MapName);
                sWriter.Write(string.Format("[{0};{1}]", gameField.Height, gameField.Width));
                sWriter.Write(parseMap.ToString());
            }
        }

        public static void SaveItemsToDB(List<Item> items)
        {
            //clear item duplicates
            List<Item> uniqueItems = new List<Item>();
            foreach (var item in items)
            {
                if (Database.SearchItemDB(item.Name) == -1)
                    uniqueItems.Add(item);
            }

            XDocument itemsDB = XDocument.Load(ITEMS_SAVE_FILE);
            XElement itemsEle = itemsDB.Element("items");

            foreach (var item in uniqueItems)
            {
                if (item.ItemType.BaseType == BaseType.Weapon)
                    itemsEle.Add(
                         new XElement("item",
                            new XElement("id", item.ID),
                            new XElement("attr",
                                new XAttribute("name", item.Name),
                                new XAttribute("str", item.ItemAttr[0]),
                                new XAttribute("dex", item.ItemAttr[1]),
                                new XAttribute("con", item.ItemAttr[2]),
                                new XAttribute("wis", item.ItemAttr[3]),
                                new XAttribute("spi", item.ItemAttr[4]),
                                new XAttribute("luck", item.ItemAttr[5]),
                                new XAttribute("weight", item.ItemAttr.Weight)),
                            new XElement("itemtype",
                                new XAttribute("equipslot", (int)item.Slot),
                                new XAttribute("basetype", (int)item.ItemType.BaseType),
                                new XAttribute("subtype", item.ItemType.SubType)),
                            new XElement("weapon_attr",
                                new XAttribute("base_dmg", item.ItemAttr.BaseDamage),
                                new XAttribute("speed", item.ItemAttr.Speed),
                                new XAttribute("accuracy", item.ItemAttr.Accuracy),
                                new XAttribute("random_ele", item.ItemAttr.RandomElement))
                    ));
                else
                    itemsEle.Add(
                         new XElement("item",
                            new XElement("id", item.ID),
                            new XElement("attr",
                                new XAttribute("name", item.Name),
                                new XAttribute("str", item.ItemAttr[0]),
                                new XAttribute("dex", item.ItemAttr[1]),
                                new XAttribute("con", item.ItemAttr[2]),
                                new XAttribute("wis", item.ItemAttr[3]),
                                new XAttribute("spi", item.ItemAttr[4]),
                                new XAttribute("luck", item.ItemAttr[5]),
                                new XAttribute("weight", item.ItemAttr.Weight)),
                            new XElement("itemtype",
                                new XAttribute("equipslot", (int)item.Slot),
                                new XAttribute("basetype", (int)item.ItemType.BaseType),
                                new XAttribute("subtype", item.ItemType.SubType))
                    ));
            }

            itemsEle.SetAttributeValue("last_id", Item.LastItemID);
            itemsDB.Save(ITEMS_SAVE_FILE);
        }
    }
}
