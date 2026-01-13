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

        bool is_on_ground = false, is_on_ladder = false;
        float jump_look_y = 0F;

        public BehaviorPlayer(Entity e)
            : base()
        {
            Target = e;
        }
        public override string Update()
        {
            float oldX = Target.X, oldY = Target.Y;
            bool Q = GameMain.KB.IsKeyDown(Keys.Q);
            bool D = GameMain.KB.IsKeyDown(Keys.D);
            bool Z = GameMain.KB.IsKeyDown(Keys.Z);
            bool S = GameMain.KB.IsKeyDown(Keys.S);
            bool Space = GameMain.KB.IsKeyDown(Keys.Space);

            vecf last_vec = new(Target.X, Target.Y);
            vec last_tile = new(new(Target.TileX, Target.TileY));

            is_on_ladder = Tile.Tiles[map.Tiles[0, Target.TileX, Target.TileY]].IsLadder || Tile.Tiles[map.Tiles[0, Target.TileX, Target.GetTileY((int)(Target.H / 2 + 2))]].IsLadder;
            is_on_ground = map.Collider(Target, (0F, 5F).Vf()) != null || is_on_ladder;
            float speed = 1.5F;
            float speed_jump = 1.0F;
            float jump_force = 2.0F;
            float input_look_x = (Q ? -1F : 0F) + (D ? 1F : 0F);
            if(is_on_ladder && (Z||S))
            {
                move(new vecf(0F, 1F * (Z?-1:1)), speed);
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
                jump_look_y = -3F;
            }
            if (jump_look_y < 0F)
            {
                move(new vecf(input_look_x, jump_look_y), speed_jump * jump_force, true);
                jump_look_y += 0.2F;
                if (jump_look_y > 0F) jump_look_y = 0F;
            }
            else
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

            //if (Z)
            //    (DB.Room.PhysicalObjects.FirstOrDefault(po => po is ITriggerable && (vec / GraphicsManager.TSZ).i == (po.vec / GraphicsManager.TSZ).i) as ITriggerable)?.Trigger(this);


            //if (DB.Room.isout(vec.x / GraphicsManager.TSZ, vec.y / GraphicsManager.TSZ))
            //{
            //    var warp = DB.Room.Warps.FirstOrDefault(w => (w.source_x, w.source_y).V() == last_tile);
            //    if (warp != null)
            //    {
            //        var next_room = Room.Load((byte)warp.target_room);
            //        var next_vec = (warp.target_x, warp.target_y).Vf() * GraphicsManager.TSZ + h % GraphicsManager.TSZ;
            //        var door = next_room.Doors.FirstOrDefault(d => d.vec.tile(GraphicsManager.TSZ) == next_vec.tile(GraphicsManager.TSZ) || d.vec.tile(GraphicsManager.TSZ) == next_vec.tile(GraphicsManager.TSZ) - (0, 1).Vf());
            //        bool blocked_by_locked_door = false;
            //        if (door != null)
            //        {
            //            if (door.Locked)
            //                blocked_by_locked_door = true;
            //            else
            //                door.LoadState(2);
            //        }
            //        if (!blocked_by_locked_door)
            //        {
            //            DB.Room = next_room;
            //            DB.FluidsManager.Initialize();
            //            vec = next_vec;
            //        }
            //        else
            //        {
            //            vec = last_vec;
            //        }
            //    }
            //}

            if (oldX != Target.X || oldY != Target.Y)
                Target.Animation?.Update();

            return "";
        }

        /// <returns>false if colliding (then no move)</returns>
        private bool move(vecf look, float speed, bool isJump = false)
        {
            bool collides;
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
            return !collides;
        }

    }
}
