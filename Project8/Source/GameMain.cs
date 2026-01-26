using GeonBit.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project8.Editor;
using System.IO;
using Tools.Inputs;

namespace Project8
{
    public partial class GameMain : Game
    {
        public static GameMain Instance { get; private set; }

        public static int ScreenWidth => 1024;
        public static int ScreenHeight => 720;


        private GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public static KB KB = new KB();
        public static MS MS = new MS();
        public long Ticks = 0;
        public Texture2D TexCursor;
        public SpriteFont font;

        public GameMain()
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.IsBorderless = false;
        }
        protected override void Initialize()
        {
            int w = ScreenWidth;
            int h = ScreenHeight + EditorManager.EditorUIBox.Height;
            graphics.PreferredBackBufferWidth = w;
            graphics.PreferredBackBufferHeight = h;
            graphics.IsFullScreen = false;
            System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.FromPoint(System.Windows.Forms.Cursor.Position);
            Window.Position = new Point(screen.Bounds.X + screen.Bounds.Width / 2 - w / 2, screen.Bounds.Y + screen.Bounds.Height / 2 - h / 2);
            graphics.ApplyChanges();
            UserInterface.Initialize(Content, BuiltinThemes.hd);
            UserInterface.Active.UseRenderTarget = true;
            UserInterface.Active.IncludeCursorInRenderTarget = false;
            UserInterface.Active.ShowCursor = false;
            spriteBatch = spriteBatch ?? new SpriteBatch(GraphicsDevice);
            Graphics.Graphics.Instance.Initialize(GraphicsDevice, spriteBatch);
            base.Initialize();

            font = Resources.Instance.Fonts[0];
            TexCursor = Texture2D.FromFile(GraphicsDevice, Path.Combine(GlobalPaths.UI, "cursor.png"));
            ResourcesLoader.Load(GraphicsDevice);
            LoadSFX();
            LoadUpdate();
            LoadDraw();
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KB.Update();
            MS.Update();

            Update();

            UserInterface.Active.Update(gameTime);

            base.Update(gameTime);

            Ticks++;

            KB.CheckReleases();
        }
        protected override void Draw(GameTime gameTime)
        {
            if (EditorManager.OtherEditor)
                return;

            UserInterface.Active.Draw(spriteBatch);

            // == AlphaBlend

            Graphics.Graphics.Instance.BeginDraw(Color.Black, BlendState.AlphaBlend);
            Draw_Tiles();
            Graphics.Graphics.Instance.EndDraw();

            // == NonPremultiplied

            Graphics.Graphics.Instance.BeginDraw(null, BlendState.NonPremultiplied);
            Draw_Entities();
            Draw_Particles();
            Draw_UI();
            Graphics.Graphics.Instance.EndDraw();

            UserInterface.Active.DrawMainRenderTarget(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
