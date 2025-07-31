using Microsoft.Xna.Framework;
using Project7.Source.Arcade.Common;
using Project7.Source.Arcade.scenes.space;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Tooling;
using static Project7.Source.Arcade.scenes.plateform.Enums;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Project7.Source.Arcade.scenes.plateform
{
    public class Map
    {
        int TSZ => ArcadePlateform.TSZ;
        int SC => ArcadePlateform.SCALE;
        int TSZSC => ArcadePlateform.TSZ_SCALED;
        ArcadePlateform scene => ArcadeMain.Scenes[ArcadeMain.instance.scene_index] as ArcadePlateform;

        public int w = 256;
        public int h = 64;
        public byte[][] Tiles;
        public Map()
        {
            Load();
        }
        public void New()
        {
            Tiles = new byte[w][];
            for (int x = 0; x < w; x++)
                Tiles[x] = new byte[h];
        }
        public void Load()
        {
            New();

            string filename = assets_bindings.Resources["arcade/plateform/files/map"];
            if (File.Exists(filename))
            {
                byte[] bytes = File.ReadAllBytes(filename);
                int index = 0;
                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        Tiles[x][y] = bytes[index++];
                    }
                }
            }
        }
        public void Save()
        {
            var SaveButton = (ArcadeMain.Scenes[ArcadeMain.instance.scene_index] as ArcadePlateform).SaveButton;

            SaveButton.BackColor = new Color(25,0,0);
            string filename = assets_bindings.Resources["arcade/plateform/files/map"];
            string directory = Path.GetDirectoryName(filename);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            using (BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.Create)))
            {
                for (int x = 0; x < w; x++)
                {
                    writer.Write(Tiles[x]);
                }
            }
            SaveButton.BackColor = Color.Black;
        }

        public bool Test(int x, int y) => x >= 0 && y >= 0 && x < w && y < h;
        public bool Set(int x, int y, int v)
        {
            if (Test(x, y))
            {
                Tiles[x][y] = (byte)v;
                return true;
            }
            return false;
        }
        public byte GetSafe(int x, int y) => Test(x, y) ? Tiles[x][y] : (byte)2;
        public Point GetSafePos(int x, int y) => Test(x, y) ? new Point(x, y) : Point.Zero;

        public List<Point> FindAllCharactersPositions()
        {
            var list = new List<Point>();
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    if (Tiles[x][y] == (byte)TexAssets.character)
                        list.Add(new Point(x, y));
                }
            }
            return list;
        }

        public object Collider(Entity obj, vecf offset = null, bool isJump = false)
        {
            offset ??= vecf.Zero;
            List<Point> corners_tiles = new();
            Point t;
            //if (!corners_tiles.Contains(t = GetSafePos((int)((obj.X + 0 - 64 + offset.x) / TSZSC), (int)((obj.Y + 0 - 64 + offset.y) / TSZSC)))) corners_tiles.Add(t);
            //if (!corners_tiles.Contains(t = GetSafePos((int)((obj.X + obj.W - 64 + offset.x) / TSZSC), (int)((obj.Y + 0 - 64 + offset.y) / TSZSC)))) corners_tiles.Add(t);
            //if (!isJump && !corners_tiles.Contains(t = GetSafePos((int)((obj.X + 0 - 64 + offset.x) / TSZSC), (int)((obj.Y + obj.H - 64 + offset.y) / TSZSC)))) corners_tiles.Add(t);
            //if (!isJump && !corners_tiles.Contains(t = GetSafePos((int)((obj.X + obj.W - 64 + offset.x) / TSZSC), (int)((obj.Y + obj.H - 64 + offset.y) / TSZSC)))) corners_tiles.Add(t);
            if (!corners_tiles.Contains(t = GetSafePos((int)((obj.X - obj.W / 2 - 64+32 + offset.x) / TSZSC), (int)((obj.Y - obj.H / 2 - 64 + offset.y) / TSZSC + 1)))) corners_tiles.Add(t);
            if (!corners_tiles.Contains(t = GetSafePos((int)((obj.X + obj.W / 2 - 64 + offset.x) / TSZSC), (int)((obj.Y - obj.H / 2 - 64 + offset.y) / TSZSC + 1)))) corners_tiles.Add(t);
            if (!isJump && !corners_tiles.Contains(t = GetSafePos((int)((obj.X - obj.W / 2 - 64 + offset.x) / TSZSC), (int)((obj.Y + obj.H / 2 - 64 + offset.y) / TSZSC + 1)))) corners_tiles.Add(t);
            if (!isJump && !corners_tiles.Contains(t = GetSafePos((int)((obj.X + obj.W / 2 - 64 + offset.x) / TSZSC), (int)((obj.Y + obj.H / 2 - 64 + offset.y) / TSZSC + 1)))) corners_tiles.Add(t);

            if (corners_tiles.All(ct => !Test((int)((obj.X - 64) / TSZSC), (int)((obj.Y - 64) / TSZSC))))
                return null;

            var rect = obj.Box.rectf;
            rect.Offset(offset.x, offset.y);

            // player

            var player = scene.Player;
            if (obj != player && player.Box.rectf.IntersectsWith(rect))
                return player;

            //// Doors

            //var c_door = Doors.Clone().FirstOrDefault(d => d.RealBounds.Intersects(rect));
            //if (c_door != null && obj != c_door)
            //    return c_door;

            //// Mobs

            //if (obj is not Samus)
            //{
            //    var c_mob = Mobs.Clone().FirstOrDefault(m => m.RealBounds.Intersects(rect));
            //    if (c_mob != null && obj != c_mob)
            //        return c_mob;
            //}

            //// Objects

            //if (obj is Harmful)
            //{
            //    var objs = PhysicalObjects.Clone();
            //    if (obj is PhysicalObject)
            //        objs = objs.Except(obj as PhysicalObject).ToList();
            //    var c_obj = objs.FirstOrDefault(o => o.RealBounds.Intersects(rect));
            //    if (c_obj != null)
            //        return c_obj;
            //}

            // Tiles

            //var c_tile = corners_tiles.FirstOrDefault(ct => ct.Type == XnaTooling.Enumerations.TYPE.SOLID);
            var c_tile = corners_tiles.FirstOrDefault(ct => Tiles[ct.X][ct.Y] != 0);
            if (c_tile != Point.Zero)
                return c_tile;

            return null;
        }
    }
}
