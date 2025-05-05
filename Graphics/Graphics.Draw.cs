using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphics
{
    public partial class Graphics
    {
        public void DrawTexture(string name, float x, float y, Color? color = null, Rectangle? source = null) => DrawTexture(Textures[name], x, y, color, source);
        public void DrawTexture(string name, Vector2 position, Color? color = null, Rectangle? source = null) => DrawTexture(Textures[name], position, color, source);
        public void DrawTexture(Texture2D texture, float x, float y, Color? color = null, Rectangle? source = null) => DrawTexture(texture, new Vector2(x, y), color, source);
        public void DrawTexture(Texture2D texture, Vector2 position, Color? color = null, Rectangle? source = null)
        {
            SpriteBatch.Draw(texture, position, source, color ?? Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public void DrawTexture(string name, float x, float y, float scale, Rectangle? source = null) => DrawTexture(Textures[name], x, y, scale, source);
        public void DrawTexture(string name, Vector2 position, float scale, Rectangle? source = null) => DrawTexture(Textures[name], position, scale, source);
        public void DrawTexture(Texture2D texture, float x, float y, float scale, Rectangle? source = null) => DrawTexture(texture, new Vector2(x, y), scale, source);
        public void DrawTexture(Texture2D texture, Vector2 position, float scale, Rectangle? source = null)
        {
            SpriteBatch.Draw(texture, position, source, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        public void DrawTexture(string name, float x, float y, float scale, float depth = 0f) => DrawTexture(Textures[name], new Vector2(x, y), scale, depth);
        public void DrawTexture(string name, Vector2 position, float scale, float depth = 0f) => DrawTexture(Textures[name], position, scale, depth);
        public void DrawTexture(Texture2D texture, float x, float y, float scale, float depth = 0f) => DrawTexture(texture, new Vector2(x, y), scale, depth);
        public void DrawTexture(Texture2D texture, Vector2 position, float scale, float depth = 0f)
        {
            SpriteBatch.Draw(texture, position, null, Color.White, scale, Vector2.Zero, 1f, SpriteEffects.None, depth);
        }
    }
}
