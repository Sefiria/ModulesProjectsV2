﻿using GeonBit.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tools.Inputs;

namespace Pixanim
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

        public Game1()
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.IsBorderless = true;
        }
        protected override void Initialize()
        {
            int _ScreenWidth = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            int _ScreenHeight = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            graphics.PreferredBackBufferWidth = _ScreenWidth;
            graphics.PreferredBackBufferHeight = _ScreenHeight;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            UserInterface.Initialize(Content, BuiltinThemes.hd);
            UserInterface.Active.UseRenderTarget = true;
            UserInterface.Active.IncludeCursorInRenderTarget = false;
            spriteBatch = spriteBatch ?? new SpriteBatch(GraphicsDevice);
            InitializeUI();
            base.Initialize();
        }
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KB.Update();
            MS.Update();

            UpdateUI();

            UserInterface.Active.Update(gameTime);
            base.Update(gameTime);

            Ticks++;
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            UserInterface.Active.Draw(spriteBatch);
            UserInterface.Active.DrawMainRenderTarget(spriteBatch);
            base.Draw(gameTime);
        }
    }
}
