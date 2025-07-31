using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project7.Source.Arcade.Common;
using Project7.Source.Arcade.scenes.plateform;
using System;
using Tooling;
using static Project7.Source.Arcade.scenes.plateform.Enums;

namespace Project7.Source.Arcade.scenes.space
{
    public class Character : Entity
    {
        float jump_look_y;
        System.Drawing.PointF previous_position;
        bool is_on_ground = false;
        float floating_time_before_falling = 0F;

        int TSZ => ArcadePlateform.TSZ;
        int SC => ArcadePlateform.SCALE;
        int TSZSC => ArcadePlateform.TSZ_SCALED;
        ArcadePlateform scene => ArcadeMain.Scenes[ArcadeMain.instance.scene_index] as ArcadePlateform;

        public Character() : base()
        {
            TexId = 2;
            Centered = false;
        }
        public Character(Point pos) : base()
        {
            TexId = 2;
            Centered = false;
            X = 64 + pos.X * TSZSC;
            Y = 64 + pos.Y * TSZSC - H * SC;
            Scale = SC;
        }
        public override void Update()
        {
            bool Q = Game1.KB.IsKeyDown(Keys.Q);
            bool Z = Game1.KB.IsKeyDown(Keys.Z);
            bool S = Game1.KB.IsKeyDown(Keys.S);
            bool D = Game1.KB.IsKeyDown(Keys.D);
            bool Space = Game1.KB.IsKeyDown(Keys.Space);

            bool prev_on_ground = is_on_ground;
            is_on_ground = scene.map.Collider(this, (0F, jump_look_y * 2 + 1F).Vf()) != null;
            if (prev_on_ground && !is_on_ground && floating_time_before_falling <= 0F)
                floating_time_before_falling = 1F;
            bool grounded = is_on_ground || floating_time_before_falling > 0F;

            float speed = 2F;
            float input_look_x = (Q ? -1F : 0F) + (D ? 1F : 0F);
            if (jump_look_y == 0F)
            {
                if (Q)
                    move(new vecf(-1F, 0F), speed);
                if (D)
                    move(new vecf(1F, 0F), speed);
            }
            if (Space && grounded && jump_look_y == 0F)
            {
                jump_look_y = -3F;
            }
            if (jump_look_y < 0F)
            {
                move(new vecf(input_look_x, jump_look_y), speed, true);
                jump_look_y += 0.2F;
                if (jump_look_y > 0F) jump_look_y = 0F;
            }
            else
            {
                if (grounded)
                    jump_look_y = 0F;
                else
                {
                    jump_look_y += 0.1F;
                    if (jump_look_y > 5F) jump_look_y = 5F;
                    move(new vecf(input_look_x, jump_look_y), speed);
                }
            }

            if (floating_time_before_falling > 0F)
                floating_time_before_falling -= 0.05F;
        }

        /// <returns>false if colliding (then no move)</returns>
        private bool move(vecf look, float speed, bool isJump = false)
        {
            bool collides;

            // y
            if (!(collides = scene.map.Collider(this, (0F, look.y * speed).Vf(), isJump) != null))
                Y += look.y * speed;
            else
            {
                collides = false;
                float n = -1F;
                for (float j = 0.1F; j <= 1.05F && !collides && n == -1F; j += 0.1F)
                    if (!(collides = scene.map.Collider(this, (0F, look.y * speed * j).Vf()) != null))
                        n = j;
                //if (!collides && n != -1F)
                //    Y += look.y * speed * n;
            }

            // x
            if (look.x != 0F)
            {
                if (!(collides = scene.map.Collider(this, (look.x * speed, 0F).Vf()) != null))
                    X += look.x * speed;
                else
                {
                    collides = false;
                    float n = -1F;
                    for (float i = 0.1F; i <= 1.05F && !collides && n == -1F; i += 0.1F)
                        if (!(collides = scene.map.Collider(this, (look.x * speed * i, 0F).Vf()) != null))
                            n = i;
                    //if (!collides && n != -1F)
                    //    X += look.x * speed * n;
                }
            }
            return !collides;
        }

        public override void Draw(GraphicsDevice gd)
        {
            base.Draw(gd);
        }
    }
}
