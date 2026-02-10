using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project10.Sources.Entities;
using System;
using Tooling;

namespace Project10.Helpers
{
    public static class DrawHelper
    {
        public static void DrawCircle(BasicEffect effect, Vector2 center, float radius, int segments, Color color)
        {
            var vertices = CreateCircleVertices(center, radius, segments, color);
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                Game1.Instance.GraphicsDevice.DrawUserPrimitives(PrimitiveType.LineStrip, vertices, 0, segments);
            }
        }
        public static void DrawCircleLook(Vector2 center, Vector2 look, float radius, float min_line_length, float amount, Color color)
        {
            Vector2 dir = look == Vector2.Zero ? Vector2.UnitY : Vector2.Normalize(look);
            float line_length = min_line_length + look.Length() * amount;
            Vector2 start = center + dir * radius;
            float angle = (float)Math.Atan2(dir.Y, dir.X);
            int thickness = 2;
            Rectangle rect = new Rectangle(
                (int)start.X,
                (int)start.Y,
                (int)line_length,
                thickness
            );
            Game1.Instance._spriteBatch.Draw(
                Game1.Instance._pixel,
                rect,
                null,
                color,
                angle,
                new Vector2(0, thickness / 2f),
                SpriteEffects.None,
                0f
            );
        }

        public static VertexPositionColor[] CreateCircleVertices(Vector2 center, float radius, int segments, Color color)
        {
            var vertices = new VertexPositionColor[segments + 1];

            for (int i = 0; i <= segments; i++)
            {
                float angle = MathF.Tau * i / segments;

                vertices[i] = new VertexPositionColor(
                    new Vector3(
                        center.X + MathF.Cos(angle) * radius,
                        center.Y + MathF.Sin(angle) * radius,
                        0
                    ),
                    color);
            }

            return vertices;
        }

        public static Vector2 Forward(this Entity e) => e.Look == Vector2.Zero ? Vector2.UnitY : Vector2.Normalize(e.Look);
        public static Vector2 Right(this Entity e)
        {
            var forward = e.Forward();
            return new Vector2(forward.Y, -forward.X);
        }

    }
}
