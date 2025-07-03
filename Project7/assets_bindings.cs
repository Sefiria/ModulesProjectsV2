using GeonBit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project7
{
    internal class assets_bindings
    {
        public static readonly Dictionary<string, string> Resources = new Dictionary<string, string>()
        {
            ["tilesets/grass"] = "Assets/Textures/Tilesets/grass.png",
            ["tilesets/flowers"] = "Assets/Textures/Tilesets/flowers.png",
            ["tilesets/arcade"] = "Assets/Textures/Tilesets/arcade.png",
            ["cursor"] = "Assets/Textures/UI/cursor.png",
            ["pinou_idle"] = "Assets/Textures/pinou_idle.png",
            ["pinou_run"] = "Assets/Textures/pinou_run.png",
            ["pinou_hold"] = "Assets/Textures/pinou_hold.png",
            ["wooden_fence"] = "Assets/Textures/wooden_fence.png",
            ["ei_heart"] = "Assets/Textures/EI/heart.png",
            ["fly_idle"] = "Assets/Textures/fly_idle.png",
            ["fly_flying"] = "Assets/Textures/fly_flying.png",
        };
    }
}
