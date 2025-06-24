using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project7.Source.Map
{
    public class Autotile
    {
        /// <param name="pattern">Separated by a comma (no spacing). Possibilities : a (alone), z (above), q (left), s (bottom), d (right), h (horizontal), v (vertical), zq (above-left), zd (above-right), sq (botttom-left), sd (botttom-right), f (full), ns (except bottom), nz (except above), nd (except left), nq (except right)</param>
        /// <returns>Index in the pattern. -1 if incorrect.</returns>
        public static int Calculate(Map map, int layer, int x, int y, string pattern)
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
    }
}
