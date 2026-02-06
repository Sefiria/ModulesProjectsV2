using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tools.Inputs;

namespace Project10
{
    public class Game1 : Game
    {
        public static Game1 Instance;

        public GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;

        public MAP _map;
        public PCA _pca;
        public int _cellSize = 16;
        public Texture2D _pixel;
        public SpriteFont _font;
        public BasicEffect basicEffect;
        public static KB KB = new KB();
        public static MS MS = new MS();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            int tcw = _cellSize * MAP.Width;
            int tch = _cellSize * MAP.Height;
            _graphics.PreferredBackBufferWidth = tcw;
            _graphics.PreferredBackBufferHeight = tch;
            _graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            Instance = this;
            _map = new MAP();
            _pca = new PCA();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _font = Content.Load<SpriteFont>("Ui");
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData(new[] { Color.White });

            basicEffect = new BasicEffect(GraphicsDevice)
            {
                VertexColorEnabled = true,
                World = Matrix.Identity,
                View = Matrix.CreateLookAt(new Vector3(0, 0, 1f), Vector3.Zero, Vector3.Up),
                Projection = Matrix.CreateOrthographicOffCenter
                    (
                        0,
                        GraphicsDevice.Viewport.Width,
                        GraphicsDevice.Viewport.Height,
                        0,
                        1f,
                        1000f
                    )
            };

        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _pca.Update(gameTime);

            KB.Update();
            MS.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            DrawMap();
            _pca.SpriteDraw(gameTime);
            //DrawGenomePanel();
            _spriteBatch.End();

            _pca.ShaderDraw(gameTime);

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
                        case CellType.Wall: fill = Color.DimGray; break;
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
                    //var c = new Color(50, 50, 50);
                    //_spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Y, rect.Width, 1), c);
                    //_spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Bottom - 1, rect.Width, 1), c);
                    //_spriteBatch.Draw(_pixel, new Rectangle(rect.X, rect.Y, 1, rect.Height), c);
                    //_spriteBatch.Draw(_pixel, new Rectangle(rect.Right - 1, rect.Y, 1, rect.Height), c);
                }
            }
        }

        private void DrawGenomePanel()
        {
            //int TOP_N = 24;
            //var ordered = _pca.Genome.Genes
            //    .Select((g, i) => (g, i))
            //    .OrderByDescending(t => t.g.Weight)
            //    .Take(TOP_N)
            //    .ToList();

            //int panelX = MAP.Width * _cellSize;   // 640
            //int panelW = _graphics.PreferredBackBufferWidth - panelX; // 160
            //var panelRect = new Rectangle(panelX, 0, panelW, _graphics.PreferredBackBufferHeight);

            //// fond du panneau
            //_spriteBatch.Draw(_pixel, panelRect, new Color(0, 0, 0, 200));

            //// titre
            //var pos = new Vector2(panelX + 8, 8);
            //_spriteBatch.DrawString(_font, "Genome", pos, Color.White);
            //pos.Y += 22;

            //// construire les lignes du génome
            //// Type: 0=Move,1=Eat,2=See,3=Talk,4=LifeSpan
            //for (int i = 0; i < ordered.Count; i++)
            //{
            //    var g = ordered[i].g;

            //    string typeName = g.Type switch
            //    {
            //        0 => "Move",
            //        1 => "Eat",
            //        2 => "See",
            //        3 => "Talk",
            //        4 => "Life",
            //        _ => $"T{g.Type}"
            //    };

            //    // pour Move/See: ParamA=dir (0..7), ParamB=dist/speed
            //    // pour Eat: ParamA=0 herb,1 meat,2 omni ; ParamB=intensité
            //    // pour Talk: ParamA=symbol ; ParamB=-
            //    string line = g.Type switch
            //    {
            //        0 => $"{i:D2}  Move  dir={Helper.DirName(g.ParamA)} v={g.ParamB}  W={g.Weight:F2}",
            //        2 => $"{i:D2}  See   dir={Helper.DirName(g.ParamA)} r={g.ParamB}  W={g.Weight:F2}",
            //        1 => $"{i:D2}  Eat   mode={(g.ParamA == 0 ? "Herb" : g.ParamA == 1 ? "Meat" : "Omni")}  W={g.Weight:F2}",
            //        3 => $"{i:D2}  Talk  sym={g.ParamA}        W={g.Weight:F2}",
            //        4 => $"{i:D2}  Life  B={g.ParamB}          W={g.Weight:F2}",
            //        _ => $"{i:D2}  T{g.Type} A={g.ParamA} B={g.ParamB} W={g.Weight:F2}"
            //    };


            //    // couleur indicative par type
            //    var color = g.Type switch
            //    {
            //        0 => Color.CornflowerBlue, // Move
            //        1 => Color.LimeGreen,      // Eat
            //        2 => Color.Gold,           // See
            //        3 => Color.Violet,         // Talk
            //        4 => Color.OrangeRed,      // Life
            //        _ => Color.White
            //    };

            //    _spriteBatch.DrawString(_font, line, pos, color);
            //    pos.Y += 18;
            //    if (pos.Y > panelRect.Height - 20) break; // éviter de déborder
            //}

            //// Optionnel : stats rapides
            //pos.Y += 8;
            //_spriteBatch.DrawString(_font, $"Hunger: {_pca.hunger:F0}", pos, Color.LightGray); pos.Y += 16;
            //_spriteBatch.DrawString(_font, $"Social: {_pca.social:F0}", pos, Color.LightGray); pos.Y += 16;
            //_spriteBatch.DrawString(_font, $"Stress: {_pca.stress:F2}", pos, Color.LightGray); pos.Y += 32;

            //// action en cours
            //_spriteBatch.DrawString(_font, $"Action: {_pca.CurrentAction}", pos, Color.Yellow); pos.Y += 16;
        }
    }
}
