using SFX;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tooling;

namespace Project7.Source.Entities.Behaviors
{
    public class BehaviorArcade : Behavior
    {
        public Entity Target;
        
        Game1 Context => Game1.Instance;

        public BehaviorArcade(Entity e)
            : base()
        {
            Target = e;
        }
        public override string Update()
        {
            return "";
        }
    }
}
