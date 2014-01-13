using WorldOfCSharp.FieldOfView;

namespace WorldOfCSharp
{
    public class GameCell : IFovCell
    {
        private bool isVisible;

        private TerrainType terrain;
        private Unit unit;
        private IngameObject ingameObject;
        private Item item;

        public bool IsTransparent
        {
            get { return this.Terrain.GetFlag(3); } //isTransparent flag
            set { }
        }

        public bool IsVisible
        {
            get { return this.isVisible; }
            set { this.isVisible = value; }
        }

        public TerrainType Terrain
        {
            get { return this.terrain; }
            set { this.terrain = value; }
        }

        public Unit Unit
        {
            get { return this.unit; }
            set { this.unit = value; }
        }

        public IngameObject IngameObject
        {
            get { return this.ingameObject; }
            set { this.ingameObject = value; }
        }

        public Item Item
        {
            get { return this.item; }
            set { this.item = value; }
        }
    }
}
