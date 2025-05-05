using GeonBit.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tools.Inputs;

namespace Project3
{
    public partial class Game1 : Game
    {
        public static Game1 Instance { get; private set; }

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        public static KB KB = new KB();
        public static MS MS = new MS();
        public long Ticks = 0;
        public Texture2D tex_tilemap = null;
        int screenWidth, screenHeight;

        public Game1()
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.IsBorderless = true;
        }
        protected override void Initialize()
        {
            screenWidth = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            screenHeight = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            UserInterface.Initialize(Content, BuiltinThemes.hd);
            UserInterface.Active.UseRenderTarget = true;
            UserInterface.Active.IncludeCursorInRenderTarget = false;
            spriteBatch = spriteBatch ?? new SpriteBatch(GraphicsDevice);
            Graphics.Graphics.Instance.Initialize(GraphicsDevice, spriteBatch);
            InitializeUI();
            InitUpdate();
            InitDraw();
            base.Initialize();
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            Update();

            KB.Update();
            MS.Update();

            UpdateUI();

            UserInterface.Active.Update(gameTime);
            base.Update(gameTime);

            Ticks++;
        }
        protected override void Draw(GameTime gameTime)
        {
            UserInterface.Active.Draw(spriteBatch);
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            Draw();
            spriteBatch.End();
            UserInterface.Active.DrawMainRenderTarget(spriteBatch);
            base.Draw(gameTime);
        }
    }
}
