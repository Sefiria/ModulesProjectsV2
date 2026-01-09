using Microsoft.Xna.Framework.Graphics;
using Project8.Source.Map;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Project8.Source.TiledMap
{
    public class Tile
    {
        public enum Modes
        {
            [EnumMember(Value = "Single")]
            Single,
            [EnumMember(Value = "Autotile")]
            Autotile,
            [EnumMember(Value = "MultiTile")]
            MultiTile
        }

        public static Dictionary<int, Tile> Tiles = new Dictionary<int, Tile>();
        public static Tile GetTile(int id) => Tiles[Tiles.ContainsKey(id) ? id : 0];

        [JsonRequired]
        public string Name { get; set; }
        [JsonRequired]
        public string[] Filename { get; set; }
        [JsonRequired]
        public int id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Modes Mode { get; set; } = Modes.Single;
        /// <summary>
        /// s: solid
        /// </summary>
        public string Characteristics { get; set; } = "s";
        /// <summary>
        /// An autotile is a sprite containing a set of tiles, drawn in runtime according to the tiles around (using a pattern indicating the different combinations)
        /// </summary>
        public Autotile Autotile { get; set; }
        /// <summary>
        /// Used if IsMultiTile is enabled. Max value defined by the sprite width / tilesize
        /// </summary>
        public int MultiTileIndex { get; set; } = 0;
        public Texture2D[] Tex;
        public Tile() { }

        public bool IsSolid => Characteristics.Contains('s');
        public bool IsGravityApplies => Characteristics.Contains('g');
    }
}
