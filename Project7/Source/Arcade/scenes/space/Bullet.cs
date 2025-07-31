using Microsoft.Xna.Framework.Graphics;
using Project7.Source.Arcade.Common;
using System;
using Tooling;
using static Project7.Source.Arcade.Common.Collider;
using Collider = Project7.Source.Arcade.Common.Collider;

namespace Project7.Source.Arcade.scenes.space
{
    internal class Bullet : EntitySpace
    {
        float prev_look_x, prev_look_y;
        float look_x, look_y;
        public EntitySpace Owner;

        public Bullet(EntitySpace owner, float look_angle) : base()
        {
            Owner = owner;
            TexId = 3;
            Velocity = 5F;
            var look = look_angle.AngleToPointF();
            prev_look_x = look_x = look.X;
            prev_look_y = look_y = look.Y;
        }
        public override void Update()
        {
            int w = Game1.Instance.ScreenWidth;
            int h = Game1.Instance.ScreenHeight;
            prev_look_x = look_x;
            prev_look_y = look_y;
            X += look_x * Velocity;
            Y += look_y * Velocity;
            if(X < -20 || Y < -20 || X > w + 20 || Y > h + 20)
                Exists = false;
        }
        public override void Draw(GraphicsDevice gd)
        {
            base.Draw(gd);
        }

        public override Collider GetCollider()
        {
            return new Collider(Kind.Circle) { Circle = new Circle(X, Y, Math.Max(W, H)) };
        }

        public override void OnCollision(Entity other)
        {
            if (other != Owner)
            {
                Owner.OnEntityKilled(other);
                Exists = false;
            }
        }
    }
}
