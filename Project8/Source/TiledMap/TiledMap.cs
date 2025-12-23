using Project8.Source.Entities;
using Project8.Source.TiledMap;
using Tooling;

namespace Project8.Source.Map
{
    public class TiledMap
    {
        public int[,,] Tiles;

        public int this[int l, int x, int y] => safe(l, x, y) ? Tiles[l, x, y] : -1;
        public int w => Tiles.GetLength(1);
        public int h => Tiles.GetLength(2);
        public int z => Tiles.GetLength(0);
        public bool safe(int l, int x, int y) => l >= 0 && l < z && x >= 0 && x < w && y >= 0 && y < h;
        public bool isout(Entity d) => isout(d.X / (GlobalVariables.tilesize * GlobalVariables.scale), d.Y / (GlobalVariables.tilesize * GlobalVariables.scale));
        public bool isout(vecf v) => isout(v.i.x, v.i.y);
        public bool isout(float x, float y) => x < 0F || y < 0F || x >= w || y >= h;
        public bool isout(int x, int y) => x < 0 || y < 0 || x >= w || y >= h;
        public bool SetTile(int layer, int x, int y, int tile)
        {
            bool is_safe = safe(layer, x, y);
            if(is_safe)
                Tiles[layer, x, y] = tile;
            return is_safe;
        }

        public TiledMap(int layers, int width, int height)
        {
            Tiles = new int[layers, width, height];
            for (int l = 0; l < layers; l++)
            {
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        Tiles[l, x, y] = -1;
                    }
                }
            }
            Tile.Tiles[-1] = new Tile() { Characteristics = "", id = -1 };
        }
        public object Collider(Entity obj, vecf offset = null, bool isJump = false) => CollisionsManager.Collider(obj, offset, isJump);
    }
}
