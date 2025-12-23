using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project7.Source.Arcade.Common;
using Project7.Source.Arcade.scenes.space;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tools;

namespace Project7.Source.Arcade.scenes.plateform
{
    public partial class ArcadePlateform : IScene
    {
        public static ArcadeMain Context => ArcadeMain.instance;
        public static List<Entity> Entities => Context.EntityManager.Entities;
        public static CollisionManager CollisionManager => Context.CollisionManager;

        public string Name => "PLATEFORM";

        public Map map;
        public List<Point> Playables;
        public Character Player;

        private Texture2D tileset = Texture2D.FromFile(Game1.Instance.GraphicsDevice, assets_bindings.Resources["arcade/plateform/tileset"]);
        private Point center_screen = new Point(Game1.Instance.ScreenWidth / 2, Game1.Instance.ScreenHeight / 2);

        public void Initialize()
        {
            ArcadeMain.instance.Textures.AddRange(Game1.Instance.GraphicsDevice.SplitTexture(assets_bindings.Resources["arcade/plateform/tileset"], 8, 8));

            if (ArcadeMain.instance.edit)
            {
                EditInitialize();
            }
            else
            {
                map = new Map();
                if((Playables = map.FindAllCharactersPositions()).Count > 0)
                    Player = new Character(Playables.FirstOrDefault());
                foreach(var playable in Playables)
                    map.Set(playable.X, playable.Y, 0);
            }
        }
        public void Update()
        {
            if (ArcadeMain.instance.edit)
            {
                EditUpdate();
            }
            else
            {
                Player.Update();

                CollisionManager.Update();
            }
        }
        public void Draw(GraphicsDevice graphics)
        {
            graphics.Clear(new Color(123, 150, 155));

            if (map != null)
            {
                for (int i = 0; i < map.w; i++)
                    for (int j = 0; j < map.h; j++)
                        Graphics.Graphics.Instance.DrawTexture(Context.Textures[map.Tiles[i][j] + 1], 64 + i * TSZ_SCALED, 64 + j * TSZ_SCALED, rotation: 0F, scale: SCALE, flipX: false);
            }

            if (ArcadeMain.instance.edit)
            {
                EditDraw(graphics);
            }
            else
            {
                Player.Draw(graphics);
            }
        }

        public void Dispose()
        {
            if (ArcadeMain.instance.edit)
            {
                EditDispose();
            }
            else
            {
            }
        }
    }
}
