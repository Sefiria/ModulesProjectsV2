using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project7.Source.Arcade.ui;
using System.Collections.Generic;

namespace Project7.Source.Arcade
{
    public class ArcadeMenu : IArcade
    {
        public string Name => "Menu";

        public void Initialize()
        {
            var hw = UI.context.ScreenWidth / 2;
            var hh = UI.context.ScreenHeight / 2;
            int i = 0;
            new List<Button>()
            {
                new Button(x: 0, y: 0, text: "SPACE", () => ArcadeMain.instance.scene_index = 1),
                new Button(x: 0, y: 0, text: "EXIT", UI.context.ExitArcade),
            }
            .ForEach(bt =>
            {
                var sz = bt.GetBounds();
                bt.x = hw - sz.Width / 2;
                bt.y = hh + (sz.Height + UI.padding * 2) * i++;
            });
        }
        public void Update()
        {
        }
        public void Draw(GraphicsDevice graphics)
        {
        }
    }
}
