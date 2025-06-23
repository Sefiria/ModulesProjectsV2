using Project7.Source.Entities.Behaviors;
using System;

namespace Project7.Source.Entities
{
    public class EntityFactory
    {
        public static Entity CreateRabbit()
        {
            var context = Game1.Instance;

            var pinou = new Entity();
            pinou.X = Random.Shared.Next(10, context.ScreenWidth - 40);
            pinou.Y = Random.Shared.Next(10, context.ScreenHeight - 40);
            pinou.AnimationController = new Tools.Animations.AnimationController(context.GraphicsDevice);
            pinou.AnimationController.AddAnimation(context.GraphicsDevice, "idle", assets_bindings.Resources["pinou_idle"]);
            pinou.AnimationController.AddAnimation(context.GraphicsDevice, "run", assets_bindings.Resources["pinou_run"]);
            pinou.AnimationController.AddAnimation(context.GraphicsDevice, "hold", assets_bindings.Resources["pinou_hold"]);
            pinou.AnimationController.CurrentAnimation = "idle";
            pinou.Behaviors.Add(new BehaviorRabbit(pinou,
                () => pinou.AnimationController.CurrentAnimation = "idle",
                () => pinou.AnimationController.CurrentAnimation = "run",
                () => pinou.AnimationController.CurrentAnimation = "hold"
            ));
            return pinou;
        }
    }
}
