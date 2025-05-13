using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Project4
{
    public partial class Game1 : Game
    {
        float angle = 0f;
        int centerX => cw;
        int centerY => ch;
        int radius => cw - ball_diameter;
        int blackCircleRadius => cw / 4;
        int blackCircleX => centerX + (int)((radius - blackCircleRadius + ball_diameter) * Math.Cos(angle));
        int blackCircleY => centerY + (int)((radius - blackCircleRadius + ball_diameter) * Math.Sin(angle));

        private void InitDraw()
        {
        }

        private void DefineMap()
        {
        }

        private void draw(GameTime gameTime)
        {
            float speed = 1f;

            angle += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            Graphics.Graphics.Instance.DrawCircle(centerX, centerY, radius, Color.White, 2);
            Graphics.Graphics.Instance.FillCircle(blackCircleX, blackCircleY, blackCircleRadius, Color.Black);

            var list = new List<Ball>(Balls);
            Color c;
            foreach (Ball ball in list)
            {
                if(ball.c < 0.1) c = Color.Lime;
                else if (ball.c < 0.2) c = Color.Red;
                else if (ball.c < 0.3) c = Color.Blue;
                else if (ball.c < 0.4) c = Color.Cyan;
                else if (ball.c < 0.5) c = Color.Yellow;
                else if (ball.c < 0.6) c = Color.White;
                else if (ball.c < 0.7) c = Color.Orange;
                else if (ball.c < 0.8) c = Color.Azure;
                else if (ball.c < 0.9) c = Color.Green;
                else c = Color.Gray;
                Graphics.Graphics.Instance.FillCircle((int)ball.x, (int)ball.y, ball_diameter, c);
                if(CONFIG_APPLY_PHYSICS)
                    Graphics.Graphics.Instance.DrawCircle((int)ball.x, (int)ball.y, ball_diameter, Color.White);
            }
        }
    }
}
