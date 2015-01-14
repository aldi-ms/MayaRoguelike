using System;
using Maya.FieldOfView;

namespace Maya
{
    public class VisualEngine
    {
        private const int DEFAULT_RANGE = 5;
        private FieldOfView<GameCell> fieldOfView;
        private int range = DEFAULT_RANGE;
        private FOVMethod method = FOVMethod.MRPAS;
        private RangeLimitShape shape = RangeLimitShape.Circle;
        private Framework.FlatArray<GameCell> map;
        private char[] itemCharacters;

        private int xStart;
        private int xEnd;
        private int yStart;
        private int yEnd;

        public VisualEngine(Framework.FlatArray<GameCell> map, int range, FOVMethod method, RangeLimitShape shape)
        {
            this.fieldOfView = new FieldOfView<GameCell>(map);
            this.range = range;
            this.method = method;
            this.shape = shape;
            this.map = map;

            this.itemCharacters = new char[Enum.GetNames(typeof(BaseType)).Length];
            this.itemCharacters[(int)BaseType.Armor] = '[';
            this.itemCharacters[(int)BaseType.Weapon] = ')';
            this.itemCharacters[(int)BaseType.Consumable] = '%';
            this.itemCharacters[(int)BaseType.Container] = '&';
            this.itemCharacters[(int)BaseType.Gem] = '\u263c';
            this.itemCharacters[(int)BaseType.Key] = '\u2552';
            this.itemCharacters[(int)BaseType.Money] = '$';
            this.itemCharacters[(int)BaseType.Reagent] = '\u220f';
            this.itemCharacters[(int)BaseType.Recipe] = '\u222b';
            this.itemCharacters[(int)BaseType.Projectile] = '(';
            this.itemCharacters[(int)BaseType.QuestPlot] = '\u2021';
            this.itemCharacters[(int)BaseType.Quiver] = '\u00b6';
            this.itemCharacters[(int)BaseType.TradeGoods] = '\u2211';
            this.itemCharacters[(int)BaseType.Miscellaneous] = '}';
            this.itemCharacters[(int)BaseType.Jewellery] = '\u00a7';
        }

        public VisualEngine(Framework.FlatArray<GameCell> map)
            : this(map, DEFAULT_RANGE, FOVMethod.MRPAS, RangeLimitShape.Circle)
        { }

        public void PrintFOVMap(int FOV_X, int FOV_Y)
        {
            fieldOfView.ComputeFov(FOV_X, FOV_Y, this.range, true, this.method, this.shape);
            
            //calc print coords, for faster print loop
            const int PRINT_MARGINS = 2;
            if (FOV_X - (range + PRINT_MARGINS) >= 0)
                xStart = FOV_X - (range + PRINT_MARGINS);
            else
                xStart = 0;

            if (FOV_X + (range + PRINT_MARGINS) < Globals.GAME_FIELD_BOTTOM_RIGHT.X)
                xEnd = FOV_X + (range + PRINT_MARGINS);
            else
                xEnd = Globals.GAME_FIELD_BOTTOM_RIGHT.X;

            if (FOV_Y - (range + PRINT_MARGINS) >= 0)
                yStart = FOV_Y - (range + PRINT_MARGINS);
            else
                yStart = 0;

            if (FOV_Y + (range + PRINT_MARGINS) < Globals.GAME_FIELD_BOTTOM_RIGHT.Y)
                yEnd = FOV_Y + (range + PRINT_MARGINS);
            else
                yEnd = Globals.GAME_FIELD_BOTTOM_RIGHT.Y;

            //the actual print loop
            for (int x = xStart; x < xEnd; x++)
            {
                for (int y = yStart; y < yEnd; y++)
                {
                    if (map[x, y].IsVisible == true)
                    {
                        if (map[x, y].Unit != null)
                            ConsoleTools.WriteOnPosition(map[x, y].Unit);

                        else if (map[x, y].ItemList != null && map[x, y].ItemList.Count > 0)
                            ConsoleTools.WriteOnPosition(itemCharacters[(int)map[x, y].ItemList[0].ItemType.BaseType], x, y);

                        else if (map[x, y].IngameObject != null)
                            ConsoleTools.WriteOnPosition(map[x, y].IngameObject);

                        else 
                            ConsoleTools.WriteOnPosition(map[x, y].Terrain);
                    }
                    else
                        ConsoleTools.WriteOnPosition(' ', x, y);
                }
            }
        }

        public void PrintUnit(Unit unit)
        {
            GameEngine.GameField[unit.X, unit.Y].Unit = unit;
            if (unit.VisualChar == '@')
            {
                this.range += unit.Attributes.EyeSight;
                ConsoleTools.WriteOnPosition(unit);
                this.PrintFOVMap(unit.X, unit.Y);
            }
            else
                if (unit.X >= xStart && unit.X <= xEnd && unit.Y >= yStart && unit.Y <= yEnd)
                    if (GameEngine.GameField[unit.X, unit.Y].IsVisible)
                        ConsoleTools.WriteOnPosition(unit);
            this.range = DEFAULT_RANGE;
        }
                
        public void ClearGameObject(Unit unit)
        {
            if (map[unit.X, unit.Y].IsVisible == true)
                ConsoleTools.WriteOnPosition(GameEngine.GameField[unit.X, unit.Y].Terrain, unit.X, unit.Y);
            else
                ConsoleTools.WriteOnPosition(' ', unit.X, unit.Y);

            GameEngine.GameField[unit.X, unit.Y].Unit = null;
        }
    }
}
