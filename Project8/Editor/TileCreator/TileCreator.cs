using Project8.Source.TiledMap;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tooling;

namespace Project8.Editor.TileCreator
{
    public partial class TileCreator : Form
    {
        bool IsMouseDown = false;
        Point ms_old, ms;
        Bitmap Image;
        Timer timerDraw = new Timer() { Enabled = true, Interval = 10 };
        System.Drawing.Graphics g;
        float scale = 20F;

        public TileCreator()
        {
            InitializeComponent();
        }
        private void TileCreator_Load(object sender, EventArgs e)
        {
            cbbMode.Items.Clear();
            cbbMode.Items.AddRange(Enum.GetNames<Tile.Modes>());
            cbbMode.SelectedIndex = 0;

            int id = 0;
            var ids = Tile.Tiles.Values.Select(x => x.id).OrderBy(x => x).ToList();
            while (ids.Contains(id)) id++;
            numID.Value = id;

            tbCharacteristics.Text = "s";

            label6.Visible = numMultiTileID.Visible = cbbMode.SelectedText == Enum.GetName(Tile.Modes.MultiTile);
            cbbMode.SelectedIndexChanged += (s, e) => label6.Visible = numMultiTileID.Visible = cbbMode.SelectedIndex == Enum.GetNames<Tile.Modes>().ToList().IndexOf(Enum.GetName(Tile.Modes.MultiTile));

            Image = new Bitmap(16, 16);
            g = System.Drawing.Graphics.FromImage(Image);
            g.Clear(Color.White);
            timerDraw.Tick += DrawRender;
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
                    //double d = Maths.Distance(ms_old, ms);
                    //for (double t = 0.0; t <= 1.0; t += 1 / d)
                    //{
                    //    PointF p = Maths.Lerp(ms_old, ms, t);
                    //    if (p.X >= 0 && p.Y >= 0 && p.X < Render.Width && p.Y < Render.Height)
                    //        Image.SetPixel((int)(p.X / scale), (int)(p.Y / scale), IsLeft ? Color.Black : Color.White);
                    //}
                    g.DrawLine(IsLeft ? Pens.Black : Pens.White, (ms_old.vecf() / scale).pt, (ms.vecf() / scale).pt);
                    int x = (int)(ms.X / scale), y = (int)(ms.Y / scale);
                    if (x >= 0 && y >= 0 && x < 16 && y < 16)
                        Image.SetPixel(x, y, IsLeft ? Color.Black : Color.White);
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

        private void DrawRender(object sender, EventArgs e)
        {
            //g.Clear(Color.White);
            //var img = Image.ResizeExact((int)(16 * scale), (int)(16 * scale));
            //g.DrawImage(img, 0, 0);
            Render.Image = Image.ResizeExact((int)(16 * scale), (int)(16 * scale));
        }

        private void color8_MouseClick(object sender, MouseEventArgs e)
        {
            if (sender == color1) colorDialog1.Color = color1.BackColor;
            if (sender == color2) colorDialog1.Color = color2.BackColor;
            if (sender == color3) colorDialog1.Color = color3.BackColor;
            if (sender == color4) colorDialog1.Color = color4.BackColor;
            if (sender == color5) colorDialog1.Color = color5.BackColor;
            if (sender == color6) colorDialog1.Color = color6.BackColor;
            if (sender == color7) colorDialog1.Color = color7.BackColor;
            if (sender == color8) colorDialog1.Color = color8.BackColor;
            if (colorDialog1.ShowDialog(this) == DialogResult.OK)
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (sender == color1) { color1.BackColor = colorDialog1.Color; using (System.Drawing.Graphics _g = System.Drawing.Graphics.FromImage(color1.Image)) _g.Clear(color1.BackColor); }
                    if (sender == color2) { color2.BackColor = colorDialog1.Color; using (System.Drawing.Graphics _g = System.Drawing.Graphics.FromImage(color2.Image)) _g.Clear(color2.BackColor); }
                    if (sender == color3) { color3.BackColor = colorDialog1.Color; using (System.Drawing.Graphics _g = System.Drawing.Graphics.FromImage(color3.Image)) _g.Clear(color3.BackColor); }
                    if (sender == color4) { color3.BackColor = colorDialog1.Color; using (System.Drawing.Graphics _g = System.Drawing.Graphics.FromImage(color4.Image)) _g.Clear(color4.BackColor); }
                    if (sender == color5) { color3.BackColor = colorDialog1.Color; using (System.Drawing.Graphics _g = System.Drawing.Graphics.FromImage(color5.Image)) _g.Clear(color5.BackColor); }
                    if (sender == color6) { color3.BackColor = colorDialog1.Color; using (System.Drawing.Graphics _g = System.Drawing.Graphics.FromImage(color6.Image)) _g.Clear(color6.BackColor); }
                    if (sender == color7) { color3.BackColor = colorDialog1.Color; using (System.Drawing.Graphics _g = System.Drawing.Graphics.FromImage(color7.Image)) _g.Clear(color7.BackColor); }
                    if (sender == color8) { color3.BackColor = colorDialog1.Color; using (System.Drawing.Graphics _g = System.Drawing.Graphics.FromImage(color8.Image)) _g.Clear(color8.BackColor); }
                }
                else if (e.Button == MouseButtons.Left)
                {
                    if (sender == color1) usedColor.BackColor = color1.BackColor;
                    if (sender == color2) usedColor.BackColor = color2.BackColor;
                    if (sender == color3) usedColor.BackColor = color3.BackColor;
                    if (sender == color4) usedColor.BackColor = color4.BackColor;
                    if (sender == color5) usedColor.BackColor = color5.BackColor;
                    if (sender == color6) usedColor.BackColor = color6.BackColor;
                    if (sender == color7) usedColor.BackColor = color7.BackColor;
                    if (sender == color8) usedColor.BackColor = color8.BackColor;
                    using (System.Drawing.Graphics _g = System.Drawing.Graphics.FromImage(usedColor.Image)) _g.Clear(usedColor.BackColor);
                }
                else if (e.Button == MouseButtons.Middle)
                {
                    if (sender == color1) { color1.BackColor = usedColor.BackColor; using (System.Drawing.Graphics _g = System.Drawing.Graphics.FromImage(color1.Image)) _g.Clear(color1.BackColor); }
                    if (sender == color2) { color2.BackColor = usedColor.BackColor; using (System.Drawing.Graphics _g = System.Drawing.Graphics.FromImage(color2.Image)) _g.Clear(color2.BackColor); }
                    if (sender == color3) { color3.BackColor = usedColor.BackColor; using (System.Drawing.Graphics _g = System.Drawing.Graphics.FromImage(color3.Image)) _g.Clear(color3.BackColor); }
                    if (sender == color4) { color4.BackColor = usedColor.BackColor; using (System.Drawing.Graphics _g = System.Drawing.Graphics.FromImage(color4.Image)) _g.Clear(color4.BackColor); }
                    if (sender == color5) { color5.BackColor = usedColor.BackColor; using (System.Drawing.Graphics _g = System.Drawing.Graphics.FromImage(color5.Image)) _g.Clear(color5.BackColor); }
                    if (sender == color6) { color6.BackColor = usedColor.BackColor; using (System.Drawing.Graphics _g = System.Drawing.Graphics.FromImage(color6.Image)) _g.Clear(color6.BackColor); }
                    if (sender == color7) { color7.BackColor = usedColor.BackColor; using (System.Drawing.Graphics _g = System.Drawing.Graphics.FromImage(color7.Image)) _g.Clear(color7.BackColor); }
                    if (sender == color8) { color8.BackColor = usedColor.BackColor; using (System.Drawing.Graphics _g = System.Drawing.Graphics.FromImage(color8.Image)) _g.Clear(color8.BackColor); }
                    using (System.Drawing.Graphics _g = System.Drawing.Graphics.FromImage(usedColor.Image)) _g.Clear(usedColor.BackColor);
                }
            }
        }
    }
}
