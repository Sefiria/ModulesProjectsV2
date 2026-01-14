using Project8.Source.Entities;
using Project8.Source.Entities.Behaviors;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Tools.Animations;
using static Project8.Source.Entities.Behaviors.Behavior;

namespace Project8.Source.Runtime
{
    internal class Entitytest : Entity
    {
        string[] behaviors = [];
        Dictionary<Behavior.AnimationsNeeds, string> Animations;
        Alignments alignment;
        float AnimationSpeed = 5F;

        public Entitytest(int x, int y)
        {
            // inputs
            behaviors = ["BehaviorPlayer"];
            alignment = Alignments.bottom;
            Animations = new Dictionary<AnimationsNeeds, string>()
            {
                [AnimationsNeeds.Idle] = "player_idle",
                [AnimationsNeeds.Walk] = "player_walk",
                [AnimationsNeeds.Crouch] = "player_crouch",
            };
            AnimationSpeed = 4F;

            // impl
            var path = Directory.GetCurrentDirectory() + "/Assets/Textures/Animations/";
            var g = GameMain.Instance.GraphicsDevice;
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();
            var behaviorstypes = types
                                .Where(t => t != null
                                        && typeof(Behavior).IsAssignableFrom(t)
                                        && !t.IsAbstract);

            X = x - W / 2F;
            Y = y - H / 2F;
            Alignment = alignment;
            AnimationController = new AnimationController(g);
            foreach (var anim in Animations)
            {
                AnimationController.AddAnimation(g, Enum.GetName(anim.Key), path + anim.Value + ".png", 2);
                AnimationController.Animations.Last().Value.Speed = AnimationSpeed;
            }
            foreach (var behavior in behaviors)
            {
                var found = behaviorstypes.FirstOrDefault(t => t.Name == behavior);
                if(found != null)
                    Behaviors.Add((Behavior)Activator.CreateInstance(found, this));
            }
            GameMain.Instance.EntityManager.Entities.Add(this);
        }
    }
}
