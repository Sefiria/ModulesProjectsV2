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
        /// <summary>
        /// [min, max] (max included)
        /// </summary>
        public static int RandomRange(int min, int max) => Rng.Next(min, max+1);
        public static string DirName(byte d) => d switch
        {
            0 => "N",
            1 => "S",
            2 => "W",
            3 => "E",
            4 => "NW",
            5 => "NE",
            6 => "SW",
            7 => "SE",
            _ => d.ToString()
        };
    }
}
