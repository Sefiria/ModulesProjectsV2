using GeonBit.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project7.Source.Arcade;
using SharpDX.Direct3D9;
using Tools.Inputs;

namespace Project7
{
    public partial class Game1 : Game
    {
        public static Game1 Instance { get; private set; }

        private GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public static KB KB = new KB();
        public static MS MS = new MS();
        public long Ticks = 0;
        public Texture2D tex_tilemap = null;
        public int ScreenWidth = 1024;
        public int ScreenHeight = 720;
        public ArcadeMain Arcade;



        bool arcade_running = false;
        public void RunArcade()
        {
            UserInterface.Active.Root.Visible = false;
            arcade_running = true;
            if (Arcade != null)
                return;
            Arcade = new ArcadeMain();
            Arcade.Initialize();
        }
        public void ExitArcade()
        {
            arcade_running = false;
            Arcade = null;
            UserInterface.Active.Root.Visible = true;
        }



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
            Graphics.Graphics.Instance.Initialize(GraphicsDevice, spriteBatch);
            base.Initialize();

            ResourcesLoader.Load(GraphicsDevice);
            LoadSFX();
            LoadUpdate();
            LoadDraw();
            InitializeUI();
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KB.Update();
            MS.Update();

            if (arcade_running)
            {
                Arcade.Update();
            }
            else
            {
                Update();
                UpdateUI();
            }

            UserInterface.Active.Update(gameTime);

            base.Update(gameTime);

            Ticks++;
        }
        protected override void Draw(GameTime gameTime)
        {
            UserInterface.Active.Draw(spriteBatch);

            if (arcade_running)
            {
                Graphics.Graphics.Instance.BeginDraw(null, BlendState.NonPremultiplied);
                Arcade.Draw(GraphicsDevice);
                Graphics.Graphics.Instance.EndDraw();
            }
            else
            {
                // == AlphaBlend

                Graphics.Graphics.Instance.BeginDraw(Color.Black, BlendState.AlphaBlend);
                Draw_Tiles();
                Graphics.Graphics.Instance.EndDraw();

                // == NonPremultiplied

                Graphics.Graphics.Instance.BeginDraw(null, BlendState.NonPremultiplied);
                Draw_Entities();
                Draw_Particles();
                Draw_Events();

                Graphics.Graphics.Instance.EndDraw();
            }

            UserInterface.Active.DrawMainRenderTarget(spriteBatch);

            // Draw Cursor
            if (!UserInterface.Active.ShowCursor)
            {
                Graphics.Graphics.Instance.BeginDraw(null, BlendState.NonPremultiplied);
                spriteBatch.Draw(cursor_texture, new Vector2(MS.X - 8, MS.Y - 16), null, Color.White, 0F, Vector2.Zero, 1F, SpriteEffects.None, 0F);
                Graphics.Graphics.Instance.EndDraw();
            }

            base.Draw(gameTime);
        }
    }
}
