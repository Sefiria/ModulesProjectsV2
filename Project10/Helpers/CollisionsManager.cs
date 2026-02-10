using Microsoft.Xna.Framework;
using Project10.Sources.Entities;
using System;
using Tooling;
using static System.Net.WebRequestMethods;

namespace Project10.Helpers
{
    public static class CollisionsManager
    {
        static int csz => Game1.Instance._cellSize;

        public static bool MoveAndCheckCollisions(this Entity e)
        {
            bool collided = false;
            float r = e.Radius * 1F;

            Vector2 center = e.RenderPosition;
            Point? hitpt;

            // target look
            float dx = e.Look.X;
            float dy = e.Look.Y;

            // --- X ---
            if (dx != 0f)
            {
                Vector2 nextCenterX = center + new Vector2(dx, 0);
                if ((hitpt = CollidesCircle(nextCenterX, r)) == null)
                {
                    e.Position = new Vector2(e.Position.X + dx, e.Position.Y);
                }
                else
                {
                    e.Look.X = 0F;
                    var look = new Vector2(hitpt.Value.X * csz - e.Position.X, 0);
                    look.Normalize();
                    e.Position -= look;
                    collided = true;
                }
            }
            center = e.RenderPosition;

            // --- Y ---
            if (dy != 0f)
            {
                Vector2 nextCenterY = center + new Vector2(0, dy);
                if ((hitpt = CollidesCircle(nextCenterY, r)) == null)
                {
                    e.Position = new Vector2(e.Position.X, e.Position.Y + dy);
                }
                else
                {
                    e.Look.Y = 0F;
                    var look = new Vector2(0, hitpt.Value.Y * csz - e.Position.Y);
                    look.Normalize();
                    e.Position -= look;
                    collided = true;
                }
            }

            return collided;
        }
        private static Point? CollidesCircle(Vector2 center, float r)
        {
            // 9 cells around (because corners may collide too)
            Vector2[] pts =
            {
                center + new Vector2( r, 0),
                center + new Vector2(-r, 0),
                center + new Vector2(0,  r),
                center + new Vector2(0, -r),
                center + new Vector2(-r, -r),
                center + new Vector2(r, -r),
                center + new Vector2(-r, r),
                center + new Vector2(r, r)
            };
            foreach (var p in pts)
            {
                Point c = new Point((int)(p.X / csz), (int)(p.Y / csz));
                if (Game1.Instance._map.GetCell(c) != CellType.Floor)
                    return c;

            }
            return null;
        }
    }
}
