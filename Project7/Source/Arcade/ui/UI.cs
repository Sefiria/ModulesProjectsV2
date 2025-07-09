using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project7.Source.Arcade.ui
{
    public class UI
    {
        public static Game1 context => Game1.Instance;
        public static Graphics.Graphics graphics => Graphics.Graphics.Instance;


        public bool exists = true;
        public int x, y;

        public static int padding => 5;
        public SpriteFont font => GeonBit.UI.Resources.Instance.Fonts[0];

        public UI()
        {
        }

        public virtual void update()
        {
        }
        public virtual void draw(GraphicsDevice gd)
        {
        }
    }
}
