using Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Security.Principal;
using Tools.Inputs;

namespace Project6
{
    public partial class Game1 : Game
    {
        public static Game1 Instance { get; private set; }

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        public static KB KB = new KB();
        public static MS MS = new MS();
        public long Ticks = 0;
        int screenWidth, screenHeight;

        public Game1()
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.IsBorderless = false;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            //screenWidth = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            //screenHeight = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            screenWidth = 640;
            screenHeight = 640;
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            spriteBatch = spriteBatch ?? new SpriteBatch(GraphicsDevice);
            Graphics.Graphics.Instance.Initialize(GraphicsDevice, spriteBatch);
            InitUpdate();
            InitDraw();
            base.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            update(gameTime);

            KB.Update();
            MS.Update();

            base.Update(gameTime);

            Ticks++;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            draw(gameTime);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
