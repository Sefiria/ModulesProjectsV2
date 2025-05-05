using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Tooling
{
    public static class RandomThings
    {
        public static int rndbyte_int => Common.Rnd.Next(byte.MinValue, byte.MaxValue);
        public static Color rndbyte_color => Color.FromArgb(rndbyte_int, rndbyte_int, rndbyte_int);
        /// <summary>
        /// Return randomly 0 or 1
        /// </summary>
        /// <returns></returns>
        public static int rnd() => Common.Rnd.Next(2);
        public static int rnd(int min, int max) => Common.Rnd.Next(min, max);
        public static int rnd(int max) => rnd(0, max);
        public static int rnd(float int_max) => rnd(0, (int)int_max);
        public static int advanced_rnd(int min, int max, bool max_included = true) => min + Common.Rnd.Next((max + (max_included ? 1 : 0)) - min);
        public static int arnd(int min, int max, bool max_included = true) => advanced_rnd(min, max, max_included);
        public static int Around(int reference, int gap) => (int)(reference + gap * rnd1Around0());

        public static float rnd(float min, float max_excluded) => rnd((int)(min * 1000), (int)(max_excluded * 1000)) / 1000f;
        public static float rnd1(int precision = 100000) => rnd(precision + 1) / (precision + 1F);
        public static float rnd1Around0() => rnd(200001) / 100001f - 1f;
        public static bool rnd_IsInRange(float range_min, float range_max)
        {
            float rnd = rnd1((int)Math.Max(Math.Max(1F / range_min, 1F / range_max), 100000));
            return rnd >= range_min && rnd <= range_max;
        }
        public static vecf rnd_vecf_in_screen(float w, float h, float x = 0F, float y = 0F) => (arnd((int)x, (int)w, false), arnd((int)y, (int)h, false)).Vf();
        public static vec rnd_vec_in_screen(int w, int h, int x = 0, int y = 0) => (arnd(x, w, false), arnd(y, h, false)).V();
        public static string rndByteHexString() => ((byte)(rnd1() * 0xFF)).ToString("x2");

        public static string GetCurrentTickDigits(int count_lasts) => string.Concat(DateTime.UtcNow.Ticks.ToString().Skip(DateTime.UtcNow.Ticks.ToString().Length - count_lasts));
    }
}