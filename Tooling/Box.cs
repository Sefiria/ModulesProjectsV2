using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tooling;

namespace Tooling
{
    public class Box : Collider
    {
        public vecf sz = vecf.Zero;

        public float w { get => sz.x; set => sz.x = value; }
        public float h { get => sz.y; set => sz.y = value; }
        public override vecf size => sz;
        public RectangleF rectf => new RectangleF(x, y, w, h);
        public Rectangle rect => new Rectangle((int)x, (int)y, (int)w, (int)h);

        public Box(Box b)
        {
            x = b.x;
            y = b.y;
            w = b.w;
            h = b.h;
        }
        public Box(float x, float y, float w, float h)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.h = h;
        }
        public Box(Circle c)
        {
            x = c.r == 1 ? c.x : c.x - c.r;
            y = c.r == 1 ? c.y : c.y - c.r;
            w = c.r * 2;
            h = c.r * 2;
        }
        public Box()
        {
            x = y = w = h = 0;
        }

        public override bool SameAs(Collider other) => other is Box ? x == other.x && y == other.y && w == (other as Box).w && h == (other as Box).h : false;
    }
}
