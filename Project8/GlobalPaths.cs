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
        public static readonly string Data = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Directory.GetCurrentDirectory()+"/Assets/Data/");
        public static readonly string DataTilesetJson = Path.Combine(Data + "tileset.json");
        public static readonly string DataEntitiesJson = Path.Combine(Data + "entities.json");

        public static readonly string Maps = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Directory.GetCurrentDirectory()+"/Assets/Maps/");

        public static readonly string Textures = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Directory.GetCurrentDirectory()+"/Assets/Textures/");
        public static readonly string Tilesets = Path.Combine(Textures, "Tilesets");
        public static readonly string Entities = Path.Combine(Textures, "Entities");
        public static readonly string Animations = Path.Combine(Textures, "Animations");

        public static readonly string UI = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Directory.GetCurrentDirectory()+"/Assets/UI/");
    }
}
