using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LibNoise.Primitive;

namespace Project3
{
    public partial class Game1 : Game
    {
        int tilecount, mapw, maph;
        byte[] map;

        private void InitDraw()
        {
            tex_tilemap = Texture2D.FromFile(GraphicsDevice, "tilemap_default.png");
            tilecount = tex_tilemap.Width / 8;
            DefineMap();
        }

        private void DefineMap()
        {
            var perlin = new ImprovedPerlin((int)(Ticks % int.MaxValue), LibNoise.NoiseQuality.Best);
            mapw = maph = 26;
            map = new byte[mapw * maph];
            for (int j = 0; j < maph; j++)
                for (int i = 0; i < mapw; i++)
                    map[j * mapw + i] = (byte)(((perlin.GetValue(i*0.15F, j*0.15F, 0F) + 1F) / 4F + 0.5F) * tilecount);
        }

        private void Draw()
        {
            //Graphics.Graphics.Instance.DrawString("test", 50, 50, Resources.Instance.Fonts[0], Color.White);
            for (int j = 0; j < maph; j++)
                for (int i = 0; i < mapw; i++)
                    Graphics.Graphics.Instance.DrawTexture(tex_tilemap, 50 + i * 8 * 4, 50 + j * 8 * 4, 4, new Rectangle(8 * map[j * mapw + i], 0, 8, 8));
        }
    }
}
