using GeonBit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project8
{
    internal class assets_bindings
    {
        public static readonly Dictionary<string, string> Resources = new Dictionary<string, string>()
        {
            ["missing"] = "Assets/Textures/missing.png",
            ["tilesets/grass"] = "Assets/Textures/Tilesets/grass.png",
            ["wooden_fence"] = "Assets/Textures/wooden_fence.png",
            //["pinou_idle"] = "Assets/Textures/pinou_idle.png",
        }; 
    }
}
