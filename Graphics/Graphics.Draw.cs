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
        public float Rotation = 0F;

        public void DrawTexture(string name, float x, float y, Color? color = null, Rectangle? source = null) => DrawTexture(Textures[name], x, y, color, source);
        public void DrawTexture(string name, Vector2 position, Color? color = null, Rectangle? source = null) => DrawTexture(Textures[name], position, color, source);
        public void DrawTexture(Texture2D texture, float x, float y, Color? color = null, Rectangle? source = null) => DrawTexture(texture, new Vector2(x, y), color, source);
        public void DrawTexture(Texture2D texture, Vector2 position, Color? color = null, Rectangle? source = null)
        {
            SpriteBatch.Draw(texture, position, source, color ?? Color.White, Rotation, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }

        public void DrawTexture(string name, float x, float y, float scale, Color color, Rectangle? source = null) => DrawTexture(Textures[name], x, y, scale, color, source);
        public void DrawTexture(string name, Vector2 position, float scale, Color color, Rectangle? source = null) => DrawTexture(Textures[name], position, scale, color, source);
        public void DrawTexture(Texture2D texture, float x, float y, float scale, Color color, Rectangle? source = null) => DrawTexture(texture, new Vector2(x, y), scale, color, source);
        public void DrawTexture(Texture2D texture, Vector2 position, float scale, Color color, Rectangle? source = null)
        {
            SpriteBatch.Draw(texture, position, source, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }

        public void DrawTexture(string name, float x, float y, float scale, Rectangle? source = null) => DrawTexture(Textures[name], x, y, scale, source);
        public void DrawTexture(string name, Vector2 position, float scale, Rectangle? source = null) => DrawTexture(Textures[name], position, scale, source);
        public void DrawTexture(Texture2D texture, float x, float y, float scale, Rectangle? source = null) => DrawTexture(texture, new Vector2(x, y), scale, source);
        public void DrawTexture(Texture2D texture, Vector2 position, float scale, Rectangle? source = null)
        {
            SpriteBatch.Draw(texture, position, source, Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }
        public void DrawTexture(Texture2D texture, Vector2 position, float scale, Rectangle? source = null, float alpha = 1f)
        {
            SpriteBatch.Draw(texture, position, source, Color.White * alpha, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
        }


        public void DrawTexture(string name, float x, float y, float rotation, float scale, bool flipX, float depth = 0f, Vector2? origin = null) => DrawTexture(Textures[name], new Vector2(x, y), rotation, scale, flipX, depth, origin);
        public void DrawTexture(string name, Vector2 position, float rotation, float scale, bool flipX, float depth = 0f, Vector2? origin = null) => DrawTexture(Textures[name], position, rotation, scale, flipX, depth, origin);
        public void DrawTexture(Texture2D texture, float x, float y, float rotation, float scale, bool flipX, float depth = 0f, Vector2? origin = null, Color? color = null, Rectangle? source = null) => DrawTexture(texture, new Vector2(x, y), rotation, scale, flipX, depth, origin, color, source);
        public void DrawTexture(Texture2D texture, Vector2 position, float rotation, float scale, bool flipX, float depth = 0f, Vector2? origin = null, Color? color = null, Rectangle? source = null)
        {
            SpriteEffects effects = flipX ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            SpriteBatch.Draw(texture, position, source, color ?? Color.White, rotation, origin ?? Vector2.Zero, scale, effects, depth);
        }
    }
}
