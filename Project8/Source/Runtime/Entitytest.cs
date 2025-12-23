using Project8.Source.Entities;
using Project8.Source.Entities.Behaviors;
using System.IO;

namespace Project8.Source.Runtime
{
    internal class Entitytest : Entity
    {
        public Entitytest()
        {
            var g = GameMain.Instance.GraphicsDevice;
            Animation = new Animation2D("test_idle", Directory.GetCurrentDirectory()+"/Assets/Textures/Entities/test_idle.png");
            Behaviors.Add(new BehaviorPlayer(this/*, () => AnimationController.CurrentAnimation = "idle"*/));
            GameMain.Instance.EntityManager.Entities.Add(this);
        }
    }
}
