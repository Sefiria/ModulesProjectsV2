using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project7.Source.Arcade.ui
{
    public class Button : UI
    {
        public string text;
        public Action callback;

        public bool IsHover { get; private set; }

        public Vector2 text_size_in_pixels => font.MeasureString(text);
        public Rectangle GetBounds()
        {
            Vector2 sz = text_size_in_pixels;
            return new Rectangle(x, y, (int)sz.X + padding * 2, (int)sz.Y + padding * 2);
        }

        public Button(int x, int y, string text, Action callback) : base()
        {
            this.x = x;
            this.y = y;
            this.text = text;
            this.callback = callback;
            ArcadeMain.instance.UI.Add(this);
        }
        public override void update()
        {
            IsHover = GetBounds().Contains(Game1.MS.X, Game1.MS.Y);
            if (IsHover && Game1.MS.IsLeftPressed)
                callback?.Invoke();
        }
        public override void draw(GraphicsDevice gd)
        {
            var c = IsHover ? Color.White : Color.Gray;
            graphics.DrawRectangle(GetBounds(), c);
            graphics.DrawString(text, x + padding, y + padding, font, c);
        }
    }
}
