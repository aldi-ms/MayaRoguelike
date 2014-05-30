using System;
using System.IO;
using System.Text;
using MT19937;
using Maya.Framework;

namespace Maya
{
    public static class MapTools
    {
        private static readonly Encoding encoding = Encoding.ASCII;

        public static FlatArray<GameCell> LoadMap(string mapFileName)
        {
            FlatArray<GameCell> gameGrid = new FlatArray<GameCell>(Globals.GAME_FIELD_BOTTOM_RIGHT.X, Globals.GAME_FIELD_BOTTOM_RIGHT.Y);
            Database.LoadDatabase();

            using (var sReader = new StreamReader(mapFileName, encoding))
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
                return gameGrid;

            }
        }

        public static void SaveMap(FlatArray<GameCell> gameField)
        {
            StringBuilder parseMap = new StringBuilder();

            for (int x = 0; x < gameField.Width; x++)
            {
                for (int y = 0; y < gameField.Height; y++)
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

            using (var sWriter = new StreamWriter(string.Format(@"../../maps/{0}.wocm", GameEngine.MapID), false, encoding))
            {
                sWriter.Write(GameEngine.MapName);
                sWriter.Write(string.Format("[{0};{1}]", gameField.Height, gameField.Width));
                sWriter.Write(parseMap.ToString());
            }
        }
    }
}
