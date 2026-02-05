using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project10.Helpers;
using SharpDX.Direct3D9;

namespace Project10
{
    public class PCA
    {
        public Point Position { get; set; }
        public float Radius;
        public Vector2 Look;

        public Vector2 RenderPosition => new Vector2(Position.X - Game1.Instance._cellSize / 2, Position.Y - Game1.Instance._cellSize / 2);

        public PCA()
        {
            Position = new Point(MAP.Width * Game1.Instance._cellSize / 2, MAP.Height * Game1.Instance._cellSize / 2);
            Radius = Game1.Instance._cellSize / 2;
            Look = new Vector2(0F, 0F);
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(GameTime gameTime)
        {
            DrawHelper.DrawCircle(Game1.Instance.basicEffect, RenderPosition, Radius, 8, Color.Cyan);
        }
    }
}
