using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project8.Source.Entities.Behaviors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using Tooling;
using Tools;
using AnimationController = Tools.Animations.AnimationController;
using Color = Microsoft.Xna.Framework.Color;

namespace Project8.Source.Entities
{
    public class Entity
    {
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

        public Dictionary<object, object> UserData = new Dictionary<object, object>();

        public bool draw_from_center = true;
        public vecf displayed_vec => draw_from_center ? new vecf(X - W / 2F, Y - H / 2F) : new vecf(X, Y);
        public RectangleF DisplayedBounds => draw_from_center ? new RectangleF(X - W / 2F, Y - H / 2F, W, H) : GetTextureBounds();

        public bool Outlined { get; private set; } = false;

        private Texture2D CachedWhiteTex = null;

        public float W => GlobalVariables.tilesize;
        public float H => GlobalVariables.tilesize;
        public RectangleF GetTextureBounds() => new RectangleF(X, Y, W, H);
        public int TileX => (int)((X + W / 2f) / Context.scale / Context.tilesize);
        public int TileY => (int)((Y + H / 2f) / Context.scale / Context.tilesize);
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
            Animation?.Update();
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
                    tex,
                    X, Y,
                    ApplyRotationFromLook ? Maths.GetAngle(new PointF(LookX, LookY), false) + MathF.PI / 2F : 0F,
                    scale * 2F,
                    LookX < 0F, 0F,
                    ApplyRotationFromLook ? new Vector2(W / 2f, H / 2f) : null,
                    null,
                    Animation?.Get() ?? new Microsoft.Xna.Framework.Rectangle(0, 0, (int)(GlobalVariables.tilesize * scale), (int)(GlobalVariables.tilesize * scale)));
            }
        }
    }
}
