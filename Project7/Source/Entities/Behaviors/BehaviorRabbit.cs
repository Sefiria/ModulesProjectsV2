using System;
using Project7.Source.Entities;
using Tooling;
using Tools.Inputs;

namespace Project7.Source.Entities.Behaviors
{
    public class BehaviorRabbit : Behavior
    {
        public bool Held;

        Entity Target;
        Action trigger_idle, trigger_run, trigger_holding;
        bool mouse_near;
        int near_time, peaceful_time, next_random_peaceful_run, force_run_time;
        public BehaviorRabbit(Entity e, Action trigger_idle, Action trigger_run, Action trigger_holding)
        {
            Target = e;
            mouse_near = Held = false;
            near_time = int.MaxValue;
            peaceful_time = force_run_time = 0;
            next_random_peaceful_run = Random.Shared.Next(10, 100);
            this.trigger_idle = trigger_idle;
            this.trigger_run = trigger_run;
            this.trigger_holding = trigger_holding;
            Idle();
        }
        public override string Update()
        {
            int inflate_harm = 50;
            int inflate_safe = 150;

            // If click successful : hold the entity

            bool holding = (Game1.MS.IsLeftDown && Held) || (Game1.MS.IsLeftPressed && Maths.CollisionPointCercle(Game1.MS.X, Game1.MS.Y, Target.X + Target.W / 2F, Target.Y + Target.W / 2F, Target.W / 2F));
            if(holding)
            {
                if (!Held)
                {
                    trigger_holding?.Invoke();
                    Target.Velocity = 0F;
                    near_time = int.MaxValue;
                    peaceful_time = 0;
                    force_run_time = 0;
                    Held = true;
                }
                Target.X = Game1.MS.X - Target.W / 2F;
                Target.Y = Game1.MS.Y - 4;
                return "";
            }

            // Else : escape or idle

            if(Held)
            {
                Held = false;
                Idle();
                near_time = 0;
                peaceful_time = 0;
                force_run_time = 0;
            }

            int inflate = mouse_near ? inflate_safe : inflate_harm;
            var old_mouse_near = mouse_near;
            mouse_near = Maths.CollisionPointCercle(Game1.MS.X, Game1.MS.Y, Target.X + Target.W / 2F, Target.Y + Target.W / 2F, Target.W / 2F + inflate);
            if (!mouse_near && mouse_near != old_mouse_near && force_run_time == 0)
            {
                Idle();
                near_time = int.MaxValue;
                peaceful_time = 0;
                force_run_time = 0;
            }
            else if (mouse_near)
            {
                if (near_time > 35)
                {
                    Run();
                    Game1.Instance.PlaySoundAsync(Game1.Instance.SE_RABBII_JUMPS[Random.Shared.Next(0,3)]);
                    near_time = 0;
                    force_run_time = 0;
                    peaceful_time = 0;
                }
                else
                    near_time++;
            }

            if (mouse_near == old_mouse_near && old_mouse_near == false)
            {
                if (peaceful_time >= next_random_peaceful_run && force_run_time == 0)
                {
                    peaceful_time = 0;
                    next_random_peaceful_run = Random.Shared.Next(50, 200);
                    force_run_time = Random.Shared.Next(10, 100);
                    Run();
                }
                else if (force_run_time == 0)
                {
                    Idle();
                    peaceful_time++;
                }
                else
                {
                    force_run_time--;
                }
            }

            return "";
        }

        void Idle()
        {
            trigger_idle?.Invoke();
            Target.Velocity = 0F;
        }
        void Run()
        {
            trigger_run?.Invoke();
            Target.Velocity = (0.5F + (float)Random.Shared.NextDouble() / 2F) * (mouse_near ? 2F : 1F);
            if (mouse_near)
            {
                Target.LookX = Target.X + Target.W / 2F - Game1.MS.X;
                Target.LookY = Target.Y + Target.W / 2F - Game1.MS.Y;
            }
            else
            {
                Target.LookX = (float)Random.Shared.NextDouble() * 2F - 1F;
                Target.LookY = (float)Random.Shared.NextDouble() * 2F - 1F;
            }
        }
    }
}
