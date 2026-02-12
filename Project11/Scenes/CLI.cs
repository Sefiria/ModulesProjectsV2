using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;
using Tooling;
using static System.Net.Mime.MediaTypeNames;
using static Tooling.KB;
using Key = System.Windows.Input.Key;

namespace Project11.Scenes
{
    internal class CLI
    {
        static readonly int W = (int)(FormMain.Instance.Width / GV.FontWidth) - 1, H = (int)(FormMain.Instance.Height / GV.FontHeight) - 1;

        Point Cursor;
        char[] Text;
        Key key_to_repeat;
        byte repeat_cooldown;
        const byte repeat_cooldown_max = 25;

        [System.Runtime.InteropServices.DllImport("user32.dll")] static extern IntPtr GetKeyboardLayout(uint idThread);
        [System.Runtime.InteropServices.DllImport("user32.dll")] static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState, System.Text.StringBuilder pwszBuff, int cchBuff, uint wFlags, IntPtr dwhkl);
        char? FromLayout(int vk, bool shift)
        {
            var ks = new byte[256];
            if (shift) { ks[0x10] = 0x80; ks[0xA0] = 0x80; } // Shift state
            var sb = new System.Text.StringBuilder(4);
            var hkl = GetKeyboardLayout(0);
            int rc = ToUnicodeEx((uint)vk, 0, ks, sb, sb.Capacity, 0, hkl);
            return rc > 0 ? sb[0] : (char?)null;
        }

        char Get(int x, int y) => (uint)x < W && (uint)y < H ? Text[y * W + x] : '\0';
        void Set(int x, int y, char c)
        {
            if ((uint)x < W && (uint)y < H)
            {
                Text[y * W + x] = c;
                x++; if (x == W) { x = 0; y++; if (y >= H) y = H - 1; }
                Cursor = new Point(x, y);
            }
        }
        int Write(int x, int y, string s)
        {
            if ((uint)x >= W || (uint)y >= H || s == null) return 0;

            int o = y * W, i = o + x;
            for (int k = o; k < i; k++)
                if (Text[k] == '\0') Text[k] = ' ';

            int w = 0;

            foreach (var c in s)
            {
                if (c == '\n')
                {
                    x = 0;
                    y++;
                    if ((uint)y >= H) break;
                    i = y * W;
                    continue;
                }

                Text[i] = c;
                w++;
                x++;

                if (x == W)
                {
                    x = 0;
                    y++;
                    if ((uint)y >= H) break;
                    i = y * W;
                }
                else
                {
                    i++;
                }
            }

            Cursor = new Point(x, y);

            return w;
        }
        string GetLineTrimmed(int y)
        {
            if ((uint)y >= H) return "";
            int o = y * W, n = 0;
            while (n < W && Text[o + n] != '\0') n++;
            return new string(Text, o, n);
        }
        void Cls() { for (int i = 0; i < Text.Length; i++) Text[i] = '\0'; }

        public CLI()
        {
            Text = new char[W * H];
            Cls();
            Cursor = new Point(0, 0);

            FormMain.Instance.KeyPress += (s, e) =>
            {
                if (e.KeyChar < 10 || e.KeyChar == 13)
                    return;
                Write(Cursor.X, Cursor.Y, e.KeyChar.ToString());
            };

            FormMain.Instance.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape) FormMain.Instance.Close();
                if (e.KeyCode == Keys.Back) Backspace();
                if (e.KeyCode == Keys.Enter)  Write(Cursor.X, Cursor.Y, "\n");
            };
        }

        void Backspace()
        {
            if (Cursor.X == 0 && Cursor.Y == 0) return;
            int x = Cursor.X, y = Cursor.Y;
            if (x > 0) x--; else { y--; x = W - 1; }
            Text[y * W + x] = '\0';
            Cursor = new Point(x, y);
            while (x > 1 && Text[y * W + x - 1] == '\0')
                Cursor.X = --x;
        }

        public void Update()
        {
        }
        public void Draw()
        {
            FormMain.Graphics.Clear(Color.Black);

            // Dessine tout le buffer Text
            for (int y = 0; y < H; y++)
            {
                int o = y * W;
                for (int x = 0; x < W; x++)
                {
                    char c = Text[o + x];
                    if (c != '\0')
                    {
                        Size sz = TextRenderer.MeasureText(c.ToString(), GV.Font, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.NoPadding);
                        int dx = (int)(GV.FontWidth + (GV.FontWidth - sz.Width) / 2);

                        TextRenderer.DrawText(
                            FormMain.Graphics,
                            c.ToString(),
                            GV.Font,
                            new Point((int)(x * GV.FontWidth + dx), (int)(y * GV.FontHeight)),
                            Color.White,
                            TextFormatFlags.NoPadding
                        );
                    }
                }
            }

            // Curseur clignotant
            if ((GV.Ticks & 32) == 0)
            {
                int cx = (int)(Cursor.X * GV.FontWidth);
                int cy = (int)(Cursor.Y * GV.FontHeight);

                FormMain.Graphics.FillRectangle(
                    Brushes.White,
                    cx, cy,
                    GV.FontWidth, GV.FontHeight
                );
            }

            // Sur le point de fermer le programme
            Point p = FormMain.Instance.close.PointToClient(Control.MousePosition);
            var rect = new Rectangle(0, 0, FormMain.Instance.Render.Width - 1, FormMain.Instance.Render.Height - 1);
            if (FormMain.Instance.close.ClientRectangle.Contains(p))
                FormMain.Graphics.DrawRectangle(new Pen(Color.Red, 16F), rect);
            else
                FormMain.Graphics.DrawRectangle(new Pen(Color.DimGray, 1F), rect);
        }
    }
}
