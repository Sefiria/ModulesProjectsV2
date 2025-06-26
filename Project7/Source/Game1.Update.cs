using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project7.Source.Entities;
using Project7.Source.Particles;
using Project7.Source.Entities.Behaviors;
using Project7.Source.Events;
using Project7.Source.Map;
using Tools;
using GeonBit.UI.Entities;
using System.Security.Policy;
using Entity = Project7.Source.Entities.Entity;
using System.Linq;

namespace Project7
{
    public partial class Game1 : Game
    {
        public Map Map;
        public EntityManager EntityManager;
        public EventManager EventManager;
        public ParticleManager ParticleManager;
        public QuestManager QuestManager;

        public string pinou_gift_name = "pinou_gift_name";

        void LoadUpdate()
        {
            Init_Map();
            Init_Entities();
            Init_Particles();
            Init_Events();
            Init_Quests();
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
                () => { (EntityFactory.CreateRabbit(MS.X, MS.Y, pinou_gift_name).Behaviors[0] as BehaviorRabbit).Held = true; },
                panel_size: new Vector2(480, 240),
                dragdrop_panel_size: new Vector2(64, 64),
                dragdrop_panel_offset: new Vector2(0, 56),
                img_size: new Vector2(32),
                img_offset: new Vector2(-8, -8)
            );
        }
        void Init_Quests()
        {
            QuestManager = new QuestManager();
            QuestManager.AddQuest("Apprivoise le pinou 0/1", (quest) =>
            {
                if (!quest.Success && (Entity.GetByName(pinou_gift_name)?.Behaviors.FirstOrDefault() as BehaviorRabbit)?.Trust == 1F)
                    quest.Validate("Apprivoise le pinou 1/1");
            });
            QuestManager.AddQuest("Ticks d'existence 0/1000", (quest) =>
            {
                quest.SetText($"Ticks d'existence {Ticks}/1000");
                if (Ticks >= 1000)
                    quest.Validate();
            });
        }
        void Update()
        {
            EntityManager.Update();
            ParticleManager.Update();
            EventManager.Update();
            QuestManager.Update();
        }
    }
}