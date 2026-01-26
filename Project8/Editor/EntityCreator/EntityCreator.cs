using NAudio.MediaFoundation;
using Project8.Source;
using Project8.Source.JsonHelpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static Project8.Source.Entities.Entity;
using G = System.Drawing.Graphics;

namespace Project8.Editor.EntityCreator
{
    public partial class EntityCreator : Form
    {
        public static int sz = GlobalVariables.tilesize;
        PaletteRoll[] PaletteRolls = new PaletteRoll[4];
        Bitmap[] Frames;
        int CurrentFrame = 0;
        DrawBox DrawFrame;
        static string PathAnimations = Directory.GetCurrentDirectory() + "/Assets/Textures/Animations/";
        Dictionary<string, Entity> Metadata = new Dictionary<string, Entity>();
        string Target = null;

        public EntityCreator()
        {
            InitializeComponent();
        }

        private void EntityCreator_Load(object sender, EventArgs e)
        {
            LoadGraphics();
            LoadFilename();
            LoadEntityConfiguration();
        }
        private void NewAnimation()
        {
        }
        private void RenameAnimation()
        {
        }
        private void DeleteAnimation()
        {
            int id = listAnimations.SelectedIndex;
            if (id != -1)
            {
                RemoveAnimationByName(Metadata[Target], listAnimations.Items[id].ToString());
                listAnimations.Items.RemoveAt(id);
            }
        }
        public static void RemoveAnimationByName(Entity entity, string animationName)
        {
            var key = entity.Animations .FirstOrDefault(a => a.Value == animationName) .Key;
            if (!entity.Animations.ContainsKey(key))
                return;
            entity.Animations.Remove(key);
        }
        private void LoadGraphics()
        {
            ToolStripDropDownMenu menu = new ToolStripDropDownMenu();
            menu.Items[menu.Items.Add(new ToolStripButton("New...") { Name = "New" })].Click += (s, _e) => NewAnimation();
            menu.Items[menu.Items.Add(new ToolStripButton("Rename...") { Name = "Rename" })].Click += (s, _e) => RenameAnimation();
            menu.Items.Add(new ToolStripSeparator());
            menu.Items[menu.Items.Add(new ToolStripButton("Delete", SystemIcons.Warning.ToBitmap()) { Name = "Delete" })].Click += (s, _e) => DeleteAnimation();
            listAnimations.MouseDown += (s, _e) => { if (_e.Button != MouseButtons.Right) return; menu.Items["Rename"].Enabled = menu.Items["Delete"].Enabled = listAnimations.SelectedIndex != -1; menu.Show(listAnimations, _e.Location); };
            listAnimations.Items.Add("test");
            var colorsbtn = new List<PictureBox>() { usedColor, color1, color2, color3, color4, color5, color6, color7, color8, colorBuffer };
            foreach (var c in colorsbtn)
            {
                c.Image = new Bitmap(c == usedColor ? 64 : 32, c == usedColor ? 64 : 32);
                using (G g = G.FromImage(c.Image))
                {
                    g.Clear(Color.White);
                    g.DrawRectangle(Pens.Black, 0, 0, c.Image.Width, c.Image.Height);
                }
            }

            int gX = groupGraphics.Location.X;
            int gY = groupGraphics.Location.Y;
            for (int i = 0; i < PaletteRolls.Length; i++)
            {
                PaletteRolls[i] = new PaletteRoll(ref usedColor) { Location = new Point(gX + 37, gY + 212 + 21 * i) };
                groupGraphics.Controls.Add(PaletteRolls[i]);
            }
            PaletteRolls[0].SetMainColor(Color.FromArgb(167, 123, 91));
            PaletteRolls[1].SetMainColor(Color.FromArgb(138, 176, 96));
            PaletteRolls[2].SetMainColor(Color.FromArgb(106, 83, 110));
            PaletteRolls[3].SetMainColor(Color.FromArgb(128, 128, 128));

            Frames = [new Bitmap(sz, sz), new Bitmap(sz, sz), new Bitmap(sz, sz), new Bitmap(sz, sz)];
            DrawFrame = new DrawBox(ref Frames[CurrentFrame], ref usedColor, ref colorBuffer)
            {
                Location = new Point(gX + 20, gY + 361),
                Size = new Size(160, 160)
            };
            DrawFrame.RenderUpdated += (s, e) => Draw_FramesRender();
            groupGraphics.Controls.Add(DrawFrame);

            FramesRender.Image = new Bitmap(FramesRender.Width, FramesRender.Height);
            Draw_FramesRender();
        }
        private void LoadFilename()
        {
            Metadata = EntityLoader.Load(GlobalPaths.DataEntitiesJson);
        }
        private void LoadEntityConfiguration()
        {
            cbbAlignment.Items.AddRange(Enum.GetNames<Alignments>());
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
                    _g.DrawRectangle(Pens.Black, 0, 0, usedColor.Image.Width, usedColor.Image.Height);
                }
            }
            else if (e.Button == MouseButtons.Middle)
            {
                c.BackColor = Color.White;
                usedColor.BackColor = c.BackColor;
                using (G _g = G.FromImage(c.Image))
                {
                    _g.Clear(c.BackColor);
                    _g.DrawRectangle(Pens.Black, 0, 0, c.Image.Width, c.Image.Height);
                }
                using (G _g = G.FromImage(usedColor.Image))
                {
                    _g.Clear(usedColor.BackColor);
                    _g.DrawRectangle(Pens.Black, 0, 0, usedColor.Image.Width, usedColor.Image.Height);
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
                            _g.DrawRectangle(Pens.Black, 0, 0, c.Image.Width, c.Image.Height);
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
                    }
                }
                Draw_FramesRender();
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

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            PaletteRolls.ToList().ForEach(pr => pr.ChangeGap((int)numGap.Value));
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            EntityLoader.Save(GlobalPaths.DataEntitiesJson, Metadata);
        }
    }
}
