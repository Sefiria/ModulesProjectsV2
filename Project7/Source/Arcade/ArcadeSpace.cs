using Microsoft.Xna.Framework.Graphics;
using Project7.Source.Arcade.Common;
using Project7.Source.Arcade.scenes.space;
using System;
using System.Collections.Generic;
using System.Linq;
using static Project7.Source.Arcade.scenes.space.Enums;

namespace Project7.Source.Arcade
{
    public class ArcadeSpace : IArcade
    {
        public static ArcadeMain Context => ArcadeMain.instance;
        public static List<Entity> Entities => Context.EntityManager.Entities;
        public static CollisionManager CollisionManager => Context.CollisionManager;

        public string Name => "SPACE";
        public Starship Starship;
        Dictionary<PowerUps, int> PowerUpsSpawned = new Dictionary<PowerUps, int>();

        public void Initialize()
        {
            ArcadeMain.instance.Textures.Add(Texture2D.FromFile(Game1.Instance.GraphicsDevice, assets_bindings.Resources["arcade/space/starship"]));
            ArcadeMain.instance.Textures.Add(Texture2D.FromFile(Game1.Instance.GraphicsDevice, assets_bindings.Resources["arcade/space/mobship"]));
            ArcadeMain.instance.Textures.Add(Texture2D.FromFile(Game1.Instance.GraphicsDevice, assets_bindings.Resources["arcade/space/bullet"]));
            ArcadeMain.instance.Textures.Add(Texture2D.FromFile(Game1.Instance.GraphicsDevice, assets_bindings.Resources["arcade/space/pu_zqsd"]));
            Starship = new Starship()
            {
                X = Game1.Instance.ScreenWidth / 2,
                Y = Game1.Instance.ScreenHeight / 2,
            };
        }
        public void Update()
        {
            MobSpawn();
            CollisionManager.Update();
            PowerUps();
        }

        private void MobSpawn()
        {
            if (Entities.Count(x => x is MobShip) < 10 && Random.Shared.Next(10) == Random.Shared.Next(100))
            {
                var e = new MobShip();
                int w = Game1.Instance.ScreenWidth;
                int h = Game1.Instance.ScreenHeight;
                switch (Random.Shared.Next(4))
                {
                    case 0: e.X = -20; e.Y = Random.Shared.Next(h); break;
                    case 1: e.X = w + 20; e.Y = Random.Shared.Next(h); break;
                    case 2: e.Y = -20; e.X = Random.Shared.Next(w); break;
                    case 3: e.Y = h + 20; e.X = Random.Shared.Next(w); break;
                }
            }
        }
        private void PowerUps()
        {
            if(Starship.killcount >= 10 && !PowerUpsSpawned.ContainsKey(Enums.PowerUps.Zqsd) && Random.Shared.Next(1000) == 500)
            {
                PowerUpsSpawned[Enums.PowerUps.Zqsd] = 1;
                var x = Random.Shared.Next(50, Game1.Instance.ScreenWidth - 50);
                var y = Random.Shared.Next(50, Game1.Instance.ScreenHeight - 50);
                new PowerUp(Enums.PowerUps.Zqsd, x, y);
            }
        }

        public void Draw(GraphicsDevice graphics)
        {
        }
    }
}
