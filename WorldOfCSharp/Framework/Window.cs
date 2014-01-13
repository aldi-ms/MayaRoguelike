﻿using System;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace WorldOfCSharp
{
    public sealed class Window
    {
        private const string TEMP_SAVE_FILE = @"../../saves/temp.wocs";
        private PlayerCharacter pc;
        private string title;
        private int windowBottomLeftX = 0;
        private int windowBottomLeftY = Globals.CONSOLE_HEIGHT - (Globals.CONSOLE_HEIGHT - Globals.GAME_FIELD_BOTTOM_RIGHT.Y);
        private int windowWidth = Globals.GAME_FIELD_BOTTOM_RIGHT.X;
        private int windowHeight = Globals.CONSOLE_HEIGHT - (Globals.CONSOLE_HEIGHT - Globals.GAME_FIELD_BOTTOM_RIGHT.Y);
        private int windowMargin = 3;
        private Coordinate topLeft, topRight, bottomLeft, bottomRight;  //window frame coordinates!
        private int linePosition;

        //constructor using the default values, window over the whole game field
        public Window(PlayerCharacter pc, string title)
        {
            this.pc = new PlayerCharacter(pc);
            this.title = title.ToUpper();

            //save window coordinates
            this.bottomLeft = new Coordinate((windowBottomLeftX + windowMargin) - 1, windowBottomLeftY - windowMargin);
            this.topLeft = new Coordinate((windowHeight - windowBottomLeftY) + windowMargin - 1, windowBottomLeftX + windowMargin);
            this.bottomRight = new Coordinate((windowBottomLeftX + windowWidth) - windowMargin, windowBottomLeftY - windowMargin);
            this.topRight = new Coordinate((windowBottomLeftX + windowWidth) - windowMargin, (windowHeight - windowBottomLeftY) + windowMargin);
            this.linePosition = this.TopLeft.Y + 2;
        }
        
        //constructor for manually setting the values
        public Window(PlayerCharacter pc, string title, int windowBottomLeftX, int windowBottomLeftY, int windowWidth, int windowHeight)
        {
            this.pc = new PlayerCharacter(pc);
            this.title = title.ToUpper();

            if (windowBottomLeftX <= Globals.CONSOLE_WIDTH && windowBottomLeftX >= 0)
            {
                this.windowBottomLeftX = windowBottomLeftX;
            }
            else
            {
                throw new ArgumentOutOfRangeException(string.Format("windowBottomLeftX = {0}. windowBottomLeftX should be in range 0 - {1}.",
                    windowBottomLeftX, Globals.CONSOLE_WIDTH));
            }

            if (windowBottomLeftY <= Globals.CONSOLE_HEIGHT && windowBottomLeftY > 0)
            {
                this.windowBottomLeftY = windowBottomLeftY;
            }
            else
            {
                throw new ArgumentOutOfRangeException(string.Format("windowBottomLeftY = {0}. windowBottomLeftY should be in range 0 - {1}.",
                    windowBottomLeftY, Globals.CONSOLE_HEIGHT));
            }

            if (windowWidth <= Globals.CONSOLE_WIDTH && windowWidth >= 0)
            {
                this.windowWidth = windowWidth;
            }
            else
            {
                throw new ArgumentOutOfRangeException(string.Format("windowWidth = {0}. windowWidth should be in range 0 - {1}.",
                    windowWidth, Globals.CONSOLE_WIDTH));
            }

            if (windowHeight <= Globals.CONSOLE_HEIGHT && windowHeight > 0)
            {
                this.windowHeight = windowHeight;
            }
            else
            {
                throw new ArgumentOutOfRangeException(string.Format("windowHeight = {0}. windowHeight should be in range 0 - {1}.",
                    windowHeight, Globals.CONSOLE_HEIGHT));
            }

            this.linePosition = this.TopLeft.Y + 1;
        }

        public Coordinate TopLeft
        {
            get { return this.topLeft; }
        }

        public Coordinate TopRight
        {
            get { return this.topRight; }
        }
        
        public Coordinate BottomLeft
        {
            get { return this.bottomLeft; }
        }

        public Coordinate BottomRight
        {
            get { return this.bottomRight; }
        }

        public Coordinate BottomLeftAnchor
        {
            get { return new Coordinate(this.windowBottomLeftX, windowBottomLeftY); }
        }

        public int WindowWidth
        {
            get { return this.windowWidth; }
        }

        public int WindowHeight
        {
            get { return this.windowHeight; }
        }

        public int WindowMargin
        {
            get { return this.windowMargin; }
        }

        public void Show()
        {
            SaveGame(pc);
            ConsoleTools.Clear(windowBottomLeftX, windowBottomLeftY, windowWidth, windowHeight);
            DrawWindow();
            ConsoleTools.PrintDebugInfo(string.Format("Window \"{0}\" opened.", title));
        }

        public bool Write(string str, ConsoleColor color = ConsoleColor.Gray)
        {
            linePosition++;
            if (!(linePosition >= BottomLeft.Y - 2))
            {
                ConsoleTools.WriteOnPosition(str, (windowHeight - windowBottomLeftY) + (windowMargin + 2),
                    windowBottomLeftX + (linePosition - 1), color);
                return true;
            }
            return false;
        }

        //temporarily save the game to file other than the main save
        private void SaveGame(PlayerCharacter pc)    //pause
        {
            SaveLoadTools.SaveGame(pc, TEMP_SAVE_FILE);
        }

        //draw the window over the game field
        private void DrawWindow()
        {
            //bottom left corner
            ConsoleTools.WriteOnPosition('\u255a', (windowBottomLeftX + windowMargin) - 1, windowBottomLeftY - windowMargin, ConsoleColor.Cyan);

            //bottom side
            for (int y = windowBottomLeftX + windowMargin; y < (windowBottomLeftX + windowWidth) - windowMargin; y++)
            {
                ConsoleTools.WriteOnPosition('\u2550', y, windowBottomLeftY - windowMargin, ConsoleColor.Cyan);
            }

            //bottom right corner
            ConsoleTools.WriteOnPosition('\u255d', (windowBottomLeftX + windowWidth) - windowMargin, windowBottomLeftY - windowMargin, ConsoleColor.Cyan);

            //right side
            for (int x = windowBottomLeftY - (windowMargin + 1); x >= windowMargin + 1; x--)
            {
                ConsoleTools.WriteOnPosition('\u2551', (windowBottomLeftX + windowWidth) - windowMargin, x, ConsoleColor.Cyan);
            }

            //top right corner
            ConsoleTools.WriteOnPosition('\u2557', (windowBottomLeftX + windowWidth) - windowMargin, (windowHeight - windowBottomLeftY) + windowMargin, ConsoleColor.Cyan);

            //top side
            for (int y = (windowBottomLeftX + windowWidth) - (windowMargin + 1); y >= (windowBottomLeftY - windowHeight) + windowMargin; y--)
            {
                ConsoleTools.WriteOnPosition('\u2550', y, (windowHeight - windowBottomLeftY) + windowMargin, ConsoleColor.Cyan);
            }

            //top left corner
            ConsoleTools.WriteOnPosition('\u2554', (windowHeight - windowBottomLeftY) + windowMargin - 1, windowBottomLeftX + windowMargin, ConsoleColor.Cyan);

            //left side
            for (int x = (windowBottomLeftY - windowHeight) + windowMargin + 1; x < windowBottomLeftY - windowMargin; x++)
            {
                ConsoleTools.WriteOnPosition('\u2551', windowBottomLeftX + windowMargin - 1, x, ConsoleColor.Cyan);
            }

            //write window title
            StringBuilder SBTitle = new StringBuilder();
            for (int i = 0; i < title.Length; i++)
            {
                SBTitle.Append(title[i]);
                SBTitle.Append(' ');
            }

            int titleX = (windowWidth - SBTitle.Length) / 2;
            int titleY = ((windowHeight - windowBottomLeftY) + windowMargin) - 1;

            ConsoleTools.WriteOnPosition(SBTitle.ToString(), titleX, titleY, ConsoleColor.Yellow);
        }
       
        public void CloseWindow()
        {
            ConsoleTools.Clear(windowBottomLeftX, windowBottomLeftY, windowWidth, windowHeight);
            PlayerCharacter pc = new PlayerCharacter(SaveLoadTools.LoadSavedPlayerCharacter(TEMP_SAVE_FILE));
            GameCell[,] gameField = MapTools.LoadMap(SaveLoadTools.LoadSavedMapName(TEMP_SAVE_FILE));       //load map<<<<<<<<
            File.Delete(TEMP_SAVE_FILE);
            GameEngine.VisualEngine.PrintFOVMap(pc.X, pc.Y);
            GameEngine.VisualEngine.PrintUnit(pc);
        }
    }
}
       