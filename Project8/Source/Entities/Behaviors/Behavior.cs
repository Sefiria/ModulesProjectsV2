using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project8.Source.Entities.Behaviors
{
    public class Behavior
    {
        public enum AnimationsNeeds
        {
            Idle, Walk, Crouch
        }

        public Behavior()
        {
        }
        public virtual string Update() => "";
    }
}
