using SFX;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tooling;

namespace Project7.Source.Entities.Behaviors
{
    public class BehaviorFly : Behavior
    {
        public Entity Target;
        public bool IsDead;
        public string anim_name_idle, anim_name_flying;
        
        Game1 Context => Game1.Instance;

        Action trigger_dead;
        Random rng = new Random();
        float timer = 0f;
        float angle = 0f;
        float stateTimer = 0f;
        float stateDuration = 1f;
        enum FlyState { Idle, Flying, Turning }
        FlyState currentState = FlyState.Idle;
        Task SFX_Flying = null;
        Guid ID;

        public BehaviorFly(Entity e, string anim_name_idle, string anim_name_flying, Action trigger_dead = null)
            : base()
        {
            ID = Guid.NewGuid();
            Target = e;
            IsDead = false;
            Target.Velocity = 0F;
            this.anim_name_idle = anim_name_idle;
            this.anim_name_flying = anim_name_flying;
            this.trigger_dead = trigger_dead;
        }
        public override string Update()
        {
            if(!IsDead)
            {
                if (Maths.CollisionPointCercle(Game1.MS.X, Game1.MS.Y, Target.X, Target.Y, Target.W + 50))
                    SFX.SFX.StartRepeatSoundAsync(ID.ToString(), Context.SE_FLY_FLYING, Context.Ticks, true);
                else
                    SFX.SFX.StopRepeatSoundAsync(ID.ToString());
                if (Maths.CollisionPointCercle(Game1.MS.X, Game1.MS.Y, Target.X, Target.Y, Target.W + 10))
                {
                    if (Game1.MS.IsLeftPressed)
                    {
                        IsDead = true;
                        trigger_dead?.Invoke();
                        Target.AnimationController.CurrentAnimation = anim_name_idle;
                        Target.LookX = 0F;
                        Target.LookY = 1F;
                        Target.Velocity = 6F;
                        SFX.SFX.StopRepeatSoundAsync(ID.ToString());
                        SFX.SFX.PlaySoundAsync(Context.SE_FLY_DYING);
                    }
                    else
                    {
                        Move();
                    }
                }
                else
                {
                    NormalBehavor();
                }
            }
            if (Target.Y < 0 || Target.Y > Context.ScreenHeight || Target.X < 0 || Target.X > Context.ScreenWidth)
            {
                Target.Exists = false;
            }
            return "";
        }

        void Move()
        {
            timer += 0.01f;
            angle += (float)(Math.Sin(timer * 2.0f) * 0.1f);

            float baseAngle = angle + (float)(rng.NextDouble() - 0.5) * 0.33f;
            float dx = (float)Math.Cos(baseAngle);
            float dy = (float)Math.Sin(baseAngle);

            Target.LookX = dx;
            Target.LookY = dy;
            Target.Velocity = 2F;
        }
        void NormalBehavor()
        {
            stateTimer += 0.01f;

            if (stateTimer >= stateDuration)
            {
                currentState = (FlyState)rng.Next(0, 3);
                stateDuration = 0.5f + (float)rng.NextDouble() * 4f;
                stateTimer = 0f;
            }

            switch (currentState)
            {
                case FlyState.Idle:
                    Target.Velocity = 0f;
                    Target.AnimationController.CurrentAnimation = anim_name_idle;
                    break;

                case FlyState.Flying:
                    Move();
                    Target.AnimationController.CurrentAnimation = anim_name_flying;
                    break;

                case FlyState.Turning:
                    float angle = (float)(rng.NextDouble() * Math.PI * 2);
                    Target.LookX = (float)Math.Cos(angle);
                    Target.LookY = (float)Math.Sin(angle);
                    Target.Velocity = 1f;
                    Target.AnimationController.CurrentAnimation = anim_name_flying;
                    break;
            }
        }
    }
}
