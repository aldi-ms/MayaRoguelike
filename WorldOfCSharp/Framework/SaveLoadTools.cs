using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace WorldOfCSharp
{
    public static class SaveLoadTools
    {
        private const string SAVE_FILE_NAME = @"../../saves/save.wocs";
        private static readonly Encoding ENCODING = Encoding.ASCII;

        public static void SaveGame(Unit unit)
        {
            string currentMapName = LoadSavedMapName();
            if (!string.IsNullOrWhiteSpace(currentMapName))
            {
                StreamWriter sWriter = new StreamWriter(SAVE_FILE_NAME, false, ENCODING);
                using (sWriter)
                {
                    StringBuilder save = new StringBuilder();

                    sWriter.WriteLine(currentMapName);
                    save.AppendFormat("[{0};{1};{2};{3};{4};{5};{6}]", unit.X, unit.Y, unit.Flags, unit.Speed, unit.VisualChar, unit.Color, unit.Name);
                    sWriter.WriteLine(save.ToString());
                    //ConsoleTools.PrintDebugInfo("Game saved successfully.");
                }
            }
            else
            {
                throw new ArgumentException("On save time string currentMapName is null, empty, or contains only white spaces.");
            }
        }

        public static void SaveGame(Unit unit, string fileName)
        {
            string currentMapName = LoadSavedMapName();
            if (!string.IsNullOrWhiteSpace(currentMapName))
            {
                StreamWriter sWriter = new StreamWriter(fileName, false, ENCODING);
                using (sWriter)
                {
                    StringBuilder save = new StringBuilder();

                    sWriter.WriteLine(currentMapName);
                    save.AppendFormat("[{0};{1};{2};{3};{4};{5};{6}]", unit.X, unit.Y, unit.Flags, unit.Speed, unit.VisualChar, unit.Color, unit.Name);
                    sWriter.WriteLine(save.ToString());
                    //ConsoleTools.PrintDebugInfo("Game saved successfully.");
                }
            }
            else
            {
                throw new ArgumentException("On save time string currentMapName is null, empty, or contains only white spaces.");
            }
        }

        public static string LoadSavedMapName(string saveFileName = SAVE_FILE_NAME)
        {
            StreamReader sReader = new StreamReader(saveFileName, ENCODING);
            using (sReader)
            {
                return sReader.ReadLine();
            }
        }

        public static PlayerCharacter LoadSavedPlayerCharacter(string saveFileName = SAVE_FILE_NAME)
        {
            StreamReader sReader = new StreamReader(saveFileName, ENCODING);

            using (sReader)
            {
                sReader.ReadLine();
                char readChar = (char)sReader.Read();

                StringBuilder coordX = new StringBuilder(4);    //--> read X -coord
                readChar = (char)sReader.Read();
                do
                {
                    coordX.Append(readChar);
                    readChar = (char)sReader.Read();
                } while (readChar != ';');

                StringBuilder coordY = new StringBuilder(4);    //--> read Y -coord
                readChar = (char)sReader.Read();
                do
                {
                    coordY.Append(readChar);
                    readChar = (char)sReader.Read();
                } while (readChar != ';');

                StringBuilder flag = new StringBuilder();    //--> read flag
                readChar = (char)sReader.Read();
                do
                {
                    flag.Append(readChar);
                    readChar = (char)sReader.Read();
                } while (readChar != ';');

                StringBuilder speed = new StringBuilder();      //--> read unit speed
                readChar = (char)sReader.Read();
                do
                {
                    speed.Append(readChar);
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
                } while (readChar != ';');

                StringBuilder name = new StringBuilder();       //--> read name
                readChar = (char)sReader.Read();
                do
                {
                    name.Append(readChar);
                    readChar = (char)sReader.Read();
                } while (readChar != ']');

                //to a player character object >>>
                int parsedCoordX = int.Parse(coordX.ToString());
                int parsedCoordY = int.Parse(coordY.ToString());
                int parsedFlag = int.Parse(flag.ToString());
                int parsedSpeed = int.Parse(speed.ToString());
                ConsoleColor parsedColor = ConsoleTools.ParseColor(color.ToString());

                //ConsoleTools.PrintDebugInfo("Game successfully loaded.");
                return new PlayerCharacter(parsedCoordX, parsedCoordY, parsedFlag, parsedSpeed, visCh, parsedColor, name.ToString());
            }
        }
    }
}
