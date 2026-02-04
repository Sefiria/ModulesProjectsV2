using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project9
{
    public class Helper
    {
        public static readonly Random Rng = new Random();
        public static bool RandomChance(double probability)
        {
            return Rng.NextDouble() < probability;
        }
        public static byte RandomDir()
        {
            return (byte)Rng.Next(0, 8); // 0..7
        }

    }
}
