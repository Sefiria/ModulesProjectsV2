using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tooling;

namespace Tooling
{
    public class Circle : Collider
    {
        public float r;

        public float diameter => 2F * r;
        public float w => diameter;
        public float h => diameter;
        public override vecf size => new vecf(w, h);
        public RectangleF rectF => new RectangleF(x-r, y-r, diameter, diameter);
        public Rectangle rect => new Rectangle((int)(x-r), (int)(y-r), (int)diameter, (int)diameter);

        public Circle(Circle c)
        {
            r = c.r;
            x = c.x;
            y = c.y;
        }
        public Circle(float x, float y, float r)
        {
            this.r = r;
            this.x = (float)Math.Ceiling(x);
            this.y = (float)Math.Ceiling(y);
        }
        public Circle(float x, float y, int r)
        {
            this.r = r;
            this.x = (float)Math.Ceiling(x);
            this.y = (float)Math.Ceiling(y);
        }

        public override bool SameAs(Collider other) => other is Circle ? x == other.x && y == other.y && r == (other as Circle).r : false;
    }
}
