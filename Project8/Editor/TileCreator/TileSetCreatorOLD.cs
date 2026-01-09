using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Tooling;
using KB = Tooling.KB;
using MS = Tooling.MouseStates;

namespace Project8.Editor.TileSetCreator
{
    public partial class TileSetCreatorOLD : Form
    {
        bool IsMouseDown = false;
        Point ms_old, ms;
        Bitmap Image;
        Timer timerDraw = new Timer() { Enabled = true, Interval = 10 };
        System.Drawing.Graphics g;
        float scale = 20F;

        public TileSetCreatorOLD()
        {
            InitializeComponent();
        }
        private void TileCreator_Load(object sender, EventArgs e)
        {
            Image = new Bitmap(16, 16);
            g = System.Drawing.Graphics.FromImage(Image);
            g.Clear(Color.White);
            timerDraw.Tick += DrawRender;

            var colorsbtn = new List<PictureBox>() { usedColor, color1, color2, color3, color4, color5, color6, color7, color8 };
            foreach (var c in colorsbtn)
            {
                c.Image = new Bitmap(c == usedColor ? 64 : 32, c == usedColor ? 64 : 32);
                g = System.Drawing.Graphics.FromImage(c.Image);
                g.Clear(Color.White);
            }


            PopulateTree(tv, "Assets\\Textures\\Tilesets");
        }

        void PopulateTree(TreeView tv, string rootPath)
        {
            tv.BeginUpdate();
            try
            {
                tv.Nodes.Clear();
                var rootDir = new DirectoryInfo(rootPath);
                var rootNode = CreateDirectoryNode(rootDir);
                tv.Nodes.Add(rootNode);
            }
            finally { tv.EndUpdate(); }
        }

        TreeNode CreateDirectoryNode(DirectoryInfo dir)
        {
            var node = new TreeNode(dir.Name) { Tag = dir.FullName };
            // Dossiers
            foreach (var sub in dir.EnumerateDirectories())
                node.Nodes.Add(CreateDirectoryNode(sub));
            // Fichiers .png
            foreach (var file in dir.EnumerateFiles("*.png"))
                node.Nodes.Add(new TreeNode(Path.GetFileName(file.Name)) { Tag = file.FullName });
            return node;
        }


        private void Render_MouseDown(object sender, MouseEventArgs e)
        {
            IsMouseDown = true;
            ManageMouse(e);
        }
        private void Render_MouseLeave(object sender, EventArgs e)
        {
            IsMouseDown = false;
        }
        private void Render_MouseMove(object sender, MouseEventArgs e)
        {
            ManageMouse(e);
        }
        private void Render_MouseUp(object sender, MouseEventArgs e)
        {
            IsMouseDown = false;
        }
        private void ManageMouse(MouseEventArgs e)
        {
            ms_old = ms;
            ms = Render.PointToClient(MousePosition);

            if (IsMouseDown)
            {
                bool IsLeft = e.Button == MouseButtons.Left;
                bool IsRight = e.Button == MouseButtons.Right;
                bool IsMiddle = e.Button == MouseButtons.Middle;
                if (IsLeft || IsRight)
                {
                    double d = Maths.Distance(ms_old, ms);
                    for (double t = 0.0; t <= 1.0; t += 1 / d)
                    {
                        PointF p = Maths.Lerp(ms_old, ms, t);
                        if (p.X >= 0 && p.Y >= 0 &&
                            p.X < Math.Min(Render.Width, Image.Width * scale) &&
                            p.Y < Math.Min(Render.Height, Image.Height * scale))
                        {
                            int ix = (int)(X + Math.Floor(p.X / scale));
                            int iy = (int)(Y + Math.Floor(p.Y / scale));
                            if (ix >= 0 && iy >= 0 && ix < Image.Width && iy < Image.Height)
                                Image.SetPixel(ix, iy, IsLeft ? usedColor.BackColor : Color.White);
                        }
                    }
                    int x = (int)(X + ms.X / scale), y = (int)(Y + ms.Y / scale);
                    if (x >= 0 && y >= 0 && x < 16 && y < 16)
                        Image.SetPixel(x, y, IsLeft ? usedColor.BackColor : Color.White);
                    invalidate = true;
                }
                else if (IsMiddle)
                {
                    int x = (int)(ms.X / scale), y = (int)(ms.Y / scale);
                    if (x >= 0 && y >= 0 && x < 16 && y < 16)
                        usedColor.BackColor = Image.GetPixel(x, y);
                    using (System.Drawing.Graphics _g = System.Drawing.Graphics.FromImage(usedColor.Image))
                        _g.Clear(usedColor.BackColor);
                }
            }
        }

        Bitmap renderimage;
        float X, Y, oldX, oldY, oldScale;
        bool invalidate = false;
        private void DrawRender(object sender, EventArgs e)
        {
            if (KB.IsKeyDown(KB.Key.A) || KB.IsKeyDown(KB.Key.E))
            {
                if (KB.IsKeyDown(KB.Key.A))
                    scale -= 1F;
                if (KB.IsKeyDown(KB.Key.E))
                    scale += 1F;
                scale = Math.Max(0.1F, Math.Min(scale, 20F));
            }
            if (KB.IsKeyDown(KB.Key.Z)) Y -= scale / 2;
            if (KB.IsKeyDown(KB.Key.S)) Y += scale / 2;
            if (KB.IsKeyDown(KB.Key.Q)) X -= scale / 2;
            if (KB.IsKeyDown(KB.Key.D)) X += scale / 2;
            KB.Update();

            if (invalidate || oldScale != scale || oldX != X || oldY != Y)
            {
                invalidate = false;
                oldScale = scale;
                oldX = X;
                oldY = Y;

                X = Math.Max(0, Math.Min(X, Image.Width - 1));
                Y = Math.Max(0, Math.Min(Y, Image.Height - 1));
                int sx = (int)Math.Floor(X);
                int sy = (int)Math.Floor(Y);
                int srcW = Math.Max(1, Math.Min(Image.Width - sx, (int)Math.Ceiling(Render.Width / scale)));
                int srcH = Math.Max(1, Math.Min(Image.Height - sy, (int)Math.Ceiling(Render.Height / scale)));
                var img = Image.Clone(new Rectangle(sx, sy, srcW, srcH), Image.PixelFormat);
                renderimage = img.ResizeExact((int)Math.Max(1, Math.Round(srcW * scale)),
                                               (int)Math.Max(1, Math.Round(srcH * scale)));
            }
            Render.Image = renderimage;
        }

        private void color_MouseClick(object sender, MouseEventArgs e)
        {
            var colorsbtn = new List<PictureBox>() { color1, color2, color3, color4, color5, color6, color7, color8 };
            var c = colorsbtn.First(c => sender == c);

            if (e.Button == MouseButtons.Left)
            {
                usedColor.BackColor = c.BackColor;
                using (System.Drawing.Graphics _g = System.Drawing.Graphics.FromImage(usedColor.Image)) _g.Clear(usedColor.BackColor);
            }
            else if (e.Button == MouseButtons.Middle)
            {
                c.BackColor = Color.White;
                usedColor.BackColor = c.BackColor;
                using (System.Drawing.Graphics _g = System.Drawing.Graphics.FromImage(c.Image)) _g.Clear(c.BackColor);
                using (System.Drawing.Graphics _g = System.Drawing.Graphics.FromImage(usedColor.Image)) _g.Clear(usedColor.BackColor);
            }
            else
            {
                colorDialog1.Color = c.BackColor;
                if (colorDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        c.BackColor = colorDialog1.Color;
                        using (System.Drawing.Graphics _g = System.Drawing.Graphics.FromImage(c.Image)) _g.Clear(c.BackColor);
                        usedColor.BackColor = c.BackColor;
                    }
                }
            }
        }

        private void btLoad_Click(object sender, EventArgs e)
        {
            if(tv.SelectedNode != null)
                {
                var path = tv.SelectedNode.Tag.ToString();
                if (File.Exists(path))
                {
                    var img = System.Drawing.Image.FromFile(path);
                    Image = new Bitmap(img);
                    g = System.Drawing.Graphics.FromImage(Image);
                    DrawRender(null, null);
                    invalidate = true;
                }
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {

        }

        private void btNew_Click(object sender, EventArgs e)
        {

        }
    }
}
