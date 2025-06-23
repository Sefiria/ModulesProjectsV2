using Microsoft.Xna.Framework.Graphics;
using Project7.Entities.Behaviors;
using System.Collections.Generic;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using Tooling;
using Tools.Animations;

namespace Project7.Entities
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

            X += Maths.Normalize(LookX) * Velocity;
            Y += Maths.Normalize(LookY) * Velocity;
            if (X < 0) X = 10;
            if (Y < 0) Y = 10;
            if (X > Game1.Instance.ScreenWidth - 40) X = Game1.Instance.ScreenWidth - 40;
            if (Y > Game1.Instance.ScreenHeight - 40) Y = Game1.Instance.ScreenHeight - 40;
        }
        public void Draw(GraphicsDevice graphics)
        {
            Texture2D tex = AnimationController?.GetCurrentFrame();
            if(tex != null)
            {
                Graphics.Graphics.Instance.DrawTexture(tex, X, Y, 0F, 1F, LookX < 0F, 0F);
            }
        }
    }
}
