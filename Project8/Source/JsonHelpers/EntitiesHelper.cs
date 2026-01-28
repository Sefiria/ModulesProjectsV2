using System.Collections.Generic;
using static Project8.Source.Entities.Behaviors.Behavior;
using static Project8.Source.Entities.Entity;

namespace Project8.Source.JsonHelpers
{
    public class Entity
    {
        public string[] Behaviors { get; set; }
        public Dictionary<AnimationsNeeds, string> Animations { get; set; }
        public Alignments Alignment { get; set; }
        public float AnimationSpeed { get; set; } = 5f;
        public bool CanCollect { get; set; } = false;
        public static Entity New() => new Entity()
        {
            Behaviors = [],
            Animations = new Dictionary<AnimationsNeeds, string>(),
            Alignment = Alignments.bottom,
            AnimationSpeed = 4F,
            CanCollect = false
        };
        public Entity Clone() => new Entity() {
            Behaviors = Behaviors,
            Animations = Animations,
            Alignment = Alignment,
            AnimationSpeed = AnimationSpeed,
            CanCollect = CanCollect
        };
    }

    public class RootEntities
    {
        public List<EntityData> entities { get; set; }
    }
    public class EntityData
    {
        public string EntityName { get; set; }
        public List<string> behaviors { get; set; }
        public string alignment { get; set; }
        public List<AnimationEntry> Animations { get; set; }
        public string AnimationSpeed { get; set; }
        public bool? CanCollect { get; set; }
    }
    public class AnimationEntry
    {
        public string AnimationsNeeds { get; set; }
        public string anim { get; set; }
    }
}
