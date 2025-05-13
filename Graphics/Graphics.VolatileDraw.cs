using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Tooling;

namespace Graphics
{
    public partial class Graphics
    {
        public void DrawRectangle(Rectangle rect, Color col, int thickness = 1) => DrawRectangle(rect.X, rect.Y, rect.Width, rect.Height, col, thickness);
        public void DrawRectangle(int x, int y, int w, int h, Color col, int thickness = 1)
        {
            if (w <= 0 || h <= 0)
                return;
            Texture2D tex = new Texture2D(GraphicsDevice, w, h);
            Color[] data = new Color[w * h];
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < Math.Min(h / 2, thickness); j++)
                {
                    data[j * w + i] = col;
                    data[(h - 1 - j) * w + i] = col;
                }
            }
            for (int j = 0; j < h; j++)
            {
                for (int i = 0; i < Math.Min(w / 2, thickness); i++)
                {
                    data[j * w + i] = col;
                    data[j * w + (w - 1 - i)] = col;
                }
            }
            tex.SetData(data);
            DrawTexture(tex, x, y);
        }
        public void DrawCircle(int x, int y, int radius, Color col, int thickness = 1)
        {
            thickness = Math.Min(radius * 2, thickness);

            Texture2D tex = new Texture2D(GraphicsDevice, radius * 2, radius * 2);
            Color[] data = new Color[(radius * 2) * (radius * 2)];

            for (int i = 0; i < thickness; i++)
                draw(i);

            void draw(int i)
            {
                int r = radius;
                int _r = r - i;
                int cx = _r + i;
                int cy = _r + i;
                for (float angle = 0; angle < 360; angle+=0.5F)
                {
                    double radian = angle * Math.PI / 180;
                    int px = (int)(_r * Math.Cos(radian));
                    int py = (int)(_r * Math.Sin(radian));

                    if (cx + px >= 0 && cx + px < r * 2 && cy + py >= 0 && cy + py < r * 2)
                        data[(cy + py) * (r * 2) + (cx + px)] = col;
                    if (cx - px >= 0 && cx - px < r * 2 && cy + py >= 0 && cy + py < r * 2)
                        data[(cy + py) * (r * 2) + (cx - px)] = col;
                    if (cx + px >= 0 && cx + px < r * 2 && cy - py >= 0 && cy - py < r * 2)
                        data[(cy - py) * (r * 2) + (cx + px)] = col;
                    if (cx - px >= 0 && cx - px < r * 2 && cy - py >= 0 && cy - py < r * 2)
                        data[(cy - py) * (r * 2) + (cx - px)] = col;
                }
            }

            tex.SetData(data);
            DrawTexture(tex, x - radius, y - radius);
        }
        public void DrawLine(int start_x, int start_y, int end_x, int end_y, Color color, int thickness)
        {
            int w = Math.Abs(end_x - start_x);
            int h = Math.Abs(end_y - start_y);
            if (w == 0 && h == 0)
                return;

            Texture2D tex = new Texture2D(GraphicsDevice, w + thickness, h + thickness);
            Color[] data = new Color[(w + thickness) * (h + thickness)];
            float d = Maths.Distance(start_x, start_y, end_x, end_y);

            for (float t = 0F; t <= 1F; t += 1F / d)
            {
                int x = (int)Maths.Lerp(start_x, end_x, t);
                int y = (int)Maths.Lerp(start_y, end_y, t);

                for (int j = 0; j < thickness; j++)
                {
                    for (int i = 0; i < thickness; i++)
                    {
                        int index = (y - Math.Min(start_y, end_y) + i) * (w + thickness) + (x - Math.Min(start_x, end_x) + j);
                        if (index >= 0 && index < data.Length)
                            data[index] = color;
                    }
                }
            }

            tex.SetData(data);
            DrawTexture(tex, Math.Min(start_x, end_x), Math.Min(start_y, end_y));
        }
        public void DrawString(string text, int x, int y, SpriteFont font, Color color)
        {
            SpriteBatch.DrawString(font, text, new Vector2(x, y), color, 0F, Vector2.Zero, 1F, SpriteEffects.None, 1F);
        }

        public void FillRectangle(Rectangle rect, Color col, float rotation = 0F) => FillRectangle(rect.X, rect.Y, rect.Width, rect.Height, col, rotation);
        public void FillRectangle(int x, int y, int w, int h, Color col, float rotation = 0F)
        {
            if (w <= 0 || h <= 0)
                return;

            Texture2D tex = new Texture2D(GraphicsDevice, w, h);
            Color[] data = new Color[w * h];

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    data[j * w + i] = col;
                }
            }

            tex.SetData(data);
            Rotation = rotation;
            DrawTexture(tex, x, y);
            Rotation = 0F;
        }

        public void FillCircle(int x, int y, int radius, Color col)
        {
            Texture2D tex = new Texture2D(GraphicsDevice, radius * 2, radius * 2);
            Color[] data = new Color[(radius * 2) * (radius * 2)];

            int r = radius;
            int cx = r;
            int cy = r;

            for (int py = -r; py <= r; py++)
            {
                for (int px = -r; px <= r; px++)
                {
                    if (px * px + py * py <= r * r)
                    {
                        int dx = cx + px;
                        int dy = cy + py;
                        if (dx >= 0 && dx < r * 2 && dy >= 0 && dy < r * 2)
                        {
                            data[dy * (r * 2) + dx] = col;
                        }
                    }
                }
            }

            tex.SetData(data);
            DrawTexture(tex, x - radius, y - radius);
        }

    }
}
