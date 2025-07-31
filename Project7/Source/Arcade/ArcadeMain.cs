using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project7.Source.Arcade.Common;
using Project7.Source.Arcade.scenes.menu;
using Project7.Source.Arcade.scenes.plateform;
using Project7.Source.Arcade.scenes.space;
using Project7.Source.Arcade.ui;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project7.Source.Arcade
{
    public class ArcadeMain
    {
        public static Game1 context => Game1.Instance;
        public static Graphics.Graphics graphics => Graphics.Graphics.Instance;

        public static ArcadeMain instance;
        public static List<IScene> Scenes = new List<IScene>()
        {
            new ArcadeMenu(),
            new ArcadeSpace(),
            new ArcadePlateform(),
        };
        public List<UI> UI;
        public int prev_scene_index = 0, scene_index = 0;
        public List<Texture2D> Textures;
        public EntityManager EntityManager;
        public CollisionManager CollisionManager;

        public bool edit = false;
        public ArcadeMain()
        {
            instance = this;
            UI = new List<UI>();
            Textures = new List<Texture2D>();
            EntityManager = new EntityManager();
            CollisionManager = new CollisionManager();
        }
        public void Initialize()
        {
            UI.Clear();
            Textures.Clear();
            Textures.Add(Texture2D.FromFile(Game1.Instance.GraphicsDevice, assets_bindings.Resources["arcade/null"]));
            Scenes[scene_index].Initialize();
            if (scene_index != 0)
                UI.Add(new Button(80, 80, "Retour", () => { Scene_DisposeCommon(); scene_index = 0; }));
        }
        public void Update()
        {
            if(prev_scene_index != scene_index)
            {
                prev_scene_index = scene_index;
                Initialize();
            }

            Scenes[scene_index].Update();
            EntityManager.Update();

            var list = new List<UI>(UI);
            foreach(UI ui in list)
            {
                if (ui.exists)
                    ui.update();
                else
                    UI.Remove(ui);
            }
        }
        public void Draw(GraphicsDevice gd)
        {
            Scenes[scene_index].Draw(gd);
            EntityManager.Draw(gd);

            new List<UI>(UI).Where(ui => ui.exists).ToList().ForEach(ui => ui.draw(gd));

            for (int i = 0; i < context.ScreenHeight; i += 16)
                graphics.DrawLine(0, i, context.ScreenWidth, i, new Color(Color.DimGray, (byte)(30 * (Random.Shared.Next(10)) / 10F)), 4);
            graphics.DrawRectangle(0, 0, context.ScreenWidth, context.ScreenHeight, new Color(30, 30, 30), 64);
        }

        public void Scene_DisposeCommon()
        {
            Scenes[scene_index].Dispose();
            EntityManager.Entities.Clear();
        }
    }
}
