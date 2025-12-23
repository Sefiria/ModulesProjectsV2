using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project8.Editor;
using Project8.Source;
using Project8.Source.Map;
using Project8.Source.TiledMap;
using SharpDX.XAudio2;
using System;
using System.Drawing;
using System.Linq;
using Tooling;
using Tools;
using static System.Net.Mime.MediaTypeNames;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace Project8
{
    public partial class GameMain : Game
    {
        public float scale = 2F;
        public int tilesize = 16;
        Texture2D TexMissing;
        Texture2D[] TexGrass, TexFlowers;
        Texture2D TexWoodenFence, TexArcade;
        uint RNG = (uint)Random.Shared.NextInt64();
        public int screen_tiles_width => (int)(ScreenWidth / tilesize / scale);
        public int screen_tiles_height => (int)(ScreenHeight / tilesize / scale);

        void LoadDraw()
        {
            EditorManager.Init(GraphicsDevice);
        }

        void Draw_Tiles()
        {
            if(!EditorManager.TabMenu)
                for (int z = 0; z < Map.z; z++)
                    for (int y = 0; y < Map.h; y++)
                        for (int x = 0; x < Map.w; x++)
                            draw_tile(z, x, y);
        }
        void draw_tile(int z, int x, int y)
        {
            if (Map[z, x, y] < 0)
                return;

            Texture2D tex = null;
            int index = -2;
            DrawStyle drawstyle = DrawStyle.Static;
            int w = 16, h = 16;

            tex = GetTexByLayer(x, y, z);
            if (tex == null)
                return;
            var tile = Tile.Tiles[Map[z, x, y]];
            index = tile.Autotile?.Calculate(Map, z, x, y) ?? (tile.Mode == Tile.Modes.MultiTile ? tile .MultiTileIndex: 0);
            if (index == -2)
                draw(tex, x * tilesize * scale, y * tilesize * scale, z, w, h, drawstyle);
            else if (index > -1)
                draw(tex, x * tilesize * scale, y * tilesize * scale, z, w, h, drawstyle, index);
        }

        private Texture2D GetTexByLayer(int x, int y, int z)
        {
            int v = Map[z, x, y];

            if (Tile.Tiles.ContainsKey(v))
            {
                var texArr = Tile.Tiles[v].Tex;
                int length = texArr?.Length ?? 0;
                if (length > 1)
                {
                    int idx = CoordBlendIndex(x, y, length, seed: (v * 31) ^ (z * 17));
                    return texArr[idx];
                }
                return texArr != null && length > 0 ? texArr[0] : null;
            }
            else
            {
                // Missing Tex -> fallback sur 255
                if (Tile.Tiles.ContainsKey(255))
                {
                    var texArr = Tile.Tiles[255].Tex;
                    int length = texArr?.Length ?? 0;
                    if (length == 0) return null;

                    int stripe_tex_alternation = CoordBlendIndex(x, y, length, seed: (z * 101) ^ (x * 7) ^ (y * 13));
                    return texArr[stripe_tex_alternation];
                }
            }
            return null;
        }
        // --- Helpers ---
        static int CoordBlendIndex(int x, int y, int length, int seed = 0)
        {
            if (length <= 0) return 0;
            unchecked
            {
                int h = x * 73856093 ^ y * 19349663 ^ seed * 83492791;
                int alt = (int)Maths.Abs(h) % length;
                alt = (alt + (int)Maths.Abs(x + 2 * y)) % length;
                return alt;
            }
        }


        void Draw_Entities()
        {
            EntityManager.Draw(GraphicsDevice);
        }
        void Draw_Particles()
        {
            ParticleManager.Draw(GraphicsDevice);
        }
        void Draw_UI()
        {
            // Editor
            EditorManager.Draw();

            // Cursor
            Graphics.Graphics.Instance.DrawTexture(TexCursor, MS.Position.ToVector2() - new Vector2(16, 16));
        }


        public enum DrawStyle
        {
            Static=0, Framed, Wavy
        }
        void draw(Texture2D tex, float x, float y, int z, int w, int h, int eDrawStyle, int splitofst_x = 0, int splitofst_y = 0) => draw([tex], x, y, z, w, h, (DrawStyle)eDrawStyle, splitofst_x, splitofst_y);
        void draw(Texture2D tex, float x, float y, int z, int w, int h, DrawStyle style, int splitofst_x = 0, int splitofst_y = 0) => draw([tex], x, y, z, w, h, style, splitofst_x, splitofst_y);
        void draw(Texture2D[] tex, float x, float y, int z, int w, int h, DrawStyle style, int splitofst_x = 0, int splitofst_y = 0)
        {
            switch(style)
            {
                default:
                case DrawStyle.Static: draw_static(tex, x, y, z, w, h, splitofst_x, splitofst_y); break;
                case DrawStyle.Framed: draw_framed(tex, x, y, z, w, h); break;
                case DrawStyle.Wavy: draw_wavy(tex, x, y, w, h); break;
            }
        }
        int GetTexIdByTile(Texture2D[] tex, float x, float y)
        {
            if (tex.Length == 0)
                return -1;
            if (tex.Length == 1)
                return 0;
            return HashCoordinates(x, y) % tex.Length;
        }
        int HashCoordinates(float x, float y)
        {
            int xi = (int)Math.Floor(x);
            int yi = (int)Math.Floor(y);

            unchecked
            {
                // Mélange les coordonnées avec des constantes premières
                uint hash = (uint)(xi * 73856093) ^ (uint)(yi * 19349663);

                // Optionnel : un peu plus de mélange
                hash = (hash ^ (hash >> 13)) * RNG;
                hash = (hash ^ (hash >> 16));

                return (int)(hash & 0x7FFFFFFF); // Assure un entier positif
            }
        }
        void draw_static(Texture2D[] tex, float x, float y, int z, int w, int h, int splitofst_x = 0, int splitofst_y = 0)
        {
            int id = GetTexIdByTile(tex, x, y);
            if (id == -1) return;

            Graphics.Graphics.Instance.DrawTexture(
                tex[id],
                new Vector2(x, y),
                scale,
                new Rectangle(splitofst_x * w, splitofst_y * h, w, h),
                (EditorManager.IsPlaying || z == EditorManager.SelectedLayer) ? 1F : 0.5F
            );
        }
        void draw_framed(Texture2D[] tex, float x, float y, int z, int w, int h)
        {
            int id = GetTexIdByTile(tex, x, y);
            if (id == -1) return;

            int longism = 20;
            int count = tex[id].Width / w;
            int total = longism * count;
            int t = Enumerable.Range(1, count).FirstOrDefault(i => Ticks % total < longism * i) - 1;

            Graphics.Graphics.Instance.DrawTexture(
                tex[id],
                new Vector2(x, y),
                scale,
                new Rectangle(t * w, 0, w, h),
                !EditorManager.IsPlaying && z == EditorManager.SelectedLayer ? 0.5F : 1F
            );
        }
        void draw_wavy(Texture2D[] tex, float x, float y, int w, int h)
        {
            int id = GetTexIdByTile(tex, x, y);
            if (id == -1) return;

            int longism = 20;
            int count = tex[id].Width / w;
            int total = longism * count;
            Rectangle GetFrameRect(int frame) => new Rectangle(frame * w, 0, w, h);
            Vector2 position = new Vector2(x, y);
            float progress = ((Ticks + 12 * x + 6 * y) % total) / longism;
            int currentFrame = (int)Math.Floor(progress);
            int nextFrame = (currentFrame + 1 > count ? 0 : currentFrame + 1) % count;
            float alphaNext = progress - currentFrame;
            float alphaCurrent = 2f - alphaNext;

            Graphics.Graphics.Instance.DrawTexture(
                tex[id],
                position,
                scale,
                GetFrameRect(currentFrame),
                alphaCurrent
            );

            Graphics.Graphics.Instance.DrawTexture(
                tex[id],
                position,
                scale,
                GetFrameRect(nextFrame),
                alphaNext
            );
        }
    }
}