using Microsoft.Xna.Framework.Graphics;
using Project7.Source.Entities.Behaviors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using Tooling;
using Tools.Animations;

namespace Project7.Source.Entities
{
    public class Entity
    {
        Game1 Context => Game1.Instance;

        public bool Exists;
        public AnimationController AnimationController;
        public List<Behavior> Behaviors;
        public float X, Y;
        public float LookX, LookY, Velocity;

        public float W => AnimationController?.GetCurrentFrame()?.Width ?? 0F;
        public float H => AnimationController?.GetCurrentFrame()?.Height ?? 0F;
        public RectangleF GetTextureBounds() => new RectangleF(X, Y, W, H);
        public int TileX => (int)((X + W / 2f) / Context.scale / Context.tilesize);
        public int TileY => (int)((Y + H / 2f) / Context.scale / Context.tilesize);


        public Entity()
        {
            Exists = true;
            Behaviors = new List<Behavior>();
            LookX = 1F;
            LookY = 0F;
            Velocity = 0F;
            Context.EntityManager.Entities.Add(this);
        }
        public void Update()
        {
            AnimationController?.Update();
            Behaviors.ForEach(b => b.Update());

            var vn = Maths.Normalized(new PointF(LookX, LookY));
            float delta_x = vn.X * Velocity;
            float delta_y = vn.Y * Velocity;

            if (delta_x == 0f && delta_y == 0f)
                return;

            var (tileX_X, tileY_X) = GetTileAtOffset(delta_x, 0);
            if (!CheckMapTilesCollisions(tileX_X, tileY_X) && !CheckMapBounds(X, Y, delta_x, 0))
            {
                X += delta_x;
            }

            var (tileX_Y, tileY_Y) = GetTileAtOffset(0, delta_y);
            if (!CheckMapTilesCollisions(tileX_Y, tileY_Y) && !CheckMapBounds(X, Y, 0, delta_y))
            {
                Y += delta_y;
            }
        }
        public (int TileX, int TileY) GetTileAtOffset(float dx, float dy)
        {
            float centerX = X + W / 2f + dx;
            float centerY = Y + H / 2f + dy;

            int tileX = (int)(centerX / Context.scale / Context.tilesize);
            int tileY = (int)(centerY / Context.scale / Context.tilesize);

            return (tileX, tileY);
        }
        private bool CheckMapBounds(float _x, float _y, float delta_x, float delta_y)
        {
            if (delta_x == 0F && delta_y == 0F)
                return false;
            _x += W / 2;
            _y += H / 2;
            int ofst_x = (int)(Maths.Sign(delta_x) * W / 4);
            int ofst_y = (int)(Maths.Sign(delta_y) * H / 4);
            int x = (int)((_x + delta_x + ofst_x) / Game1.Instance.scale / Game1.Instance.tilesize);
            int y = (int)((_y + delta_y + ofst_y) / Game1.Instance.scale / Game1.Instance.tilesize);
            if (x - 1 < 0) return true;
            if (y - 1 < 0) return true;
            if (x >= Game1.Instance.screen_tiles_width) return true;
            if (y >= Game1.Instance.screen_tiles_height) return true;
            return false;
        }
        /// <summary>
        /// Already tIled x, y
        /// </summary>
        private bool CheckMapTilesCollisions(int x, int y) => Context.Map[1, x, y] != -1;

        public void Draw(GraphicsDevice graphics)
        {
            Texture2D tex = AnimationController?.GetCurrentFrame();
            if (tex != null)
            {
                Graphics.Graphics.Instance.DrawTexture(tex, X, Y, 0F, 1F, LookX < 0F, 0F);
            }
        }
    }
}
