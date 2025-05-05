using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tooling
{
    public class Angle : LoopValueF
    {
        public enum Angles
        {
            Right = 0,
            Up = 90,
            Left = 180,
            Down = 270
        }

        public Angle() : base()
        {
        }
        public Angle(float angle) : base(angle, 0F, 360F)
        {
            Value = angle;
        }
        public Angle(Angles angle) : base((float)angle, 0F, 360F)
        {
            Value = (float)angle;
        }

        public static float Right = (float)Angles.Right;
        public static float Up = (float)Angles.Up;
        public static float Left = (float)Angles.Left;
        public static float Down = (float)Angles.Down;
    }
}
