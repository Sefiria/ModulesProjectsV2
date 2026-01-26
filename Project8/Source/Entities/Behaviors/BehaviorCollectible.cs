using Microsoft.Xna.Framework.Input;
using Project8.Source.TiledMap;
using System;
using Tooling;

namespace Project8.Source.Entities.Behaviors
{
    public class BehaviorCollectible : Behavior
    {
        public Entity Target;

        float srcY, angle = 0F;

        public BehaviorCollectible(Entity e)
            : base()
        {
            Target = e;
            srcY = Target.Y;
        }
        public override string Update()
        {
            CollisionsManager.Detect(Target, GameMain.Instance.EntityManager.Entities.ToArray());

            Target.Y = srcY + (float)Math.Sin(angle) * 3;
            angle += 0.1f;
            if (angle > MathF.Tau)
                angle -= MathF.Tau;
            Target.AnimationController.Update();
            return "";
        }
    }
}
