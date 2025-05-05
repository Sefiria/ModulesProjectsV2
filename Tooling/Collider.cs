using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace Tooling
{
    public class Collider
    {
        public vecf vec = vecf.Zero;

        public float x { get => vec.x; set => vec.x = value; }
        public float y { get => vec.y; set => vec.y = value; }

        public virtual vecf size { get; } = new vecf(1F, 1F);

        public virtual bool SameAs(Collider other) => false;

        public Collider Clone()
        {
            switch(this)
            {
                default : return null;
                case Circle _: return new Circle(this as Circle);
                case Box _: return new Box(this as Box);
            }
        }
    }
}
