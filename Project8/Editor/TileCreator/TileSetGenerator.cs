using Project8.Source;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Tooling;
using KB = Tooling.KB;
using MS = Tooling.MouseStates;
using G = System.Drawing.Graphics;
using Project8.Source.TiledMap;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Project8.Editor.TileSetCreator
{
    public partial class TileSetGenerator : Form
    {
        bool IsMouseDown = false;
        Point ms_old, ms;
        int scale = 20;
        Bitmap Image;

        public static int sz = GlobalVariables.tilesize;

        public TileSetGenerator()
        {
            InitializeComponent();
        }
        private void TileCreator_Load(object sender, EventArgs e)
        {
            Image = new Bitmap(sz, sz);
            var colorsbtn = new List<PictureBox>() { usedColor, color1, color2, color3, color4, color5, color6, color7, color8, colorBuffer };
            foreach (var c in colorsbtn)
            {
                c.Image = new Bitmap(c == usedColor ? 64 : 32, c == usedColor ? 64 : 32);
                using (G g = G.FromImage(c.Image))
                    g.Clear(Color.White);
            }
            btNew_Click(null, null);
        }

        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            IsMouseDown = true;
            ManageMouse(sender, e);
        }
        private void Render_MouseLeave(object sender, EventArgs e)
        {
            IsMouseDown = false;
        }
        private void Render_MouseMove(object sender, MouseEventArgs e)
        {
            ManageMouse(sender, e);
        }
        private void Render_MouseUp(object sender, MouseEventArgs e)
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

            int tx = Math.Clamp((int)(ms.X / scale), 0, sz - 1);
            int ty = Math.Clamp((int)(ms.Y / scale), 0, sz - 1);

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
                    int px = (int)(p.X / scale);
                    int py = (int)(p.Y / scale);
                    if (px >= 0 && py >= 0 && px < sz && py < sz)
                        Image.SetPixel(px, py, IsLeft ? usedColor.BackColor : Color.White);
                }
                Image.SetPixel(tx, ty, IsLeft ? usedColor.BackColor : Color.White);
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
                using (G _g = G.FromImage(colorBuffer.Image)) _g.Clear(colorBuffer.BackColor);
                using (G _g = G.FromImage(usedColor.Image)) _g.Clear(usedColor.BackColor);
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
                g.DrawImage(ResizeExact(Image, img.Width, img.Height), 0, 0);
            }
            render.Image = ResizeExact(img, sz * scale, sz * scale);
            render.Refresh();
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


        private void color_MouseClick(object sender, MouseEventArgs e)
        {
            var colorsbtn = new List<PictureBox>() { color1, color2, color3, color4, color5, color6, color7, color8, colorBuffer };
            var c = colorsbtn.First(c => sender == c);

            if (e.Button == MouseButtons.Left)
            {
                usedColor.BackColor = c.BackColor;
                using (G _g = G.FromImage(usedColor.Image)) _g.Clear(usedColor.BackColor);
            }
            else if (e.Button == MouseButtons.Middle)
            {
                c.BackColor = Color.White;
                usedColor.BackColor = c.BackColor;
                using (G _g = G.FromImage(c.Image)) _g.Clear(c.BackColor);
                using (G _g = G.FromImage(usedColor.Image)) _g.Clear(usedColor.BackColor);
            }
            else
            {
                colorDialog1.Color = c.BackColor;
                if (colorDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        c.BackColor = colorDialog1.Color;
                        usedColor.BackColor = c.BackColor;
                        using (G _g = G.FromImage(c.Image)) _g.Clear(c.BackColor);
                        using (G _g = G.FromImage(usedColor.Image)) _g.Clear(usedColor.BackColor);
                    }
                }
            }
            usedColor.Invalidate();
        }

        object obj = "lock-preview";
        private void btPreview_Click(object sender, EventArgs e)
        {
            lock (obj)
            {
                Cursor.Current = Cursors.WaitCursor;
                GeneratePreview();
                Cursor.Current = Cursors.Default;
            }
        }
        private void GeneratePreview()
        {
            var preview = new Bitmap(64, 64);
            var parts = Generate();
            using (G g = G.FromImage(preview))
            {
                //  a  f  h  v  z  q  s  d  zq  zd  sq  sd  ns  nz  nd  nq
                //  0  1  2  3  4  5  6  7   8   9  10  11  12  13  14  15
                int x = 0, y = 0;
                g.DrawImage(parts[11], x++ * sz, y * sz);
                g.DrawImage(parts[13], x++ * sz, y * sz);
                g.DrawImage(parts[10], x++ * sz, y * sz);
                x = 0; y++;
                g.DrawImage(parts[15], x++ * sz, y * sz);
                g.DrawImage(parts[01], x++ * sz, y * sz);
                g.DrawImage(parts[14], x++ * sz, y * sz);
                x = 0; y++;
                g.DrawImage(parts[09], x++ * sz, y * sz);
                g.DrawImage(parts[12], x++ * sz, y * sz);
                g.DrawImage(parts[08], x++ * sz, y * sz);
            }
            render_preview.Image = ResizeExact(preview, sz * scale, sz * scale);
            render_preview.Refresh();
        }
        private List<Image> Generate()
        {
            List<Image> parts = new List<Image>();
            var src = ResizeExact(render.Image, sz, sz);
            Bitmap img;
            /*
                        a : (Alone) no connection
                        f : (Full) all connections
                        h : horizontal connection
                        v : vertical connection
                        z q s d : z=top, q=left, s=bottom, d=right / connections
                        n : (not) all except [ns means all connections except s (ns=not s=not bottom)]

                0|1|2
                3|4|5
                6|7|8

                        a  f  h  v  z  q  s  d  zq  zd  sq  sd  ns  nz  nd  nq

                asymetric parts copy 16x16 : 5px + 6px + 5px for every row, middle pixel is a copy of the x-1 one
            */
            parts.Add(src); // a
            parts.Add(draw(src, "444444444")); // f
            parts.Add(draw(src, "012444678")); // h
            parts.Add(draw(src, "042345648")); // v
            parts.Add(draw(src, "645645678")); // z
            parts.Add(draw(src, "112445778")); // q
            parts.Add(draw(src, "012345345")); // s
            parts.Add(draw(src, "011344677")); // d
            parts.Add(draw(src, "445445778")); // zq
            parts.Add(draw(src, "344344677")); // zd
            parts.Add(draw(src, "112445445")); // sq
            parts.Add(draw(src, "011344344")); // sd
            parts.Add(draw(src, "444444777")); // ns
            parts.Add(draw(src, "111444444")); // nz
            parts.Add(draw(src, "445445445")); // nd
            parts.Add(draw(src, "344344344")); // nq
            return parts;
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

        private void btNew_Click(object sender, EventArgs e)
        {
            render_preview.Image = new Bitmap(sz * scale, sz * scale);
            Image = new Bitmap(sz, sz);
            UpdateRender();
        }
        private void btSave_Click(object sender, EventArgs e)
        {
            List<Image> parts = Generate();

            /* Ordre exact correspondant à (voir default pattern Autotile.cs) :
                a,  f,  h,  v
                z,  q,  s,  d
                zq, zd, sq, sd
                ns, nz, nd, nq
            */
            int[] order = {
                0, 1, 2, 3,
                4, 5, 6, 7,
                8, 9, 10, 11,
                12, 13, 14, 15
            };

            Bitmap final = new Bitmap(sz * 4, sz * 4);

            using (G g = G.FromImage(final))
            {
                g.InterpolationMode = InterpolationMode.NearestNeighbor;
                g.PixelOffsetMode = PixelOffsetMode.Half;
                g.SmoothingMode = SmoothingMode.None;

                int idx = 0;

                for (int y = 0; y < 4; y++)
                {
                    for (int x = 0; x < 4; x++)
                    {
                        int partIndex = order[idx++];
                        g.DrawImage(parts[partIndex], x * sz, y * sz);
                    }
                }
            }

            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Filter = "PNG Image|*.png";
                dlg.Title = "Enregistrer le tileset généré";
                dlg.InitialDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Assets\\Textures\\Tilesets");

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    final.Save(dlg.FileName, ImageFormat.Png);
                }
            }

            final.Dispose();
        }

        private void btRetro_Click(object sender, EventArgs e)
        {
            var dial = new OpenFileDialog();
            dial.InitialDirectory = Directory.GetCurrentDirectory() + @"\Assets\Textures\Tilesets";
            dial.Title = "Retrocharger un tileset pour génération";
            dial.Filter = "PNG Image|*.png";
            if (dial.ShowDialog(this) == DialogResult.OK)
            {
                if (!string.IsNullOrWhiteSpace(dial.FileName))
                {
                    render.Image = ResizeExact(new Bitmap(dial.FileName).Crop(16), sz * scale, sz * scale);
                    render_preview.Image = new Bitmap(sz * scale, sz * scale);
                    render.Refresh();
                    GeneratePreview();
                }
            }
        }
    }
}
