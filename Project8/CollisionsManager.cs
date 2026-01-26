using Project8.Source;
using Project8.Source.Entities;
using Project8.Source.Map;
using Project8.Source.Runtime;
using Project8.Source.TiledMap;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Linq;
using Tooling;

namespace Project8
{
    public class CollisionsManager
    {
        static float TSZ => GlobalVariables.tilesize * GlobalVariables.scale;
        static TiledMap map => GameMain.Instance.Map;

        public static void Detect(Entity obj, Entity[] others)
        {
            if (obj == null || others.Count() == 0) return;
            Entity collider = others.Except(obj).FirstOrDefault(o => o.GetTextureBounds().IntersectsWith(obj.GetTextureBounds()));
            if (collider != null)
            {
                //if (collider is Mob && !(collider as Mob).HasBehavior<Passive>())
                //    (obj as Hittable)?.Hit(6);
                //if (collider is Harmful)
                //    (obj as Hittable)?.Hit((collider as Harmful).DMG);
                //if (obj is IInventory)
                //    (collider as Collectible)?.Collect(obj as IInventory);
                if(collider.CanCollect && obj is Collectible)
                {
                    collider.Collect(obj as Collectible);
                    (obj as Collectible).Collected();
                }
            }
        }

        public static object Collider(Entity obj, vecf offset = null, bool isJump = false)
        {
            offset ??= vecf.Zero;
            List<Tile> corners_tiles = new();
            Tile t;
            int w = (int)(obj.W * GlobalVariables.scale);
            int h = (int)(obj.H * GlobalVariables.scale);
            if (!corners_tiles.Contains(t = Tile.GetTile(map[0, (int)((obj.X + offset.x) / TSZ), (int)((obj.Y + offset.y) / TSZ)]))) corners_tiles.Add(t);
            if (!corners_tiles.Contains(t = Tile.GetTile(map[0, (int)((obj.X + w + offset.x) / TSZ), (int)((obj.Y + offset.y) / TSZ)]))) corners_tiles.Add(t);
            if (!isJump && !corners_tiles.Contains(t = Tile.GetTile(map[0, (int)((obj.X + offset.x) / TSZ), (int)((obj.Y + h + offset.y - 1) / TSZ)]))) corners_tiles.Add(t);
            if (!isJump && !corners_tiles.Contains(t = Tile.GetTile(map[0, (int)((obj.X + w + offset.x) / TSZ), (int)((obj.Y + h + offset.y - 1) / TSZ)]))) corners_tiles.Add(t);

            if (corners_tiles.All(ct => map.isout(new vec((int)((obj.displayed_vec.x + 0) / TSZ), (int)((obj.displayed_vec.y + 0) / TSZ)).f)))
                return null;

            var rect = new RectangleF(obj.DisplayedBounds.X, obj.DisplayedBounds.Y, obj.DisplayedBounds.Width, obj.DisplayedBounds.Height);
            rect.Offset(offset.x, offset.y);

            // Samus

            //var samus = GameMain.Instance.EntityManager.Entities.FirstOrDefault(e => e is Entitytest);
            //if (obj != samus && (samus?.DisplayedBounds.IntersectsWith(rect) ?? false))
            //    return samus;

            //// Doors

            //var c_door = DB.Room.Doors.Clone().FirstOrDefault(d => d.DisplayedBounds.IntersectsWith(rect));
            //if (c_door != null && obj != c_door)
            //    return c_door;

            //// Mobs

            //var c_mobs = DB.Room.Mobs.Clone().Where(m => m.DisplayedBounds.IntersectsWith(rect));
            //foreach (var c_mob in c_mobs)
            //    if (c_mob != null && obj != c_mob)
            //        return c_mob;

            //// Objects

            //if (obj is Harmful)
            //{
            //    var objs = DB.Room.PhysicalObjects.Clone();
            //    if (obj is PhysicalObject)
            //        objs = objs.Except(obj as PhysicalObject).ToList();
            //    if (obj is Bullet)
            //        objs = objs.Where(o => o is not Collectible).ToList();
            //    var c_obj = objs.FirstOrDefault(o => o.DisplayedBounds.IntersectsWith(rect));
            //    if (c_obj != null)
            //        return c_obj;
            //}

            // Tiles

            var c_tile = corners_tiles.FirstOrDefault(ct => ct.IsSolid);
            if (c_tile != null)
                return c_tile;

            if (offset.x > 0 ? map.isout(new vecf((int)Math.Round((obj.X + offset.x) / TSZ), (int)Math.Round((obj.Y + offset.y) / TSZ))) : map.isout(new vecf((int)Math.Floor((obj.X + offset.x) / TSZ), (int)Math.Floor((obj.Y + offset.y) / TSZ))))
                return new();

            return null;
        }
    }
}
