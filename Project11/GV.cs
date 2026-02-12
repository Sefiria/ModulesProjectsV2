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
            Font = new Font("Courrier New", 12F);
            //FontWidth = (int)FormMain.Graphics.MeasureString("W", Font).Width;
            //FontHeight = Font.Height;
            FontWidth = 12;
            FontHeight = 20;
            MouseStates.Initialize(FormMain.Instance.Render);
            //KB.Init();
        }
        public static void Update()
        {
            Ticks++;
            while (Ticks < 0) Ticks += max_ticks;
            while (Ticks > max_ticks) Ticks -= max_ticks;
            MouseStates.Update();
            //KB.Update();
        }
    }
}
