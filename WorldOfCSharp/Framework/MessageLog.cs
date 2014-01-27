using System;
using System.Text;
using System.Collections.Generic;

namespace WorldOfCSharp
{
    public sealed class MessageLog
    {
        private Coordinate bottomLeftAnchor;
        private int width;      //in characters
        private int height;     //in rows
        private StringBuilder[] line;
        private StringBuilder emptySB;
        private Coordinate[] lineCoordinates;
        private readonly Encoding ENCODING = Encoding.ASCII;
        private const string LOG_FILE = @"..\..\saves\history.log";

        public MessageLog(int bottomLeftAnchorX, int bottomLeftAnchorY, int width, int height)
        {
            this.bottomLeftAnchor = new Coordinate(bottomLeftAnchorX, bottomLeftAnchorY);
            this.width = width;     //x -axis
            this.height = height;   //y -axis
        }

        public MessageLog(int bottomLeftAnchorX, int bottomLeftAnchorY, int width, int height, bool initializeNow)
            : this(bottomLeftAnchorX, bottomLeftAnchorY, width, height)
        {
            if (initializeNow)
                Initialize();
        }

        public void DeleteLog()
        {
            System.IO.File.Delete(LOG_FILE);
        }

        public void Initialize()
        {
            emptySB = new StringBuilder(width);
            emptySB.Append(' ', width);

            line = new StringBuilder[height];
            for (int i = 0; i < height; i++)
            {
                line[i] = new StringBuilder(width);
            }

            lineCoordinates = new Coordinate[height];
            for (int i = height - 1; i >= 0; i--)
            {
                lineCoordinates[i] = new Coordinate(bottomLeftAnchor.X, bottomLeftAnchor.Y - i);
            }
        }

        public void SendMessage(string text)
        {
            if (text.Length <= width)
            {
                for (int i = height - 1; i > 0; i--)
                {
                    line[i].Clear();
                    WriteOnPosition(emptySB, lineCoordinates[i]);
                    line[i].Append(line[i - 1]);
                }

                line[0].Clear();
                line[0].Append(text);
                PrintMessageLog();
                WriteLogFile(text);
            }
            else
            {
                string[] splitText = text.Split(' ');
                int firstUnappendedString = 0;
                StringBuilder firstPartText = new StringBuilder(width);

                for (int i = firstUnappendedString; i < splitText.Length; i++)
                {
                    if (!(firstPartText.Length + (splitText[i].Length + 1) > width))
                    {
                        firstPartText.Append(splitText[i]);
                        firstPartText.Append(" ");
                    }
                    else
                    {
                        firstUnappendedString = i;
                        break;
                    }
                }

                StringBuilder secondPartText = new StringBuilder(width);
                for (int i = firstUnappendedString; i < splitText.Length; i++)
                {
                    secondPartText.AppendFormat("{0} ", splitText[i]);
                }

                SendMessage(firstPartText.ToString());
                SendMessage(secondPartText.ToString());
            }
        }


        public void ClearLog()
        {
            for (int i = 0; i < height; i++)
            {
                line[i].Clear();
            }

            PrintMessageLog();
        }

        public void ShowLogFile(Unit pc)
        {
            //create a window
            Window logWindow = new Window(pc, "log");
            logWindow.Show();

            System.IO.StreamReader sReader = new System.IO.StreamReader(LOG_FILE, ENCODING);

            using (sReader)
            {
                while (sReader.Peek() != -1)
                {
                    logWindow.Write(sReader.ReadLine());
                }
            }
                ConsoleKeyInfo key;
                bool loop = true;
                do
                {
                    key = Console.ReadKey(true);
                    switch (key.Key)
                    {
                        case ConsoleKey.Escape:
                            loop = false;
                            logWindow.CloseWindow();
                            break;

                        default:
                            break;
                    }
                } while(loop);
            
        }

        private void WriteLogFile(string text)
        {
            System.IO.StreamWriter sWriter = new System.IO.StreamWriter(LOG_FILE, true, ENCODING);
            using (sWriter)
            {
                sWriter.WriteLine("[{0}] {1}", DateTime.Now, text);                
            }
        }

        private void PrintMessageLog()
        {
            WriteOnPosition(emptySB, lineCoordinates[0]);
            WriteOnPosition(line[0], lineCoordinates[0], ConsoleColor.White);

            for (int i = 1; i < height; i++)
            {
                WriteOnPosition(emptySB, lineCoordinates[i]);
                WriteOnPosition(line[i], lineCoordinates[i]);
            }
        }

        private void WriteOnPosition(
            StringBuilder text, Coordinate coords, 
            ConsoleColor foregroundColor = ConsoleColor.DarkGray, 
            ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Console.SetCursorPosition(coords.X, coords.Y);
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.Write(text.ToString());
        }
    }
}
