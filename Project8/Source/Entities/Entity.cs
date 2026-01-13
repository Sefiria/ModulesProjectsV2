using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project8.Source.Entities.Behaviors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Tooling;
using Tools;
using Color = Microsoft.Xna.Framework.Color;

namespace Project8.Source.Entities
{
    public class Entity
    {
        public enum Alignments { center, bottom, top, left, right }

        GameMain Context => GameMain.Instance;

        public Guid ID;
        public string Name;
        public bool Exists;
        public Animation2D Animation;
        public Texture2D Texture = null;
        public List<Behavior> Behaviors;
        public float X, Y, scale = 1F;
        public float LookX, LookY, Velocity;
        public bool HasCollisions = true, ApplyRotationFromLook = false;
        public bool OutlineWhenHover = false, ForceOutline = false;
        public Alignments Alignment = Alignments.center;

        public Dictionary<object, object> UserData = new Dictionary<object, object>();

        public vecf displayed_vec => Alignment switch

        {
            Alignments.center => new vecf(X - W / 2F, Y - H / 2F),
            Alignments.bottom => new vecf(X - W / 2F, Y - H),
            Alignments.top => new vecf(X - W / 2F, Y + H),
            Alignments.left => new vecf(X + W, Y - H / 2F),
            Alignments.right => new vecf(X - W, Y - H / 2F),
            _ => new vecf(X, Y)
        };
        public RectangleF DisplayedBounds => new RectangleF(X - W / 2F, Y - H / 2, W, H);

        public bool Outlined { get; private set; } = false;

        private Texture2D CachedWhiteTex = null;

        public float W => GlobalVariables.tilesize;
        public float H => GlobalVariables.tilesize;
        public RectangleF GetTextureBounds() => new RectangleF(X, Y, W, H);
        public int TileX => (int)((X + W / 2f) / Context.scale / Context.tilesize);
        public int TileY => (int)((Y + H / 2f) / Context.scale / Context.tilesize);
        public int GetTileX(int offset_in_pixels) => (int)((X + W / 2f + offset_in_pixels) / Context.scale / Context.tilesize);
        public int GetTileY(int offset_in_pixels) => (int)((Y + H / 2f + offset_in_pixels) / Context.scale / Context.tilesize);

        public static Entity GetByID(Guid id) => GameMain.Instance.EntityManager.Entities.FirstOrDefault(e => e.ID == id);
        public static Entity GetByName(string name) => GameMain.Instance.EntityManager.Entities.FirstOrDefault(e => e.Name == name);


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
            Behaviors.ForEach(b => b.Update());

            Outlined = ForceOutline || (OutlineWhenHover && Maths.CollisionPointBox(GameMain.MS.X, GameMain.MS.Y, new Box(X, Y, W, H)));

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
            int x = (int)((_x + delta_x + ofst_x) / GameMain.Instance.scale / GameMain.Instance.tilesize);
            int y = (int)((_y + delta_y + ofst_y) / GameMain.Instance.scale / GameMain.Instance.tilesize);
            if (x - 1 < 0) return true;
            if (y - 1 < 0) return true;
            if (x >= GameMain.Instance.screen_tiles_width) return true;
            if (y >= GameMain.Instance.screen_tiles_height) return true;
            return false;
        }
        /// <summary>
        /// Already tIled x, y
        /// </summary>
        private bool CheckMapTilesCollisions(int x, int y) => Context.Map[1, x, y] != -1;

        Vector2 vec_center => new Vector2(W / 2f, H / 2f);
        Vector2 vec_bottom => new Vector2(W / 2f, H);
        public void Draw(GraphicsDevice graphics)
        {
            Texture2D tex = Texture ?? Animation?.Texture;
            if (tex != null)
            {
                if (Outlined)
                {
                    if(CachedWhiteTex == null)
                        CachedWhiteTex = graphics.CloneAsWhite(tex);
                    float thin = 0.01f;
                    Context.spriteBatch.Draw(CachedWhiteTex, new Vector2(X - W * scale * thin, Y - H * scale * thin), Animation.Get(), Color.White, rotation: 0f, Vector2.Zero, scale * (1F + thin * 4F), SpriteEffects.None, 0f);
                }
                Graphics.Graphics.Instance.DrawTexture(
                    texture:    tex,
                                X, Y,
                    rotation:   ApplyRotationFromLook ? Maths.GetAngle(new PointF(LookX, LookY), false) + MathF.PI / 2F : 0F,
                    scale:      scale * 2F,
                    flipX:      LookX < 0F,
                    depth:      0F,
                    origin:     ApplyRotationFromLook ? new Vector2(W / 2f, H / 2f) : null,
                    color:      null,
                    source:     Animation?.Get() ?? new Microsoft.Xna.Framework.Rectangle(0, 0, (int)(GlobalVariables.tilesize * scale), (int)(GlobalVariables.tilesize * scale)));
            }
        }
    }
}
