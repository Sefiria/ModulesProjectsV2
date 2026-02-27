using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project7.Source.Entities.Behaviors;
using System.Linq;
using Panel = GeonBit.UI.Entities.Panel;

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
            UserInterface.Active.GlobalScale = 0.75f;
        }

        private void UpdateUI()
        {
            //test.Offset = new Vector2(MS.X, MS.Y - test.TotalHeight);
        }
    }
}
