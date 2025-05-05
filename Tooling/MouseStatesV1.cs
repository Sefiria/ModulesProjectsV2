using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Tooling
{
    public class MouseStatesV1
    {
        public static MouseButtons ButtonDown = MouseButtons.None;
        public static bool IsDown => ButtonDown != MouseButtons.None;
        public static bool IsUp => !IsDown;
        public static bool PositionChanged => Position != OldPosition;
        public static float LenghtDiff => Position.MinusF(OldPosition).Length();
        public static PointF Position = PointF.Empty, OldPosition = PointF.Empty;
        public static int Delta = 0;
        public static void Update()
        {
            Delta = 0;
        }
    }
}
