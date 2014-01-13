using System;
using System.IO;
using System.Text;
using MT19937;

namespace WorldOfCSharp
{
    public static class MapTools
    {
        private static readonly Encoding encoding = Encoding.ASCII;

        public static GameCell[,] LoadMap(string mapFileName)  //return gameField
        {
            GameCell[,] gameField = new GameCell[Globals.GAME_FIELD_BOTTOM_RIGHT.X, Globals.GAME_FIELD_BOTTOM_RIGHT.Y];
            Database.LoadDatabase();

            StreamReader sReader = new StreamReader(mapFileName, encoding);
            using (sReader)
            {
                IngameObject inGObj = null;
                bool hasIngameObject = false;

                int readInt = sReader.Peek();
                while (readInt != -1)
                {
                    char readChar = (char)sReader.Read();

                    StringBuilder coordX = new StringBuilder(4);        //--> read X -coord
                    readChar = (char)sReader.Read();
                    do
                    {
                        coordX.Append(readChar);
                        readChar = (char)sReader.Read();
                    } while (readChar != ';');

                    StringBuilder coordY = new StringBuilder(4);        //--> read Y -coord
                    readChar = (char)sReader.Read();
                    do
                    {
                        coordY.Append(readChar);
                        readChar = (char)sReader.Read();
                    } while (readChar != ';');

                    char visualChar = '\0';             //--> read visCh
                    visualChar = (char)sReader.Read();
                    visualChar = RandomTerrain();   //remove this row for normal map load
                    readChar = (char)sReader.Read();

                    int parsedCoordX;
                    int parsedCoordY;

                    if (readChar == '<')                     //ingameObject exists in this cell, begin fetchin info for it
                    {
                        hasIngameObject = true;

                        char inGObjVisualChar = '\0';               //--> read visCh
                        inGObjVisualChar = (char)sReader.Read();
                        readChar = (char)sReader.Read();        //read '>'                        
                        readChar = (char)sReader.Read();    //read ']'
                        //finish reading ingameObject info
                        //parse variables to a complete IngameObject
                        parsedCoordX = int.Parse(coordX.ToString());
                        parsedCoordY = int.Parse(coordY.ToString());
                        inGObj = Database.SearchIngameObjectDB(inGObjVisualChar);
                    }

                    readInt = sReader.Peek();

                    //parse variables to a complete TerrainType object
                    parsedCoordX = int.Parse(coordX.ToString());
                    if (parsedCoordX >= Globals.GAME_FIELD_BOTTOM_RIGHT.X)
                        break;
                    parsedCoordY = int.Parse(coordY.ToString());
                    if (parsedCoordY >= Globals.GAME_FIELD_BOTTOM_RIGHT.Y)
                        break;

                    gameField[parsedCoordX, parsedCoordY] = new GameCell();
                    gameField[parsedCoordX, parsedCoordY].Terrain = new TerrainType(Database.SearchTerrainDB(visualChar));
                    gameField[parsedCoordX, parsedCoordY].Terrain.X = parsedCoordX;
                    gameField[parsedCoordX, parsedCoordY].Terrain.Y = parsedCoordY;

                    if (hasIngameObject)
                    {
                        gameField[parsedCoordX, parsedCoordY].IngameObject = inGObj;
                        gameField[parsedCoordX, parsedCoordY].IngameObject.X = parsedCoordX;
                        gameField[parsedCoordX, parsedCoordY].IngameObject.Y = parsedCoordY;
                        hasIngameObject = false;
                    }
                }

                //ConsoleTools.PrintDebugInfo("Map loaded.");
                return gameField;
            }
        }

        private static MersenneTwister mt = new MersenneTwister();
        private static char RandomTerrain()
        {
            int roll = mt.Next(0, 101);
            if (roll >= 90)
            {
                return '#';
            }
            return '.';
        }

        //Method to generate new map     
        //public static void GenerateMapFile(GameCell[,] gameField)
        //{
        //    StringBuilder parseMap = new StringBuilder();

        //    for (int x = 0; x < gameField.GetLength(0); x++)
        //    {
        //        for (int y = 0; y < gameField.GetLength(1); y++)
        //        {
        //            parseMap.AppendFormat("[{0};{1};{2}", gameField[x, y].Terrain.X, gameField[x, y].Terrain.Y, gameField[x, y].Terrain.VisualChar);

        //            if (gameField[x, y].IngameObject == null)
        //            {
        //                parseMap.Append("]");
        //            }
        //            else
        //            {
        //                parseMap.AppendFormat("<{0}>]", gameField[x, y].IngameObject.VisualChar);
        //            }
        //        }
        //    }

        //    StreamWriter sWriter = new StreamWriter(@"../../maps/testmap.wocm", false, encoding);
        //    using (sWriter)
        //    {
        //        string nullString = null;
        //        sWriter.Write(nullString);  //creates a new file, overwrites old
        //        sWriter.Write(parseMap.ToString());
        //    }
        //}
    }
}
