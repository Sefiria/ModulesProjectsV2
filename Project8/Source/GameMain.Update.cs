using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project8.Editor;
using Project8.Source;
using Project8.Source.Entities;
using Project8.Source.Map;
using Project8.Source.Particles;
using Project8.Source.TiledMap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tooling;
using static System.Windows.Forms.Design.AxImporter;

namespace Project8
{
    public partial class GameMain : Game
    {
        public const int CONST_MAX_FLIES = 5;

        public TiledMap Map;
        public EntityManager EntityManager;
        public ParticleManager ParticleManager;

        public string pinou_gift_name = "pinou_gift_name", kill_flies_quest_name = "kill_flies_quest_name";

        private long afk_time = 0;
        private Point old_ms_pos;
        private int killed_flies = 0;
        private bool first_time_a_fly_appears = true;

        void LoadUpdate()
        {
            Init_Map();
            Init_Entities();
            Init_Particles();

            old_ms_pos = MS.Position;
        }
        void Init_Map()
        {
            LoadTiles();

            Map = new TiledMap(5, (int)(ScreenWidth / tilesize / scale), (int)((ScreenHeight) / tilesize / scale));

            for (int x = 0; x < Map.w; x++)
            {
                //for (int y = 0; y < Map.h; y++)
                //{
                //    //Map.Tiles[0, x, y] = 0;
                //    Map.SetTile(0, 0, y, 0);
                //    Map.SetTile(0, Map.w - 1, y, 0);
                //}
                //Map.SetTile(0, x, 0, 0);
                Map.SetTile(0, x, Map.h - 1, 0);
            }
        }
        void Init_Entities()
        {
            EntityManager = new EntityManager();
        }
        void Init_Particles()
        {
            ParticleManager = new ParticleManager();
        }
        void LoadTiles()
        {
            string file = File.ReadAllText(Directory.GetCurrentDirectory()+"/Assets/Data/tileset.json");
            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            options.Converters.Add(new JsonStringEnumConverter());
            var data = JsonSerializer.Deserialize<JsonTilesData>(file, options);
            Tile.Tiles.Clear();
            foreach(var tile in data.Tiles)
            {
                ManageErrorsDuringTilesLoading(tile);
                Tile.Tiles[tile.id] = tile;
                Tile.Tiles[tile.id].Tex = new Texture2D[Tile.Tiles[tile.id].Filename.Length];
                if(Tile.Tiles[tile.id].Mode == Tile.Modes.Autotile && Tile.Tiles[tile.id].Autotile == null)
                    Tile.Tiles[tile.id].Autotile = new Autotile();
                for(int i=0; i< Tile.Tiles[tile.id].Filename.Length; i++)
                    Tile.Tiles[tile.id].Tex[i] = Texture2D.FromFile(GraphicsDevice, Tile.Tiles[tile.id].Filename[i]);
            }
        }
        void ManageErrorsDuringTilesLoading(Tile tile)
        {
            //void ExitWithMessage(string msg)
            //{
            //    System.Windows.Forms.MessageBox.Show(msg, "Invalid Config", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    Exit();
            //}

            if (Tile.Tiles.ContainsKey(tile.id))
            {
                if (System.Windows.Forms.MessageBox.Show($"Tile id {tile.id} ({tile.Name}) already exists ({Tile.Tiles[tile.id].Name}).{Environment.NewLine}Would you like more details ?", "Tile ID already present", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    var cur = Tile.Tiles[tile.id];
                    string msg = $"Cannot add tile:{Environment.NewLine}  - id: {tile.id}{Environment.NewLine}  - name: {tile.Name}{Environment.NewLine}  - filename: {tile.Filename}{Environment.NewLine}Because this tile ID already exists :{Environment.NewLine}  - id: {cur.id}{Environment.NewLine}  - name: {cur.Name}{Environment.NewLine}  - filename: {cur.Filename}";
                    System.Windows.Forms.MessageBox.Show($"{msg}{Environment.NewLine}Do you want to copy this report in the clipboard ?", "Tile ID already present (more details)", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    Exit();
                }
            }
            //if (tile.Mode == Tile.Modes.Autotile && tile.Autotile == null)
            //    ExitWithMessage($"Mode is Autotile but Autotile is not defined.");
        }
        void Update()
        {
            EntityManager.Update();
            ParticleManager.Update();
            EditorManager.Update();
        }
    }
}