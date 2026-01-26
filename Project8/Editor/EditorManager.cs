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
using System.IO;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Project8.Source.Entities;

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
        public enum EditorModes { Map, Entities, Triggers }
        public static EditorModes EditorMode = EditorModes.Map;
        

        public static void Init(GraphicsDevice GraphicsDevice)
        {
            play_stop_button_tex = Texture2D.FromFile(GraphicsDevice, Path.Combine(GlobalPaths.UI, "play_stop_button.png"));
            UIButtons = new Dictionary<string, UIButton>()
            {
                ["playstop"]         = new UIButton(new Rectangle(16, EditorUIBox.Y + 48, 32, 32), () => { if(IsPlaying) DisposeTest(); else LoadTest(); IsPlaying = !IsPlaying; }),

                ["Tile Creator"]      = new UIButton(new Rectangle(16 + 160 * 0, EditorUIBox.Y + 48 + 32 * 1, 155, 32), OpenFormWithOwner<TileCreator.TileCreator>),
                ["TileSet Creator"]   = new UIButton(new Rectangle(16 + 160 * 1, EditorUIBox.Y + 48 + 32 * 1, 190, 32), OpenFormWithOwner<TileSetCreator.TileSetCreator>),
                ["TileSet Generator"]   = new UIButton(new Rectangle(16 + 177 * 2, EditorUIBox.Y + 48 + 32 * 1, 215, 32), OpenFormWithOwner<TileSetCreator.TileSetGenerator>),
                ["Entity Creator"]   = new UIButton(new Rectangle(16 + 200 * 3, EditorUIBox.Y + 48 + 32 * 1, 188, 32), OpenFormWithOwner<EntityCreator.EntityCreator>),

                ["Reset Map"]      = new UIButton(new Rectangle(16 + 160 * 0, EditorUIBox.Y + 48 + 32 * 2, 140, 32), () => Map.Reset()),
                ["Save Map"] = new UIButton(new Rectangle(16 + 160 * 1, EditorUIBox.Y + 48 + 32 * 2, 140, 32), () => Map.Save()),
                ["Load Map"]      = new UIButton(new Rectangle(16 + 160 * 2, EditorUIBox.Y + 48 + 32 * 2, 140, 32), () => Map.Load()),

                ["MapEdit Mode"]      = new UIButton(new Rectangle(16 + 160 * 0, EditorUIBox.Y + 48 + 32 * 3, 155, 32), () => EditorMode = EditorModes.Map),
                ["Entity  Mode"]      = new UIButton(new Rectangle(16 + 160 * 1, EditorUIBox.Y + 48 + 32 * 3, 155, 32), () => EditorMode = EditorModes.Entities),
                ["Trigger Mode"]      = new UIButton(new Rectangle(16 + 160 * 2, EditorUIBox.Y + 48 + 32 * 3, 155, 32), () => EditorMode = EditorModes.Triggers),
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
                if(EditorMode == EditorModes.Map)
                {
                    for (int i = 0; i < Tile.Tiles.Count; i++)
                    {
                        Tile t = Tile.Tiles.ElementAt(i).Value;
                        if (t.Tex != null)
                        {
                            g.DrawTexture(
                                t.Tex[0],
                                gap + i % tilesPerRow * (TSZ + gap),
                                gap + i / tilesPerRow * (TSZ + gap),
                                GameMain.Instance.scale,
                                new Rectangle(t.Mode == Tile.Modes.MultiTile ? t.MultiTileIndex * 16 : 0, 0, 16, 16)
                            );
                            if (i == tile_id + 1)
                            {
                                g.DrawRectangle(gap + i % tilesPerRow * (TSZ + gap), gap + i / tilesPerRow * (TSZ + gap), TSZ, TSZ, Color.Yellow, 2);
                            }
                        }
                    }
                }
                else if (EditorMode == EditorModes.Entities)
                {
                    // TODO
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
                    var thickness = ((b.Key == "MapEdit Mode" && EditorMode == EditorModes.Map)
                                  || (b.Key == "Entity Mode" && EditorMode == EditorModes.Entities)
                                  || (b.Key == "Trigger Mode" && EditorMode == EditorModes.Triggers)
                                  ) ? 3 : 1;
                    g.DrawRectangle(UIButtons[b.Key].Collider, Color.White, thickness);
                }

                // PlayTest part
                if (IsPlaying)
                {
                    DrawTest();
                }
                else if (EditorMode == EditorModes.Triggers)
                {
                    var g = Graphics.Graphics.Instance;
                    var mst = (ms.X / stsz, ms.Y / stsz).V();
                    var rate = 64;// speed of blinking
                    var mapped_value = 150F;// range of alpha (mapped_value < alpha < 255)
                    var tickmod = (ticks % rate) / (float)rate;
                    float raw_alpha = tickmod < 0.5f ? tickmod * 2f : (1f - tickmod) * 2f;
                    var alpha = (byte)(raw_alpha * (255f - mapped_value) + mapped_value);
                    if (mst.x >= 0 && mst.y >= 0 && mst.x < Map.w && mst.y < Map.h)
                        g.DrawRectangle(new Rectangle(mst.x * TSZ, mst.y * TSZ, TSZ, TSZ), Color.White, 1);
                    List<vec[]> list = new List<vec[]>(TriggerTiles)
                    {
                        TriggerTilesSelection
                    };
                    foreach (var tt in list)
                    {
                        if (tt[0] != vec.Null)
                            g.DrawRectangle(new Rectangle(tt[0].x * TSZ, tt[0].y * TSZ, TSZ, TSZ), new Color(Color.Yellow, alpha), 4);
                        if (tt[1] != vec.Null)
                            g.DrawRectangle(new Rectangle(tt[1].x * TSZ, tt[1].y * TSZ, TSZ, TSZ), new Color(Color.Red, alpha), 4);
                        if (tt[0] != vec.Null && tt[1] != vec.Null)
                            g.DrawLine(
                                start_x: tt[0].x * TSZ + TSZ / 2,
                                start_y: tt[0].y * TSZ + TSZ / 2,
                                end_x: tt[1].x * TSZ + TSZ / 2,
                                end_y: tt[1].y * TSZ + TSZ / 2,
                                color: new Color(tt == TriggerTilesSelection ? Color.White : Color.Gray, (byte)((255f + mapped_value) - alpha)),
                                thickness: 2);
                    }
                }
            }
        }

        static Point old_ms, ms;
        static float stsz = GlobalVariables.tilesize * GlobalVariables.scale;
        static int tile_id = 0;
        static KB KB => GameMain.KB;
        static MS MS => GameMain.MS;
        static TiledMap Map => GameMain.Instance.Map;
        static bool rTab;
        static vec[] TriggerTilesSelection = [vec.Null, vec.Null];
        static List<vec[]> TriggerTiles = new List<vec[]>();

        public static void Update()
        {
            if (!GameMain.Instance.IsActive)
                return;

            if (OtherEditor)
                return;

            ms = MS.Position;

            if (TabMenu)
            {
                if (MS.IsLeftDown)
                {
                    if (EditorMode == EditorModes.Map)
                    {
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
                    else if (EditorMode == EditorModes.Entities)
                    {
                        // TODO
                    }
                }
            }
            else if(EditorMode == EditorModes.Map)
            {
                // *=*=* MAPEDIT MODE *=*=*
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
            }
            else if(EditorMode == EditorModes.Entities)
            {
                // TODO
            }
            else if(EditorMode == EditorModes.Triggers)
            {
                var mst = (ms.X / stsz, ms.Y / stsz).V();
                if (mst.x >= 0 && mst.y >= 0 && mst.x < Map.w && mst.y < Map.h)
                {
                    // *=*=* TRIGGER MODE *=*=*
                    if (MS.IsLeftPressed)
                    {
                        if (TriggerTilesSelection[0] == vec.Null)
                            TriggerTilesSelection[0] = mst;
                        else if (TriggerTilesSelection[1] == vec.Null)
                        {
                            TriggerTilesSelection[1] = mst;
                            TriggerTiles.Add([TriggerTilesSelection[0], TriggerTilesSelection[1]]);
                            TriggerTilesSelection = [vec.Null, vec.Null];
                        }
                    }
                    else if (MS.IsRightPressed)
                    {
                        if (TriggerTilesSelection[0] == vec.Null && TriggerTilesSelection[1] == vec.Null && TriggerTiles.Count > 0)
                            TriggerTilesSelection = TriggerTiles[TriggerTiles.Count - 1];
                        if (TriggerTilesSelection[1] != vec.Null)
                            TriggerTilesSelection[1] = vec.Null;
                        else if (TriggerTilesSelection[0] != vec.Null)
                        {
                            TriggerTilesSelection[0] = vec.Null;
                            var item = TriggerTiles.FirstOrDefault(t => t[0] == vec.Null && t[1] == vec.Null);
                            if (item != null)
                            {
                                var id = TriggerTiles.IndexOf(item);
                                if (id != -1)
                                {
                                    TriggerTiles.RemoveAt(id);
                                    if (TriggerTiles.Count > 0)
                                        TriggerTilesSelection = TriggerTiles[TriggerTiles.Count - 1];
                                }
                            }
                        }
                    }
                    else if (MS.IsMiddlePressed)
                    {
                        var selected = TriggerTiles.FirstOrDefault(t => t.Any(v => new Rectangle(v.x * TSZ, v.y * TSZ, TSZ, TSZ).Contains(ms)));
                        if (selected != null)
                        {
                            var temp = new vec[] { selected[0], selected[1] };
                            TriggerTiles.Remove(selected);
                            TriggerTiles.Add(temp);
                            TriggerTilesSelection = temp;
                        }
                    }
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

            //Tab
            if (KB.IsKeyPressed(Keys.Tab) && EditorMode != EditorModes.Triggers)
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
            EntityFactory.CreateCollectible_Ammo(3, 20);
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
