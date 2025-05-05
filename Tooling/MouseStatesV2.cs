using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Tooling
{
    public class MouseStates
    {
        public static Dictionary<MouseButtons, bool> ButtonsDown = new Dictionary<MouseButtons, bool>();
        public static Dictionary<MouseButtons, bool> ReleasedButtons = new Dictionary<MouseButtons, bool>();
        public static bool IsDown => ButtonsDown.Any(b => b.Value);
        public static bool IsUp => !IsDown;
        public static bool IsButtonDown(MouseButtons button, bool pressed = false) => pressed ? IsButtonPressed(button) : ButtonsDown[button];
        public static bool IsButtonPressed(MouseButtons button) => ButtonsDown[button] && ReleasedButtons[button];
        public static bool PositionChanged => Math.Max(Position.X - OldPosition.X, Position.Y - OldPosition.Y) >= 1F;
        public static float LenghtDiff => Position.MinusF(OldPosition).Length();
        public static PointF Position = PointF.Empty, OldPosition = PointF.Empty;
        public static int Delta = 0;
        public static PictureBox Render = null;
        public static void Initialize()
        {
            ButtonsDown.Clear();
            ReleasedButtons.Clear();
            foreach (MouseButtons b in Enum.GetValues(typeof(MouseButtons)))
            {
                ButtonsDown[b] = false;
                ReleasedButtons[b] = true;
            }
        }
        public static void Initialize(PictureBox render)
        {
            ButtonsDown.Clear();
            ReleasedButtons.Clear();
            foreach (MouseButtons b in Enum.GetValues(typeof(MouseButtons)))
            {
                ButtonsDown[b] = false;
                ReleasedButtons[b] = true;
            }

            Render = render;
            render.MouseDown += (s, e) => ButtonsDown[e.Button] = true;
            render.MouseUp += (s, e) => ButtonsDown[e.Button] = false;
            render.MouseMove += (s, e) =>
            {
                Position = e.Location;
            };
            render.MouseWheel += (s, e) => Delta = e.Delta;
        }
        public static void Update()
        {
            if (Render?.IsDisposed ?? false)
                return;
            Delta = 0;
            foreach (MouseButtons b in Enum.GetValues(typeof(MouseButtons)))
                ReleasedButtons[b] = !ButtonsDown[b];
            OldPosition = Position;
        }
        public static void ForceReleaseAllButtons()
        {
            foreach (MouseButtons b in Enum.GetValues(typeof(MouseButtons)))
                ReleasedButtons[b] = ButtonsDown[b] = false;
        }
    }
}
