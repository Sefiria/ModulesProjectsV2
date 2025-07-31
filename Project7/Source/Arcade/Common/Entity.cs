using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Drawing;
using System.Linq;
using Tooling;

namespace Project7.Source.Arcade.Common
{
    public class Entity
    {
        public static ArcadeMain Context => ArcadeMain.instance;
        public static Graphics.Graphics graphics => Graphics.Graphics.Instance;

        public Guid ID;
        public string Name;
        public bool Exists;
        public float X, Y, A;
        public byte TexId;
        public float Scale = 2F;
        public bool Centered = true;

        public float W => Context.Textures[TexId]?.Width ?? 1;
        public float H => Context.Textures[TexId]?.Height ?? 1;
        public PointF PointF => (X, Y).P();
        public RectangleF Rectangle => new RectangleF(X - (Centered ? W / 2 : 0), Y - (Centered ? H / 2 : 0), W, H);
        public Box Box => new Box(X - (Centered ? W / 2 : 0), Y - (Centered ? H / 2 : 0), W, H);
        public PointF Forward => (X,Y).P().Add(A.AngleToPointF());
        public PointF Right => (X, Y).P().Add((A + 90F).AngleToPointF());

        public static Entity GetByID(Guid id) => Context.EntityManager.Entities.FirstOrDefault(e => e.ID == id);
        public static Entity GetByName(string name) => Context.EntityManager.Entities.FirstOrDefault(e => e.Name == name);


        public Entity(string name = null)
        {
            ID = Guid.NewGuid();
            Name = name;
            Exists = true;
            X = Y = 0F;
            A = -90F;
            Context.EntityManager.Entities.Add(this);
        }
        public virtual void Update()
        {
        }
        public virtual void Draw(GraphicsDevice graphics)
        {
            Graphics.Graphics.Instance.DrawTexture(Context.Textures[TexId], X, Y, A.ToRadians() + 90F.ToRadians(), Scale, false, 0F, Centered ? new Vector2(W / 2f, H / 2f) : null);
        }

        public virtual Collider GetCollider()
        {
            return new Collider(Collider.Kind.Box) { Box = new Box(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height) };
        }
        public virtual void OnCollision(Entity other) { }
        public virtual void OnEntityKilled(Entity killed_entity) { }
    }
}
