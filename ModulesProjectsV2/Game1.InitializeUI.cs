using GeonBit.UI;
using GeonBit.UI.Entities;
using GeonBit.UI.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Tools;

namespace Project1
{
    public partial class Game1 : Game
    {
        Texture2D tex_tilemap_splitrended = null;
        Panel TilemapLoadWindow = null, TilemapRulesWindow = null;

        int TileSize = 8;

        void InitializeUI()
        {
            CreateUI_TopMenu();
            CreateUI_MapEditWindow();
        }

        private void CreateUI_TopMenu()
        {
            var layout = new MenuBar.MenuLayout();
            layout.AddMenu("File", 180);
            layout.AddItemToMenu("File", "New", () => { MessageBox.ShowMsgBox("Not implemented yet", ""); });
            layout.AddItemToMenu("File", "Save", () => { MessageBox.ShowMsgBox("Not implemented yet", ""); });
            layout.AddItemToMenu("File", "Load", () => { MessageBox.ShowMsgBox("Not implemented yet", ""); });
            layout.AddItemToMenu("File", "Exit", () => { MessageBox.ShowMsgBox("Not implemented yet", ""); });
            layout.AddMenu("Tilemap", 220);
            layout.AddItemToMenu("Tilemap", "Load", (MenuBar.MenuCallbackContext context) =>
            {
                CreateUI_TilemapLoadWindow();
            });
            layout.AddItemToMenu("Tilemap", "Rules", (MenuBar.MenuCallbackContext context) =>
            {
                CreateUI_Rules();
            });

            var menuBar = MenuBar.Create(layout);
            menuBar.Anchor = Anchor.Auto;
            UserInterface.Active.AddEntity(menuBar);
        }
        private void CreateUI_MapEditWindow()
        {
            Panel panel = new Panel(new Vector2(640, 480));
            UserInterface.Active.AddEntity(panel);
            panel.Draggable = true;
            panel.AddChild(new Paragraph("Map edit", Anchor.TopCenter));
            panel.AddChild(new HorizontalLine());
        }
        private void CreateUI_TilemapLoadWindow()
        {
            if (TilemapLoadWindow != null)
                return;

            Button btLoad;
            Image imgRawTilemap = null;
            Button btClose, btCheckSplit;

            TilemapLoadWindow = new Panel(new Vector2(640, 640));
            TilemapLoadWindow.Draggable = true;
            TilemapLoadWindow.AddChild(new Paragraph("Load tilemap", Anchor.TopCenter));
            TilemapLoadWindow.AddChild(new HorizontalLine());
            UserInterface.Active.AddEntity(TilemapLoadWindow);

            // Load button

            (btLoad = new Button("Load File", ButtonSkin.Default)).OnClick +=
            (Entity e) =>
            {
                MessageBox.OpenLoadFileDialog("", (FileDialogResponse res) =>
                {
                    try{ tex_tilemap_splitrended = Texture2D.FromFile(GraphicsDevice, res.FullPath);}
                    catch(Exception){}
                    if (tex_tilemap_splitrended != null)
                    {
                        imgRawTilemap.Texture = tex_tilemap_splitrended;
                        tex_tilemap = tex_tilemap_splitrended.Clone(GraphicsDevice);
                    }
                    return true;
                });
            };
            TilemapLoadWindow.AddChild(btLoad);

            // Tilemap image

            tex_tilemap_splitrended = tex_tilemap_splitrended ?? new Texture2D(GraphicsDevice, 1, 1);
            imgRawTilemap = new Image(tex_tilemap_splitrended, new Vector2(48*4, 48*4), ImageDrawMode.Stretch, Anchor.Center);
            TilemapLoadWindow.AddChild(imgRawTilemap);

            // TileSize

            var tbTileSize = new TextInput();
            tbTileSize.Value = "8";
            TilemapLoadWindow.AddChild(tbTileSize);

            // _ secure_split

            void secure_split()
            {
                string newValue = new string(tbTileSize.Value.Where(char.IsDigit).ToArray());
                bool isinteger = int.TryParse(newValue, out int result);
                if (!isinteger || result < 1 || result > 99)
                {
                    tbTileSize.Value = "8";
                    MessageBox.ShowMsgBox("Invalid argument", $"TileSize value of '{newValue}' is incorrect (1>value>=99), Auto set to 8.");
                }
                TileSize = int.Parse(tbTileSize.Value);
            }

            // Check split

            (btCheckSplit = new Button("Check split")).OnClick +=
            (Entity e) =>
            {
                secure_split();
                checkSplitTileMap();
            };
            TilemapLoadWindow.AddChild(btCheckSplit);

            // Close button

            (btClose = new Button("Close", ButtonSkin.Default, Anchor.BottomCenter)).OnClick +=
            (Entity e) =>
            {
                secure_split();
                UserInterface.Active.RemoveEntity(TilemapLoadWindow);
                TilemapLoadWindow = null;
            };
            TilemapLoadWindow.AddChild(btClose);
        }
        void checkSplitTileMap()
        {
            int w = tex_tilemap.Width;
            int h = tex_tilemap.Height;
            Color[] pixels = new Color[w * h];
            tex_tilemap.GetData(pixels);

            int ofs = (int)(Ticks % 20);

            for (int x = 0; x < w; x += TileSize)
                for (int y = 0; y < h; y++)
                    pixels[y * h + x] = new Color(pixels[y * h + x].R + ofs, pixels[y * h + x].G + ofs, pixels[y * h + x].B + ofs);
            for (int y = 0; y < h; y += TileSize)
                for (int x = 0; x < w; x++)
                    pixels[y * h + x] = new Color(pixels[y * h + x].R + ofs, pixels[y * h + x].G + ofs, pixels[y * h + x].B + ofs);

            tex_tilemap_splitrended.SetData(pixels);
        }


        private void UpdateUI()
        {
            UpdateUI_TilemapLoadWindow();
        }
        private void UpdateUI_TilemapLoadWindow()
        {
            if (tex_tilemap != null && TileSize > 1 && TileSize <= 99)
                checkSplitTileMap();
        }
    }
}
