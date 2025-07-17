using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project7.Source.Arcade.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace Project7.Source.Arcade.scenes.space
{
    public class Starship : BaseShip
    {
        public int killcount = 0;
        public float cooldown_speed = 0.05F;
        public byte shot_type = 0;

        float cooldown = 2F;
        float zigzag_angle_shot = 0F;

        public Starship() : base()
        {
            TexId = 1;
            Velocity = 1.5F;
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
                    Shot();
                }
                else
                {
                    cooldown += cooldown_speed;
                }
            }
            else
            {
                cooldown = 1F;
            }
        }
        private void Shot()
        {
            Bullet bullet;
            PointF pt;
            float ticks = (Game1.Instance.Ticks % 180) * 4F;
            byte local_shot_type = Math.Min((byte)2, shot_type);// define in the Min function the max case number of the switch below
            switch (shot_type)
            {
                default:
                case 0: new Bullet(this, A) { X = X, Y = Y }; break;
                case 1:
                    bullet = new Bullet(this, A);
                    pt = Forward.GetRotatedPoint(A - 45F, W);
                    bullet.X = pt.X; bullet.Y = pt.Y;

                    bullet = new Bullet(this, A);
                    pt = Forward.GetRotatedPoint(A + 45F, W);
                    bullet.X = pt.X; bullet.Y = pt.Y;
                    break;
                case 2:
                    zigzag_angle_shot = ((float)Math.Cos(ticks.ToRadians())).ToDegrees();

                    bullet = new Bullet(this, A);
                    pt = Forward.GetRotatedPoint(A - zigzag_angle_shot, W * 2);
                    bullet.X = pt.X; bullet.Y = pt.Y;

                    bullet = new Bullet(this, A);
                    pt = Forward.GetRotatedPoint(A + zigzag_angle_shot, W* 2);
                    bullet.X = pt.X; bullet.Y = pt.Y;
                    break;
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
            switch(kind)
            {
                case Enums.PowerUps.Zqsd:
                    ArcadeMain.instance.EntityManager.Entities.Except(this).ToList().ForEach(entity => entity.Exists = false);
                    Velocity = 2.5F;
                    break;
                case Enums.PowerUps.Cooldown: cooldown_speed = Math.Min(0.5F, cooldown_speed * 1.1F); break;
                case Enums.PowerUps.Shot: shot_type = (byte)Math.Min(byte.MaxValue, shot_type + 1); break;
            }
        }
    }
}
