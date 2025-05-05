using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Tools;
using static System.Net.Mime.MediaTypeNames;

namespace Graphics
{
    public partial class Graphics
    {
        private static Graphics mInstance = null;
        public static Graphics Instance => mInstance ?? (mInstance = new Graphics());


        private GraphicsDevice GraphicsDevice;
        private SpriteBatch SpriteBatch;
        private Dictionary<string, Texture2D> Textures;
        private Dictionary<string, SpriteFont> Fonts;


        public void Initialize(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch = null)
        {
            GraphicsDevice = graphicsDevice;
            SpriteBatch = spriteBatch ?? new SpriteBatch(GraphicsDevice);
            Textures = new Dictionary<string, Texture2D>();
        }
        public bool AddFont(string name, SpriteFont font, bool @override = false)
        {
            if (@override == false && Fonts.ContainsKey(name))
                return false;
            Fonts[name] = font;
            return true;
        }
        public bool LoadTexture(string name, Color[,] pixels, bool @override = false)
        {
            if (@override == false && Textures.ContainsKey(name))
                return false;
            int w = pixels.GetLength(1);
            int h = pixels.GetLength(0);
            Texture2D tex = new Texture2D(GraphicsDevice, w, h);
            Color[] colors = pixels.ToSingleArray();
            tex.SetData(colors);
            Textures[name] = tex;
            return true;
        }
        public void BeginDraw(Color clear_color)
        {
            GraphicsDevice.Clear(clear_color);
            SpriteBatch.Begin();
        }
        public void EndDraw()
        {
            SpriteBatch.End();
        }
    }
}
