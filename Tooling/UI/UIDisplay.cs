using System.Drawing;
using static Tooling.Enumerations;

namespace Tooling.UI
{
    public class UIDisplay
    {
        public static Color FrameColor;
        public static Brush TextBrush;
        public static Font Font;
        public static UIDisplayAnchor Anchor = UIDisplayAnchor.MiddleCenter;

        static UIDisplay()
        {
            FrameColor = Color.Tan;
            TextBrush = Brushes.Black;
            Font = Common.MediumFont;
        }

        public static void Display(Graphics g, Font font, string text, float x, float y, float frame_opacity = 1F)
        {
            DrawFrame(g, font, text, x, y, frame_opacity);
            DrawText(g, font, TextBrush, text, x, y, frame_opacity);
        }
        public static void Display(Graphics g, Font font, Brush brush, string text, float x, float y, float frame_opacity = 1F)
        {
            DrawFrame(g, font, text, x, y, frame_opacity);
            DrawText(g, font, brush, text, x, y, frame_opacity);
        }
        public static void Display(Graphics g, Font font, Color color, string text, float x, float y, float frame_opacity = 1F)
            => Display(g, font, new SolidBrush(color), text, x, y, frame_opacity);
        public static void Display(Graphics g, string text, float x, float y, float frame_opacity = 1F)
            => Display(g, Font, text, x, y, frame_opacity);

        private static SizeF temp_size;
        private static void DrawFrame(Graphics g, Font font, string text, float x, float y, float frame_opacity)
        {
            float marge = 5F;
            int radius = 5;
            var size = temp_size = g.MeasureString(text, font);
            float w = size.Width;
            float h = size.Height;
            var rect = Common.CreateRoundedRect(x - marge - size.Width / 2F, y - marge - size.Height / 2F, w + marge * 2F, h + marge * 2F, radius);
            Color color = Color.FromArgb((int)(byte.MaxValue * frame_opacity), FrameColor);
            g.FillPath(new SolidBrush(color), rect);
            g.DrawPath(new Pen(color.WithBrightness(0.5F), 2F), rect);
        }
        private static void DrawText(Graphics g, Font font, Brush brush, string text, float x, float y, float frame_opacity)
        {
            var size = temp_size;

            if (brush is SolidBrush)
            {
                Color color = Color.FromArgb((int)(byte.MaxValue * frame_opacity), (brush as SolidBrush).Color);
                g.DrawString(text, font, new SolidBrush(color), x - size.Width / 2F, y - size.Height / 2F);
            }
            else
            {
                g.DrawString(text, font, brush, x - size.Width / 2F, y - size.Height / 2F);
            }
        }
    }
}
