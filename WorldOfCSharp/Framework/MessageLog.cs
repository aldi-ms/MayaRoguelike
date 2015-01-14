using System;
using System.Text;
using System.Collections.Generic;

namespace Maya
{
    public sealed class MessageLog
    {
        private const string LOG_FILE = @"..\..\saves\history.log";
        private static readonly Encoding ENCODING = Encoding.ASCII;
        private int width;      //in characters
        private int height;     //in rows
        private Coordinate bottomLeftAnchor;
        private StringBuilder[] line;
        private StringBuilder emptySB;
        private Coordinate[] lineCoordinates;

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
                line[i] = new StringBuilder(width);

            lineCoordinates = new Coordinate[height];
            for (int i = height - 1; i >= 0; i--)
                lineCoordinates[i] = new Coordinate(bottomLeftAnchor.X, bottomLeftAnchor.Y - i);
        }

        public void SendMessage(string text, ConsoleColor color = ConsoleColor.DarkGray)
        {
            if (text.Length <= width)
            {
                for (int i = height - 1; i > 0; i--)
                {
                    line[i].Clear();
                    line[i].Append(line[i - 1] + new string(' ', width - line[i - 1].Length));
                }

                line[0].Clear();
                line[0].Append(text);
                
                PrintMessageLog();
                WriteLogFile(text);     //make log file save on game save instead of every message?
            }
                //else block also present (copied from here) in Window.Write
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
                    secondPartText.AppendFormat("{0} ", splitText[i]);

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

            using (var sReader = new System.IO.StreamReader(LOG_FILE, ENCODING))
            {
                while (sReader.Peek() != -1)
                    logWindow.Write(sReader.ReadLine());
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
            } while (loop);
        }

        private void WriteLogFile(string text)
        {
            using (var sWriter = new System.IO.StreamWriter(LOG_FILE, true, ENCODING))
                sWriter.WriteLine("[{0}] {1}", GameEngine.GameTime.Now.TimeToString(), text);   
        }

        private void PrintMessageLog()
        {
            for (int i = 0; i < height; i++)
            {
                FormatWriteOnPosition(emptySB, lineCoordinates[i]);
                FormatWriteOnPosition(line[i], lineCoordinates[i]);
            }
        }

        /*private void WriteOnPosition(
            StringBuilder text, Coordinate coords, 
            ConsoleColor foregroundColor = ConsoleColor.DarkGray, 
            ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Console.SetCursorPosition(coords.X, coords.Y);
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.Write(text.ToString());
        }*/
        /// <summary>
        /// Gives the ability to select color letter/word/whole string in a color chosen with a code in the string.
        /// For more detailed information check coloredMessagesDesign.txt
        /// </summary>
        private void FormatWriteOnPosition(
            StringBuilder text, Coordinate coords,
            ConsoleColor foregroundColor = ConsoleColor.DarkGray,
            ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Console.SetCursorPosition(coords.X, coords.Y);
            string workText = text.ToString();
            char selector = '\0';
            string color = null;
            //bool colorText = false;

            for (int i = 0; i < workText.Length; i++)
            {
                if (workText[i] == '~')
                {
                    //beggining of a escape formatting sequence
                    //colorText = true;
                    selector = workText[++i];
                    color = workText[++i].ToString();
                    if (workText[++i] == '!')
                        i++;
                    else if (char.IsDigit(workText[i]))
                    {
                        color = string.Format("{0}{1}", color, workText[i]);
                        i += 2;
                    }
                    else
                    {
                        SendMessage("Wrong format sequence in MessageLog. Terminating FormatWriteOnPosition method.");
                        return;
                    }

                    //select text to color
                    string selectedText = null;
                    switch (selector)
                    {
                        case 'L':
                        case 'l':
                            selectedText = workText[i].ToString();
                            i--;
                            break;

                        case 'W':
                        case 'w':
                            var sb = new StringBuilder(10);
                            for (int j = i; j < workText.Length; j++)
                            {
                                if (workText[j] != ' ' && workText[j] != '\n')
                                    sb.Append(workText[j]);
                                else
                                    break;
                            }
                            selectedText = sb.ToString();
                            i--;
                            break;

                        case 'S':
                        case 's':
                            selectedText = new string(workText.ToCharArray(), i, workText.Length - i);
                            i--;
                            break;
                    }

                    int col = int.Parse(color.ToString());
                    if (col >= 0 && col <= 15)
                        Console.ForegroundColor = (ConsoleColor)col;
                    else
                        Console.ForegroundColor = foregroundColor;

                    Console.BackgroundColor = backgroundColor;
                    Console.Write(selectedText);
                    i += selectedText.Length;
                }
                else
                {
                    Console.ForegroundColor = foregroundColor;
                    Console.BackgroundColor = backgroundColor;
                    Console.Write(workText[i]);
                }
            }
        }
    }
}
