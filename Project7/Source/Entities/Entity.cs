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
        public bool Exists;
        public AnimationController AnimationController;
        public List<Behavior> Behaviors;
        public float X, Y;
        public float W => AnimationController?.GetCurrentFrame()?.Width ?? 0F;
        public float H => AnimationController?.GetCurrentFrame()?.Height ?? 0F;
        public RectangleF GetTextureBounds() => new RectangleF(X, Y, W, H);
        public float LookX, LookY, Velocity;

        public Entity()
        {
            Exists = true;
            Behaviors = new List<Behavior>();
            LookX = 1F;
            LookY = 0F;
            Velocity = 0F;
            Game1.Instance.EntityManager.Entities.Add(this);
        }
        public void Update()
        {
            AnimationController?.Update();
            Behaviors.ForEach(b => b.Update());

            float delta_x = Maths.Normalize(LookX) * Velocity;
            float delta_y = Maths.Normalize(LookY) * Velocity;
            bool stucked = Game1.Instance.Map[1, (int)((X + W / 2) / Game1.Instance.scale / Game1.Instance.tilesize), (int)((Y + H / 2) / Game1.Instance.scale / Game1.Instance.tilesize)] != -1;
            if (stucked || CheckCollisions(X, Y, delta_x, delta_y))
            {
                X += delta_x;
                Y += delta_y;
            }
        }
        private bool CheckCollisions(float _x, float _y, float delta_x, float delta_y)
        {
            if (delta_x == 0F && delta_y == 0F)
                return false;
            _x += W / 2;
            _y += H / 2;
            int ofst_x = (int)(Maths.Sign(delta_x) * W / 4);
            int ofst_y = (int)(Maths.Sign(delta_y) * H / 4);
            int x = (int)((_x + delta_x + ofst_x) / Game1.Instance.scale / Game1.Instance.tilesize);
            int y = (int)((_y + delta_y + ofst_y) / Game1.Instance.scale / Game1.Instance.tilesize);
            if (x - 1 < 0) return false;
            if (y - 1 < 0) return false;
            if (x >= Game1.Instance.screen_tiles_width) return false;
            if (y >= Game1.Instance.screen_tiles_height) return false;
            if (Game1.Instance.Map[1, x, y] != -1)
                return false;
            return true;
        }
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
