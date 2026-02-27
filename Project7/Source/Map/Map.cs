namespace Project7.Source.Map
{
    public class Map
    {
        public int[,,] Tiles;
        public int this[int l, int x, int y] => safe(l, x, y) ? Tiles[l, x, y] : -1;
        public int w => Tiles.GetLength(1);
        public int h => Tiles.GetLength(2);
        public int z => Tiles.GetLength(0);
        public bool safe(int l, int x, int y) => l >= 0 && l < z && x >= 0 && x < w && y >= 0 && y < h;
        public bool SetTile(int layer, int x, int y, int tile)
        {
            bool is_safe = safe(layer, x, y);
            if(is_safe)
                Tiles[layer, x, y] = tile;
            return is_safe;
        }
        public Map(int layers, int width, int height)
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
        }
    }
}
