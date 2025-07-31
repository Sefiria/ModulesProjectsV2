using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project7.Source.Arcade.ui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tools.Inputs;
using static Project7.Source.Arcade.scenes.plateform.Enums;
using static System.Net.Mime.MediaTypeNames;

namespace Project7.Source.Arcade.scenes.plateform
{
    public partial class ArcadePlateform : IScene
    {
        public static readonly int TSZ = 8;
        public static readonly int SCALE = 4;
        public static int TSZ_SCALED => TSZ * SCALE;

        public Button SaveButton, DoFillAirButton;
        public bool DoFillAir = true;

        int[][] edit_selection = [[(int)TexAssets.bricks]];
        Point edit_new_selection_origin = Point.Zero, edit_new_selection_start = Point.Zero, edit_new_selection_end = Point.Zero;

        int mstx => ((Game1.MS.Position.X - 64)) / TSZ_SCALED % map.w;
        int msty => ((Game1.MS.Position.Y - 64)) / TSZ_SCALED % map.h;

        public void EditInitialize()
        {
            map = new Map();

            SaveButton = new Button(
                x: Game1.Instance.ScreenWidth - 140,
                y: 80,
                text: "Save",
                map.Save
            );

            DoFillAirButton = new Button(
                SaveButton.x - 60,
                SaveButton.y + (int)SaveButton.text_size_in_pixels.Y + 15,
                "DoFillAir",
                () => { DoFillAir = !DoFillAir; DoFillAirButton.BackColor = DoFillAir ? new Color(50, 50, 0) : Color.Black; }
            );
        }

        public void EditUpdate()
        {
            if (new List<Button> { SaveButton, DoFillAirButton }.Any(ui => ui.GetBounds().Contains(Game1.MS.Position)))
                return;

            Point mst = new Point(mstx, msty);

            if (Game1.KB.IsKeyDown(Keys.Tab) && Game1.KB.IsKeyDown(Keys.Delete))
                edit_selection = [[0]];
            if (Game1.MS.IsLeftDown)
            {
                if (Game1.KB.IsKeyDown(Keys.Tab) && new Rectangle(center_screen.X - tileset.Width / 2 * SCALE, center_screen.Y - tileset.Height / 2 * SCALE, tileset.Width * SCALE, tileset.Height * SCALE).Contains(Game1.MS.Position))
                {
                    int x = ((Game1.MS.Position.X - center_screen.X + tileset.Width * 2)) / TSZ_SCALED % 16;
                    int y = ((Game1.MS.Position.Y - center_screen.Y + tileset.Height * 2)) / TSZ_SCALED % 16;
                    var index = y * 16 + x;
                    if(index < (int)Enum.GetValues(typeof(TexAssets)).Cast<TexAssets>().Max())
                        edit_selection = [[index]];
                }
                else
                {
                    if (mst.X >= 0 && mst.X < (Game1.Instance.ScreenWidth - 128) / TSZ_SCALED - 1 && mst.Y >= 0 && mst.Y < (Game1.Instance.ScreenHeight - 128) / TSZ_SCALED - 1)
                    {
                        for (int i = 0; i < edit_selection.Length; i++)
                            for (int j = 0; j < edit_selection[i].Length; j++)
                                if(DoFillAir || edit_selection[i][j] != 0)
                                    map.Set(mst.X + i, mst.Y + j, edit_selection[i][j]);
                    }
                }
            }
            if (!Game1.KB.IsKeyDown(Keys.Tab))
            {
                if (Game1.MS.IsRightPressed)
                {
                    edit_new_selection_origin = mst;
                    edit_new_selection_start = mst;
                    edit_new_selection_end = mst;
                }
                else if (Game1.MS.IsRightDown)
                {
                    if (mst.X == edit_new_selection_origin.X) edit_new_selection_start.X = edit_new_selection_end.X = mst.X;
                    else if (mst.X < edit_new_selection_origin.X) edit_new_selection_start.X = mst.X;
                    else if (mst.X > edit_new_selection_origin.X) edit_new_selection_end.X   = mst.X;

                    if (mst.Y == edit_new_selection_origin.Y) edit_new_selection_start.Y = edit_new_selection_end.Y = mst.Y;
                    else if (mst.Y < edit_new_selection_origin.Y) edit_new_selection_start.Y = mst.Y;
                    else if (mst.Y > edit_new_selection_origin.Y) edit_new_selection_end.Y   = mst.Y;
                }
                else if(edit_new_selection_origin != Point.Zero)
                {
                    int w = edit_new_selection_end.X - edit_new_selection_start.X + 1;
                    int h = edit_new_selection_end.Y - edit_new_selection_start.Y + 1;
                    edit_selection = new int[w][];
                    for (int i = 0; i <= edit_new_selection_end.X - edit_new_selection_start.X; i++)
                    {
                        edit_selection[i] = new int[h];
                        for (int j = 0; j <= edit_new_selection_end.Y - edit_new_selection_start.Y; j++)
                            edit_selection[i][j] = map.Tiles[edit_new_selection_start.X + i][edit_new_selection_start.Y + j];
                    }
                    edit_new_selection_origin = Point.Zero;
                }
            }
        }
        public void EditDraw(GraphicsDevice graphics)
        {
            if (new List<Button> { SaveButton, DoFillAirButton }.Any(ui => ui.GetBounds().Contains(Game1.MS.Position)))
                return;

            if (Game1.KB.IsKeyDown(Keys.Tab))
            {
                Graphics.Graphics.Instance.DrawTexture(tileset, center_screen.X, center_screen.Y, rotation: 0F, scale: 4F, flipX: false, depth: 0F, origin: new Vector2(tileset.Width / 2f, tileset.Height / 2f));
                Graphics.Graphics.Instance.DrawRectangle((int)(center_screen.X - tileset.Width / 2f * SCALE), (int)(center_screen.Y - tileset.Height / 2f * SCALE), tileset.Width * SCALE, tileset.Height * SCALE, Color.Gray);
                Graphics.Graphics.Instance.DrawRectangle(center_screen.X - tileset.Width / 2 * SCALE + edit_selection[0][0] % 16 * TSZ_SCALED, center_screen.Y - tileset.Height / 2 * SCALE + edit_selection[0][0] / 16 * TSZ_SCALED, edit_selection.Length * TSZ_SCALED, edit_selection[0].Length * TSZ_SCALED, new Color(Color.White, Game1.Instance.Ticks % 5F));
            }
            else if(edit_new_selection_origin != Point.Zero)
            {
                Color rndcol = new Color((float)Random.Shared.NextDouble(), (float)Random.Shared.NextDouble(), (float)Random.Shared.NextDouble());
                Color color = new Color(rndcol, 0.5F);
                for (int i = edit_new_selection_start.X; i <= edit_new_selection_end.X; i++)
                    for (int j = edit_new_selection_start.Y; j <= edit_new_selection_end.Y; j++)
                        Graphics.Graphics.Instance.FillRectangle(64 + i * TSZ_SCALED, 64 + j * TSZ_SCALED, TSZ_SCALED, TSZ_SCALED, color);
            }
            else
            {
                for (int i = 0; i < edit_selection.Length; i++)
                    for (int j = 0; j < edit_selection[i].Length; j++)
                        Graphics.Graphics.Instance.DrawTexture(Context.Textures[edit_selection[i][j] + 1], 64 + (mstx + i) * TSZ_SCALED, 64 + (msty + j) * TSZ_SCALED, color: new Color(Color.White, 0.5F), rotation: 0F, scale: SCALE, flipX: false);
                Graphics.Graphics.Instance.DrawRectangle(64 + mstx * TSZ_SCALED, 64 + msty * TSZ_SCALED, edit_selection.Length * TSZ_SCALED, edit_selection[0].Length * TSZ_SCALED, new Color(Color.White, 0.5F), 2);
            }
        }

        public void EditDispose()
        {
        }
    }
}
