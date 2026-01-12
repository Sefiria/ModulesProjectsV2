using Project8.Source.TiledMap;
using System.Text.Json.Serialization;

namespace Project8.Source.Map
{
    public class Autotile
    {
        public static string DefaultPattern = "a,f,h,v,z,q,s,d,zq,zd,sq,sd,ns,nz,nd,nq";


        public bool UseDefaultPattern { get; set; } = true;
        public string Pattern { get; set; } = string.Empty;

        public Autotile()
        {
        }


        /// <param name="pattern">Separated by a comma (no spacing). Possibilities : a (alone), z (above), q (left), s (bottom), d (right), h (horizontal), v (vertical), zq (above-left), zd (above-right), sq (botttom-left), sd (botttom-right), f (full), ns (except bottom), nz (except above), nd (except left), nq (except right)</param>
        /// <returns>Index in the pattern. -1 if incorrect.</returns>
        public static int Calculate(TiledMap map, int layer, int x, int y, string pattern)
        {
            int v = map[layer, x, y];
            bool z = map[layer, x, y - 1] == v;
            bool q = map[layer, x - 1, y] == v;
            bool s = map[layer, x, y + 1] == v;
            bool d = map[layer, x + 1, y] == v;

            string key = "";

            if (!z && !s && !q && !d) key = "a";
            else if (z && q && s && d) key = "f";
            else if (!z && q && !s && d) key = "h";
            else if (z && !q && s && !d) key = "v";
            else if (!z && q && s && d) key = "nz";
            else if (z && !q && s && d) key = "nq";
            else if (z && q && !s && d) key = "ns";
            else if (z && q && s && !d) key = "nd";
            else if (z && q && !s && !d) key = "zq";
            else if (z && !q && !s && d) key = "zd";
            else if (!z && q && s && !d) key = "sq";
            else if (!z && !q && s && d) key = "sd";
            else
            {
                if (z) key += "z";
                if (q) key += "q";
                if (s) key += "s";
                if (d) key += "d";
            }

            var patterns = pattern.Split(',');
            for (int i = 0; i < patterns.Length; i++)
            {
                if (patterns[i] == key)
                    return i;
            }

            return -1;
        }
        public (int, int) Calculate(TiledMap map, int layer, int x, int y)
        {
            int v = map[layer, x, y];
            bool connect_to_all = Tile.Tiles[v].IsConnectingToAllSolid;
            bool z = connect_to_all ? Tile.Tiles[map[layer, x, y - 1]].IsSolid : map[layer, x, y - 1] == v;
            bool q = connect_to_all ? Tile.Tiles[map[layer, x - 1, y]].IsSolid : map[layer, x - 1, y] == v;
            bool s = connect_to_all ? Tile.Tiles[map[layer, x, y + 1]].IsSolid : map[layer, x, y + 1] == v;
            bool d = connect_to_all ? Tile.Tiles[map[layer, x + 1, y]].IsSolid : map[layer, x + 1, y] == v;

            string key = "";

            if (!z && !s && !q && !d) key = "a";
            else if (z && q && s && d) key = "f";
            else if (!z && q && !s && d) key = "h";
            else if (z && !q && s && !d) key = "v";
            else if (!z && q && s && d) key = "nz";
            else if (z && !q && s && d) key = "nq";
            else if (z && q && !s && d) key = "ns";
            else if (z && q && s && !d) key = "nd";
            else if (z && q && !s && !d) key = "zq";
            else if (z && !q && !s && d) key = "zd";
            else if (!z && q && s && !d) key = "sq";
            else if (!z && !q && s && d) key = "sd";
            else
            {
                if (z) key += "z";
                if (q) key += "q";
                if (s) key += "s";
                if (d) key += "d";
            }

            var pattern = UseDefaultPattern ? DefaultPattern : Pattern;
            var patterns = pattern.Split(',');
            for (int i = 0; i < patterns.Length; i++)
            {
                if (patterns[i] == key)
                    return (i % 4, i / 4);
            }

            return (-1, -1);
        }
    }
}
