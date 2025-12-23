using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Tools;

namespace Project8
{
    public class ResourcesLoader
    {
        public static List<Texture2D> pinou_idle, pinou_run, pinou_hold, tilesets_arcade, fly_idle, fly_flying, ei_heart;

        public static void Load(GraphicsDevice graphics)
        {
            //pinou_idle = graphics.SplitTexturePerCount(assets_bindings.Resources["pinou_idle"], 4, 1).ToList();
        }
    }
}
