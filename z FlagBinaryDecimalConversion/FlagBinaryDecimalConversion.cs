using System;

namespace FlagConverter
{
    public static class FlagBinaryDecimalConversion
    {
        public static void ShowBinary(int flag)
        {
            string binaryFlags = Convert.ToString(flag, 2);
            Console.WriteLine("Flag {0}: {1}\n", flag, binaryFlags);
            for (int i = 0; i < binaryFlags.Length; i++)
            {
                Console.Write("{0}", i);
            }

            Console.WriteLine();
            for (int k = binaryFlags.Length - 1; k >= 0; k--)
            {
                Console.Write("{0}", binaryFlags[k]);
            }
            Console.WriteLine();
            ShowActiveFlags(binaryFlags);
        }

        public static void ShowDecimal(string flags)
        {
            int decFlag = Convert.ToInt32(flags, 2);
            Console.WriteLine("Flag {0}: {1}\n", flags, decFlag);
        }

        public static void ShowActiveFlags(string binaryFlags)
        {
            Console.WriteLine("\nActive Flags (==true):");

            string str = new string('0', 32 - binaryFlags.Length);
            binaryFlags = str + binaryFlags;

            if (binaryFlags[binaryFlags.Length - 1] == '1')
            {
                Console.WriteLine("Is Movable");
            }
            if (binaryFlags[binaryFlags.Length - 2] == '1')
            {
                Console.WriteLine("Is Collidable");
            }
            if (binaryFlags[binaryFlags.Length - 3] == '1')
            {
                Console.WriteLine("Has Effect");
            }
            if (binaryFlags[binaryFlags.Length - 4] == '1')
            {                
                Console.WriteLine("Is Transparent");
            }
            Console.WriteLine("\n");
        }

        public static void Main()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.CursorVisible = false;
            Console.WriteLine("BINARY -> DECIMAL [b]");
            Console.WriteLine("DECIMAL -> BINARY [d]\n\n");

            bool loop = true;
            while (loop)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.B:
                        Console.Write("Input the binary flag: ");
                        ShowDecimal(Console.ReadLine());
                        break;

                    case ConsoleKey.D:
                        Console.Write("Input the decimal flag: ");
                        ShowBinary(int.Parse(Console.ReadLine()));
                        break;

                    case ConsoleKey.Escape:
                        loop = false;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}