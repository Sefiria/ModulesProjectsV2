using Project8.Source.TiledMap;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
        float scale = 10F;
        Tile tile;

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

            label6.Visible = numMultiTileID.Visible = cbbMode.SelectedItem.ToString() == Enum.GetName(Tile.Modes.MultiTile);
            cbbMode.SelectedIndexChanged += (s, e) =>
            {
                label6.Visible = numMultiTileID.Visible = cbbMode.SelectedIndex == Enum.GetNames<Tile.Modes>().ToList().IndexOf(Enum.GetName(Tile.Modes.MultiTile));
                label3.Visible = tbPattern.Visible = cbbMode.SelectedIndex == Enum.GetNames<Tile.Modes>().ToList().IndexOf(Enum.GetName(Tile.Modes.Autotile));
            };

            Image = new Bitmap(16, 16);
            g = System.Drawing.Graphics.FromImage(Image);
            g.Clear(Color.White);
            timerDraw.Tick += DrawRender;
        }

        private void DrawRender(object sender, EventArgs e)
        {
            int rw = Render.ClientSize.Width, rh = Render.ClientSize.Height;
            float f = Math.Min((float)rw / Image.Width, (float)rh / Image.Height);
            int w = (int)(Image.Width * f), h = (int)(Image.Height * f);

            var bmp = new Bitmap(w, h);
            using (var g = System.Drawing.Graphics.FromImage(bmp))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.DrawImage(Image, 0, 0, w, h);

                if (cbbMode.SelectedItem.ToString() == Enum.GetName(typeof(Tile.Modes), Tile.Modes.MultiTile))
                {
                    int tileId = (int)numMultiTileID.Value;
                    int tilesX = Image.Width / 16;
                    int tileW = (int)(16 * f);
                    int tx = tileId % tilesX;
                    int ty = tileId / tilesX;
                    g.DrawRectangle(new Pen(Color.Black, 2), tx * tileW, ty * tileW, tileW, tileW);
                }
            }
            Render.Image = bmp;
        }

        private void btLoad_Click(object sender, EventArgs e)
        {
            var dial = new FormLoadTile();
            if (dial.ShowDialog(this) == DialogResult.OK)
            {
                tile = Tile.Tiles[dial.SelectedTileIndex];
                numID.Value = tile.id;
                tbFileNameA.Text = tile.Filename.Length > 0 ? tile.Filename[0] : "";
                tbFileNameB.Text = tile.Filename.Length > 1 ? tile.Filename[1] : "";
                tbFileNameC.Text = tile.Filename.Length > 2 ? tile.Filename[2] : "";
                tbName.Text = tile.Name;
                tbCharacteristics.Text = tile.Characteristics;
                cbbMode.SelectedIndex = Enum.GetNames<Tile.Modes>().ToList().IndexOf(tile.Mode.ToString());
                numMultiTileID.Value = tile.MultiTileIndex;
                tbPattern.Text = tile.Autotile.Pattern;
                DefineImage();
                g = System.Drawing.Graphics.FromImage(Image);
                DrawRender(null, null);
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            if (Image == null) return;

            // Récupère le tile par l’ID affiché (inverse du Load)
            tile = Tile.Tiles.FirstOrDefault(t => t.Value.id == (int)numID.Value).Value;
            if (tile == null) return;

            // Met à jour les métadonnées depuis la UI
            tile.Name = tbName.Text;
            tile.Characteristics = tbCharacteristics.Text;
            tile.Mode = (Tile.Modes)Enum.Parse(typeof(Tile.Modes), cbbMode.SelectedItem.ToString());
            tile.MultiTileIndex = (int)numMultiTileID.Value;
            if(tile.Mode == Tile.Modes.Autotile)
            {
                if (tile.Autotile == null)
                    tile.Autotile = new Source.Map.Autotile();
                tile.Autotile.Pattern = tbPattern.Text;
            }

            var fA = tbFileNameA.Text?.Trim();
            var fB = tbFileNameB.Text?.Trim();
            var fC = tbFileNameC.Text?.Trim();
            tile.Filename = new[] { fA, fB, fC }.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
        }

        private void btManage_Click(object sender, EventArgs e)
        {
            new TileEditor.TileEditor().ShowDialog(this);
        }

        private void radFileName_CheckedChanged(object sender, EventArgs e)
        {
            DefineImage();
        }
        void DefineImage()
        {
            if (radFileNameA.Checked && tile.Filename.Length > 0 && File.Exists(tile.Filename[0]))
                Image = (Bitmap)System.Drawing.Image.FromFile(tile.Filename[0]);
            else if (radFileNameB.Checked && tile.Filename.Length > 1 && File.Exists(tile.Filename[1]))
                Image = (Bitmap)System.Drawing.Image.FromFile(tile.Filename[1]);
            else if (radFileNameC.Checked && tile.Filename.Length > 2 && File.Exists(tile.Filename[2]))
                Image = (Bitmap)System.Drawing.Image.FromFile(tile.Filename[2]);
            else
                Image = new Bitmap(16, 16);
        }
    }
}
