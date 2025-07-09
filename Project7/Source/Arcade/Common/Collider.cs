using System.Drawing;
using Tooling;

namespace Project7.Source.Arcade.Common
{
    public class Collider
    {
        public enum Kind
        {
            Dot, Circle, Box
        }
        public Kind kind;
        public Point Dot;
        public Circle Circle;
        public Box Box;
        public Collider(Kind kind)
        {
            this.kind = kind;
        }
    }
}
