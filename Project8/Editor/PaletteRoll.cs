using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Color = System.Drawing.Color;
using G = System.Drawing.Graphics;

namespace Project8.Editor
{
    public partial class PaletteRoll : UserControl
    {
        PictureBox usedColor;
        public int gap = 40;
        public PaletteRoll(ref PictureBox usedColor)
        {
            InitializeComponent();
            this.usedColor = usedColor;
            var colorsbtn = Controls.OfType<PictureBox>().ToList();
            foreach (var c in colorsbtn)
            {
                c.Image = new Bitmap(c.Width, c.Height);
                using (G g = G.FromImage(c.Image))
                {
                    g.Clear(Color.White);
                    g.DrawRectangle(Pens.Black, 0, 0, c.Image.Width, c.Image.Height);
                }
            }
        }

        public void ChangeGap(int gap)
        {
            this.gap = gap;
            set_darkers_and_brighters_colors();
        }
        public void SetMainColor(Color color)
        {
            colorDialog1.Color = color;
            c0.BackColor = colorDialog1.Color;
            using (G _g = G.FromImage(c0.Image))
            {
                _g.Clear(c0.BackColor);
                _g.DrawRectangle(Pens.Black, 0, 0, c0.Image.Width, c0.Image.Height);
                set_darkers_and_brighters_colors();
            }
            using (G _g = G.FromImage(usedColor.Image))
            {
                _g.Clear(usedColor.BackColor);
                _g.DrawRectangle(Pens.Black, 0, 0, usedColor.Image.Width, usedColor.Image.Height);
            }
        }
        public void color_MouseClick(object sender, MouseEventArgs e)
        {
            var c = sender as PictureBox;

            if (e.Button == MouseButtons.Left)
            {
                usedColor.BackColor = c.BackColor;
                using (G _g = G.FromImage(usedColor.Image))
                {
                    _g.Clear(usedColor.BackColor);
                    _g.DrawRectangle(Pens.Black, 0, 0, usedColor.Image.Width, usedColor.Image.Height);
                }
            }
            else if(c == c0)
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
                            _g.DrawRectangle(Pens.Black, 0, 0, c.Image.Width, c.Image.Height);
                            set_darkers_and_brighters_colors();
                        }
                        using (G _g = G.FromImage(usedColor.Image))
                        {
                            _g.Clear(usedColor.BackColor);
                            _g.DrawRectangle(Pens.Black, 0, 0, usedColor.Image.Width, usedColor.Image.Height);
                        }
                    }
                }
            }
            usedColor.Invalidate();
        }
        void set_darkers_and_brighters_colors()
        {
            cd1.BackColor = c0.BackColor;
            cb1.BackColor = c0.BackColor;
            atomic_set(cd1, -1);
            atomic_set(cb1, 1);
        }
        void atomic_set(PictureBox target, int direction)
        {
            Color color;
            using (G _g = G.FromImage(target.Image))
            {
                color = Color.FromArgb(
                    (byte)Math.Min(Math.Max(0, target.BackColor.R + (direction == -1 ? -gap : gap)), 255),
                    (byte)Math.Min(Math.Max(0, target.BackColor.G + (direction == -1 ? -gap : gap)), 255),
                    (byte)Math.Min(Math.Max(0, target.BackColor.B + (direction == -1 ? -gap : gap)), 255)
                );
                target.BackColor = color;
                _g.Clear(color);
                _g.DrawRectangle(Pens.Black, 0, 0, target.Image.Width, target.Image.Height);
            }
            PictureBox new_target;
            if(target == cd1) new_target = cd2;
            else if(target == cd2) new_target = cd3;
            else if (target == cd3) new_target = cd4;
            else if (target == cb1) new_target = cb2;
            else if (target == cb2) new_target = cb3;
            else if (target == cb3) new_target = cb4;
            else return;
            new_target.BackColor = color;
            atomic_set(new_target, direction);
        }
    }
}
