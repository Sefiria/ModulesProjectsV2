using Microsoft.Xna.Framework.Graphics;
using Project7.Source.Arcade.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace Project7.Source.Arcade.scenes.space
{
    internal class MobShip : BaseShip
    {
        public MobShip() : base()
        {
            TexId = 2;
            Velocity = 0.5F + Random.Shared.Next(100) / 100F;
        }
        public override void Update()
        {
            var starship = ArcadeSpace.Entities[0];
            X += Maths.Normalize(starship.X - X) * Velocity;
            Y += Maths.Normalize(starship.Y - Y) * Velocity;
            A = Maths.GetAngle(new System.Drawing.PointF(starship.X - X, starship.Y - Y));
        }
        public override void Draw(GraphicsDevice gd)
        {
            base.Draw(gd);
        }

        public override void OnCollision(Entity other)
        {
            Exists = false;
        }
    }
}
