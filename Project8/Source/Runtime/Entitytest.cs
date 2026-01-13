using Microsoft.Xna.Framework.Graphics;
using Project8.Source.Entities;
using Project8.Source.Entities.Behaviors;
using System.IO;
using Tools.Animations;

namespace Project8.Source.Runtime
{
    internal class Entitytest : Entity
    {
        public Entitytest(int x, int y)
        {
            X = x - W / 2F;
            Y = y - H / 2F;
            var g = GameMain.Instance.GraphicsDevice;
            Animation = new Animation2D("player", Directory.GetCurrentDirectory()+"/Assets/Textures/Entities/test_idle.png");
            Animation.Speed = 5F;
            Behaviors.Add(new BehaviorPlayer(this));
            Alignment = Alignments.bottom;
            GameMain.Instance.EntityManager.Entities.Add(this);
        }
    }
}
