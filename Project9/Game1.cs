using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Project9
{
    public class Game1 : Game
    {
        public static Game1 Instance;

        public GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;

        public MAP _map;
        public ChunkManager _chunks;
        public PCA _pca;
        public double _updateTimer = 0;
        public const double UpdateInterval = 0.5;

        public int _cellSize = 32; // 32x32 pixels par cellule
        public Texture2D _pixel;   // 1x1 blanc pour tout dessiner

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            Instance = this;
            _map = new MAP();
            _chunks = new ChunkManager(_map);
            _pca = new PCA();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _updateTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (_updateTimer >= UpdateInterval)
            {
                _pca.Update(gameTime, _chunks);
                _updateTimer = 0;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            DrawMap();
            DrawPca();

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawMap()
        {
            for (int y = 0; y < MAP.Height; y++)
            {
                for (int x = 0; x < MAP.Width; x++)
                {
                    var cellPos = new Point(x, y);
                    var cell = _map.GetCell(cellPos);

                    Color fill = Color.Black;
                    switch (cell)
                    {
                        case CellType.Floor: fill = Color.Black; break;
                        case CellType.FoodVegetable: fill = Color.Green; break;
                        case CellType.FoodMeat: fill = Color.SaddleBrown; break;
                        case CellType.Danger: fill = Color.Red; break;
                        case CellType.Empty: fill = Color.Transparent; break;
                    }

                    var rect = new Rectangle(
                        x * _cellSize,
                        y * _cellSize,
                        _cellSize,
                        _cellSize);

                    // fond
                    if (fill.A > 0)
                        _spriteBatch.Draw(_pixel, rect, fill);

                    // encadré
                    _spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Y, rect.Width, 1), Color.Gray);
                    _spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Bottom - 1, rect.Width, 1), Color.Gray);
                    _spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Y, 1, rect.Height), Color.Gray);
                    _spriteBatch.Draw(_pixel, new Rectangle(rect.Right - 1, rect.Y, 1, rect.Height), Color.Gray);
                }
            }
        }

        private void DrawPca()
        {
            var rect = new Rectangle(
                _pca.Position.X * _cellSize,
                _pca.Position.Y * _cellSize,
                _cellSize,
                _cellSize);

            _spriteBatch.Draw(_pixel, rect, Color.Cyan * 0.6f);
        }
    }
}
