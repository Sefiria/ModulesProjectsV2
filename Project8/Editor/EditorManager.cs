using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Project8.Source;
using Project8.Source.Map;
using Project8.Source.Runtime;
using Project8.Source.TiledMap;
using System;
using System.Collections.Generic;
using System.Linq;
using Tooling;
using Tools.Inputs;
using KB = Tools.Inputs.KB;
using Microsoft.Xna.Framework.Input;
using System.Threading;
using System.Windows.Forms;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Project8.Editor.TileCreator;

namespace Project8.Editor
{
    public class EditorManager
    {
        public class UIButton
        {
            public Rectangle Collider;
            public Action Behavior;
            public UIButton(Rectangle Collider, Action Behavior) { this.Collider = Collider; this.Behavior = Behavior; }
        }

        public static Rectangle EditorUIBox => new(0, GameMain.ScreenHeight, GameMain.ScreenWidth, 200);
        public static bool IsPlaying = false, TabMenu = false, OtherEditor = false;
        public static Texture2D play_stop_button_tex;
        public static Dictionary<string, UIButton> UIButtons;
        public static int SelectedLayer = 0;
        

        public static void Init(GraphicsDevice GraphicsDevice)
        {
            play_stop_button_tex = Texture2D.FromFile(GraphicsDevice, "Assets/UI/play_stop_button.png");
            UIButtons = new Dictionary<string, UIButton>()
            {
                ["playstop"]         = new UIButton(new Rectangle(16, EditorUIBox.Y + 48, 32, 32), () => { if(IsPlaying) DisposeTest(); else LoadTest(); IsPlaying = !IsPlaying; }),
                ["Tile Creator"]      = new UIButton(new Rectangle(16 + 160 * 0, EditorUIBox.Y + 48 + 32, 155, 32), OpenFormWithOwner<TileCreator.TileCreator>),
                ["TileSet Creator"]   = new UIButton(new Rectangle(16 + 160 * 1, EditorUIBox.Y + 48 + 32, 190, 32), OpenFormWithOwner<TileSetCreator.TileSetCreator>),
                ["TileSet Generator"]   = new UIButton(new Rectangle(16 + 177 * 2, EditorUIBox.Y + 48 + 32, 215, 32), OpenFormWithOwner<TileSetCreator.TileSetGenerator>),
            };
        }
        sealed class WindowHandleWrapper : IWin32Window
        {
            public WindowHandleWrapper(IntPtr handle) => Handle = handle;
            public IntPtr Handle { get; }
        }
        static void OpenFormWithOwner<T>() where T:Form
        {
            var t = new Thread(() =>
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var ownerForm = new Form { Opacity = 0, ShowInTaskbar = false };
                ownerForm.Load += (s, e) =>
                {
                    OtherEditor = true;
                    using (var dlg = Activator.CreateInstance<T>())
                        dlg.ShowDialog(ownerForm);
                    ownerForm.Close();
                    OtherEditor = false;
                };
                Application.Run(ownerForm);
            });
            t.SetApartmentState(ApartmentState.STA);
            t.Start();
        }


        public static Graphics.Graphics g => Graphics.Graphics.Instance;
        public static int TSZ => (int)(GlobalVariables.tilesize * GlobalVariables.scale);
        public static void Draw()
        {
            if (TabMenu)
            {
                int gap = 5;
                int tilesPerRow = Math.Max(1, (GameMain.ScreenWidth - gap) / (TSZ + gap));
                for (int i = 0; i < Tile.Tiles.Count; i++)
                {
                    Tile t = Tile.Tiles.ElementAt(i).Value;
                    if (t.Tex != null)
                    {
                        g.DrawTexture(
                            t.Tex[t.Mode == Tile.Modes.Autotile ? Tile.Tiles[i].id - 1 : 0],
                            gap + i % tilesPerRow * (TSZ + gap),
                            gap + i / tilesPerRow * (TSZ + gap),
                            GameMain.Instance.scale,
                            new Rectangle(t.Mode == Tile.Modes.MultiTile ? t.MultiTileIndex * 16 : 0, 0, 16, 16)
                        );
                        if(i == tile_id + 1)
                        {
                            g.DrawRectangle(gap + i % tilesPerRow * (TSZ + gap), gap + i / tilesPerRow * (TSZ + gap), TSZ, TSZ, Color.Yellow, 2);
                        }
                    }
                }
            }
            else
            {
                // Selected tile
                long ticks = GameMain.Instance.Ticks;
                var tile = Tile.Tiles[tile_id];
                var tex = tile.Tex;
                double period = 100D;
                float sin = (float)Math.Sin(ticks % period * 2 * Math.PI / period);
                g.DrawString($"Tile :               Layer : {SelectedLayer}", 16, EditorUIBox.Y + 16, GameMain.Instance.font, Color.White);
                g.DrawTexture(tex[0], GameMain.Instance.font.MeasureString("Tile :").X + 16, EditorUIBox.Y + 16 - 3 + sin * 4F, GameMain.Instance.scale, new Rectangle(tile.Mode == Tile.Modes.MultiTile ? tile.MultiTileIndex * 16 : 0, 0, 16, 16));

                // buttons
                // -- Play/Stop
                g.DrawTexture(play_stop_button_tex, 16, EditorUIBox.Y + 48, 1F, new Rectangle(IsPlaying ? 32 : 0, 0, 32, 32));
                // -- buttons
                foreach (var b in UIButtons.Skip(1))
                {
                    g.DrawString(b.Key, b.Value.Collider.X, b.Value.Collider.Y, GameMain.Instance.font, Color.White);
                    g.DrawRectangle(UIButtons[b.Key].Collider, Color.White, 1);
                }

                // PlayTest part
                if (IsPlaying)
                    DrawTest();
            }
        }

        static Point old_ms, ms;
        static float stsz = GlobalVariables.tilesize * GlobalVariables.scale;
        static int tile_id = 0;
        static KB KB => GameMain.KB;
        static MS MS => GameMain.MS;
        static TiledMap Map => GameMain.Instance.Map;
        static bool rTab;
        public static void Update()
        {
            if (!GameMain.Instance.IsActive)
                return;

            if (OtherEditor)
                return;

            if (TabMenu)
            {
                if (MS.IsLeftDown)
                {
                    var ms = MS.Position;
                    int gap = 5;
                    int tilesPerRow = Math.Max(1, (GameMain.ScreenWidth - gap) / (TSZ + gap));
                    if (ms.X < gap || ms.Y < gap) return;
                    int col = (ms.X - gap) / (TSZ + gap);
                    int row = (ms.Y - gap) / (TSZ + gap);
                    if (col < 0 || col >= tilesPerRow || row < 0) return;
                    int index = row * tilesPerRow + col;
                    if (index < 0 || index >= Tile.Tiles.Count) return;
                    int dxInCell = (ms.X - gap) % (TSZ + gap);
                    int dyInCell = (ms.Y - gap) % (TSZ + gap);
                    if (dxInCell >= TSZ || dyInCell >= TSZ) return;
                    tile_id = Tile.Tiles.Keys.ElementAt(index);
                }
            }
            else
            {
                // PlayTest part
                if (IsPlaying)
                {
                    if (MS.IsLeftPressed && EditorUIBox.Contains(MS.Position) && UIButtons["playstop"].Collider.Contains(MS.Position))
                        UIButtons["playstop"].Behavior();
                    else
                        UpdateTest();
                    return;
                }

                // Click in Map
                System.Drawing.PointF a = (ms.X / stsz, ms.Y / stsz).P();
                old_ms = ms;
                ms = MS.Position;
                if (MS.IsLeftDown || MS.IsRightDown)
                {
                    System.Drawing.PointF b = (old_ms.X / stsz, old_ms.Y / stsz).P();
                    float d = Maths.Distance(a, b);
                    if ((int)d == 0)
                        Map.SetTile(SelectedLayer, (int)a.X, (int)a.Y, MS.IsLeftDown ? tile_id : -1);
                    else
                    {
                        for (float t = 0F; t <= 1F; t += 1F / d)
                        {
                            Map.SetTile(SelectedLayer, (int)Maths.Lerp(b.X, a.X, t), (int)Maths.Lerp(b.Y, a.Y, t), MS.IsLeftDown ? tile_id : -1);
                        }
                    }
                }
                if (MS.IsMiddleDown)
                {
                    var id = Map[SelectedLayer, (int)a.X, (int)a.Y];
                    if (id > -1)
                        tile_id = id;
                }

                // Wheel
                if (MS.IsWheelScrolling)
                {
                    int add()
                    {
                        var id = Tile.Tiles.Keys.ToList().IndexOf(tile_id);
                        if (!Tile.Tiles.ContainsKey(id))
                            id = 0;
                        return id;
                    }
                    int sub()
                    {
                        var id = Tile.Tiles.Keys.ToList().IndexOf(tile_id) - 2;
                        if (id < 0)
                            id = Tile.Tiles.Keys.Except([-1, 255]).Order().Last();
                        return id;
                    }

                    if (KB.IsKeyDown(Keys.LeftControl))
                    {
                        SelectedLayer = Math.Min(Math.Max(0, SelectedLayer + (MS.ScrollWheelSignInt > 0 ? 1 : -1)), Map.z - 1);
                    }
                    else
                    {
                        tile_id = MS.ScrollWheelSignInt > 0 ? add() : sub();
                    }
                }

                // Click in EditorBox
                if (MS.IsLeftPressed && EditorUIBox.Contains(MS.Position))
                {
                    foreach (var btn in UIButtons.Values)
                    {
                        if (btn.Collider.Contains(MS.Position))
                            btn.Behavior();
                    }
                }
            }

            //Tab
            if (KB.IsKeyPressed(Keys.Tab))
                TabMenu = !TabMenu;
        }


        static int[,,] SavedMap;
        public static void LoadTest()
        {
            SavedMap = new int[Map.z, Map.w, Map.h];
            for (int z = 0; z < Map.z; z++)
                for (int x = 0; x < Map.w; x++)
                    for (int y = 0; y < Map.h; y++)
                        SavedMap[z, x, y] = Map.Tiles[z, x, y];
        }
        public static void DrawTest()
        {
        }
        public static void UpdateTest()
        {
            Map.Update();

            if (MS.IsLeftPressed)
            {
                var hit = (from x in Enumerable.Range(0, Map.Tiles.GetLength(1))
                           from y in Enumerable.Range(0, Map.Tiles.GetLength(2))
                           let tileIndex = Map.Tiles[0, x, y]
                           where Tile.Tiles[tileIndex].id == 4 // 4 should be player in tileset.json
                           let rect = new Rectangle(x * TSZ, y * TSZ, TSZ, TSZ)
                           where rect.Contains(MS.Position)
                           select new { x, y, tileIndex })
                    .FirstOrDefault();
                var e = GameMain.Instance.EntityManager.Entities.FirstOrDefault(e => e is Entitytest);
                if (hit != null && (e == null ? true : (hit.x != e.X / TSZ || e.Y / TSZ != hit.y)))
                {
                    if (e != null)
                    {
                        Map.Tiles[0, (int)((e.X + 16) / TSZ), (int)((e.Y + 16) / TSZ)] = 4;
                        e.Exists = false;
                    }
                    Map.Tiles[0, hit.x, hit.y] = -1;
                    var _e = new Entitytest(hit.x * TSZ, hit.y * TSZ);
                    _e.X += (TSZ - _e.W) / 2F;
                    _e.Y += (TSZ - _e.H) / 2F - 1;
                }
            }
        }
        public static void DisposeTest()
        {
            GameMain.Instance.EntityManager.Entities.Clear();
            GameMain.Instance.ParticleManager.Particles.Clear();
            for (int z = 0; z < Map.z; z++)
                for (int x = 0; x < Map.w; x++)
                    for (int y = 0; y < Map.h; y++)
                        Map.Tiles[z, x, y] = SavedMap[z, x, y];
        }
    }
}
