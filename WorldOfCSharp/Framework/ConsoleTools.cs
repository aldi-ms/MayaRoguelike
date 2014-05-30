using System;
using System.Text;
using System.Collections.Generic;

namespace Maya
{
    public static class ConsoleTools
    {
        public static void WriteOnPosition(
            string text,
            int left = 0,
            int top = 0,
            ConsoleColor foregroundColor = ConsoleColor.Gray,
            ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Console.SetCursorPosition(left, top);
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.Write(text);
        }

        public static void WriteOnPosition(
            char character,
            int left = 0,
            int top = 0,
            ConsoleColor foregroundColor = ConsoleColor.Gray,
            ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Console.SetCursorPosition(left, top);
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            Console.Write(character);
        }

        public static void WriteOnPosition(GameObject gameObject)
        {
            Console.SetCursorPosition(gameObject.X, gameObject.Y);
            Console.ForegroundColor = gameObject.Color;
            Console.Write(gameObject.VisualChar);
        }

        public static void WriteOnPosition(
            GameObject gameObject,
            int left = 0,
            int top = 0,
            ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Console.SetCursorPosition(left, top);
            Console.ForegroundColor = gameObject.Color;
            Console.BackgroundColor = backgroundColor;
            Console.Write(gameObject.VisualChar);
        }

        public static void ResizeConsole(int width, int height)
        {
            Console.WindowWidth = width;
            Console.WindowHeight = height;
            Console.BufferWidth = Console.WindowWidth;
            Console.BufferHeight = Console.WindowHeight;
        }

        public static void ClearAndResetConsole()
        {
            Console.ResetColor();
            Console.Clear();
        }

        public static void Clear(int bottomLeftAnchorX, int bottomLeftAnchorY, int width, int height)
        {
            for (int x = bottomLeftAnchorX; x < bottomLeftAnchorX + width; x++)
            {
                for (int y = 0; y < bottomLeftAnchorY; y++)
                    WriteOnPosition(' ', x, y);
            }
        }

        public static ConsoleColor ParseColor(string color)
        {
            switch (color)
            {
                case "Black":
                    return ConsoleColor.Black;
                case "Blue":
                    return ConsoleColor.Blue;
                case "Cyan":
                    return ConsoleColor.Cyan;
                case "DarkBlue":
                    return ConsoleColor.DarkBlue;
                case "DarkCyan":
                    return ConsoleColor.DarkCyan;
                case "DarkGray":
                    return ConsoleColor.DarkGray;
                case "DarkGreen":
                    return ConsoleColor.DarkGreen;
                case "DarkMagenta":
                    return ConsoleColor.DarkMagenta;
                case "DarkRed":
                    return ConsoleColor.DarkRed;
                case "DarkYellow":
                    return ConsoleColor.DarkYellow;
                case "Gray":
                    return ConsoleColor.Gray;
                case "Green":
                    return ConsoleColor.Green;
                case "Magenta":
                    return ConsoleColor.Magenta;
                case "Red":
                    return ConsoleColor.Red;
                case "White":
                    return ConsoleColor.White;
                case "Yellow":
                    return ConsoleColor.Yellow;
                default:
                    return ConsoleColor.Gray;
            }
        }

        public static void Quit()
        {
            //show confirmation & goodbye message!
            Environment.Exit(0);
        }

        #region QuickSort
        public static void QuickSort(List<int> intList, int left, int right) //left = 0, right = IDList.Count - 1
        {
            if (left < right)       //for recursion
            {
                int pivot = Partition(intList, left, right);

                if (pivot > 1)
                    QuickSort(intList, left, pivot - 1);

                if (pivot + 1 < right)
                    QuickSort(intList, pivot + 1, right);
            }
        }

        private static int Partition(List<int> numbers, int left, int right)
        {
            int pivot = numbers[left + (right - left) / 2];

            while (true)
            {
                while (numbers[left] < pivot)
                    left++;

                while (numbers[right] > pivot)
                    right--;

                if (left < right)
                {
                    int temp = numbers[right];
                    numbers[right] = numbers[left];
                    numbers[left] = temp;
                }
                else
                    return right;
            }
        }
        #endregion
    }
}
