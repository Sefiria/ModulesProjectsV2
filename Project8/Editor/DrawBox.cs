using Project8.Source;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tooling;
using G = System.Drawing.Graphics;

namespace Project8.Editor
{
    public partial class DrawBox : UserControl
    {
        bool IsMouseDown = false;
        Point ms_old, ms;
        int scale = 20;
        public Bitmap Image;
        public static int sz = GlobalVariables.tilesize;
        PictureBox usedColor, colorBuffer;
        public event EventHandler RenderUpdated;

        public DrawBox(ref Bitmap img, ref PictureBox usedColor, ref PictureBox colorBuffer)
        {
            InitializeComponent();
            Image = img;
            this.usedColor = usedColor;
            this.colorBuffer = colorBuffer;
            UpdateRender();
        }
        private void DrawBox_MouseDown(object sender, MouseEventArgs e)
        {
            IsMouseDown = true;
            ManageMouse(sender, e);
        }
        private void DrawBox_MouseLeave(object sender, EventArgs e)
        {
            IsMouseDown = false;
        }
        private void DrawBox_MouseMove(object sender, MouseEventArgs e)
        {
            ManageMouse(sender, e);
        }
        private void DrawBox_MouseUp(object sender, MouseEventArgs e)
        {
            IsMouseDown = false;
        }
        private void ManageMouse(object sender, MouseEventArgs e)
        {
            ms_old = ms;
            ms = render.PointToClient(MousePosition);
            if (!IsMouseDown) return;

            bool IsLeft = e.Button == MouseButtons.Left;
            bool IsRight = e.Button == MouseButtons.Right;
            bool IsMiddle = e.Button == MouseButtons.Middle;
            bool IsFill = IsLeft && ModifierKeys.HasFlag(Keys.Shift) && ModifierKeys.HasFlag(Keys.Control);

            int tx = Math.Clamp((int)(ms.X / scale * 2), 0, sz - 1);
            int ty = Math.Clamp((int)(ms.Y / scale * 2), 0, sz - 1);

            if (IsFill)
            {
                Color target = Image.GetPixel(tx, ty);
                Color replacement = usedColor.BackColor;

                if (target != replacement)
                    FloodFill(Image, tx, ty, target, replacement);

                UpdateRender();
                return;
            }

            if (IsLeft || IsRight)
            {
                double d = Maths.Distance(ms_old, ms);
                double step = 1.0 / Math.Max(d, 1.0);
                for (double t = 0.0; t <= 1.0; t += step)
                {
                    PointF p = Maths.Lerp(ms_old, ms, t);
                    int px = (int)(p.X / scale * 2);
                    int py = (int)(p.Y / scale * 2);
                    if (px >= 0 && py >= 0 && px < sz && py < sz)
                        Image.SetPixel(px, py, IsLeft ? usedColor.BackColor : Color.Transparent);
                }
                UpdateRender();
                return;
            }
            else if (IsMiddle)
            {
                usedColor.BackColor = Image.GetPixel(tx, ty);
                colorBuffer.BackColor = usedColor.BackColor;
                usedColor.BackColor = colorBuffer.BackColor;

                if (usedColor.Image == null)
                    usedColor.Image = new Bitmap(usedColor.Width, usedColor.Height);
                using (G _g = G.FromImage(colorBuffer.Image))
                {
                    _g.Clear(colorBuffer.BackColor);
                    _g.DrawRectangle(Pens.Black, 0, 0, colorBuffer.Image.Width - 1, colorBuffer.Image.Height - 1);
                }
                using (G _g = G.FromImage(usedColor.Image))
                {
                    _g.Clear(usedColor.BackColor);
                    _g.DrawRectangle(Pens.Black, 0, 0, usedColor.Image.Width - 1, usedColor.Image.Height - 1);
                }
                colorBuffer.Refresh();
                usedColor.Refresh();
            }
        }
        private void UpdateRender()
        {
            var img = new Bitmap(sz * scale / 2, sz * scale / 2);
            using (G g = G.FromImage(img))
            {
                g.Clear(Color.Black);
                g.SmoothingMode = SmoothingMode.None; g.InterpolationMode = InterpolationMode.NearestNeighbor; g.CompositingQuality = CompositingQuality.HighSpeed;
                using var hb = new HatchBrush(HatchStyle.LargeCheckerBoard, Color.DarkGray, Color.LightGray);
                g.FillRectangle(hb, 0, 0, img.Width, img.Height);
                g.DrawImage(ResizeExact(Image, img.Width / 2, img.Height / 2), 0, 0);
            }
            render.Image = ResizeExact(img, sz * scale, sz * scale);
            render.Refresh();
            RenderUpdated?.Invoke(this, null);
        }

        private void FloodFill(Bitmap bmp, int x, int y, Color target, Color replacement)
        {
            int w = bmp.Width;
            int h = bmp.Height;

            Stack<Point> stack = new Stack<Point>();
            stack.Push(new Point(x, y));

            while (stack.Count > 0)
            {
                Point p = stack.Pop();
                int px = p.X;
                int py = p.Y;

                if (px < 0 || py < 0 || px >= w || py >= h)
                    continue;

                if (bmp.GetPixel(px, py) != target)
                    continue;

                bmp.SetPixel(px, py, replacement);

                stack.Push(new Point(px + 1, py));
                stack.Push(new Point(px - 1, py));
                stack.Push(new Point(px, py + 1));
                stack.Push(new Point(px, py - 1));
            }
        }
        private Bitmap draw(Bitmap src, string pattern)
        {
            Bitmap img = new Bitmap(16, 16);

            int[] S = { 5, 6, 5 };
            int[] D = { 0, 5, 11 };
            int[] map5 = { 0, 1, 2, 3, 4 };
            int[] map6 = { 0, 1, 2, 2, 3, 4 };

            for (int dstIndex = 0; dstIndex < 9; dstIndex++)
            {
                int srcIndex = pattern[dstIndex] - '0';

                // bloc source
                int srcx = D[srcIndex % 3];
                int srcy = D[srcIndex / 3];

                // bloc destination
                int destx = D[dstIndex % 3];
                int destw = S[dstIndex % 3];
                int desty = D[dstIndex / 3];
                int desth = S[dstIndex / 3];

                // mapping en fonction de la TAILLE DEST (5 ou 6)
                int[] mapX = (destw == 6) ? map6 : map5;
                int[] mapY = (desth == 6) ? map6 : map5;

                for (int dy = 0; dy < desth; dy++)
                {
                    int sy = srcy + mapY[dy];   // 0..4 remis dans le bon bloc source

                    for (int dx = 0; dx < destw; dx++)
                    {
                        int sx = srcx + mapX[dx]; // idem pour X

                        img.SetPixel(destx + dx, desty + dy, src.GetPixel(sx, sy));
                    }
                }
            }

            return img;
        }
        static Bitmap ResizeExact(Image src, int w, int h)
        {
            var dst = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            using var g = G.FromImage(dst);
            g.InterpolationMode = InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode = PixelOffsetMode.Half;
            g.SmoothingMode = SmoothingMode.None;
            g.DrawImage(src, new Rectangle(0, 0, w, h), new Rectangle(0, 0, src.Width, src.Height), GraphicsUnit.Pixel);
            return dst;
        }
        public void UpdateImage(ref Bitmap bitmap)
        {
            Image = bitmap;
            UpdateRender();
        }
    }
}
