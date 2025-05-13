using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Tooling;

namespace Project5
{
    public partial class Game1 : Game
    {
        List<Chain> Chains;
        List<bool[]> chains_holding;

        private void InitUpdate()
        {
            chains_holding = [[false, false], [false, false]];
            Chains =
            [
                new Chain(
                    A:new Vector2(320-100, 240),
                    B: new Vector2(400-100, 300),
                    totalLength:200F,
                    linkLength:20f,
                    linkSpacing:5F,
                    gravity:new Vector2(0F, 0.5F)),
                new Chain(
                    A: new Vector2(320+100, 240),
                    B: new Vector2(400+100, 300),
                    totalLength: 200F,
                    linkLength: 20f,
                    linkSpacing: 5F,
                    gravity: new Vector2(0F, 0.5F)),
            ];

            KB.OnKeyPressed += KB_OnKeyPressed;
            MS.OnButtonPressed += MS_OnButtonPressed;
            MS.OnButtonDown+= MS_OnButtonDown;
        }
        private void KB_OnKeyPressed(char key)
        {
        }

        private void MS_OnButtonPressed(Tools.Inputs.MS.MouseButtons button)
        {
            bool escape = false;
            for (int i = 0; !escape && i < Chains.Count; i++)
                for (int j = 0; !escape && j < 2; j++)
                    if (button == Tools.Inputs.MS.MouseButtons.Left && Maths.Distance(MS.X, MS.Y, j == 0 ? Chains[i].A.X: Chains[i].B.X, j == 0 ? Chains[i].A.Y : Chains[i].B.Y) < Chains[i].point_size)
                        chains_holding[i][j] = escape = true;
        }

        private void MS_OnButtonDown(Tools.Inputs.MS.MouseButtons button)
        {
            for (int i = 0; i < Chains.Count; i++)
            {
                if (chains_holding[i][0])
                    Chains[i].A = MS.Position.ToVector2();
                if (chains_holding[i][1])
                    Chains[i].B = MS.Position.ToVector2();
            }
            Chains.ForEach(c => c.Update(new GameTime()));
        }

        private void update(GameTime gameTime)
        {
            Chains.ForEach(c => c.Update(gameTime));

            if (!MS.IsLeftDown)
            {
                for (int i = 0; i < Chains.Count; i++)
                    for (int j = 0; j < 2; j++)
                        chains_holding[i][j] = false;
            }
        }
    }
}
