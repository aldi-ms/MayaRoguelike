using System;
using System.Collections.Generic;
using System.Text;

namespace WorldOfCSharp
{
    public static class UIElements
    {
        private static int maxLengthOption = 0;

        public static void MainMenu()
        {
            ConsoleTools.ClearAndResetConsole();

            Coordinate menuCoords = new Coordinate(4, 2);
            List<string> menuOptions = new List<string> { "new game", "load game", "options", "exit" };
            
            foreach (string menuOption in menuOptions)
            {
                ConsoleTools.WriteOnPosition(menuOption.ToLower(), menuCoords.X, menuCoords.Y);
                char firstChar = char.Parse(menuOption[0].ToString().ToLower());
                ConsoleTools.WriteOnPosition(firstChar, menuCoords.X, menuCoords.Y, foregroundColor: ConsoleColor.Yellow);
                menuCoords.Y += 2;
                if (menuOption.Length > maxLengthOption)
                    maxLengthOption = menuOption.Length;
            }

            ConsoleKeyInfo key = Console.ReadKey(true);
            do
            {
                switch (key.Key)
                {
                    case ConsoleKey.N:
                        GameEngine.New();
                        break;

                    case ConsoleKey.L:
                        GameEngine.Load();
                        break;

                    case ConsoleKey.E:
                    case ConsoleKey.Escape:
                        ConsoleTools.Quit();
                        break;

                    default:
                        break;
                }
                key = Console.ReadKey(true);
            }
            while (key.KeyChar != 'n' || key.KeyChar != 'l');
        }

        public static string PromptForName()
        {
            Coordinate dialogCoords = new Coordinate(6 + maxLengthOption, 2);
            ConsoleTools.WriteOnPosition(">>>", dialogCoords.X, dialogCoords.Y);
            ConsoleTools.WriteOnPosition("What's your name?", dialogCoords.X + 6, dialogCoords.Y, ConsoleColor.Cyan);
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(dialogCoords.X + 6, dialogCoords.Y + 2);
            return Console.ReadLine();
        }
        
        public static void InGameUI()
        {
            ConsoleTools.ClearAndResetConsole();

            for (int x = 0; x < Globals.CONSOLE_WIDTH; x++)
            {
                if (x < Globals.GAME_FIELD_BOTTOM_RIGHT.X)
                {
                    ConsoleTools.WriteOnPosition('\u2550', x, Globals.GAME_FIELD_BOTTOM_RIGHT.Y);
                }
                else
                {
                    ConsoleTools.WriteOnPosition(' ', x, Globals.GAME_FIELD_BOTTOM_RIGHT.Y);      //without this the horizontal \(O.O)/
                }                                                                           //line symbols get space between them  >>thefuck.jpg
            }

            for (int y = 0; y < Globals.CONSOLE_HEIGHT; y++)
            {
                ConsoleTools.WriteOnPosition('\u2551', Globals.GAME_FIELD_BOTTOM_RIGHT.X, y);

                if (y == Globals.GAME_FIELD_BOTTOM_RIGHT.Y)
                {
                    ConsoleTools.WriteOnPosition('\u2563', Globals.GAME_FIELD_BOTTOM_RIGHT.X, y);
                }
            }

        }
    }
}
