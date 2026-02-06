using Microsoft.Xna.Framework;

namespace Project10.Helpers
{
    public abstract class Entity
    {
        public Vector2 Position { get; set; }
        public float Radius;
        public Vector2 Look;
        public Vector2 RenderPosition => new Vector2(Position.X - Game1.Instance._cellSize / 2, Position.Y - Game1.Instance._cellSize / 2);
        public Point ToCell(Vector2 pos)
        {
            int cs = Game1.Instance._cellSize;
            return new Point((int)(pos.X / cs), (int)(pos.Y / cs));
        }
    }
}
