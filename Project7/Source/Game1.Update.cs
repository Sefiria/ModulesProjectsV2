using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project7.Source.Entities;
using Project7.Source.Particles;
using Project7.Source.Entities.Behaviors;
using Project7.Source.Events;
using Project7.Source.Map;
using Tools;

namespace Project7
{
    public partial class Game1 : Game
    {
        public Map Map;
        public EntityManager EntityManager;
        public EventManager EventManager;
        public ParticleManager ParticleManager;

        void LoadUpdate()
        {
            Init_Map();
            Init_Entities();
            Init_Particles();
            Init_Events();
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
        }
        void Init_Particles()
        {
            ParticleManager = new ParticleManager();
        }
        void Init_Events()
        {
            EventManager = new EventManager();
            new GiftEvent(
                "Voici un cadeau de bienvenue, attrape-le (drag&drop) avec ton curseur afin de le placer quelque part sur la map !",
                GraphicsDevice.CropTexture2D(assets_bindings.Resources["pinou_idle"], 0, 0, 32, 32),
                () => { (EntityFactory.CreateRabbit(MS.X, MS.Y).Behaviors[0] as BehaviorRabbit).Held = true; },
                panel_size: new Vector2(480, 240),
                dragdrop_panel_size: new Vector2(64, 64),
                dragdrop_panel_offset: new Vector2(0, 56),
                img_size: new Vector2(32),
                img_offset: new Vector2(-8, -8)
            );
        }
        void Update()
        {
            EntityManager.Update();
            ParticleManager.Update();
            EventManager.Update();
        }
    }
}