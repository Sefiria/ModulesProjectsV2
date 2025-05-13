using LibNoise.Primitive;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Tooling;

namespace Project3
{
    public partial class Game1 : Game
    {
        public const int mapw = 48, maph = 32;
        public int tilecount;
        public byte[] map;
        public byte[] map_blocs_durability;

        private void InitDraw()
        {
            tex_tilemap = Texture2D.FromFile(GraphicsDevice, "tilemap_default.png");
            tilecount = tex_tilemap.Width / tilesize;
            DefineMap();
        }

        private void DefineMap()
        {
            var perlin = new ImprovedPerlin((int)(Ticks % int.MaxValue), LibNoise.NoiseQuality.Best);
            map = new byte[mapw * maph];
            map_blocs_durability = new byte[tilecount];
            float min = 0.33F;
            float max = 1F;
            for (int k = 0; k < tilecount; k++)
                map_blocs_durability[tilecount - 1 - k] = (byte)(k + Math.Min(byte.MaxValue, k * k * 0.5F));
            for (int j = 0; j < maph; j++)
            {
                for (int i = 0; i < mapw; i++)
                {
                    double noiseValue = perlin.GetValue(i * 0.15F, j * 0.15F, 0F);
                    double normalizedValue = ((noiseValue + 1) / 2) * (max - min) + min;
                    byte value = (byte)(normalizedValue * tilecount);
                    map[j * mapw + i] = value;
                }
            }
        }

        private void Draw()
        {
            //Graphics.Graphics.Instance.DrawString("test", 50, 50, Resources.Instance.Fonts[0], Color.White);
            for (int j = 0; j < maph; j++)
                for (int i = 0; i < mapw; i++)
                    Graphics.Graphics.Instance.DrawTexture(tex_tilemap, screenWidth / 2 - mapw * scaleFactor / 2 + i * scaleFactor + RandomThings.rnd1Around0()/2f, screenHeight / 2 - maph * scaleFactor / 2 + j * scaleFactor + RandomThings.rnd1Around0()/2f, tilescale /*- RandomThings.rnd()/20F*/, new Rectangle(tilesize * map[j * mapw + i], 0, tilesize, tilesize));
        }
    }
}
