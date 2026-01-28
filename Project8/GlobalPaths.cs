using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project8
{
    public class GlobalPaths
    {
        public static string Data => Directory.GetCurrentDirectory() + @"\Assets\Data\";
        public static string DataTilesetJson => Path.Combine(Data + "tileset.json");
        public static string DataEntitiesJson => Path.Combine(Data + "entities.json");

        public static string Maps => Directory.GetCurrentDirectory() + @"\Assets\Maps\";

        public static string Textures => Directory.GetCurrentDirectory() + @"\Assets\Textures\";
        public static string Tilesets => Path.Combine(Textures, "Tilesets");
        public static string Entities => Path.Combine(Textures, "Entities");
        public static string Animations => Path.Combine(Textures, "Animations");

        public static string UI => Directory.GetCurrentDirectory() + @"\Assets\UI\";
    }
}
