using Microsoft.Xna.Framework;
using Project7.Source.Entities;
using Project7.Source.Map;
using SharpDX.Direct3D9;

namespace Project7
{
    public partial class Game1 : Game
    {
        public Map Map;
        public EntityManager EntityManager;

        void LoadUpdate()
        {
            Init_Map();
            Init_Entities();
        }
        void Init_Map()
        {
            Map = new Map(2, (int)(ScreenWidth / tilesize / scale), (int)(ScreenHeight / tilesize / scale));

            for (int x = 0; x < Map.w; x++)
            {
                for (int y = 0; y < Map.h; y++)
                {
                    Map.Tiles[0, x, y] = 0;
                }
            }
            for (int x = 2; x < 14; x++)
            {
                Map.SetTile(1, x, 2, 0);
                Map.SetTile(1, x, 9, 0);
            }
            for (int y = 2; y < 10; y++)
            {
                Map.SetTile(1, 2, y, 0);
                Map.SetTile(1, 13, y, 0);
            }
        }
        void Init_Entities()
        {
            EntityManager = new EntityManager();
            //EntityFactory.CreateRabbit(10, 10);
        }
        void Update()
        {
            EntityManager.Update();
        }
    }
}