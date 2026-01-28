using Project8.Source;
using Project8.Source.Entities.Behaviors;
using Project8.Source.JsonHelpers;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Tooling;
using static Project8.Source.Entities.Entity;
using G = System.Drawing.Graphics;

namespace Project8.Editor.EntityCreator
{
    public partial class EntityCreator : Form
    {
        public static int sz = GlobalVariables.tilesize;
        PaletteRoll[] PaletteRolls = new PaletteRoll[4];
        Bitmap[] Frames;
        int CurrentFrame = 0, dgvAnimsLastRowIndex = -1;
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

            FillListExistingBehaviors();
            lbExistingBehaviors.SelectedIndex = 0;
        }
        private void FillListExistingBehaviors()
        {
            lbExistingBehaviors.Items.Clear();
            lbExistingBehaviors.Items.AddRange(
                AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass
                            && !t.IsAbstract
                            && t.IsSubclassOf(typeof(Behavior)))
                .Select(t => t.Name)
                .ToArray()
            );
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
            tbEntityName.Text = Target;
            FillListExistingBehaviors();
            lbEntityBehaviors.Items.Clear();
            foreach (string behavior in Metadata[Target].Behaviors)
            {
                lbEntityBehaviors.Items.Add(behavior);
                lbExistingBehaviors.Items.Remove(behavior);
            }
            cbbEntityAlignment.SelectedText = Enum.GetName(Metadata[Target].Alignment);
            numEntityAnimationSpeed.Value = (int)Metadata[Target].AnimationSpeed;
            cbEntityCanCollect.Checked = Metadata[Target].CanCollect;
            ReloadDataGridViewAnimations();
        }
        private void UIToMetadata(string target)
        {
            if (Target == null || !Metadata.ContainsKey(Target))
                return;
            Metadata[target].Behaviors = lbEntityBehaviors.Items.Cast<string>().ToArray();
            Metadata[target].Alignment = Enum.Parse<Alignments>(cbbEntityAlignment.SelectedItem.ToString());
            Metadata[target].AnimationSpeed = (float)numEntityAnimationSpeed.Value;
            Metadata[target].CanCollect = cbEntityCanCollect.Checked;
            if (dgvAnimsLastRowIndex > -1 && dgvAnimsLastRowIndex < dgvAnims.RowCount)
                UIToMetadata_Anim(dgvAnims.Rows[dgvAnimsLastRowIndex], target);
        }
        private void UIToMetadata_Anim(DataGridViewRow anim, string target = null)
        {
            if (anim.Cells["cType"].Value == null || anim.Cells["cName"].Value == null)
                return;

            var need = Enum.Parse<Behavior.AnimationsNeeds>(anim.Cells["cType"].Value.ToString());
            var name = anim.Cells["cName"].Value.ToString();
            Metadata[target ?? Target].Animations[need] = name;

            var final = new Bitmap(Frames.Length * sz, sz);
            using (var g = G.FromImage(final))
                for (int i = 0; i < Frames.Length; i++)
                    g.DrawImage(Frames[i], new Point(i * sz, 0));
            Metadata[target ?? Target].AnimationsTextures[name] = final;
        }
        private void ReloadDataGridViewAnimations()
        {
            if (dgvAnims.Columns.Count == 0)
            {
                var column = new DataGridViewComboBoxColumn();
                column.Items.AddRange(Enum.GetNames<Behavior.AnimationsNeeds>());
                column.Name = "cType";
                column.HeaderText = "Type";
                dgvAnims.Columns[dgvAnims.Columns.Add(column)].ReadOnly = false;
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
            PreviousTarget = Target;
            Target = cbbEntities.SelectedItem.ToString();
            if (PreviousTarget != null && Metadata.ContainsKey(PreviousTarget))
                UIToMetadata(PreviousTarget);
            MetadataToUI();
        }

        private void LoadAnimation(int rowIndex)
        {
            if (rowIndex == -1 || dgvAnims["cName", rowIndex].Value == null)
                return;
            string name = dgvAnims["cName", rowIndex].Value.ToString();
            if (!Metadata[Target].AnimationsTextures.ContainsKey(name))
                return;
            var img = Metadata[Target].AnimationsTextures[name];
            Frames = img.Split(x: sz, y: sz).ToArray();
            CurrentFrame = 0;
            Draw_FramesRender();
            BeginInvoke(new Action(() => { DrawFrame.UpdateImage(ref Frames[CurrentFrame]); DrawFrame.Invalidate(); }));
        }
        private void NewAnimation()
        {
            int i = 0;
            while (Metadata[Target].Animations.ContainsKey((Behavior.AnimationsNeeds)i))
            {
                i++;
                if (i >= Enum.GetNames<Behavior.AnimationsNeeds>().Length)
                    return;
            }
            var need = Enum.GetNames<Behavior.AnimationsNeeds>()[i];
            var row_id = dgvAnims.Rows.Add(need, need);
            Metadata[Target].Animations.Add((Behavior.AnimationsNeeds)i, need);
            Metadata[Target].AnimationsTextures.Add(need, new Bitmap(sz * 4, sz));
            dgvAnims.Rows[row_id].Selected = true;
            LoadAnimation(row_id);
        }
        private void DeleteAnimation(int rowIndex)
        {
            if (rowIndex == -1)
                return;
            var need = dgvAnims["cType", rowIndex].Value.ToString();
            Metadata[Target].Animations.Remove(Enum.Parse<Behavior.AnimationsNeeds>(need));
            Metadata[Target].AnimationsTextures.Remove(need);
            dgvAnims.Rows.RemoveAt(rowIndex);
        }
        private void dgvAnims_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (dgvAnimsLastRowIndex > -1 && dgvAnimsLastRowIndex < dgvAnims.RowCount)
                    UIToMetadata_Anim(dgvAnims.Rows[dgvAnimsLastRowIndex]);
                LoadAnimation(e.RowIndex);
            }
            else if (e.Button == MouseButtons.Right)
            {
                ToolStripDropDownMenu menu = new ToolStripDropDownMenu();
                menu.Items[menu.Items.Add(new ToolStripButton("New...") { Name = "New" })].Click += (s, _e) => NewAnimation();
                menu.Items.Add(new ToolStripSeparator());
                menu.Items[menu.Items.Add(new ToolStripButton("Delete", SystemIcons.Warning.ToBitmap()) { Name = "Delete" })].Click += (s, _e) => DeleteAnimation(e.RowIndex);
                menu.Show(Cursor.Position);
            }
            dgvAnimsLastRowIndex = e.RowIndex;
        }

        private void btNewEntity_Click(object sender, EventArgs e)
        {
            string new_entity_name = "NewEntity";
            while (Metadata.ContainsKey(new_entity_name))
                new_entity_name += "_new";
            Metadata[new_entity_name] = Entity.New();
            cbbEntities.Items.Add(new_entity_name);
            cbbEntities.SelectedItem = new_entity_name;
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

        private void btAddBehavior_Click(object sender, EventArgs e)
        {
            if (lbExistingBehaviors.SelectedIndex == -1)
                return;
            lbEntityBehaviors.Items.Add(lbExistingBehaviors.SelectedItem);
            lbExistingBehaviors.Items.RemoveAt(lbExistingBehaviors.SelectedIndex);
            if (lbExistingBehaviors.SelectedIndex >= lbExistingBehaviors.Items.Count)
                lbExistingBehaviors.SelectedIndex = -1;
        }
        private void btRemoveBehavior_Click(object sender, EventArgs e)
        {
            if (lbEntityBehaviors.SelectedIndex == -1)
                return;
            lbExistingBehaviors.Items.Add(lbEntityBehaviors.SelectedItem);
            lbEntityBehaviors.Items.RemoveAt(lbEntityBehaviors.SelectedIndex);
            if (lbEntityBehaviors.SelectedIndex >= lbEntityBehaviors.Items.Count)
                lbEntityBehaviors.SelectedIndex = -1;
        }

        private void dgvAnims_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                // TODO + doublons?
            }
        }
        private void dgvAnims_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvAnims.IsCurrentCellDirty)
                dgvAnims.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }
    }
}
