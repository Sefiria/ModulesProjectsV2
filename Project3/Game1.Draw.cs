using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LibNoise.Primitive;
using LibNoise.Combiner;

namespace Project3
{
    public partial class Game1 : Game
    {
        int tilecount, mapw, maph;
        byte[] map;

        private void InitDraw()
        {
            tex_tilemap = Texture2D.FromFile(GraphicsDevice, "tilemap_default.png");
            tilecount = tex_tilemap.Width / tilesize;
            DefineMap();
        }

        private void DefineMap()
        {
            var perlin = new ImprovedPerlin((int)(Ticks % int.MaxValue), LibNoise.NoiseQuality.Best);
            mapw = 48; maph = 32;
            map = new byte[mapw * maph];
            float min = 0.33F;
            float max = 1F;
            for (int j = 0; j < maph; j++)
            {
                for (int i = 0; i < mapw; i++)
                {
                    double noiseValue = perlin.GetValue(i * 0.15F, j * 0.15F, 0F);
                    double normalizedValue = ((noiseValue + 1) / 2) * (max - min) + min;
                    map[j * mapw + i] = (byte)(normalizedValue * tilecount);
                }
            }
        }

        private void Draw()
        {
            //Graphics.Graphics.Instance.DrawString("test", 50, 50, Resources.Instance.Fonts[0], Color.White);
            for (int j = 0; j < maph; j++)
                for (int i = 0; i < mapw; i++)
                    Graphics.Graphics.Instance.DrawTexture(tex_tilemap, screenWidth / 2 - mapw * scaleFactor / 2 + i * scaleFactor, screenHeight / 2 - maph * scaleFactor / 2 + j * scaleFactor, tilescale, new Rectangle(8 * map[j * mapw + i], 0, tilesize, tilesize));
        }
    }
}
