using GeonBit.UI;
using GeonBit.UI.Entities;
using GeonBit.UI.Utils;
using Microsoft.VisualBasic.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using Tools;

namespace Project7
{
    public partial class Game1 : Game
    {
        float scale = 2F;
        int tilesize = 16;
        Texture2D[] TexGrass;
        uint RNG = (uint)Random.Shared.NextInt64();

        void LoadDraw()
        {
            TexGrass = GraphicsDevice.SplitTexture("Assets/Textures/Tilesets/grass.png", 0, 16);
        }

        void Draw()
        {
            for (int y = 0; y < 32; y++)
                for (int x = 0; x < 32; x++)
                    draw(TexGrass, x * tilesize * scale, y * tilesize * scale, 16, 16, DrawStyle.Wavy);

            EntityManager.Draw(GraphicsDevice);
        }

        public enum DrawStyle
        {
            Static=0, Framed, Wavy
        }
        void draw(Texture2D tex, float x, float y, int w, int h, int eDrawStyle) => draw([tex], x, y, w, h, (DrawStyle)eDrawStyle);
        void draw(Texture2D tex, float x, float y, int w, int h, DrawStyle style) => draw([tex], x, y, w, h, style);
        void draw(Texture2D[] tex, float x, float y, int w, int h, DrawStyle style)
        {
            switch(style)
            {
                default:
                case DrawStyle.Static: draw_static(tex, x, y, w, h); break;
                case DrawStyle.Framed: draw_framed(tex, x, y, w, h); break;
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
        void draw_static(Texture2D[] tex, float x, float y, int w, int h)
        {
            int id = GetTexIdByTile(tex, x, y);
            if (id == -1) return;

            Graphics.Graphics.Instance.DrawTexture(
                tex[id],
                x,
                y,
                scale,
                new Rectangle(0, 0, w, h)
            );
        }
        void draw_framed(Texture2D[] tex, float x, float y, int w, int h)
        {
            int id = GetTexIdByTile(tex, x, y);
            if (id == -1) return;

            int longism = 20;
            int count = tex[id].Width / w;
            int total = longism * count;
            int t = Enumerable.Range(1, count).FirstOrDefault(i => Ticks % total < longism * i) - 1;

            Graphics.Graphics.Instance.DrawTexture(
                tex[id],
                x,
                y,
                scale,
                new Rectangle(t * w, 0, w, h)
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