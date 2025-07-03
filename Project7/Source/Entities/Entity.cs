using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project7.Source.Entities.Behaviors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Tooling;
using Tools;
using AnimationController = Tools.Animations.AnimationController;
using Color = Microsoft.Xna.Framework.Color;

namespace Project7.Source.Entities
{
    public class Entity
    {
        Game1 Context => Game1.Instance;

        public Guid ID;
        public string Name;
        public bool Exists;
        public AnimationController AnimationController;
        public List<Behavior> Behaviors;
        public float X, Y, scale = 1F;
        public float LookX, LookY, Velocity;
        public bool HasCollisions = true, ApplyRotationFromLook = false;
        public bool OutlineWhenHover = false, ForceOutline = false;

        private bool Outlined = false;
        private Texture2D CachedWhiteTex = null;

        public float W => AnimationController?.GetCurrentFrame()?.Width * scale ?? 0F;
        public float H => AnimationController?.GetCurrentFrame()?.Height * scale ?? 0F;
        public RectangleF GetTextureBounds() => new RectangleF(X, Y, W, H);
        public int TileX => (int)((X + W / 2f) / Context.scale / Context.tilesize);
        public int TileY => (int)((Y + H / 2f) / Context.scale / Context.tilesize);
        public static Entity GetByID(Guid id) => Game1.Instance.EntityManager.Entities.FirstOrDefault(e => e.ID == id);
        public static Entity GetByName(string name) => Game1.Instance.EntityManager.Entities.FirstOrDefault(e => e.Name == name);


        public Entity(string name = null)
        {
            ID = Guid.NewGuid();
            Name = name;
            Exists = true;
            Behaviors = new List<Behavior>();
            LookX = 1F;
            LookY = 0F;
            Velocity = 0F;
            Context.EntityManager.Entities.Add(this);
        }
        public void Update()
        {
            AnimationController?.Update();
            Behaviors.ForEach(b => b.Update());

            Outlined = ForceOutline || (OutlineWhenHover && Maths.CollisionPointBox(Game1.MS.X, Game1.MS.Y, new Box(X, Y, W, H)));

            var vn = Maths.Normalized(new PointF(LookX, LookY));
            float delta_x = vn.X * Velocity;
            float delta_y = vn.Y * Velocity;

            if (delta_x == 0f && delta_y == 0f)
                return;

            if (HasCollisions == false)
            {
                X += delta_x;
                Y += delta_y;
            }
            else
            {
                var (tileX_X, tileY_X) = GetTileAtOffset(delta_x, 0);
                if (!CheckMapTilesCollisions(tileX_X, tileY_X) && !CheckMapBounds(X, Y, delta_x, 0))
                {
                    X += delta_x;
                }
                var (tileX_Y, tileY_Y) = GetTileAtOffset(0, delta_y);
                if (!CheckMapTilesCollisions(tileX_Y, tileY_Y) && !CheckMapBounds(X, Y, 0, delta_y))
                {
                    Y += delta_y;
                }
            }
        }
        public (int TileX, int TileY) GetTileAtOffset(float dx, float dy)
        {
            float centerX = X + W / 2f + dx;
            float centerY = Y + H / 2f + dy;

            int tileX = (int)(centerX / Context.scale / Context.tilesize);
            int tileY = (int)(centerY / Context.scale / Context.tilesize);

            return (tileX, tileY);
        }
        private bool CheckMapBounds(float _x, float _y, float delta_x, float delta_y)
        {
            if (delta_x == 0F && delta_y == 0F)
                return false;
            _x += W / 2;
            _y += H / 2;
            int ofst_x = (int)(Maths.Sign(delta_x) * W / 4);
            int ofst_y = (int)(Maths.Sign(delta_y) * H / 4);
            int x = (int)((_x + delta_x + ofst_x) / Game1.Instance.scale / Game1.Instance.tilesize);
            int y = (int)((_y + delta_y + ofst_y) / Game1.Instance.scale / Game1.Instance.tilesize);
            if (x - 1 < 0) return true;
            if (y - 1 < 0) return true;
            if (x >= Game1.Instance.screen_tiles_width) return true;
            if (y >= Game1.Instance.screen_tiles_height) return true;
            return false;
        }
        /// <summary>
        /// Already tIled x, y
        /// </summary>
        private bool CheckMapTilesCollisions(int x, int y) => Context.Map[1, x, y] != -1;

        public void Draw(GraphicsDevice graphics)
        {
            Texture2D tex = AnimationController?.GetCurrentFrame();
            if (tex != null)
            {
                if (Outlined)
                {
                    if(CachedWhiteTex == null)
                        CachedWhiteTex = graphics.CloneAsWhite(tex);
                    float thin = 0.01f;
                    Context.spriteBatch.Draw(CachedWhiteTex, new Vector2(X - W * scale * thin, Y - H * scale * thin), tex.Bounds, Color.White, rotation: 0f, Vector2.Zero, scale * (1F + thin * 4F), SpriteEffects.None, 0f);
                }
                Graphics.Graphics.Instance.DrawTexture(tex, X, Y, ApplyRotationFromLook ? Maths.GetAngle(new PointF(LookX, LookY), false) + MathF.PI / 2F : 0F, scale, LookX < 0F, 0F, ApplyRotationFromLook ? new Vector2(W / 2f, H / 2f) : null);
            }
        }
    }
}
