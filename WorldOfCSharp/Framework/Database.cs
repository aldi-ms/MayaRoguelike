using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace Maya
{
    public static class Database
    {
        private const string TERRAIN_DB = @"../../db/terrain.db";
        private const string INGAME_OBJECT_DB = @"../../db/ingameobjects.db";
        private static readonly Encoding ENCODING = Encoding.ASCII;
        
        private static List<Terrain> terrainDB;
        private static List<InGameObject> ingameObjectDB;
        private static List<Item> itemDB;

        public static List<Terrain> TerrainDatabase
        {
            get { return terrainDB; }
            set { terrainDB = value; }
        }
        
        public static List<InGameObject> ObjectDatabase
        {
            get { return ingameObjectDB; }
            set { ingameObjectDB = value; }
        }

        public static List<Item> ItemDatabase
        {
            get { return itemDB; }
            set { itemDB = value; }
        }

        public static void LoadDatabases()
        {
            terrainDB = LoadTerrain();
            ingameObjectDB = LoadIngameObjects();
            itemDB = LoadItems();
        }

        /// <summary>
        /// Search the Terrain DB for a terrain-type by its character.
        /// </summary>
        /// <param name="visCh">The visual character of the terrain.</param>
        /// <returns>The position of the terrain in the database, or -1 if not found.</returns>
        public static int SearchTerrainDB(char visCh)
        {
            foreach (var terrain in terrainDB)
            {
                if (terrain.VisualChar == visCh)
                    return terrain.PositionInDB;
            }
            return -1;
        }

        /// <summary>
        /// Search the Ingame Object DB for a object-type by its character.
        /// </summary>
        /// <param name="visCh">The visual character of the object.</param>
        /// <returns>The position of the object in the database, or -1 if not found.</returns>
        public static int SearchIngameObjectDB(char visCh)
        {
            foreach (var inGObj in ingameObjectDB)
            {
                if (inGObj.VisualChar == visCh)
                {
                    return inGObj.PositionInDB;
                }
            }
            return -1;
        }

        /// <summary>
        /// Search the Item DB for a item-type by its name;
        /// </summary>
        /// <param name="name">The name of the item to search for.</param>
        /// <returns>The ID of the item in the database, or -1 if not found.</returns>
        public static int SearchItemDB(string name)
        {
            foreach (var item in itemDB)
            {
                if (string.Compare(item.Name, name, true) == 0)
                    return item.ID;
            }
            return -1;
        }

        private static List<Item> LoadItems()
        {
            XDocument itemXML = XDocument.Load(SaveLoadTools.ITEMS_SAVE_FILE);
            List<Item> itemsList = new List<Item>();
            var items = itemXML.Element("items").Elements("item");

            foreach (var item in items)
            {
                int id = int.Parse(item.Element("id").Value);

                XElement attr = item.Element("attr");
                string name = attr.Attribute("name").Value;
                int str = int.Parse(attr.Attribute("str").Value);
                int dex = int.Parse(attr.Attribute("dex").Value);
                int con = int.Parse(attr.Attribute("con").Value);
                int wis = int.Parse(attr.Attribute("wis").Value);
                int spi = int.Parse(attr.Attribute("spi").Value);
                int luck = int.Parse(attr.Attribute("luck").Value);
                float weight = float.Parse(attr.Attribute("weight").Value);

                XElement itemType = item.Element("itemtype");
                int equipSlot = int.Parse(itemType.Attribute("equipslot").Value);
                int baseType = int.Parse(itemType.Attribute("basetype").Value);
                int subType = int.Parse(itemType.Attribute("subtype").Value);

                ItemType currItemType = new ItemType((BaseType)baseType, subType, (EquipSlot)equipSlot);
                if (currItemType.BaseType == BaseType.Weapon)
                {
                    XElement wepAttr = item.Element("weapon_attr");
                    int baseDmg = int.Parse(wepAttr.Attribute("base_dmg").Value);
                    int speed = int.Parse(wepAttr.Attribute("speed").Value);
                    int accuracy = int.Parse(wepAttr.Attribute("accuracy").Value);
                    string randomEle = wepAttr.Attribute("random_ele").Value;
                    //create weapon, add to list
                    ItemAttributes weaponAttr = new ItemAttributes(currItemType, weight, baseDmg, randomEle, speed, accuracy, str, dex, con, wis, spi, luck);
                    itemsList.Add(new Item(name, weaponAttr, id));
                }
                //else create item, add to list!
                else
                {
                    ItemAttributes itemAttr = new ItemAttributes(currItemType, weight, str, dex, con, wis, spi, luck);
                    itemsList.Add(new Item(name, itemAttr, id));
                }
            }
            return itemsList;
        }

        private static List<Terrain> LoadTerrain()
        {
            StreamReader sReader = new StreamReader(TERRAIN_DB, ENCODING);
            List<Terrain> DB = new List<Terrain> { };

            using (sReader)
            {
                int iTerrain = 0;
                int readInt = sReader.Peek();
                while (readInt != -1)
                {
                    char readChar = (char)sReader.Read();

                    StringBuilder name = new StringBuilder();    //--> read name/label
                    readChar = (char)sReader.Read();
                    do
                    {
                        name.Append(readChar);
                        readChar = (char)sReader.Read();
                    } while (readChar != ';');

                    StringBuilder flag = new StringBuilder();    //--> read flag
                    readChar = (char)sReader.Read();
                    do
                    {
                        flag.Append(readChar);
                        readChar = (char)sReader.Read();
                    } while (readChar != ';');

                    char visCh = '\0';      //--> read visCh
                    visCh = (char)sReader.Read();
                    readChar = (char)sReader.Read();

                    StringBuilder color = new StringBuilder();          //--> read color
                    readChar = (char)sReader.Read();
                    do
                    {
                        color.Append(readChar);
                        readChar = (char)sReader.Read();
                    } while (readChar != ']');

                    int parsedFlag = int.Parse(flag.ToString());
                    ConsoleColor parsedColor = color.ToString().ToColor();
                    DB.Add(new Terrain(iTerrain++, name.ToString(), (Flags)parsedFlag, visCh, parsedColor));
                    readInt = sReader.Peek();
                }
            }

            return DB;
        }

        private static List<InGameObject> LoadIngameObjects()
        {
            StreamReader sReader = new StreamReader(INGAME_OBJECT_DB, ENCODING);
            List<InGameObject> DB = new List<InGameObject> { };

            using (sReader)
            {
                int iInGameObj = 0;
                int readInt = sReader.Peek();
                while (readInt != -1)
                {
                    char readChar = (char)sReader.Read();

                    StringBuilder name = new StringBuilder();    //--> read name/label
                    readChar = (char)sReader.Read();
                    do
                    {
                        name.Append(readChar);
                        readChar = (char)sReader.Read();
                    } while (readChar != ';');

                    StringBuilder flag = new StringBuilder();    //--> read flag
                    readChar = (char)sReader.Read();
                    do
                    {
                        flag.Append(readChar);
                        readChar = (char)sReader.Read();
                    } while (readChar != ';');

                    char visCh = '\0';      //--> read visCh
                    visCh = (char)sReader.Read();
                    readChar = (char)sReader.Read();

                    StringBuilder color = new StringBuilder();          //--> read color
                    readChar = (char)sReader.Read();
                    do
                    {
                        color.Append(readChar);
                        readChar = (char)sReader.Read();
                    } while (readChar != ']');

                    int parsedFlag = int.Parse(flag.ToString());
                    ConsoleColor parsedColor = ConsoleTools.ToColor(color.ToString());
                    DB.Add(new InGameObject(iInGameObj++, name.ToString(), (Flags)parsedFlag, visCh, parsedColor));
                    readInt = sReader.Peek();
                }
            }

            return DB;
        }
    }
}
