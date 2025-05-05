using GeonBit.UI;
using GeonBit.UI.Entities;
using GeonBit.UI.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Tools;

namespace Project3
{
    public partial class Game1 : Game
    {
        int tilesize, tilescale;
        int scaleFactor => tilesize * tilescale;

        private void InitUpdate()
        {
            KB.OnKeyPressed += KB_OnKeyPressed;
            tilesize = 8;
            tilescale = 4;
        }

        private void KB_OnKeyPressed(char key)
        {
            if (key == 'R')
                DefineMap();
        }

        private void Update()
        {
        }
    }
}
