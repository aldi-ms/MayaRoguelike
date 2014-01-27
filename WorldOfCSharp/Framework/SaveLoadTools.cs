using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace WorldOfCSharp
{
    public static class SaveLoadTools
    {
        private const string UNITS_SAVE_FILE = @"../../saves/units.wocs";   //contains info on units, as well as the current map name
        private const string UNITS_INFO_FILE = @"../../saves/units_info.wocs";
        private static readonly Encoding ENCODING = Encoding.ASCII;

        //saves all basic unit info in UNITS_SAVE_FILE
        public static void SaveGame()
        {
            string currentMapName = LoadSavedMapName();
            if (!string.IsNullOrWhiteSpace(currentMapName))
            {
                StreamWriter unitFile = new StreamWriter(UNITS_SAVE_FILE, false, ENCODING);
                using (unitFile)
                {
                    unitFile.WriteLine(currentMapName);
                    unitFile.WriteLine(GameEngine.Ticks);

                    StringBuilder saveSB = new StringBuilder();
                    foreach (Unit unit in GameEngine.Units)
                    {
                        saveSB.AppendFormat("[{0};{1};{2};{3};{4};{5};{6}]", unit.X, unit.Y, unit.Flags, unit.VisualChar, unit.Color, unit.Name, unit.UniqueID);
                        unitFile.WriteLine(saveSB.ToString());
                        saveSB.Clear();
                    }
                }

                StreamWriter infoFile = new StreamWriter(UNITS_INFO_FILE, false, ENCODING);
                using (infoFile)
                {
                    StringBuilder saveSB = new StringBuilder();
                    foreach (Unit unit in GameEngine.Units)
                    {
                        saveSB.AppendFormat("[{0};{1}]", unit.UniqueID, unit.Stats.ToString());
                        infoFile.WriteLine(saveSB.ToString());
                        saveSB.Clear();
                    }
                }
            }
            else
                throw new ArgumentException("On save time string currentMapName is null, empty, or contains only white spaces.");
        }

        public static void SaveGame(string fileName)
        {
            string currentMapName = LoadSavedMapName();
            const string TEMP_INFO_FILE = @"../../saves/temp_info.wocs";

            if (!string.IsNullOrWhiteSpace(currentMapName))
            {
                StreamWriter file = new StreamWriter(fileName, false, ENCODING);
                using (file)
                {
                    file.WriteLine(currentMapName);
                    file.WriteLine(GameEngine.Ticks);

                    StringBuilder saveSB = new StringBuilder();
                    foreach (Unit unit in GameEngine.Units)
                    {
                        saveSB.AppendFormat("[{0};{1};{2};{3};{4};{5};{6}]", unit.X, unit.Y, unit.Flags, unit.VisualChar, unit.Color, unit.Name, unit.UniqueID);
                        file.WriteLine(saveSB.ToString());
                        saveSB.Clear();
                    }
                }

                StreamWriter infoFile = new StreamWriter(TEMP_INFO_FILE, false, ENCODING);
                using (infoFile)
                {
                    StringBuilder saveSB = new StringBuilder();

                    foreach (Unit unit in GameEngine.Units)
                    {
                        saveSB.AppendFormat("[{0};{1}]", unit.UniqueID, unit.Stats.ToString());
                        infoFile.WriteLine(saveSB.ToString());
                        saveSB.Clear();
                    }
                }
            }
            else
                throw new ArgumentException("On save time string currentMapName is null, empty, or contains only white spaces.");
        }

        public static string LoadSavedMapName(string saveFileName = UNITS_SAVE_FILE)
        {
            string mapName;
            StreamReader sReader = new StreamReader(saveFileName, ENCODING);

            using (sReader)
                mapName = sReader.ReadLine();

            return mapName;
        }

        public static Unit LoadUnits(string unitSaveFile = UNITS_SAVE_FILE)
        {
            StreamReader unitFile = new StreamReader(unitSaveFile, ENCODING);

            using (unitFile)
            {
                Unit unit;
                Unit pc = new Unit(0,0,0,'\0', ConsoleColor.Red, "temp"); 
                int readInt = unitFile.Peek();
                StreamReader infoFile = new StreamReader(UNITS_INFO_FILE, ENCODING);

                using (infoFile)
                {
                    while (readInt != -1 && readInt != 13)
                    {
                        unitFile.ReadLine();
                        GameEngine.Ticks = int.Parse(unitFile.ReadLine());
                        GameEngine.GameTime.Tick(GameEngine.Ticks);
                        char readChar = (char)unitFile.Read();

                        StringBuilder coordX = new StringBuilder(4);    //--> read X -coord
                        readChar = (char)unitFile.Read();
                        do
                        {
                            coordX.Append(readChar);
                            readChar = (char)unitFile.Read();
                        } while (readChar != ';');

                        StringBuilder coordY = new StringBuilder(4);    //--> read Y -coord
                        readChar = (char)unitFile.Read();
                        do
                        {
                            coordY.Append(readChar);
                            readChar = (char)unitFile.Read();
                        } while (readChar != ';');

                        StringBuilder flag = new StringBuilder();    //--> read flag
                        readChar = (char)unitFile.Read();
                        do
                        {
                            flag.Append(readChar);
                            readChar = (char)unitFile.Read();
                        } while (readChar != ';');

                        char visCh = '\0';      //--> read visCh
                        visCh = (char)unitFile.Read();
                        readChar = (char)unitFile.Read();

                        StringBuilder color = new StringBuilder();          //--> read color
                        readChar = (char)unitFile.Read();
                        do
                        {
                            color.Append(readChar);
                            readChar = (char)unitFile.Read();
                        } while (readChar != ';');

                        StringBuilder name = new StringBuilder();       //--> read name
                        readChar = (char)unitFile.Read();
                        do
                        {
                            name.Append(readChar);
                            readChar = (char)unitFile.Read();
                        } while (readChar != ';');

                        StringBuilder unitID = new StringBuilder();       //--> read ID from basic unit file
                        readChar = (char)unitFile.Read();
                        do
                        {
                            unitID.Append(readChar);
                            readChar = (char)unitFile.Read();
                        } while (readChar != ']');

                        StringBuilder infoID = new StringBuilder();       //--> read ID from unit info file
                        readChar = (char)infoFile.Read();
                        readChar = (char)infoFile.Read();
                        do
                        {
                            infoID.Append(readChar);
                            readChar = (char)infoFile.Read();
                        } while (readChar != ';');

                        //to a player character object >>>
                        int parsedCoordX = int.Parse(coordX.ToString());
                        int parsedCoordY = int.Parse(coordY.ToString());
                        int parsedFlag = int.Parse(flag.ToString());
                        int parsedUnitID = int.Parse(unitID.ToString());
                        int parsedInfoID = int.Parse(infoID.ToString());
                        ConsoleColor parsedColor = ConsoleTools.ParseColor(color.ToString());

                        if (parsedUnitID == parsedInfoID)
                        {
                            unit = new Unit(parsedCoordX, parsedCoordY, parsedFlag, visCh, parsedColor, name.ToString(), parsedUnitID);

                            StringBuilder strength = new StringBuilder();
                            readChar = (char)infoFile.Read();
                            do
                            {
                                strength.Append(readChar);
                                readChar = (char)infoFile.Read();
                            } while (readChar != ';');

                            StringBuilder dexterity = new StringBuilder();
                            readChar = (char)infoFile.Read();
                            do
                            {
                                dexterity.Append(readChar);
                                readChar = (char)infoFile.Read();
                            } while (readChar != ';');

                            StringBuilder stamina = new StringBuilder();
                            readChar = (char)infoFile.Read();
                            do
                            {
                                stamina.Append(readChar);
                                readChar = (char)infoFile.Read();
                            } while (readChar != ';');

                            StringBuilder intelligence = new StringBuilder();
                            readChar = (char)infoFile.Read();
                            do
                            {
                                intelligence.Append(readChar);
                                readChar = (char)infoFile.Read();
                            } while (readChar != ';');

                            StringBuilder spirit = new StringBuilder();
                            readChar = (char)infoFile.Read();
                            do
                            {
                                spirit.Append(readChar);
                                readChar = (char)infoFile.Read();
                            } while (readChar != ';');

                            StringBuilder currentHP = new StringBuilder();
                            readChar = (char)infoFile.Read();
                            do
                            {
                                currentHP.Append(readChar);
                                readChar = (char)infoFile.Read();
                            } while (readChar != ';');

                            StringBuilder actionSpeed = new StringBuilder();
                            readChar = (char)infoFile.Read();
                            do
                            {
                                actionSpeed.Append(readChar);
                                readChar = (char)infoFile.Read();
                            } while (readChar != ']');

                            unit.Stats.Strength = int.Parse(strength.ToString());
                            unit.Stats.Dexterity = int.Parse(dexterity.ToString());
                            unit.Stats.Stamina = int.Parse(stamina.ToString());
                            unit.Stats.Intelligence = int.Parse(intelligence.ToString());
                            unit.Stats.Spirit = int.Parse(spirit.ToString());
                            unit.Stats.CurrentHP = int.Parse(currentHP.ToString());
                            unit.Stats.ActionSpeed = int.Parse(actionSpeed.ToString());

                            GameEngine.AddUnit(unit);
                            if (unit.GetFlag(4))
                                pc = unit;
                        }

                        readInt = unitFile.Peek();
                    }
                }

                return pc;
            }
        }
    }
}
