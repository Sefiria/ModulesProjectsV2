using GeonBit.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tools.Inputs;

namespace Project7
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
        public int ScreenWidth = 1024;
        public int ScreenHeight = 720;

        public Game1()
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.IsBorderless = false;
        }
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.IsFullScreen = false;
            System.Windows.Forms.Screen screen = System.Windows.Forms.Screen.FromPoint(System.Windows.Forms.Cursor.Position);
            Window.Position = new Point(screen.Bounds.X + screen.Bounds.Width / 2 - ScreenWidth / 2, screen.Bounds.Y + screen.Bounds.Height / 2 - ScreenHeight / 2);
            graphics.ApplyChanges();
            UserInterface.Initialize(Content, BuiltinThemes.hd);
            UserInterface.Active.UseRenderTarget = true;
            UserInterface.Active.IncludeCursorInRenderTarget = false;
            UserInterface.Active.ShowCursor = false;
            spriteBatch = spriteBatch ?? new SpriteBatch(GraphicsDevice);
            InitializeUI();
            Graphics.Graphics.Instance.Initialize(GraphicsDevice, spriteBatch);
            base.Initialize();

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
            UpdateUI();

            UserInterface.Active.Update(gameTime);
            base.Update(gameTime);

            Ticks++;
        }
        protected override void Draw(GameTime gameTime)
        {
            UserInterface.Active.Draw(spriteBatch);

            // == AlphaBlend

            Graphics.Graphics.Instance.BeginDraw(Color.Black, BlendState.AlphaBlend);
            Draw_Tiles();
            Graphics.Graphics.Instance.EndDraw();

            // == NonPremultiplied

            Graphics.Graphics.Instance.BeginDraw(null, BlendState.NonPremultiplied);
            Draw_Entities();
            // Draw Cursor
            Graphics.Graphics.Instance.DrawTexture(cursor_texture, MS.X - 8, MS.Y - 16, 0F, 1F, false);
            Graphics.Graphics.Instance.EndDraw();

            UserInterface.Active.DrawMainRenderTarget(spriteBatch);
            base.Draw(gameTime);
        }
    }
}
