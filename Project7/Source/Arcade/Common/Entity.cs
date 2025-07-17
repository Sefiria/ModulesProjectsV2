using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project7.Source.Arcade.scenes.space;
using Project7.Source.Entities.Behaviors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Tooling;
using Tools;
using AnimationController = Tools.Animations.AnimationController;
using Color = Microsoft.Xna.Framework.Color;

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
        public float Velocity;
        public byte TexId;
        public float Scale = 2F;

        public float W => Context.Textures[TexId]?.Width ?? 1;
        public float H => Context.Textures[TexId]?.Height ?? 1;
        public PointF PointF => (X, Y).P();
        public RectangleF Rectangle => new RectangleF(X - W / 2, Y - H / 2, W, H);
        public Box Box => new Box(X - W / 2, Y - H / 2, W, H);
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
            Velocity = 0F;
            Context.EntityManager.Entities.Add(this);
        }
        public virtual void Update()
        {
            //Maths.CollisionPointBox(Game1.MS.X, Game1.MS.Y, Box)}
        }
        public virtual void Draw(GraphicsDevice graphics)
        {
            Graphics.Graphics.Instance.DrawTexture(Context.Textures[TexId], X, Y, A.ToRadians() + 90F.ToRadians(), Scale, false, 0F, new Vector2(W / 2f, H / 2f));
        }

        public virtual Collider GetCollider()
        {
            return new Collider(Collider.Kind.Box) { Box = new Box(Rectangle.X, Rectangle.Y, Rectangle.Width, Rectangle.Height) };
        }
        public virtual void OnCollision(Entity other) { }
        public virtual void OnEntityKilled(Entity killed_entity) { }
    }
}
