using Project8.Source.TiledMap;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Tooling;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Project8.Editor.TileCreator
{
    public partial class FormLoadTile : Form
    {
        public List<int> TileIndexBinding = new List<int>();
        public int SelectedTileIndex = 0;
        public FormLoadTile()
        {
            InitializeComponent();
        }

        public void LoadForm(object sender, EventArgs e)
        {
            int sz = 32;
            imgListTiles.ImageSize = new Size(sz, sz);
            imgListTiles.Images.AddRange(Tile.Tiles.Values.Where(t => t.Filename != null && t.Filename.Length > 0).Select(t => { TileIndexBinding.Add(t.id); return (Image.FromFile(t.Filename[0]) as Bitmap).Crop(16, sz, t.MultiTileIndex, 0); }).ToArray());
            lv.View = View.Tile;
            lv.LargeImageList = imgListTiles;
            lv.TileSize = new Size(sz + 10, sz);
            for (int i = 0; i < imgListTiles.Images.Count; i++)
            {
                var item = new ListViewItem($"Tile {i}", i);
                lv.Items.Add(item);
            }
        }

        private void lv_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            lv_SelectTile();
        }
        private void btSelect_Click(object sender, EventArgs e)
        {
            lv_SelectTile();
        }
        private void lv_SelectTile()
        {
            if (lv.SelectedIndices.Count > 0)
            {
                SelectedTileIndex = TileIndexBinding[lv.SelectedIndices[0]];
                DialogResult = DialogResult.OK;
            }
            else
            {
                DialogResult = DialogResult.Cancel;
            }
            Close();
        }
    }
}
