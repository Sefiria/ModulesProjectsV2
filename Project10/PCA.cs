using Microsoft.Xna.Framework;
using Project10.Helpers;
using System;
using Tooling;

namespace Project10
{
    public class PCA : Entity
    {
        public PCA()
        {
            Position = new Vector2(MAP.Width * Game1.Instance._cellSize / 2, MAP.Height * Game1.Instance._cellSize / 2);
            Radius = Game1.Instance._cellSize / 2;
            Look = new Vector2(0F, 0F);
        }

        public void Update(GameTime gameTime)
        {
            var ms = Game1.MS.Position.ToVector2();
            Vector2 targetDir = ms - RenderPosition;

            if (Game1.MS.IsLeftDown)
            {
                Vector2 dir = targetDir == Vector2.Zero ? Vector2.Zero : Vector2.Normalize(targetDir);
                float forceFactor = 0.001f;

                float dot = Vector2.Dot(
                    Look == Vector2.Zero ? dir : Vector2.Normalize(Look),
                    dir
                );

                if (dot < 0.5f)
                    Look *= 0.85f;

                Look += targetDir * forceFactor;
            }
            else
            {
                Look *= 0.50f;
            }

            Collisions();
        }

        public void Collisions()
        {
            float r = Radius;
            int cs = Game1.Instance._cellSize;

            Vector2 center = RenderPosition;

            // déplacement proposé
            float dx = Look.X;
            float dy = Look.Y;

            // --- déplacement X ---
            if (dx != 0f)
            {
                Vector2 nextCenterX = center + new Vector2(dx, 0);
                if (!CollidesCircle(nextCenterX, r))
                {
                    Position = new Vector2(Position.X + dx, Position.Y);
                    center = RenderPosition;
                }
                else
                {
                    Look.X = 0f;
                }
            }

            // --- déplacement Y ---
            if (dy != 0f)
            {
                Vector2 nextCenterY = center + new Vector2(0, dy);
                if (!CollidesCircle(nextCenterY, r))
                {
                    Position = new Vector2(Position.X, Position.Y + dy);
                }
                else
                {
                    Look.Y = 0f;
                }
            }
        }
        private bool CollidesCircle(Vector2 center, float r)
        {
            int cs = Game1.Instance._cellSize;

            Vector2[] pts =
            {
                center + new Vector2( r, 0),
                center + new Vector2(-r, 0),
                center + new Vector2(0,  r),
                center + new Vector2(0, -r)
            };

            foreach (var p in pts)
            {
                Point c = new Point((int)(p.X / cs), (int)(p.Y / cs));
                if (Game1.Instance._map.GetCell(c) != CellType.Floor)
                    return true;
            }

            return false;
        }


        public void SpriteDraw(GameTime gameTime)
        {
            DrawHelper.DrawCircleLook(RenderPosition, Look, Radius, 4F, 2F, Color.Cyan);
        }
        public void ShaderDraw(GameTime gameTime)
        {
            DrawHelper.DrawCircle(Game1.Instance.basicEffect, RenderPosition, Radius, 8, Color.Cyan);
        }
    }
}
