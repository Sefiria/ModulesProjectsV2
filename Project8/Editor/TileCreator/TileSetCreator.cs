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

namespace Project8.Editor.TileSetCreator
{
    public partial class TileSetCreator : Form
    {
        bool IsMouseDown = false;
        Point ms_old, ms;
        Bitmap BaseImage = null, ImageCopy = null;
        Timer timerDraw = new Timer() { Enabled = true, Interval = 10 };
        G g;
        float scale = 20F;
        List<Image> Parts = new List<Image>();
        List<PictureBox> Renders;
        Font bigfont = new Font("SegoeUI", 32F);

        public static int sz = GlobalVariables.tilesize;

        public TileSetCreator()
        {
            InitializeComponent();
        }
        private void TileCreator_Load(object sender, EventArgs e)
        {
            Renders = new List<PictureBox>() { r_a, r_f, r_h, r_v, r_z, r_q, r_s, r_d, r_zq, r_zd, r_sq, r_sd, r_ns, r_nz, r_nd, r_nq };
            BaseImage = new Bitmap(16, 16);
            g = G.FromImage(BaseImage);
            g.Clear(Color.White);
            timerDraw.Tick += DrawRender;

            var colorsbtn = new List<PictureBox>() { usedColor, color1, color2, color3, color4, color5, color6, color7, color8 };
            foreach (var c in colorsbtn)
            {
                c.Image = new Bitmap(c == usedColor ? 64 : 32, c == usedColor ? 64 : 32);
                g = G.FromImage(c.Image);
                g.Clear(Color.White);
            }

            PopulateTree(tv, "Assets\\Textures\\Tilesets");
            ContextMenuStrip cms = new ContextMenuStrip();
            cms.Items.Add("Renommer", null, RenameNode_Click);
            cms.Items.Add("Delete", null, DeleteNode_Click);
            tv.ContextMenuStrip = cms;
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

        private void RenameNode_Click(object sender, EventArgs e)
        {
            if (tv.SelectedNode == null) return;

            string oldPath = tv.SelectedNode.Tag?.ToString();
            if (string.IsNullOrWhiteSpace(oldPath) || !File.Exists(oldPath)) return;

            using (var dlg = new RenameDialog(tv.SelectedNode.Text)) // Créez une petite Form avec TextBox
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    string newName = dlg.NewName.Trim();
                    if (string.IsNullOrWhiteSpace(newName)) return;

                    string dir = Path.GetDirectoryName(oldPath);
                    string ext = Path.GetExtension(oldPath);
                    string newPath = Path.Combine(dir, newName + ext);

                    // Vérifier collision
                    if (File.Exists(newPath))
                    {
                        MessageBox.Show("Un fichier avec ce nom existe déjà.");
                        return;
                    }

                    // Renommer le fichier
                    File.Move(oldPath, newPath);

                    // Mettre à jour le TreeNode
                    tv.SelectedNode.Text = newName;
                    tv.SelectedNode.Tag = newPath;
                }
            }
        }
        private void DeleteNode_Click(object sender, EventArgs e)
        {
            var node = tv.SelectedNode;
            if (node == null) return;
            var path = node.Tag?.ToString();
            if (string.IsNullOrWhiteSpace(path)) return;
            if (File.Exists(path)) File.Delete(path);
            else if (Directory.Exists(path)) Directory.Delete(path, true);
            node.Remove();
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
            int currentIdx = Renders.IndexOf(sender as PictureBox);
            ms_old = ms;
            var Render = sender as PictureBox;
            ms = Render.PointToClient(MousePosition);
            if (!IsMouseDown) return;

            bool IsLeft = e.Button == MouseButtons.Left;
            bool IsRight = e.Button == MouseButtons.Right;
            bool IsMiddle = e.Button == MouseButtons.Middle;

            if (BaseImage == null) return;

            int tilesX = BaseImage.Width / sz;
            int tilesY = BaseImage.Height / sz;
            if (currentIdx < 0 || currentIdx >= tilesX * tilesY) return;

            int ti = currentIdx % tilesX;
            int tj = currentIdx / tilesX;
            int ox = ti * sz;
            int oy = tj * sz;

            int tx = Math.Clamp((int)(ms.X / scale), 0, sz - 1);
            int ty = Math.Clamp((int)(ms.Y / scale), 0, sz - 1);

            var tileRect = new Rectangle(ox, oy, sz, sz);

            if (IsLeft || IsRight)
            {
                if (KB.IsKeyDown(KB.Key.LeftCtrl))
                {
                    if (ImageCopy != null)
                    {
                        using (var g = G.FromImage(BaseImage))
                        {
                            g.InterpolationMode = InterpolationMode.NearestNeighbor;
                            g.PixelOffsetMode = PixelOffsetMode.Half;
                            g.SmoothingMode = SmoothingMode.None;
                            g.DrawImage(ImageCopy, tileRect,
                                new Rectangle(0, 0, ImageCopy.Width, ImageCopy.Height),
                                GraphicsUnit.Pixel);
                        }
                    }
                    UpdatePreview(BaseImage, tileRect, currentIdx, Render);
                    return;
                }
                if (KB.IsKeyDown(KB.Key.LeftAlt))
                {
                    using (var img = new Bitmap(BaseImage.Clone(tileRect, BaseImage.PixelFormat)))
                    {
                        if (IsLeft)
                            img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        else
                            img.RotateFlip(RotateFlipType.RotateNoneFlipY);

                        for (int y = 0; y < sz; y++)
                        {
                            for (int x = 0; x < sz; x++)
                            {
                                BaseImage.SetPixel(ox + x, oy + y, img.GetPixel(x, y));
                            }
                        }
                    }

                    UpdatePreview(BaseImage, tileRect, currentIdx, Render);
                    return;
                }

                double d = Maths.Distance(ms_old, ms);
                double step = 1.0 / Math.Max(d, 1.0);
                for (double t = 0.0; t <= 1.0; t += step)
                {
                    PointF p = Maths.Lerp(ms_old, ms, t);
                    int px = (int)(p.X / scale);
                    int py = (int)(p.Y / scale);
                    if (px >= 0 && py >= 0 && px < sz && py < sz)
                        BaseImage.SetPixel(ox + px, oy + py, IsLeft ? usedColor.BackColor : Color.White);
                }
                BaseImage.SetPixel(ox + tx, oy + ty, IsLeft ? usedColor.BackColor : Color.White);
                UpdatePreview(BaseImage, tileRect, currentIdx, Render);
                return;
            }
            else if (IsMiddle)
            {
                if (KB.IsKeyDown(KB.Key.LeftCtrl))
                {
                    ImageCopy = new Bitmap(BaseImage.Clone(tileRect, BaseImage.PixelFormat));
                }
                else
                {
                    usedColor.BackColor = BaseImage.GetPixel(ox + tx, oy + ty);
                    color1.BackColor = usedColor.BackColor;
                    usedColor.BackColor = color1.BackColor;
                    if (usedColor.Image == null)
                        usedColor.Image = new Bitmap(usedColor.Width, usedColor.Height);
                    using (G _g = G.FromImage(color1.Image)) _g.Clear(color1.BackColor);
                    using (G _g = G.FromImage(usedColor.Image)) _g.Clear(usedColor.BackColor);
                    color1.Refresh();
                    usedColor.Refresh();
                }
            }
        }

        private void UpdatePreview(Bitmap baseImg, Rectangle tileRect, int idx, PictureBox render)
        {
            using (var tileCopy = baseImg.Clone(tileRect, baseImg.PixelFormat))
            {
                var preview = ResizeExact(tileCopy, (int)(sz * scale), (int)(sz * scale), InterpolationMode.NearestNeighbor);
                Parts[idx]?.Dispose();
                Parts[idx] = preview;

                var old = render.Image;
                render.Image = new Bitmap(preview);
                old?.Dispose();

                render.Refresh();
            }
        }



        private void DrawRender(object sender, EventArgs e)
        {
            for (int i = 0; i < Renders.Count && i < Parts.Count; i++)
            {
                if (IsDisposed)
                    return;
                Renders[i].Image = new Bitmap(Parts[i]);
                var ms = Cursor.Position;
                if (radAtutotile.Checked && !Renders[i].ClientRectangle.Contains(Renders[i].PointToClient(ms)))
                {
                    using (G g = G.FromImage(Renders[i].Image))
                    {
                        var dspstr = GetAffichageString(i, 0);
                        g.DrawString(dspstr.STR, bigfont, new SolidBrush(Color.FromArgb(150, 255, 255, 255)), 80 - dspstr.W / 2 - 2, 80 - dspstr.H / 2 - 2);
                        g.DrawString(dspstr.STR, bigfont, new SolidBrush(Color.FromArgb(150, 0, 0, 0)), 80 - dspstr.W / 2, 80 - dspstr.H / 2);
                    }
                }
            }
        }
        (string STR, int W, int H) GetAffichageString(int i, int j)
        {
            string str = String.Empty;
            if (j == 0)// Autotile
            {
                switch (i)
                {
                    case 0: str = "•"; break;
                    case 1: str = "↔↨"; break;
                    case 2: str = "↔"; break;
                    case 3: str = "↨"; break;
                    case 4: str = "↑"; break;
                    case 5: str = "←"; break;
                    case 6: str = "↓"; break;
                    case 7: str = "→"; break;
                    case 8: str = "←↑"; break;
                    case 9: str = "↑→"; break;
                    case 10: str = "←↓"; break;
                    case 11: str = "↓→"; break;
                    case 12: str = "↔↑"; break;
                    case 13: str = "↔↓"; break;
                    case 14: str = "←↨"; break;
                    case 15: str = "↨→"; break;
                    default: str = ""; break;
                }
            }
            else if (j == 1) // MultiTile
            {
                str = i.ToString();
            }

            SizeF str_sz = SizeF.Empty;
            if (!string.IsNullOrWhiteSpace(str))
                using (G g = G.FromImage(Renders[i].Image))
                    str_sz = g.MeasureString(str, bigfont);

            return (str, (int)str_sz.Width, (int)str_sz.Height);
        }

        private void color_MouseClick(object sender, MouseEventArgs e)
        {
            var colorsbtn = new List<PictureBox>() { color1, color2, color3, color4, color5, color6, color7, color8 };
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

        private void btLoad_Click(object sender, EventArgs e)
        {
            if (tv.SelectedNode != null)
            {
                var path = tv.SelectedNode.Tag.ToString();
                if (File.Exists(path))
                {
                    var img = Image.FromFile(path);
                    if (img.Width != sz * 4 || img.Height != sz * 4)
                    {
                        MessageBox.Show(this, $"Failed during load file : image size should be exactly {sz * 4}x{sz * 4}");
                        return;
                    }
                    BaseImage = new Bitmap(img);
                    Parts.Clear();
                    var tileRect = new Rectangle(0, 0, sz, sz);
                    for (int j = 0; j < 4; j++)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            using (var tileCopy = BaseImage.Clone(new Rectangle(i * sz, j * sz, sz, sz), BaseImage.PixelFormat).ResizeExact(160, 160))
                            {
                                var preview = ResizeExact(tileCopy, (int)(sz * scale), (int)(sz * scale), InterpolationMode.NearestNeighbor);
                                Parts.Add(preview);
                                Renders[Parts.Count - 1].Image = preview;
                                Renders[Parts.Count - 1].Invalidate();
                            }
                        }
                    }
                }
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if (tv.SelectedNode != null)
            {
                var path = tv.SelectedNode.Tag.ToString();

                int tilesX = BaseImage.Width / sz;
                int tilesY = BaseImage.Height / sz;
                using var composed = new Bitmap(BaseImage.Width, BaseImage.Height, PixelFormat.Format32bppArgb);

                using (var g = G.FromImage(composed))
                {
                    g.InterpolationMode = InterpolationMode.NearestNeighbor; // préserve le pixel-art
                    g.PixelOffsetMode = PixelOffsetMode.Half;
                    g.SmoothingMode = SmoothingMode.None;

                    for (int j = 0; j < tilesY; j++)
                    {
                        for (int i = 0; i < tilesX; i++)
                        {
                            int idx = j * tilesX + i;
                            var part = Parts[idx]; // 160×160

                            // Redescend la vignette vers sz×sz pour recoller dans l'image composée
                            using var tileSz = ResizeExact(part, sz, sz, InterpolationMode.NearestNeighbor);
                            g.DrawImage(tileSz,
                                new Rectangle(i * sz, j * sz, sz, sz),
                                new Rectangle(0, 0, sz, sz),
                                GraphicsUnit.Pixel);
                        }
                    }
                }
                if (File.Exists(path))
                    File.Delete(path);
                composed.Save(path, ImageFormat.Png);
            }
        }
        static Bitmap ResizeExact(Image src, int w, int h, InterpolationMode mode = InterpolationMode.NearestNeighbor)
        {
            var dst = new Bitmap(w, h, PixelFormat.Format32bppArgb);
            using var g = G.FromImage(dst);
            g.InterpolationMode = mode;
            g.PixelOffsetMode = PixelOffsetMode.Half;
            g.SmoothingMode = SmoothingMode.None;
            g.DrawImage(src, new Rectangle(0, 0, w, h), new Rectangle(0, 0, src.Width, src.Height), GraphicsUnit.Pixel);
            return dst;
        }




        private void btNew_Click(object sender, EventArgs e)
        {
            if (tv.SelectedNode == null) return;

            var tag = tv.SelectedNode.Tag?.ToString();
            if (string.IsNullOrWhiteSpace(tag)) return;

            var parentNode = Directory.Exists(tag) ? tv.SelectedNode : tv.SelectedNode.Parent;
            if (parentNode == null) return;

            var baseDir = Directory.Exists(tag) ? tag : Path.GetDirectoryName(tag);
            if (string.IsNullOrWhiteSpace(baseDir) || !Directory.Exists(baseDir)) return;

            string fileName = "NouveauTileset.png";
            string fullPath = Path.Combine(baseDir, fileName);
            int n = 1;
            while (File.Exists(fullPath))
            {
                fileName = $"NouveauTileset_{n}.png";
                fullPath = Path.Combine(baseDir, fileName);
                n++;
            }

            using (var bmp = new Bitmap(sz * 4, sz * 4, PixelFormat.Format32bppArgb))
            using (var g = G.FromImage(bmp))
            {
                g.Clear(Color.Transparent);
                bmp.Save(fullPath, ImageFormat.Png);
            }

            var child = new TreeNode(Path.GetFileNameWithoutExtension(fileName)) { Tag = fullPath };
            parentNode.Nodes.Add(child);
            parentNode.Expand();
        }

        private void btHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, @"Middle_Click : Copy pixel color
Ctrl+Middle_Click : Copy Image in Memory
Ctrl+Left_Click : Paste In Memory image
Alt+Left_Click : Flip image horizontally,
Alt+Right_Click : Flip image vertically");
        }
    }




    public class RenameDialog : Form
    {
        public string NewName => tb.Text;
        TextBox tb = new TextBox();

        public RenameDialog(string currentName)
        {
            Text = "Renommer";
            tb.Text = currentName;
            tb.Dock = DockStyle.Top;
            Controls.Add(tb);
            var ok = new Button { Text = "OK", DialogResult = DialogResult.OK, Dock = DockStyle.Bottom };
            Controls.Add(ok);
            AcceptButton = ok;
        }
    }

}
