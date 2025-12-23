using Project8.Source.Entities.Behaviors;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Project8.Source.Entities
{
    public class EntityFactory
    {
        //public static Entity CreateRabbit(int tile_x, int tile_y, string name = null) => CreateRabbit((float)tile_x * GameMain.Instance.tilesize, (float)tile_y * GameMain.Instance.tilesize, name);
        //public static Entity CreateRabbit(float x = -1F, float y = -1F, string name = null)
        //{
        //    var context = GameMain.Instance;

        //    var pinou = new Entity(name);
        //    pinou.X = x != -1F ? x : Random.Shared.Next(10, context.ScreenWidth - 40);
        //    pinou.Y = y != -1F ? y : Random.Shared.Next(10, context.ScreenHeight - 40);
        //    pinou.AnimationController = new Tools.Animations.AnimationController(context.GraphicsDevice);
        //    pinou.AnimationController.AddAnimation(context.GraphicsDevice, "idle", ResourcesLoader.pinou_idle);
        //    pinou.AnimationController.AddAnimation(context.GraphicsDevice, "run", ResourcesLoader.pinou_run);
        //    pinou.AnimationController.AddAnimation(context.GraphicsDevice, "hold", ResourcesLoader.pinou_hold);
        //    pinou.AnimationController.CurrentAnimation = "idle";
        //    pinou.Behaviors.Add(new BehaviorRabbit(pinou,
        //        () => pinou.AnimationController.CurrentAnimation = "idle",
        //        () => pinou.AnimationController.CurrentAnimation = "run",
        //        () => pinou.AnimationController.CurrentAnimation = "hold"
        //    ));
        //    return pinou;
        //}
    }
}
