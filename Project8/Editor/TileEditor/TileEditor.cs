using Project8.Source.TiledMap;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Windows.Forms;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Project8.Source.Map;

namespace Project8.Editor.TileEditor
{
    public partial class TileEditor : Form
    {
        public TileEditor()
        {
            InitializeComponent();
        }

        private BindingList<TileEditable> _tilesBinding;

        private void Form_Load(object sender, EventArgs e)
        {
            // Colonnes éditables
            DGV.Columns.Clear();
            DGV.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                HeaderText = "Id",
                DataPropertyName = nameof(TileEditable.id),
                Width = 60
            });
            DGV.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "Name",
                DataPropertyName = nameof(TileEditable.Name),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            DGV.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Filenames",
                HeaderText = "Filenames (CSV)",
                DataPropertyName = nameof(TileEditable.FilenamesCsv),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            DGV.Columns.Add(new DataGridViewComboBoxColumn
            {
                Name = "Mode",
                HeaderText = "Mode",
                DataPropertyName = nameof(TileEditable.Mode),
                DataSource = Enum.GetValues(typeof(Tile.Modes)),
                Width = 100
            });
            DGV.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MultiTileIndex",
                HeaderText = "MultiTileIndex",
                DataPropertyName = nameof(TileEditable.MultiTileIndex),
                Width = 110
            });
            DGV.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Characteristics",
                HeaderText = "Characteristics",
                DataPropertyName = nameof(TileEditable.Characteristics),
                Width = 120
            });

            _tilesBinding = new BindingList<TileEditable>(
                Tile.Tiles
                    .OrderBy(kv => kv.Key)
                    .Select(kv => kv.Value)
                    .Select(t => new TileEditable
                    {
                        id = t.id,
                        Name = t.Name,
                        FilenamesCsv = t.Filename != null ? string.Join(", ", t.Filename) : "",
                        Mode = t.Mode,
                        MultiTileIndex = t.MultiTileIndex,
                        Characteristics = t.Characteristics
                    })
                    .ToList()
            );

            DGV.DataSource = _tilesBinding;

            DGV.CellValidating += DGV_CellValidating;
        }

        private void DGV_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var col = DGV.Columns[e.ColumnIndex].Name;
            if (col == "Id")
            {
                if (!int.TryParse(e.FormattedValue?.ToString(), out _))
                {
                    e.Cancel = true;
                    DGV.Rows[e.RowIndex].ErrorText = "Id doit être un entier.";
                }
            }
            else if (col == "Mode")
            {
                // avec ComboBoxColumn, la validation est simple ; sinon vérifie la valeur
            }
        }

        public class TileEditable : INotifyPropertyChanged
        {
            private int _id;
            private string _name;
            private string _filenamesCsv;
            private Tile.Modes _mode;
            private int _multiTileIndex;
            private string _characteristics;

            public int id { get => _id; set { _id = value; OnChanged(nameof(id)); } }
            public string Name { get => _name; set { _name = value; OnChanged(nameof(Name)); } }
            public string FilenamesCsv { get => _filenamesCsv; set { _filenamesCsv = value; OnChanged(nameof(FilenamesCsv)); } }
            public Tile.Modes Mode { get => _mode; set { _mode = value; OnChanged(nameof(Mode)); } }
            public int MultiTileIndex { get => _multiTileIndex; set { _multiTileIndex = value; OnChanged(nameof(MultiTileIndex)); } }
            public string Characteristics { get => _characteristics; set { _characteristics = value; OnChanged(nameof(Characteristics)); } }

            public string[] ToFilenamesArray() =>
                string.IsNullOrWhiteSpace(FilenamesCsv)
                    ? Array.Empty<string>()
                    : FilenamesCsv.Split(',')
                                  .Select(s => s.Trim())
                                  .Where(s => !string.IsNullOrEmpty(s))
                                  .ToArray();

            public event PropertyChangedEventHandler PropertyChanged;
            private void OnChanged(string p) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
        }

        // DTO racine pour correspondre au JSON fourni
        public class TilesRoot
        {
            public List<Tile> tiles { get; set; } = new();
        }

        // Options JSON (enum en texte, case-sensitive, indentation…)
        private static JsonSerializerOptions JsonOptions => new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() }
        };

        private string _jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Directory.GetCurrentDirectory()+"/Assets/Data/tileset.json");

        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // (1) Reconstruire la liste de Tile à partir des lignes éditées
                var tilesList = _tilesBinding
                    .Where(te => te != null)
                    .Select(te => new Tile
                    {
                        id = te.id,
                        Name = te.Name ?? "",
                        Filename = te.ToFilenamesArray(),
                        Mode = te.Mode,
                        MultiTileIndex = te.MultiTileIndex,
                        Characteristics = te.Characteristics ?? "s",
                        // Autotile et Tex ne sont pas persistés tels quels dans le JSON d’exemple
                        Autotile = null,
                        Tex = null
                    })
                    .OrderBy(t => t.id)
                    .ToList();

                // (2) Rebuild du dictionnaire en mémoire (optionnel si tu veux refléter immédiatement en runtime)
                Tile.Tiles = tilesList.ToDictionary(t => t.id, t => t);
                foreach (Tile tile in Tile.Tiles.Values)
                {
                    if (tile.Filename.Length == 0) continue;
                    tile.Tex = new Texture2D[tile.Filename.Length];
                    if (tile.Mode == Tile.Modes.Autotile && tile.Autotile == null)
                        tile.Autotile = new Autotile();
                    for (int i = 0; i < tile.Filename.Length; i++)
                        tile.Tex[i] = Texture2D.FromFile(GameMain.Instance.GraphicsDevice, tile.Filename[i]);
                }

                // (3) Sérialiser le JSON
                var root = new TilesRoot { tiles = tilesList };
                var json = JsonSerializer.Serialize(root, JsonOptions);

                File.WriteAllText(_jsonPath, json, System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l’enregistrement du JSON : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // e.Cancel = true; // si tu veux empêcher la fermeture en cas d’erreur
            }
        }
    }
}
