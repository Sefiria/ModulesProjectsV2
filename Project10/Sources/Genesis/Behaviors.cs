using Microsoft.Xna.Framework;
using Project10.Helpers;
using Project10.Sources.Domain;
using Project10.Sources.Entities;
using System;
using Tooling;

namespace Project10.Sources.Genesis
{
    public static class Behaviors
    {
        public static ExperienceResult Move(this Entity e, Vector2? targetDir = null)
        {
            float maxSpeed = 5F;

            if (targetDir != null)
            {
                Vector2 dir = targetDir == Vector2.Zero ? Vector2.Zero : Vector2.Normalize(targetDir.Value);
                float forceFactor = 0.001f;
                float dot = Vector2.Dot(
                    e.Look == Vector2.Zero ? dir : Vector2.Normalize(e.Look),
                    dir
                );
                if (dot < 0.9f)
                    e.Look *= 0.95f;
                e.Look += targetDir.Value * forceFactor;
            }
            else
            {
                e.Look *= 0.85f;
            }

            var m = Maths.Sqrt(e.Look.X * e.Look.X + e.Look.Y * e.Look.Y);
            if (m > maxSpeed) { var k = maxSpeed / m; e.Look.X *= k; e.Look.Y *= k; }

            bool collided = e.MoveAndCheckCollisions();
            return new ExperienceResult(collided: collided);
        }
    }
}
