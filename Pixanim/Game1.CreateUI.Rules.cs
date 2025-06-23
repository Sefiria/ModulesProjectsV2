using GeonBit.UI;
using GeonBit.UI.Entities;
using GeonBit.UI.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Tools;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace Pixanim
{
    public partial class Game1 : Game
    {
        private void CreateUI_Rules()
        {
            if (TilemapRulesWindow != null)
                return;

            TilemapRulesWindow = new Panel(new Vector2(640, 640));
            UserInterface.Active.AddEntity(TilemapRulesWindow);
            TilemapRulesWindow.Draggable = true;
            TilemapRulesWindow.AddChild(new Paragraph("Tilemap rules", Anchor.TopCenter));
            TilemapRulesWindow.AddChild(new HorizontalLine());

            create_close_button();
        }
        void create_close_button()
        {
            Button btClose;
            (btClose = new Button("Close", ButtonSkin.Default, Anchor.BottomCenter)).OnClick +=
            (Entity e) =>
            {
                UserInterface.Active.RemoveEntity(TilemapRulesWindow);
                TilemapRulesWindow = null;
            };
            TilemapRulesWindow.AddChild(btClose);
        }
    }
}