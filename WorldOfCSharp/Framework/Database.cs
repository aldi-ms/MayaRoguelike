using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WorldOfCSharp
{
    public static class Database
    {
        private const string TERRAIN_DB = @"../../db/terrain.db";
        private const string INGAME_OBJECT_DB = @"../../db/ingameobjects.db";
        private static readonly Encoding ENCODING = Encoding.ASCII;
        
        private static List<TerrainType> terrainDatabase;
        private static List<IngameObject> ingameObjectDatabase;

        public static List<TerrainType> TerrainDatabase
        {
            get { return terrainDatabase; }
        }

        public static List<IngameObject> IngameObjectDatabase
        {
            get { return ingameObjectDatabase; }
        }

        public static void LoadDatabase()
        {
            terrainDatabase = LoadTerrain();
            ingameObjectDatabase = LoadIngameObjects();
        }

        public static TerrainType SearchTerrainDB(char visCh)
        {
            foreach (var terrain in terrainDatabase)
            {
                if (terrain.VisualChar == visCh)
                {
                    return terrain;
                }
            }
            throw new ArgumentException("Search does not find such terrain in the database.");
            //return new TerrainType("eff off", 0, ' ', ConsoleColor.Black);
        }

        public static IngameObject SearchIngameObjectDB(char visCh)
        {
            foreach (var inGObj in ingameObjectDatabase)
            {
                if (inGObj.VisualChar == visCh)
                {
                    return inGObj;
                }
                else throw new ArgumentException("Search does not find such in-game object in the database.");
            }
            return new IngameObject("eff off", 0, ' ', ConsoleColor.Black);
        }

        private static List<TerrainType> LoadTerrain()
        {
            StreamReader sReader = new StreamReader(TERRAIN_DB, ENCODING);
            List<TerrainType> DB = new List<TerrainType> { };

            using (sReader)
            {
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
                    ConsoleColor parsedColor = ConsoleTools.ParseColor(color.ToString());
                    DB.Add(new TerrainType(name.ToString(), parsedFlag, visCh, parsedColor));

                    readInt = sReader.Peek();
                }
            }

            return DB;
        }

        private static List<IngameObject> LoadIngameObjects()
        {
            StreamReader sReader = new StreamReader(INGAME_OBJECT_DB, ENCODING);
            List<IngameObject> DB = new List<IngameObject> { };

            using (sReader)
            {
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
                    ConsoleColor parsedColor = ConsoleTools.ParseColor(color.ToString());
                    DB.Add(new IngameObject(name.ToString(), parsedFlag, visCh, parsedColor));

                    readInt = sReader.Peek();
                }
            }

            return DB;
        }
    }
}
