using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SFX;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tooling;

namespace Project4
{
    public partial class Game1 : Game
    {
        public struct Ball
        {
            public float x, y, u, v, c;
        }

        public List<Ball> Balls;
        public Ball NewBall() => new Ball { x = 20+(float)random.NextDouble() * (cw*2-40), y = ch, u = 0, v = 0, c = (float)random.NextDouble() };
        Random random = new Random((int)DateTime.UtcNow.Ticks);

        int cw, ch;
        int ball_diameter = 10;
        int max_sfx = 1;
        int max_balls_count = 20;
        Sample[] se;
        private static int activeSounds = 0;
        private static readonly object lockObject = new object();

        bool CONFIG_APPLY_PHYSICS = true;

        private void InitUpdate()
        {
            cw = screenWidth / 2;
            ch = screenHeight / 2;
            Balls = new List<Ball>();
            InitSE();
            KB.OnKeyPressed += KB_OnKeyPressed;
            MS.OnButtonPressed += MS_OnButtonPressed;
        }
        private void InitSE()
        {
            int samples_count = 10;

            se = new Sample[samples_count];
            for (int i = 0; i < samples_count; i++)
            {
                var note = (random.Next(0, 7) << 12) | (random.Next(2, 4) << 8) | 0xF3;
                Instrument instru = Instruments.Sinus;
                switch(random.Next(0, 4))
                {
                    case 0: instru = Instruments.Sinus; break;
                    case 1: instru = Instruments.Saw; break;
                    case 2: instru = Instruments.Square; break;
                    case 3: instru = Instruments.Triangle; break;
                }
                se[i] = new Sample([note], SampleRates.VeryLow, instru, 0.2);
            }
        }

        private void KB_OnKeyPressed(char key)
        {
            if (key == 'P')
                CONFIG_APPLY_PHYSICS = !CONFIG_APPLY_PHYSICS;
            if (key == 'S')
                InitSE();
        }

        private void MS_OnButtonPressed(Tools.Inputs.MS.MouseButtons button)
        {
            if(button == Tools.Inputs.MS.MouseButtons.Left)
                Balls.Add(NewBall());
            if (button == Tools.Inputs.MS.MouseButtons.Right)
                Balls.Clear();
        }

        private void update(GameTime gameTime)
        {
            float g = 0.1f;
            float spd = 2F;
            float max_spd = 5F;
            float separationForce = 0.1f;
            float min_force_for_sfx = 1F;

            for (int i = 0; i < Balls.Count; i++)
            {
                Ball ball = Balls[i];
                bool has_col = false; // Drapeau pour vérifier la collision

                // col balls
                for (int b = 0; CONFIG_APPLY_PHYSICS && b < Balls.Count; b++)
                {
                    if (i == b) continue;
                    Ball otherBall = Balls[b];
                    if (Maths.CollisionCercleCercle(new Circle(ball.x, ball.y, ball_diameter), new Circle(otherBall.x, otherBall.y, ball_diameter)))
                    {
                        // Vérification des vecteurs de direction similaires
                        if (Math.Abs(ball.u - otherBall.u) < 0.1f && Math.Abs(ball.v - otherBall.v) < 0.1f)
                        {
                            // Appliquer une force contraire pour séparer les balles
                            float dx = ball.x - otherBall.x;
                            float dy = ball.y - otherBall.y;
                            float distance = (float)Math.Sqrt(dx * dx + dy * dy);
                            // Normalisation du vecteur de séparation
                            float nx = dx / distance;
                            float ny = dy / distance;
                            // Appliquer la force contraire
                            ball.u += separationForce * nx;
                            ball.v += separationForce * ny;
                            otherBall.u -= separationForce * nx;
                            otherBall.v -= separationForce * ny;
                        }
                        else
                        {
                            // Échange des vecteurs de direction
                            float tempU = ball.u;
                            float tempV = ball.v;
                            ball.u = otherBall.u;
                            ball.v = otherBall.v;
                            otherBall.u = tempU;
                            otherBall.v = tempV;
                        }
                        if (Maths.Distance(ball.x, ball.y, otherBall.x, otherBall.y) < ball_diameter * 2)
                        {
                            float overlap = ball_diameter * 2 - Maths.Distance(ball.x, ball.y, otherBall.x, otherBall.y);
                            float dx = ball.x - otherBall.x;
                            float dy = ball.y - otherBall.y;
                            float distance = (float)Math.Sqrt(dx * dx + dy * dy);
                            // Normalisation du vecteur de séparation
                            float nx = dx / distance;
                            float ny = dy / distance;
                            // Appliquer la modification en x et y de ball
                            float _d = Maths.Distance(centerX, centerY, overlap * nx, overlap * ny);
                            if (_d >= cw - ball_diameter * 2 && _d < cw - ball_diameter && !Maths.CollisionPointCercle(ball.x, ball.y, blackCircleX, blackCircleY, blackCircleRadius))
                            {
                                otherBall.x += overlap * nx;
                                otherBall.y += overlap * ny;
                            }
                            else
                            {
                                ball.x += overlap * nx;
                                ball.y += overlap * ny;
                            }
                        }
                        has_col = true; // Marquer la collision
                    }
                    Balls[b] = otherBall;
                }

                // col circle
                float d = Maths.Distance(centerX, centerY, ball.x, ball.y);
                if (d >= cw - ball_diameter * 2 && d < cw - ball_diameter && !Maths.CollisionPointCercle(ball.x, ball.y, blackCircleX, blackCircleY, blackCircleRadius))
                {
                    float normalX = ball.x - centerX;
                    float normalY = ball.y - centerY;
                    float magnitude = (float)Math.Sqrt(normalX * normalX + normalY * normalY);
                    normalX /= magnitude;
                    normalY /= magnitude;
                    float dotProduct = ball.u * normalX + ball.v * normalY;
                    ball.u = ball.u - 2 * dotProduct * normalX;
                    ball.v = ball.v - 2 * dotProduct * normalY;
                    ball.x += ball.u * 15F;
                    ball.y += ball.v * 15F;
                    ball.u *= 0.9F;
                    ball.v *= 0.9F;
                    has_col = true; // Marquer la collision
                }

                // mov
                ball.x += ball.u * spd;
                ball.y += ball.v * spd;
                ball.v += g;
                if (ball.u > max_spd) ball.u = max_spd;
                if (ball.v > max_spd) ball.v = max_spd;
                Balls[i] = ball;

                // out
                if (ball.x < -ball_diameter || ball.x > cw * 2 + ball_diameter || ball.y < -ball_diameter || ball.y > ch * 2 + ball_diameter)
                {
                    Balls.Remove(ball);
                    Balls.Add(NewBall());
                    Balls.Add(NewBall());
                }
                else
                {
                    if (has_col && (ball.u * ball.u > min_force_for_sfx * min_force_for_sfx || ball.v * ball.v > min_force_for_sfx * min_force_for_sfx))
                    {
                        PlaySoundAsync(se[random.Next(0, se.Length)]).ConfigureAwait(false);
                    }
                }
            }

            while (Balls.Count > max_balls_count)
                Balls.RemoveAt(0);
        }


        private async Task PlaySoundAsync(Sample se)
        {
            lock (lockObject)
            {
                if (activeSounds >= max_sfx)
                    return;
                activeSounds++;
            }

            await SFX.SFX.PlayAsync(se);

            lock (lockObject)
            {
                activeSounds--;
            }
        }

    }
}
