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
            GameCell[,] gameGrid = new GameCell[Globals.GAME_FIELD_BOTTOM_RIGHT.X, Globals.GAME_FIELD_BOTTOM_RIGHT.Y];
            Database.LoadDatabase();

            StreamReader sReader = new StreamReader(mapFileName, encoding);
            using (sReader) 
            {

                StringBuilder mapName = new StringBuilder();        //--> map name
                char procCh = (char)sReader.Read();
                do
                {
                    mapName.Append(procCh);
                    procCh = (char)sReader.Read();
                } while (procCh != '[');

                GameEngine.MapName = mapName.ToString();
                GameEngine.MapFileName = string.Format(@"../../maps/{0}.wocm", mapName.ToString());

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
                            gameGrid[x, y].Terrain = new TerrainType(index, Database.TerrainDatabase[index]);
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
                                gameGrid[x, y].IngameObject = new InGameObject(objIndex, Database.IngameObjectDatabase[objIndex]);
                                gameGrid[x, y].IngameObject.X = x;
                                gameGrid[x, y].IngameObject.Y = y;
                            }
                        }
                    }
                }

                GameEngine.MessageLog.SendMessage("Map loaded.");
                return gameGrid;

                //InGameObject inGObj = null;
                //bool hasIngameObject = false;
                //
                //int readInt = sReader.Peek();
                //while (readInt != -1)
                //{
                //    char readChar = (char)sReader.Read();
                //
                //    StringBuilder coordX = new StringBuilder(4);        //--> read X -coord
                //    readChar = (char)sReader.Read();
                //    do
                //    {
                //        coordX.Append(readChar);
                //        readChar = (char)sReader.Read();
                //    } while (readChar != ';');
                //
                //    StringBuilder coordY = new StringBuilder(4);        //--> read Y -coord
                //    readChar = (char)sReader.Read();
                //    do
                //    {
                //        coordY.Append(readChar);
                //        readChar = (char)sReader.Read();
                //    } while (readChar != ';');
                //
                //    char visualChar = '\0';             //--> read visCh
                //    visualChar = (char)sReader.Read();
                //    visualChar = RandomTerrain();   //remove this row for normal map load
                //    readChar = (char)sReader.Read();
                //
                //    int parsedCoordX;
                //    int parsedCoordY;
                //
                //    if (readChar == '<')                     //ingameObject exists in this cell, begin fetchin info for it
                //    {
                //        hasIngameObject = true;
                //
                //        char inGObjVisualChar = '\0';               //--> read visCh
                //        inGObjVisualChar = (char)sReader.Read();
                //        readChar = (char)sReader.Read();        //read '>'                        
                //        readChar = (char)sReader.Read();    //read ']'
                //        //finish reading ingameObject info
                //        //parse variables to a complete IngameObject
                //        parsedCoordX = int.Parse(coordX.ToString());
                //        parsedCoordY = int.Parse(coordY.ToString());
                //        inGObj = Database.SearchIngameObjectDB(inGObjVisualChar);
                //    }
                //
                //    readInt = sReader.Peek();
                //
                //    //parse variables to a complete TerrainType object
                //    parsedCoordX = int.Parse(coordX.ToString());
                //    if (parsedCoordX >= Globals.GAME_FIELD_BOTTOM_RIGHT.X)
                //        break;
                //    parsedCoordY = int.Parse(coordY.ToString());
                //    if (parsedCoordY >= Globals.GAME_FIELD_BOTTOM_RIGHT.Y)
                //        break;
                //
                //    gameGrid[parsedCoordX, parsedCoordY] = new GameCell();
                //    TerrainType tt = Database.SearchTerrainDB(visualChar);
                //    gameGrid[parsedCoordX, parsedCoordY].Terrain = new TerrainType(tt.PositionInDB, tt);
                //    gameGrid[parsedCoordX, parsedCoordY].Terrain.X = parsedCoordX;
                //    gameGrid[parsedCoordX, parsedCoordY].Terrain.Y = parsedCoordY;
                //
                //    if (hasIngameObject)
                //    {
                //        gameGrid[parsedCoordX, parsedCoordY].IngameObject = inGObj;
                //        gameGrid[parsedCoordX, parsedCoordY].IngameObject.X = parsedCoordX;
                //        gameGrid[parsedCoordX, parsedCoordY].IngameObject.Y = parsedCoordY;
                //        hasIngameObject = false;
                //    }
                //}
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

             
        public static void SaveMap(GameCell[,] gameField)
        {
            StringBuilder parseMap = new StringBuilder();

            for (int x = 0; x < gameField.GetLength(0); x++)
            {
                for (int y = 0; y < gameField.GetLength(1); y++)
                {
                    parseMap.AppendFormat("[{0}", gameField[x, y].Terrain.PositionInDB);

                    if (gameField[x, y].IngameObject == null)
                    {
                        parseMap.Append("]");
                    }
                    else
                    {
                        parseMap.AppendFormat("<{0}>]", gameField[x, y].IngameObject.PositionInDB);
                    }
                }
            }

            StreamWriter sWriter = new StreamWriter(GameEngine.MapName, false, encoding);
            using (sWriter)
            {
                string nullString = null;
                sWriter.Write(nullString);  //creates a new file, overwrites old

                sWriter.Write(GameEngine.MapName);
                sWriter.Write(string.Format("[{0};{1}]", gameField.GetLength(0), gameField.GetLength(1)));
                sWriter.Write(parseMap.ToString());
            }
        }
    }
}
