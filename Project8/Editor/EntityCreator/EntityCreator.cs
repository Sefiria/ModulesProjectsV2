using Project8.Source;
using Project8.Source.Entities.Behaviors;
using Project8.Source.JsonHelpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static Project8.Source.Entities.Entity;
using G = System.Drawing.Graphics;
using Tooling;

namespace Project8.Editor.EntityCreator
{
    public partial class EntityCreator : Form
    {
        public static int sz = GlobalVariables.tilesize;
        PaletteRoll[] PaletteRolls = new PaletteRoll[4];
        Bitmap[] Frames;
        int CurrentFrame = 0;
        DrawBox DrawFrame;
        Dictionary<string, Entity> Metadata = new Dictionary<string, Entity>();
        string Target = null, PreviousTarget = null;

        public EntityCreator()
        {
            InitializeComponent();
        }

        private void EntityCreator_Load(object sender, EventArgs e)
        {
            LoadGraphics();
            LoadEntityDefaultConfiguration();
            LoadFilename();
        }
        public static void RemoveAnimationByName(Entity entity, string animationName)
        {
            var key = entity.Animations.FirstOrDefault(a => a.Value == animationName).Key;
            if (!entity.Animations.ContainsKey(key))
                return;
            entity.Animations.Remove(key);
        }
        private void LoadGraphics()
        {
            ReloadDataGridViewAnimations();
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
                PaletteRolls[i] = new PaletteRoll(ref usedColor) { Location = new Point(gX + 133, gY + 325 + 21 * i) };
                groupGraphics.Controls.Add(PaletteRolls[i]);
            }
            PaletteRolls[0].SetMainColor(Color.FromArgb(167, 123, 91));
            PaletteRolls[1].SetMainColor(Color.FromArgb(138, 176, 96));
            PaletteRolls[2].SetMainColor(Color.FromArgb(106, 83, 110));
            PaletteRolls[3].SetMainColor(Color.FromArgb(128, 128, 128));

            Frames = [new Bitmap(sz, sz), new Bitmap(sz, sz), new Bitmap(sz, sz), new Bitmap(sz, sz)];
            DrawFrame = new DrawBox(ref Frames[CurrentFrame], ref usedColor, ref colorBuffer)
            {
                Location = new Point(gX + 106, gY + 473),
                Size = new Size(160, 160)
            };
            DrawFrame.RenderUpdated += (s, e) => Draw_FramesRender();
            groupGraphics.Controls.Add(DrawFrame);
            CurrentFrame = 0;
            BeginInvoke(new Action(() => { DrawFrame.UpdateImage(ref Frames[CurrentFrame]); DrawFrame.Invalidate(); }));
        }
        private void LoadFilename()
        {
            Metadata = EntityLoader.Load(GlobalPaths.DataEntitiesJson);
            tbMetadataPath.Text = GlobalPaths.DataEntitiesJson;
            ReloadEntitiesInList();
        }
        private void ReloadEntitiesInList()
        {
            cbbEntities.Items.Clear();
            cbbEntities.Items.AddRange(Metadata.Keys.ToArray());
            cbbEntities.SelectedIndex = Target != null && Metadata.ContainsKey(Target) ? Metadata.Keys.ToList().IndexOf(Target) : (cbbEntities.Items.Count == 0 ? -1 : 0);
        }
        private void LoadEntityDefaultConfiguration()
        {
            cbbEntityAlignment.Items.AddRange(Enum.GetNames<Alignments>());
            cbbEntityAlignment.SelectedItem = Enum.GetName(Alignments.bottom);
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
            if (FramesRender.InvokeRequired)
            {
                FramesRender.Invoke(new Action(Draw_FramesRender));
                return;
            }
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
            FramesRender.Refresh();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            PaletteRolls.ToList().ForEach(pr => pr.ChangeGap((int)numGap.Value));
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            UIToMetadata(Target);
            EntityLoader.Save(GlobalPaths.DataEntitiesJson, Metadata);
        }

        private void MetadataToUI()
        {
            if (Target == null || !Metadata.ContainsKey(Target))
                return;
            cbbEntityAlignment.SelectedText = Enum.GetName(Metadata[Target].Alignment);
            numEntityAnimationSpeed.Value = (int)Metadata[Target].AnimationSpeed;
            cbEntityCanCollect.Checked = Metadata[Target].CanCollect;
            ReloadDataGridViewAnimations();
        }
        private void UIToMetadata(string target)
        {
            if (Target == null || !Metadata.ContainsKey(Target))
                return;
            Metadata[target].Alignment = Enum.Parse<Alignments>(cbbEntityAlignment.SelectedText);
            Metadata[target].AnimationSpeed = (float)numEntityAnimationSpeed.Value;
            Metadata[target].CanCollect = cbEntityCanCollect.Checked;
            var anims = new Dictionary<Behavior.AnimationsNeeds, string>();
            foreach (DataGridViewRow anim in dgvAnims.Rows)
                anims[Enum.Parse<Behavior.AnimationsNeeds>(anim.Cells["cType"].Value.ToString())] = anim.Cells["cName"].Value.ToString();
            Metadata[target].Animations = anims;
        }
        private void ReloadDataGridViewAnimations()
        {
            if (dgvAnims.Columns.Count == 0)
            {
                var column = new DataGridViewComboBoxColumn();
                column.Items.AddRange(Enum.GetNames<Behavior.AnimationsNeeds>());
                column.Name = "cType";
                column.HeaderText = "Type";
                dgvAnims.Columns[dgvAnims.Columns.Add(column)].ReadOnly = true;
                dgvAnims.Columns[dgvAnims.Columns.Add("cName", "Name")].ReadOnly = false;
            }

            if (Target == null || !Metadata.ContainsKey(Target))
                return;

            dgvAnims.Rows.Clear();
            foreach (var data_anim in Metadata[Target].Animations)
                dgvAnims.Rows.Add(Enum.GetName(data_anim.Key), data_anim.Value);

            if (dgvAnims.Rows.Count > 0)
                LoadAnimation(0);
        }

        private void btEntityValidateRenaming_Click(object sender, EventArgs _)
        {
            if (Target.CompareTo(tbEntityName.Text) != 0)
            {
                if (string.IsNullOrWhiteSpace(tbEntityName.Text))
                {
                    MessageBox.Show(this, "Cannot update : the entity name is missing in the configuration.", "Wrong Entity Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (Metadata.ContainsKey(tbEntityName.Text))
                {
                    MessageBox.Show(this, "Cannot update : the entity name already exists.", "Wrong Entity Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                UIToMetadata(Target);
                var e = Metadata[Target].Clone();
                Metadata.Remove(Target);
                Target = tbEntityName.Text;
                Metadata[Target] = e;
                ReloadEntitiesInList();
            }
        }
        private void cbbEntities_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PreviousTarget != null)
                UIToMetadata(PreviousTarget);
            PreviousTarget = Target;
            Target = cbbEntities.SelectedItem.ToString();
            MetadataToUI();
        }

        private void LoadAnimation(int rowIndex)
        {
            string animation_filename = Path.Combine(GlobalPaths.Animations, dgvAnims["cName", rowIndex].Value.ToString()) + ".png";
            Bitmap img = null;
            if (!File.Exists(animation_filename))
                (img = new Bitmap(sz * 4, sz)).Save(animation_filename);
            else
                img = (Bitmap)Image.FromFile(animation_filename);
            Frames = img.Split(x: sz, y: sz).ToArray();
            CurrentFrame = 0;
            Draw_FramesRender();
        }
        private void NewAnimation()
        {
            dgvAnims.Rows.Add("Idle", "unnamed");
        }
        private void DeleteAnimation(int rowIndex)
        {
            dgvAnims.Rows.RemoveAt(rowIndex);
        }
        private void dgvAnims_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                LoadAnimation(e.RowIndex);
            }
            else if (e.Button == MouseButtons.Right)
            {
                ToolStripDropDownMenu menu = new ToolStripDropDownMenu();
                menu.Items[menu.Items.Add(new ToolStripButton("New...") { Name = "New" })].Click += (s, _e) => NewAnimation();
                menu.Items.Add(new ToolStripSeparator());
                menu.Items[menu.Items.Add(new ToolStripButton("Delete", SystemIcons.Warning.ToBitmap()) { Name = "Delete" })].Click += (s, _e) => DeleteAnimation(e.RowIndex);
            }
        }

        private void btNewEntity_Click(object sender, EventArgs e)
        {
            string new_entity_name = "NewEntity";
            while (Metadata.ContainsKey(new_entity_name))
                new_entity_name += "_new";
            Metadata[new_entity_name] = Entity.New();
            cbbEntities.Items.Add(new_entity_name);
            cbbEntities.SelectedText = new_entity_name;
        }
        private void btRemoveEntity_Click(object sender, EventArgs e)
        {
            if (cbbEntities.SelectedIndex == -1)
                return;
            if (MessageBox.Show(this, $"Are you sure about removing the entity '{Target}' ?", "Remove Entity", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                Metadata.Remove(Target);
                cbbEntities.Items.Remove(Target);
                if (cbbEntities.Items.Count == 0)
                    btNewEntity_Click(null, null);
                else
                    cbbEntities.SelectedIndex = 0;
            }
        }
    }
}
