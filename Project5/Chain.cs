using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Tooling;
using Tools.Inputs;

namespace Project5
{
    public class ChainLink
    {
        public Vector2 Position;
        public float Length;

        public ChainLink(Vector2 position, float length)
        {
            Position = position;
            Length = length;
        }
    }

    public class Chain
    {
        public float point_size = 10;
        public Vector2 A;
        public Vector2 B;
        public List<ChainLink> Links;
        public float LinkSpacing, ChainLength, LinkLength;
        public Vector2 Gravity;

        public Chain(Vector2 A, Vector2 B, float totalLength, float linkLength, float linkSpacing, Vector2 gravity)
        {
            this.A = A;
            this.B = B;
            LinkSpacing = linkSpacing;
            ChainLength = totalLength;
            LinkLength = linkLength;
            Gravity = gravity;
            Links = new List<ChainLink>();

            // Calculer le nombre maximal de maillons
            int linkCount = (int)(totalLength / (linkLength + linkSpacing));

            // Initialiser les maillons de la chaîne
            for (int i = 0; i < linkCount; i++)
            {
                Vector2 position = this.A + new Vector2(i * (linkLength + linkSpacing), 0);
                Links.Add(new ChainLink(position, linkLength));
            }
        }

        public void Update(GameTime gameTime)
        {
            B += Gravity * 2F * (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            Vector2 direction = B - A;
            float distance = direction.Length();
            if (distance > ChainLength - LinkLength)
            {
                direction.Normalize();
                B = A + direction * (ChainLength - LinkLength);
            }

            // Mettre à jour le premier maillon avec la position de la souris
            Links[Links.Count-1].Position = B;

            // Appliquer les contraintes pour les autres maillons
            for (int i = Links.Count - 2; i > 0; i--)
            {
                Vector2 dir = Links[i].Position - Links[i + 1].Position;
                float dist = dir.Length();
                float targetDistance = Links[i].Length + LinkSpacing;
                if (dist > targetDistance)
                {
                    dir.Normalize();
                    Links[i].Position = Links[i + 1].Position + dir * targetDistance;
                }
            }

            // Appliquer la gravité à chaque maillon
            for (int i = 0; i < Links.Count; i++)
            {
                Links[i].Position += Gravity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }

            // Assurer que le premier maillon est toujours à pointA
            Links[0].Position = A;

            // Mettre à jour les autres maillons en partant du pointA
            for (int i = 1; i < Links.Count; i++)
            {
                Vector2 dir = Links[i].Position - Links[i - 1].Position;
                float dist = dir.Length();
                float targetDistance = Links[i].Length + LinkSpacing;
                if (dist > targetDistance)
                {
                    dir.Normalize();
                    Links[i].Position = Links[i - 1].Position + dir * targetDistance;
                }
            }
        }

        internal void Draw(GameTime gameTime)
        {
            // point A
            Graphics.Graphics.Instance.FillCircle((int)A.X, (int)A.Y, (int)point_size, Color.DimGray);
            Graphics.Graphics.Instance.DrawCircle((int)A.X, (int)A.Y, (int)point_size, Color.White);

            // point B
            Graphics.Graphics.Instance.FillCircle((int)B.X, (int)B.Y, (int)point_size, Color.Gray);

            // chaine
            for (int i = 1; i < Links.Count; i++)
            {
                Vector2 start = Links[i - 1].Position + new Vector2(Links[i - 1].Length / 2, point_size / 2) - new Vector2(point_size, point_size / 2);
                Vector2 end = Links[i].Position + new Vector2(Links[i].Length / 2, point_size / 2) - new Vector2(point_size, point_size / 2);
                Graphics.Graphics.Instance.DrawLine((int)start.X, (int)start.Y, (int)end.X, (int)end.Y, Color.White, 1);
                Rectangle rect = new Rectangle((int)start.X, (int)start.Y, (int)Links[i].Length, (int)point_size);
                Graphics.Graphics.Instance.FillRectangle(rect, Color.Gray, ICoordsFactory.Create(start.X, start.Y).GetAngleWith(ICoordsFactory.Create(end.X, end.Y)).ToRadians());
            }
        }
    }
}
