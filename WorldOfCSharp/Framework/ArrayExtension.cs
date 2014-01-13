using System;

namespace WorldOfCSharp
{
    public static class ArrayExtension
    {
        public static int Width(this Array arr) // X
        {
            return arr.GetLength(0);
        }

        public static int Height(this Array arr) // Y
        {
            return arr.GetLength(1);
        }
    }
}
