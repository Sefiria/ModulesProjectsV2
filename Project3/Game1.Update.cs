using Microsoft.Xna.Framework;

namespace Project3
{
    public partial class Game1 : Game
    {
        const int tilesize = 16, tilescale = 2;
        int scaleFactor => tilesize * tilescale;

        int bounds_x => screenWidth / 2 - mapw * scaleFactor / 2;
        int bounds_y => screenHeight / 2 - maph * scaleFactor / 2;
        int bounds_w => mapw * scaleFactor;
        int bounds_h => maph * scaleFactor;
        Rectangle Bounds => new Rectangle(bounds_x, bounds_y, bounds_w, bounds_h);

        private void InitUpdate()
        {
            KB.OnKeyPressed += KB_OnKeyPressed;
            MS.OnButtonPressed += MS_OnButtonPressed;
        }

        private void KB_OnKeyPressed(char key)
        {
            if (key == 'R')
                DefineMap();
        }

        private void MS_OnButtonPressed(Tools.Inputs.MS.MouseButtons button)
        {
            if (MS.Rect.Intersects(Bounds))
            {
                int tile_x = (MS.X - bounds_x) / scaleFactor;
                int tile_y = (MS.Y - bounds_y) / scaleFactor;
                if (map[tile_y * mapw + tile_x] > 0)
                {
                    if (map_blocs_durability[tile_y * mapw + tile_x] > 1)
                    {
                        map_blocs_durability[tile_y * mapw + tile_x]--;
                    }
                    else
                    {
                        map[tile_y * mapw + tile_x]--;
                        map_blocs_durability[tile_y * mapw + tile_x] = map_blocs_types_max_durability[map[tile_y * mapw + tile_x]];
                    }
                }
            }
        }

        private void Update()
        {
        }
    }
}
