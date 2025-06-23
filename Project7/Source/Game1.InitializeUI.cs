using GeonBit.UI;
using GeonBit.UI.Entities;
using GeonBit.UI.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Tools;

namespace Project7
{
    public partial class Game1 : Game
    {
        Texture2D cursor_texture;
        //Paragraph test;
        void InitializeUI()
        {
            //test = new Paragraph("Test\nun\ndeux___________zd_______________zd", Anchor.TopLeft);
            //UserInterface.Active.AddEntity(test);
            cursor_texture = Texture2D.FromFile(GraphicsDevice, assets_bindings.Resources["cursor"]);
        }

        private void UpdateUI()
        {
            //test.Offset = new Vector2(MS.X, MS.Y - test.TotalHeight);
        }
    }
}
