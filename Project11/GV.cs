using System.Drawing;
using Tooling;

namespace Project11
{
    internal static class GV
    {
        private static long max_ticks = 100000000L;

        public static long Ticks;
        public static Font Font;
        public static float FontWidth, FontHeight;

        public static void Initialize()
        {
            Ticks = 0L;
            Font = new Font("Segoe UI", 10F);
            FontWidth = (int)FormMain.Graphics.MeasureString("i", Font).Width;
            FontHeight = Font.Height;
            MouseStates.Initialize(FormMain.Instance.Render);
        }
        public static void Update()
        {
            Ticks++;
            while (Ticks < 0) Ticks += max_ticks;
            while (Ticks > max_ticks) Ticks -= max_ticks;
            MouseStates.Update();
        }
    }
}
