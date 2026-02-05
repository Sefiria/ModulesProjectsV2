using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project10.Helpers
{
    public class DrawHelper
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
    }
}
