using Project7.Source.Arcade.Common;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace Project7.Source.Arcade.scenes.space
{
    public class PowerUp : Entity
    {
        public Enums.PowerUps Kind;

        private float init_x, init_y, target_move_x = 0, target_move_y = 0;

        public PowerUp(Enums.PowerUps kind, float x, float y)
        {
            Kind = kind;
            target_move_x = init_x = X = x;
            target_move_y = init_y = Y = y;
            Velocity = 0.5F;
            Scale = 1F;

            switch(Kind)
            {
                case Enums.PowerUps.Zqsd: TexId = 4; break;
                case Enums.PowerUps.Cooldown: TexId = 5; break;
                case Enums.PowerUps.Shot: TexId = 6; break;
            }
        }
        public override void Update()
        {
            if ((int)target_move_x == (int)X && (int)target_move_y == (int)Y)
            {
                target_move_x = Random.Shared.Next((int)(init_x - W/2), (int)(init_x + W/2));
                target_move_y = Random.Shared.Next((int)(init_y - H/2), (int)(init_y + H/2));
            }
            else
            {
                X += Maths.Sign(target_move_x - X) * Velocity;
                Y += Maths.Sign(target_move_y - Y) * Velocity;
            }
        }
        public override void OnCollision(Entity other)
        {
            if (other is BaseShip)
            {
                (other as BaseShip).AddPowerUp(Kind);
                Exists = false;
            }
        }
    }
}
