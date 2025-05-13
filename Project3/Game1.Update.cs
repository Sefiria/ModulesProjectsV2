using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Project3.particles;
using System.Linq;
using Tooling;
using Particle = Project3.particles.Particle;
using UI = GeonBit.UI.Entities;

namespace Project3
{
    public partial class Game1 : Game
    {
        public const int tilesize = 16, tilescale = 2;
        public int scaleFactor => tilesize * tilescale;
        int ms_tile_index_prv = -1, ms_tile_index_cur = -1, tile_break_ticks = 0;

        public int bounds_x => screenWidth / 2 - mapw * scaleFactor / 2;
        public int bounds_y => screenHeight / 2 - maph * scaleFactor / 2;
        public int bounds_w => mapw * scaleFactor;
        public int bounds_h => maph * scaleFactor;
        public Rectangle Bounds => new Rectangle(bounds_x, bounds_y, bounds_w, bounds_h);

        Panel Menu = null;

        private void InitUpdate()
        {
            KB.OnKeyPressed += KB_OnKeyPressed;
            MS.OnButtonDown += MS_OnButtonDown;
        }

        private void KB_OnKeyPressed(char key)
        {
            if (key == 'R')
                DefineMap();
        }

        private void MS_OnButtonDown(Tools.Inputs.MS.MouseButtons button)
        {
            if (MS.Rect.Intersects(Bounds) && (Menu == null || !MS.Rect.Intersects(Menu.CalcDestRect())))
            {
                int tile_x = (MS.X - bounds_x) / scaleFactor;
                int tile_y = (MS.Y - bounds_y) / scaleFactor;
                ms_tile_index_cur = tile_y * mapw + tile_x;
                if (map[ms_tile_index_cur] <= 0)
                    return;
                if(ms_tile_index_cur != ms_tile_index_prv)
                {
                    tile_break_ticks = 0;
                    ms_tile_index_prv = ms_tile_index_cur;
                }
                if (tile_break_ticks >= map_blocs_durability[map[ms_tile_index_cur]])
                {
                    map[ms_tile_index_cur]--;
                    tile_break_ticks = 0;

                    for (int i = 0; i < 5; i++)
                        new Particle(
                            tile_index: map[ms_tile_index_cur],
                            size_x_in_pixels: 8,
                            size_y_in_pixels: 8,
                            scale: 2,
                            lifetime_in_ticks: 100,
                            init_x: bounds_x + (int)((tile_x + .5f) * scaleFactor),
                            init_y: bounds_y + (int)((tile_y + .5f) * scaleFactor),
                            vel_x: RandomThings.rnd1Around0() * 2F,
                            vel_y: RandomThings.rnd1Around0() * 2F
                            );
                }
                else
                {
                    if (Ticks % 20 == 0)
                    {
                        tile_break_ticks++;
                        for (int i = 0; i < 3; i++)
                            new Particle(
                                tile_index: map[ms_tile_index_cur],
                                size_x_in_pixels: 4,
                                size_y_in_pixels: 4,
                                scale: 2,
                                lifetime_in_ticks: 50,
                                init_x: bounds_x + (int)((tile_x + .5f) * scaleFactor),
                                init_y: bounds_y + (int)((tile_y + .5f) * scaleFactor),
                                vel_x: RandomThings.rnd1Around0() * 2F,
                                vel_y: RandomThings.rnd1Around0() * 2F
                                );
                    }
                    else
                    {
                        if(RandomThings.rnd(5) == 0)
                            new Particle(
                                tile_index:map[ms_tile_index_cur],
                                size_x_in_pixels:4,
                                size_y_in_pixels:4,
                                scale:1,
                                lifetime_in_ticks:20,
                                init_x: bounds_x + (int)((tile_x + .5f) * scaleFactor),
                                init_y: bounds_y + (int)((tile_y + .5f) * scaleFactor),
                                vel_x:RandomThings.rnd1Around0() * 2F,
                                vel_y:RandomThings.rnd1Around0() * 2F
                                );
                    }
                }
            }
        }

        private void Update()
        {
            if (KB.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space))
                CreateUI_Menu();
        }

        private void CreateUI_Menu()
        {
            if (Menu != null)
                return;

            Button btClose;

            Menu = new Panel(new Vector2(480, 640));
            Menu.Draggable = true;
            Menu.AddChild(new Paragraph("Menu", Anchor.TopCenter));
            Menu.AddChild(new HorizontalLine());
            UserInterface.Active.AddEntity(Menu);

            // Close button

            (btClose = new Button("Close", ButtonSkin.Default, Anchor.BottomCenter)).OnClick +=
            (UI.Entity e) =>
            {
                UserInterface.Active.RemoveEntity(Menu);
                Menu = null;
            };
            Menu.AddChild(btClose);
        }
    }
}
