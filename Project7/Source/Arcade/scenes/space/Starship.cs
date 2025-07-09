using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project7.Source.Arcade.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace Project7.Source.Arcade.scenes.space
{
    public class Starship : BaseShip
    {
        float cooldown = 1F;
        public int killcount = 0;

        public Starship() : base()
        {
            TexId = 1;
            Velocity = 2.5F;
        }
        public override void Update()
        {
            A = Maths.GetAngle(new System.Drawing.PointF(Game1.MS.X - X, Game1.MS.Y - Y));

            if (PowerUps.ContainsKey(Enums.PowerUps.Zqsd))
            {
                if (Game1.KB.IsKeyDown(Keys.Z)) Y -= Velocity;
                if (Game1.KB.IsKeyDown(Keys.Q)) X -= Velocity;
                if (Game1.KB.IsKeyDown(Keys.S)) Y += Velocity;
                if (Game1.KB.IsKeyDown(Keys.D)) X += Velocity;
            }
            else
            {
                var coords = A.AngleToPointF();
                X += coords.X * Velocity;
                Y += coords.Y * Velocity;
            }

            // Shot

            if (Game1.MS.IsLeftDown)
            {
                if (cooldown >= 1F)
                {
                    cooldown = 0F;
                    new Bullet(this, A) { X = X, Y = Y };
                }
                else
                {
                    cooldown += 0.05F;
                }
            }
            else
            {
                cooldown = 1F;
            }
        }
        public override void Draw(GraphicsDevice gd)
        {
            base.Draw(gd);
        }

        public override void OnCollision(Entity other)
        {

        }
        public override void OnEntityKilled(Entity killed_entity)
        {
            killcount++;
        }
        public override void AddPowerUp(Enums.PowerUps kind)
        {
            base.AddPowerUp(kind);
            if(kind == Enums.PowerUps.Zqsd)
                ArcadeMain.instance.EntityManager.Entities.Except(this).ToList().ForEach(entity => entity.Exists = false);
        }
    }
}
