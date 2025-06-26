using System;
using Tooling;

namespace Project7.Source.Entities.Behaviors
{
    public class BehaviorRabbit : Behavior
    {
        float m_trust;
        Action trigger_idle, trigger_run, trigger_holding;
        bool mouse_near;
        int near_time, peaceful_time, next_random_peaceful_run, force_run_time;
        int emotion_heart_cooldown = 0;
        bool can_play_atframe_escape_sound = true;

        public Entity Target;
        public bool Held;
        public float Trust
        {
            get => m_trust;
            set
            {
                m_trust = value;
                if (m_trust < 0F) m_trust = 0F;
                if (m_trust > 1F) m_trust = 1F;
            }
        }

        Game1 Context => Game1.Instance;

        public BehaviorRabbit(Entity e, Action trigger_idle, Action trigger_run, Action trigger_holding)
            : base()
        {
            Target = e;
            Trust = 0F;
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
            string result = "";
            Emotions();
            result = Movement();
            return result;
        }
        void Emotions()
        {
            if (Trust == 1F && emotion_heart_cooldown > 0)
                emotion_heart_cooldown--;
            else
            {
                int inflate = 50;
                bool mouse_near = Maths.CollisionPointCercle(Game1.MS.X, Game1.MS.Y, Target.X + Target.W / 2F, Target.Y + Target.W / 2F, Target.W / 2F + inflate);
                if (mouse_near)
                {
                    if (Trust < 1F)
                    {
                        if (!Held)
                        {
                            Trust += 0.0025F;
                        }
                    }
                    else
                    {
                        emotion_heart_cooldown += 200;
                        float x;
                        float y;
                        for (int n = 0; n < 1 + Random.Shared.Next(2); n++)
                        {
                            x = Target.X - 1F * Context.tilesize + 2F * Context.tilesize * (float)Random.Shared.NextDouble();
                            y = Target.Y - 1F * Context.tilesize + 1F * Context.tilesize * (float)Random.Shared.NextDouble();
                            new Particle(assets_bindings.Resources["ei_heart"], x, y, 1F + (float)Random.Shared.NextDouble());
                        }
                    }
                }
            }
        }
        string Movement()
        {
            int inflate_harm = 50;
            int inflate_safe = 150;

            // If click successful : hold the entity

            bool holding = (Game1.MS.IsLeftDown && Held) || (Game1.MS.IsLeftPressed && Maths.CollisionPointCercle(Game1.MS.X, Game1.MS.Y, Target.X + Target.W / 2F, Target.Y + Target.W / 2F, Target.W / 2F));
            if(holding)
            {
                if (!Held)
                {
                    Context.PlaySoundAsync(Context.SE_RABBII_GRIP);
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
                Context.PlaySoundAsync(Context.SE_RABBII_RELEASE);
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
            else if (mouse_near && Trust < 1F)
            {
                if (near_time > 35)
                {
                    Run();
                    near_time = 0;
                    force_run_time = 0;
                    peaceful_time = 0;
                }
                else
                {
                    near_time++;
                    if ((int)Target.AnimationController.GetCurrentAnimation().FrameIndex == 1)
                    {
                        if (can_play_atframe_escape_sound)
                        {
                            Context.PlaySoundAsync(Context.SE_RABBII_JUMPS[Random.Shared.Next(0, 3)]);
                            can_play_atframe_escape_sound = false;
                        }
                    }
                    else
                        can_play_atframe_escape_sound = true;
                }
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
