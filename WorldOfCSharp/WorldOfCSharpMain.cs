using System;
using System.Text;
using System.Threading;
using System.Globalization;
using MT19937;
using WorldOfCSharp.Tests;

namespace WorldOfCSharp
{
    class WorldOfCSharpMain
    {
        static void Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-GB");
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "World of C#";
            ConsoleTools.ResizeConsole(Globals.CONSOLE_WIDTH, Globals.CONSOLE_HEIGHT);
            ConsoleTools.ClearAndResetConsole();
            Console.CursorVisible = false;
            
            UIElements.MainMenu();
        }
    }
}