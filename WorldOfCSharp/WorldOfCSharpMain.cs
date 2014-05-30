using System;
using System.Globalization;
using System.Text;
using System.Threading;

namespace Maya
{
    internal class MayaMain
    {
        private static void Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-GB");
            Console.OutputEncoding = Encoding.UTF8;
            Console.Title = "Maya";
            ConsoleTools.ResizeConsole(Globals.CONSOLE_WIDTH, Globals.CONSOLE_HEIGHT);
            ConsoleTools.ClearAndResetConsole();
            Console.CursorVisible = false;

            UIElements.MainMenu();
            //Tests.Testing.StatTesting();
        }
    }
}