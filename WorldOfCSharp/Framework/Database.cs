using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Maya
{
    public static class Database
    {
        private const string TERRAIN_DB = @"../../db/terrain.db";
        private const string INGAME_OBJECT_DB = @"../../db/ingameobjects.db";
        private static readonly Encoding ENCODING = Encoding.ASCII;
        
        private static List<Terrain> terrainDatabase;
        private static List<InGameObject> ingameObjectDatabase;

        public static List<Terrain> TerrainDatabase
        {
            get { return terrainDatabase; }
            set { terrainDatabase = value; }
        }
        
        public static List<InGameObject> ObjectDatabase
        {
            get { return ingameObjectDatabase; }
            set { ingameObjectDatabase = value; }
        }


        public static void LoadDatabase()
        {
            terrainDatabase = LoadTerrain();
            ingameObjectDatabase = LoadIngameObjects();
        }

        public static Terrain SearchTerrainDB(char visCh)
        {
            foreach (var terrain in terrainDatabase)
            {
                if (terrain.VisualChar == visCh)
                {
                    return terrain;
                }
            }
            throw new ArgumentException("Search does not find such terrain in the database.");
        }

        public static InGameObject SearchIngameObjectDB(char visCh)
        {
            foreach (var inGObj in ingameObjectDatabase)
            {
                if (inGObj.VisualChar == visCh)
                {
                    return inGObj;
                }
            }
            throw new ArgumentException("Search does not find such in-game object in the database.");
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
                    ConsoleColor parsedColor = ConsoleTools.ParseColor(color.ToString());
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
                    ConsoleColor parsedColor = ConsoleTools.ParseColor(color.ToString());
                    DB.Add(new InGameObject(iInGameObj++, name.ToString(), (Flags)parsedFlag, visCh, parsedColor));
                    readInt = sReader.Peek();
                }
            }

            return DB;
        }
    }
}
