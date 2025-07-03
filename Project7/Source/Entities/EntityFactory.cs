using Project7.Source.Entities.Behaviors;
using System;

namespace Project7.Source.Entities
{
    public class EntityFactory
    {
        public static Entity CreateRabbit(int tile_x, int tile_y, string name = null) => CreateRabbit((float)tile_x * Game1.Instance.tilesize, (float)tile_y * Game1.Instance.tilesize, name);
        public static Entity CreateRabbit(float x = -1F, float y = -1F, string name = null)
        {
            var context = Game1.Instance;

            var pinou = new Entity(name);
            pinou.X = x != -1F ? x : Random.Shared.Next(10, context.ScreenWidth - 40);
            pinou.Y = y != -1F ? y : Random.Shared.Next(10, context.ScreenHeight - 40);
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
        public static Entity CreateFly(float x = -1F, float y = -1F, string name = null, Action trigger_dead = null)
        {
            var context = Game1.Instance;

            var fly = new Entity(name);
            fly.X = x != -1F ? x : Random.Shared.Next(10, context.ScreenWidth - 40);
            fly.Y = y != -1F ? y : Random.Shared.Next(10, context.ScreenHeight - 40);
            fly.AnimationController = new Tools.Animations.AnimationController(context.GraphicsDevice);
            fly.AnimationController.AddAnimation(context.GraphicsDevice, "idle", assets_bindings.Resources["fly_idle"], 1);
            fly.AnimationController.AddAnimation(context.GraphicsDevice, "flying", assets_bindings.Resources["fly_flying"]);
            fly.AnimationController.CurrentAnimation = "idle";
            fly.Behaviors.Add(new BehaviorFly(fly, "idle", "flying", trigger_dead));
            fly.HasCollisions = false;
            fly.ApplyRotationFromLook = true;
            return fly;
        }
        public static Entity CreateArcade(float x = -1F, float y = -1F, string name = null)
        {
            var context = Game1.Instance;

            var arcade = new Entity(name);
            arcade.X = x != -1F ? x : Random.Shared.Next(10, context.ScreenWidth - 40);
            arcade.Y = y != -1F ? y : Random.Shared.Next(10, context.ScreenHeight - 40);
            arcade.scale = Game1.Instance.scale;
            arcade.AnimationController = new Tools.Animations.AnimationController(context.GraphicsDevice);
            arcade.AnimationController.AddAnimation(context.GraphicsDevice, "idle", assets_bindings.Resources["tilesets/arcade"], 4);
            arcade.AnimationController.CurrentAnimation = "idle";
            arcade.Behaviors.Add(new BehaviorArcade(arcade));
            arcade.OutlineWhenHover = true;
            return arcade;
        }
    }
}
