using System;
using System.Linq;
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

        public BehaviorFly(Entity e, string anim_name_idle, string anim_name_flying, Action trigger_dead = null)
            : base()
        {
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
                        Context.PlaySoundAsync(Context.SE_KILLED_FLY);
                    }
                    else
                        Move();
                }
                else
                {
                    NormalBehavor();
                }
            }
            if (Target.Y < 0 || Target.Y > Context.ScreenHeight || Target.X < 0 || Target.X > Context.ScreenWidth)
                Target.Exists = false;
            return "";
        }

        void Move()
        {
            // Variation périodique de la direction pour simuler des virages
            timer += 0.01f; // à ajuster selon le framerate
            angle += (float)(Math.Sin(timer * 2.0f) * 0.1f); // permet une trajectoire courbée

            // Génère une direction de vol légèrement erratique autour d'un angle de base
            float baseAngle = angle + (float)(rng.NextDouble() - 0.5) * 0.5f; // bruit aléatoire
            float dx = (float)Math.Cos(baseAngle);
            float dy = (float)Math.Sin(baseAngle);

            // Met à jour la direction et la vitesse
            Target.LookX = dx;
            Target.LookY = dy;
            Target.Velocity = 2F; // ajustable
        }
        void NormalBehavor()
        {
            stateTimer += 0.05f; // Ajuste selon ton framerate

            if (stateTimer >= stateDuration)
            {
                // Changement aléatoire d'état
                currentState = (FlyState)rng.Next(0, 3);
                stateDuration = 0.5f + (float)rng.NextDouble() * 4f; // entre 0.5 et 2.5 sec
                stateTimer = 0f;
            }

            switch (currentState)
            {
                case FlyState.Idle:
                    Target.Velocity = 0f;
                    Target.AnimationController.CurrentAnimation = anim_name_idle;
                    break;

                case FlyState.Flying:
                    Move(); // On garde la trajectoire courbée de ta méthode Move()
                    Target.AnimationController.CurrentAnimation = anim_name_flying;
                    break;

                case FlyState.Turning:
                    // Change la direction de façon plus brutale
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
