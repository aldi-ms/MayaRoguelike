namespace Maya.Framework
{
    public class FlatArray<T>
    {
        private int height;
        private int width;
        private T[] array;

        public FlatArray(int height, int width)
        {
            this.height = height;
            this.width = width;
            this.array = new T[height * width];
        }

        public T this[int x, int y]
        {
            get { return this.array[x + y * height]; }
            set { this.array[x + y * height] = value; }
        }

        public int Height
        {
            get { return this.height; }
        }

        public int Width
        {
            get { return this.width; }
        }
    }
}