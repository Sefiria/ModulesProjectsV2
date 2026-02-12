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
            Text = new char[W*H];
            Cls();
            Cursor = new Point(0, 0);
            KB.OnKeyDown += KB_OnKeyDown;
            KB.OnKeyPressed += KB_OnKeyPressed;
            KB.OnKeyReleased += KB_OnKeyReleased;
        }

        private void KB_OnKeyReleased(Key key)
        {
            if (key == key_to_repeat)
                repeat_cooldown = 0;
        }

        private void KB_OnKeyDown(Key key)
        {
            if (key != key_to_repeat)
            {
                repeat_cooldown = 0;
                key_to_repeat = key;
            }

            if (repeat_cooldown == repeat_cooldown_max)
                WriteKey(key);
            else
                repeat_cooldown++;
        }

        void KB_OnKeyPressed(Key key)
        {
            WriteKey(key);
        }
        private void WriteKey(Key key)
        {
            if (key == Key.Back)
            {
                if (Cursor.X == 0 && Cursor.Y == 0) return;
                int x = Cursor.X, y = Cursor.Y;
                if (x > 0) x--; else { y--; x = W - 1; }
                Text[y * W + x] = '\0';
                Cursor = new Point(x, y);
                return;
            }

            if (key == Key.Enter) { Write(Cursor.X, Cursor.Y, "\n"); return; }

            char? ch = ToChar(key);
            if (ch.HasValue) Write(Cursor.X, Cursor.Y, ch.Value.ToString());
        }

        char? ToChar(Key k)
        {
            if (k == Key.LeftShift || k == Key.RightShift || k == Key.LeftCtrl || k == Key.RightCtrl || k == Key.LeftAlt || k == Key.RightAlt)
                return null;
            bool sh = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
            bool caps = Keyboard.IsKeyToggled(Key.CapsLock);

            if (k >= Key.A && k <= Key.Z)
            {
                char c = (char)('a' + (k - Key.A));
                return (caps ^ sh) ? char.ToUpper(c) : c;
            }

            if (k >= Key.D0 && k <= Key.D9)
            {
                string normal = "0123456789";
                string shifted = ")!@#$%^&*(";
                return (sh ? shifted : normal)[k - Key.D0];
            }

            if (k == Key.Space) return ' ';
            if (k == Key.OemMinus) return sh ? '_' : '-';
            if (k == Key.OemPlus) return sh ? '+' : '=';
            if (k == Key.OemComma) return sh ? '?' : ',';
            if (k == Key.OemPeriod) return sh ? ':' : '.';

            int vk = KeyInterop.VirtualKeyFromKey(k);
            var ch = FromLayout(vk, sh);
            if (ch.HasValue) return ch.Value;

            return null;
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
                        FormMain.Graphics.DrawString(c.ToString(), GV.Font,
                            Brushes.White, x * GV.FontWidth, y * GV.FontHeight);
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
                    2, GV.FontHeight
                );
            }
        }
    }
}
