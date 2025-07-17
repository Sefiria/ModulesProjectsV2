using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Tools;

namespace Project7
{
    public class ResourcesLoader
    {
        public static List<Texture2D> pinou_idle, pinou_run, pinou_hold, tilesets_arcade, fly_idle, fly_flying, ei_heart;

        public static void Load(GraphicsDevice graphics)
        {
            pinou_idle = graphics.SplitTexturePerCount(assets_bindings.Resources["pinou_idle"], 4, 1).ToList();
            pinou_run = graphics.SplitTexturePerCount(assets_bindings.Resources["pinou_run"], 4, 1).ToList();
            pinou_hold = graphics.SplitTexturePerCount(assets_bindings.Resources["pinou_hold"], 4, 1).ToList();
            tilesets_arcade = graphics.SplitTexturePerCount(assets_bindings.Resources["tilesets/arcade"], 4, 1).ToList();
            fly_idle = graphics.SplitTexturePerCount(assets_bindings.Resources["fly_idle"], 1, 1).ToList();
            fly_flying = graphics.SplitTexturePerCount(assets_bindings.Resources["fly_flying"], 4, 1).ToList();
            ei_heart = graphics.SplitTexturePerCount(assets_bindings.Resources["ei_heart"], 10, 1).ToList();
        }
    }
}
