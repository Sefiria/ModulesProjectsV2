using Project8.Source;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using G = System.Drawing.Graphics;

namespace Project8.Editor.EntityCreator
{
    public partial class EntityCreator : Form
    {
        public static int sz = GlobalVariables.tilesize;
        Bitmap[] Frames;
        int CurrentFrame = 0;
        DrawBox DrawFrame;

        public EntityCreator()
        {
            InitializeComponent();
        }

        private void EntityCreator_Load(object sender, EventArgs e)
        {
            var colorsbtn = new List<PictureBox>() { usedColor, color1, color2, color3, color4, color5, color6, color7, color8, colorBuffer };
            foreach (var c in colorsbtn)
            {
                c.Image = new Bitmap(c == usedColor ? 64 : 32, c == usedColor ? 64 : 32);
                using (G g = G.FromImage(c.Image))
                {
                    g.Clear(Color.White);
                    g.DrawRectangle(Pens.Black, 0, 0, c.Image.Width - 1, c.Image.Height - 1);
                }
            }

            Frames = [new Bitmap(sz, sz), new Bitmap(sz, sz), new Bitmap(sz, sz), new Bitmap(sz, sz)];
            DrawFrame = new DrawBox(ref Frames[CurrentFrame], ref usedColor, ref colorBuffer)
            {
                Location = new Point(37, 144),
                Size = new Size(160, 160)
            };
            Controls.Add(DrawFrame);

            FramesRender.Image = new Bitmap(FramesRender.Width, FramesRender.Height);
            Draw_FramesRender();
        }
        private void color_MouseClick(object sender, MouseEventArgs e)
        {
            var colorsbtn = new List<PictureBox>() { color1, color2, color3, color4, color5, color6, color7, color8, colorBuffer };
            var c = colorsbtn.First(c => sender == c);

            if (e.Button == MouseButtons.Left)
            {
                usedColor.BackColor = c.BackColor;
                using (G _g = G.FromImage(usedColor.Image))
                {
                    _g.Clear(usedColor.BackColor);
                    _g.DrawRectangle(Pens.Black, 0, 0, usedColor.Image.Width - 1, usedColor.Image.Height - 1);
                }
            }
            else if (e.Button == MouseButtons.Middle)
            {
                c.BackColor = Color.White;
                usedColor.BackColor = c.BackColor;
                using (G _g = G.FromImage(c.Image))
                {
                    _g.Clear(c.BackColor);
                    _g.DrawRectangle(Pens.Black, 0, 0, c.Image.Width - 1, c.Image.Height - 1);
                }
                using (G _g = G.FromImage(usedColor.Image))
                {
                    _g.Clear(usedColor.BackColor);
                    _g.DrawRectangle(Pens.Black, 0, 0, usedColor.Image.Width - 1, usedColor.Image.Height - 1);
                }
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
                        using (G _g = G.FromImage(c.Image))
                        {
                            _g.Clear(c.BackColor);
                            _g.DrawRectangle(Pens.Black, 0, 0, c.Image.Width - 1, c.Image.Height - 1);
                        }
                        using (G _g = G.FromImage(usedColor.Image))
                        {
                            _g.Clear(usedColor.BackColor);
                            _g.DrawRectangle(Pens.Black, 0, 0, usedColor.Image.Width - 1, usedColor.Image.Height - 1);
                        }
                    }
                }
            }
            usedColor.Invalidate();
        }
        private void FramesRender_MouseClick(object sender, MouseEventArgs e)
        {
            int cx = FramesRender.Width / 2;
            int cy = FramesRender.Height / 2;
            int fsz = FramesRender.Height / 2;
            int y = cy - fsz / 2;
            int gap = 5;
            for (int x = 0; x < Frames.Length; x++)
            {
                if (x != CurrentFrame)
                {
                    Rectangle rect = new Rectangle(cx - (Frames.Length / 2) * (fsz + gap * 2) + x * (fsz + gap * 2), y, fsz, fsz);
                    if (rect.Contains(e.Location))
                    {
                        CurrentFrame = x;
                        DrawFrame.UpdateImage(ref Frames[CurrentFrame]);
                        Draw_FramesRender();
                    }
                }
            }
        }
        private void Draw_FramesRender()
        {
            int cx = FramesRender.Width / 2;
            int cy = FramesRender.Height / 2;
            int fsz = FramesRender.Height / 2;
            int y = cy - fsz / 2;
            int gap = 5;
            FramesRender.Image = new Bitmap(FramesRender.Width, FramesRender.Height);
            using (G g = G.FromImage(FramesRender.Image))
            {
                for (int x = 0; x < Frames.Length; x++)
                {
                    if (x == CurrentFrame)
                    {
                        Rectangle rect = new Rectangle(cx - (Frames.Length / 2) * (fsz + gap * 2) + x * (fsz + gap * 2) - gap, y - gap, fsz + gap * 2, fsz + gap * 2);
                        g.DrawImage(Frames[x], rect);
                        g.DrawRectangle(Pens.Black, rect);
                    }
                    else
                    {
                        Rectangle rect = new Rectangle(cx - (Frames.Length / 2) * (fsz + gap * 2) + x * (fsz + gap * 2), y, fsz, fsz);
                        g.DrawImage(Frames[x], rect);
                        g.DrawRectangle(Pens.Black, rect);
                    }
                }
            }
        }
    }
}
