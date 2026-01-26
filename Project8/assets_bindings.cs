using GeonBit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project8
{
    internal class assets_bindings
    {
        public static readonly Dictionary<string, string> Resources = new Dictionary<string, string>()
        {
            ["missing"] = Path.Combine(GlobalPaths.Textures, "missing.png"),
        }; 
    }
}
