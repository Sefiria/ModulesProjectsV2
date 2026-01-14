using Tooling;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System;
using Tools.Animations;
using Project8.Source.TiledMap;

namespace Project8.Source.Entities.Behaviors
{
    public class BehaviorPlayer : Behavior
    {
        public Entity Target;
        GameMain Context => GameMain.Instance;
        static Map.TiledMap map => GameMain.Instance.Map;

        bool is_on_ground = false, is_on_ladder = false, is_crouching = false;
        float jump_look_y = 0F;

        public BehaviorPlayer(Entity e)
            : base()
        {
            Target = e;
            e.AnimationController.CurrentAnimation = Enum.GetName(AnimationsNeeds.Idle);
        }
        float test = 0F;
        public override string Update()
        {
            float oldX = Target.X, oldY = Target.Y;
            bool Q = GameMain.KB.IsKeyDown(Keys.Q);
            bool D = GameMain.KB.IsKeyDown(Keys.D);
            bool Z = GameMain.KB.IsKeyDown(Keys.Z);
            bool S = GameMain.KB.IsKeyDown(Keys.S);
            bool Space = GameMain.KB.IsKeyDown(Keys.Space);
            bool is_crouching = S;

            vecf last_vec = new(Target.X, Target.Y);
            vec last_tile = new(new(Target.TileX, Target.TileY));

            is_on_ladder = Tile.Tiles[map.Tiles[0, Target.TileX, Target.TileY]].IsLadder || Tile.Tiles[map.Tiles[0, Target.TileX, Target.GetTileY((int)(Target.H / 2 + 16))]].IsLadder;
            is_on_ground = (map.Collider(Target, (0F, 5F).Vf()) as Tile)?.IsSolid ?? false || is_on_ladder;
            float speed = 1.5F;
            float speed_jump = 1.0F;
            float jump_force = 2.0F;
            float input_look_x = (Q ? -1F : 0F) + (D ? 1F : 0F);
            if (is_on_ladder && ((Z||S) || ((Q||D) && map.Collider(Target, (Q ? -speed : speed, 0F).Vf()) != null)))
            {
                move(new vecf(0F, S ? 1 : -1), speed);
                jump_look_y = 0F;
            }
            if (jump_look_y == 0F)
            {
                if (Q)
                    move(new vecf(-1F, 0F), speed);
                if (D)
                    move(new vecf(1F, 0F), speed);
            }
            if (Space && is_on_ground && jump_look_y == 0F)
            {
                jump_look_y = is_on_ladder  ? -speed  : -3F;
            }
            if (jump_look_y < 0F)
            {
                move(new vecf(input_look_x, jump_look_y), speed_jump * jump_force, true);
                jump_look_y += 0.2F;
                if (jump_look_y > 0F) jump_look_y = 0F;
            }
            else
            {
                if (!is_on_ladder)
                {
                    if (is_on_ground)
                        jump_look_y = 0F;
                    else
                    {
                        jump_look_y += 0.2F;
                        if (jump_look_y > 5F) jump_look_y = 5F;
                        move(new vecf(input_look_x, jump_look_y), speed_jump);
                    }
                }
            }
            //if(Target.Y-test < -0.5F)
            //{
            //}
            //test = Target.Y;
            //System.Diagnostics.Debug.WriteLine(Target.Y);


            if (is_crouching && !is_on_ladder)
            {
                set_anim(AnimationsNeeds.Crouch);
            }
            else
            {
                bool is_moving = oldX != Target.X || oldY != Target.Y;
                set_anim(is_moving ? AnimationsNeeds.Walk : AnimationsNeeds.Idle);
            }
            Target.AnimationController?.Update();

            return "";
        }
        private void set_anim(AnimationsNeeds anim)
        {
            int ha = Target.AnimationController.GetCurrentFrame().Height;
            int hb = Target.AnimationController.Animations[Enum.GetName(anim)].Frames[0].Height;
            if(ha < hb)
                Target.Y -= hb - ha + 4;
            else if(ha > hb)
                Target.Y += ha - hb;
            Target.AnimationController.CurrentAnimation = Enum.GetName(anim);
        }

        /// <returns>false if colliding (then no move)</returns>
        private bool move(vecf look, float speed, bool isJump = false)
        {
            bool collides;
            float oldx = Target.X, oldy = Target.Y;
            Target.LookX = look.x;
            Target.LookY = look.y;

            // y
            if (!(collides = map.Collider(Target, (0F, look.y * speed).Vf(), isJump) != null))
                Target.Y += look.y * speed;
            else
            {
                collides = false;
                float n = -1F;
                for (float j = 0.1F; j <= 1.05F && !collides && n == -1F; j += 0.1F)
                    if (!(collides = map.Collider(Target, (0F, look.y * speed * j).Vf()) != null))
                        n = j;
                if (!collides && n != -1F)
                    Target.Y += look.y * speed * n;
            }

            // x
            if (look.x != 0F)
            {
                if (!(collides = map.Collider(Target, (look.x * speed, 0F).Vf()) != null))
                    Target.X += look.x * speed;
                else
                {
                    collides = false;
                    float n = -1F;
                    for (float i = 0.1F; i <= 1.05F && !collides && n == -1F; i += 0.1F)
                        if (!(collides = map.Collider(Target, (look.x * speed * i, 0F).Vf()) != null))
                            n = i;
                    if (!collides && n != -1F)
                        Target.X += look.x * speed * n;
                }
            }

            if(map.Collider(Target) is Tile)
            {
                Target.X = oldx;
                Target.Y = oldy;
            }

            return !collides;
        }

    }
}
