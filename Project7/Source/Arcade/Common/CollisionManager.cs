using Project7.Source.Arcade.scenes.space;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace Project7.Source.Arcade.Common
{
    public static class CollisionManagerHelper
    {
        public static bool Collides(this Collider collider, Collider other)
        {
            switch(collider.kind)
            {
                case Collider.Kind.Dot:
                    switch (other.kind)
                    {
                        case Collider.Kind.Dot: return collider.Dot == other.Dot;
                        case Collider.Kind.Circle: return Maths.CollisionPointCercle(collider.Dot, other.Circle);
                        case Collider.Kind.Box: return Maths.CollisionPointBox(collider.Dot, other.Box);
                    }
                    break;
                case Collider.Kind.Circle:
                    switch (other.kind)
                    {
                        case Collider.Kind.Dot: return Maths.CollisionPointCercle(other.Dot, collider.Circle);
                        case Collider.Kind.Circle: return Maths.CollisionCercleCercle(collider.Circle, other.Circle);
                        case Collider.Kind.Box: return Maths.CollisionCercleBox(collider.Circle, other.Box) != 0;
                    }
                    break;
                case Collider.Kind.Box:
                    switch (other.kind)
                    {
                        case Collider.Kind.Dot: return Maths.CollisionPointBox(other.Dot, collider.Box);
                        case Collider.Kind.Circle: return Maths.CollisionCercleBox(other.Circle, collider.Box) != 0;
                        case Collider.Kind.Box: return Maths.CollisionBoxBox(collider.Box, other.Box);
                    }
                    break;
            }
            return false;
        }
    }
    public class CollisionManager
    {
        public static ArcadeMain Context => ArcadeMain.instance;
        public static EntityManager EntityManager => Context.EntityManager;

        public void Update()
        {
            void execute_on_single(IEnumerable<Entity> entities, Entity other)
            {
                foreach (var target in entities)
                {
                    if (target.GetCollider().Collides(other.GetCollider()))
                    {
                        target.OnCollision(other);
                        other.OnCollision(target);
                    }
                }
            }
            void execute(IEnumerable<Entity> entities, IEnumerable<Entity> others)
            {
                foreach (var target in entities)
                {
                    foreach (var other in others.Except([target]))
                    {
                        if (target.GetCollider().Collides(other.GetCollider()))
                        {
                            target.OnCollision(other);
                            other.OnCollision(target);
                        }
                    }
                }
            }

            if (Game1.Instance.Ticks % 20 != 0)
            {
                switch(ArcadeMain.Scenes[Context.scene_index])
                {
                    case ArcadeSpace space:
                        execute(EntityManager.Entities.OfType<BaseShip>(), EntityManager.Entities.OfType<BaseShip>());
                        execute(EntityManager.Entities.OfType<Bullet>(), EntityManager.Entities.OfType<BaseShip>());
                        execute_on_single(EntityManager.Entities.OfType<PowerUp>(), space.Starship);
                        break;
                }
            }
        }
    }
}
