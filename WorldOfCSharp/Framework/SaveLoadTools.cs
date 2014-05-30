using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Maya
{
    public static class SaveLoadTools
    {
        private const string UNITS_SAVE_FILE = @"../../saves/units.wocs";   //contains info on units, as well as the current map name
        private const string UNITS_INFO_FILE = @"../../saves/units_info.wocs";
        private static readonly Encoding ENCODING = Encoding.ASCII;

        public static void SaveGameToXML()
        {
            XDocument units = new XDocument(
                new XElement("Units")
                );
            XElement unitsElement = units.Element("Units");
            foreach (var unit in GameEngine.Units)
            {
                unitsElement.Add(new XElement("Unit", 
                    new XAttribute("x", unit.X),
                    new XAttribute("y", unit.Y),
                    new XAttribute("z", unit.Z),
                    new XAttribute("flags", (int)unit.Flags),
                    new XAttribute("char", unit.VisualChar),
                    new XAttribute("color", unit.Color.ToString()),
                    new XAttribute("name", unit.Name),
                    new XAttribute("id", unit.UniqueID),
                    new XElement("stats") //re-work stats first
                    ));
            }
            units.Save(@"../../saves/units.xml");
        }

        //saves all basic unit info in UNITS_SAVE_FILE
        public static void SaveGame()
        {
            if (string.IsNullOrWhiteSpace(GameEngine.MapFileName))
                GameEngine.MapFileName = LoadMapID();

            using (var unitFile = new StreamWriter(UNITS_SAVE_FILE, false, ENCODING))
            {
                unitFile.WriteLine(GameEngine.MapID);
                unitFile.WriteLine(GameEngine.GameTime.Ticks);

                StringBuilder saveSB = new StringBuilder();
                foreach (Unit unit in GameEngine.Units)
                {
                    saveSB.AppendFormat("[{0};{1};{2};{3};{4};{5};{6}]", unit.X, unit.Y, (int)unit.Flags, unit.VisualChar, unit.Color, unit.Name, unit.UniqueID);
                    unitFile.WriteLine(saveSB.ToString());
                    saveSB.Clear();
                }
            }

            using (var infoFile = new StreamWriter(UNITS_INFO_FILE, false, ENCODING))
            {
                StringBuilder saveSB = new StringBuilder();
                foreach (Unit unit in GameEngine.Units)
                {
                    saveSB.AppendFormat("[{0};{1}]", unit.UniqueID, unit.UnitStats.ToString());
                    infoFile.WriteLine(saveSB.ToString());
                    saveSB.Clear();
                }
            }
        }

        public static string LoadMapID(string saveFileName = UNITS_SAVE_FILE)
        {
            using (var sReader = new StreamReader(saveFileName, ENCODING))
                return string.Format(@"../../maps/{0}.wocm", sReader.ReadLine());
        }

        public static List<Unit> LoadUnits(string unitSaveFile = UNITS_SAVE_FILE)
        {
            using (var unitFile = new StreamReader(unitSaveFile, ENCODING))
            {
                GameEngine.MapFileName = unitFile.ReadLine();
                GameEngine.GameTime = new GameTime(int.Parse(unitFile.ReadLine()));

                List<Unit> loadedUnits = new List<Unit>();
                int readInt = unitFile.Peek();

                using (var infoFile = new StreamReader(UNITS_INFO_FILE, ENCODING))
                {
                    char readChar;
                    while (readInt > -1) //at eof returns 13 (CR) for some reason...
                    {
                        while ((readChar = (char)unitFile.Read()) == '\r' || readChar == '\n' || readChar == '[') ;
                        if ((int)readChar > 255) break;
                        StringBuilder coordX = new StringBuilder(4);    //--> read X -coord
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

                        while ((readChar = (char)infoFile.Read()) == '\n' || readChar == '\r' || readChar == '[') ;
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
                            Unit unit = new Unit(parsedCoordX, parsedCoordY, (Flags)parsedFlag, visCh, parsedColor, name.ToString(), parsedUnitID);

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

                            StringBuilder constitution = new StringBuilder();
                            readChar = (char)infoFile.Read();
                            do
                            {
                                constitution.Append(readChar);
                                readChar = (char)infoFile.Read();
                            } while (readChar != ';');

                            StringBuilder wisdom = new StringBuilder();
                            readChar = (char)infoFile.Read();
                            do
                            {
                                wisdom.Append(readChar);
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

                            unit.UnitStats[0] = int.Parse(strength.ToString());
                            unit.UnitStats[1] = int.Parse(dexterity.ToString());
                            unit.UnitStats[2] = int.Parse(constitution.ToString());
                            unit.UnitStats[3] = int.Parse(wisdom.ToString());
                            unit.UnitStats[4] = int.Parse(spirit.ToString());
                            unit.UnitStats.CurrentHealth = int.Parse(currentHP.ToString());
                            //unit.UnitStats.ActionSpeed = int.Parse(actionSpeed.ToString());

                            loadedUnits.Add(unit);
                        }

                        readInt = unitFile.Peek();
                    }
                }

                return loadedUnits;
            }
        }
    }
}
