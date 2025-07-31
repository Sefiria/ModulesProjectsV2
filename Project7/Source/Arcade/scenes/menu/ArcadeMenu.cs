using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project7.Source.Arcade.scenes.plateform;
using Project7.Source.Arcade.ui;
using System.Collections.Generic;

namespace Project7.Source.Arcade.scenes.menu
{
    public class ArcadeMenu : IScene
    {
        public string Name => "Menu";

        public void Initialize()
        {
            ArcadeMain.instance.edit = false;
            var hw = UI.context.ScreenWidth / 2;
            var hh = UI.context.ScreenHeight / 2;
            int i = 0;
            new List<Button>()
            {
                new Button(x: 0, y: 0, text: "PLATEFORM", () => ArcadeMain.instance.scene_index = 2){ alt_callback= ()=>{ ArcadeMain.instance.scene_index = 2; ArcadeMain.instance.edit = true; } },
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
        public void Dispose() { }
    }
}
