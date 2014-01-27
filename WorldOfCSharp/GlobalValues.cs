namespace WorldOfCSharp
{
    public static class Globals
    {
        public const int CONSOLE_WIDTH = 180;
        public const int CONSOLE_HEIGHT = 60;
        public readonly static Coordinate GAME_FIELD_BOTTOM_RIGHT = 
            new Coordinate((CONSOLE_WIDTH / 4) * 3, CONSOLE_HEIGHT - (CONSOLE_HEIGHT / 8));
    }
}
