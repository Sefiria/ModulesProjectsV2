using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Tooling
{
    public class KB
    {
        public delegate void KBHandler(System.Windows.Input.Key key);
        public static  event KBHandler OnKeyPressed, OnKeyDown, OnKeyReleased;

        public enum Key
        {
            Escape = 0,
            Space,
            Enter,
            Up,
            Down,
            Left,
            Right,
            A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,
            Num1, Num2,Num3,Num4,Num5,Num6,Num7,Num8,Num9, Num0, CharDegree, CharPlus, Add, Substract,
            Numpad1, Numpad2, Numpad3, Numpad4, Numpad5, Numpad6, Numpad7, Numpad8, Numpad9, Numpad0,
            LeftAlt, RightAlt, LeftShift, RightShift, LeftCtrl, RightCtrl,
            Tab, Supr, Back, Insert
        }

        static Dictionary<System.Windows.Input.Key, bool> Released = new Dictionary<System.Windows.Input.Key, bool>()
        {
            [System.Windows.Input.Key.Escape] = true,
            [System.Windows.Input.Key.Space] = true,
            [System.Windows.Input.Key.Enter] = true,
            [System.Windows.Input.Key.Up] = true,
            [System.Windows.Input.Key.Down] = true,
            [System.Windows.Input.Key.Left] = true,
            [System.Windows.Input.Key.Right] = true,
            [System.Windows.Input.Key.A] = true,
            [System.Windows.Input.Key.B] = true,
            [System.Windows.Input.Key.C] = true,
            [System.Windows.Input.Key.D] = true,
            [System.Windows.Input.Key.E] = true,
            [System.Windows.Input.Key.F] = true,
            [System.Windows.Input.Key.G] = true,
            [System.Windows.Input.Key.H] = true,
            [System.Windows.Input.Key.I] = true,
            [System.Windows.Input.Key.J] = true,
            [System.Windows.Input.Key.K] = true,
            [System.Windows.Input.Key.L] = true,
            [System.Windows.Input.Key.M] = true,
            [System.Windows.Input.Key.N] = true,
            [System.Windows.Input.Key.O] = true,
            [System.Windows.Input.Key.P] = true,
            [System.Windows.Input.Key.Q] = true,
            [System.Windows.Input.Key.R] = true,
            [System.Windows.Input.Key.S] = true,
            [System.Windows.Input.Key.T] = true,
            [System.Windows.Input.Key.U] = true,
            [System.Windows.Input.Key.V] = true,
            [System.Windows.Input.Key.W] = true,
            [System.Windows.Input.Key.X] = true,
            [System.Windows.Input.Key.Y] = true,
            [System.Windows.Input.Key.Z] = true,
            [System.Windows.Input.Key.D1] = true,
            [System.Windows.Input.Key.D2] = true,
            [System.Windows.Input.Key.D3] = true,
            [System.Windows.Input.Key.D4] = true,
            [System.Windows.Input.Key.D5] = true,
            [System.Windows.Input.Key.D6] = true,
            [System.Windows.Input.Key.D7] = true,
            [System.Windows.Input.Key.D8] = true,
            [System.Windows.Input.Key.D9] = true,
            [System.Windows.Input.Key.D0] = true,
            [System.Windows.Input.Key.Oem4] = true,
            [System.Windows.Input.Key.OemPlus] = true,
            [System.Windows.Input.Key.Add] = true,
            [System.Windows.Input.Key.Subtract] = true,
            [System.Windows.Input.Key.NumPad1] = true,
            [System.Windows.Input.Key.NumPad2] = true,
            [System.Windows.Input.Key.NumPad3] = true,
            [System.Windows.Input.Key.NumPad4] = true,
            [System.Windows.Input.Key.NumPad5] = true,
            [System.Windows.Input.Key.NumPad6] = true,
            [System.Windows.Input.Key.NumPad7] = true,
            [System.Windows.Input.Key.NumPad8] = true,
            [System.Windows.Input.Key.NumPad9] = true,
            [System.Windows.Input.Key.NumPad0] = true,
            [System.Windows.Input.Key.LeftAlt] = true,
            [System.Windows.Input.Key.RightAlt] = true,
            [System.Windows.Input.Key.LeftShift] = true,
            [System.Windows.Input.Key.RightShift] = true,
            [System.Windows.Input.Key.LeftCtrl] = true,
            [System.Windows.Input.Key.RightCtrl] = true,
            [System.Windows.Input.Key.Tab] = true,
            [System.Windows.Input.Key.Delete] = true,
            [System.Windows.Input.Key.Back] = true,
            [System.Windows.Input.Key.Insert] = true,
        };

        static readonly List<System.Windows.Input.Key> AvailableKeys = new List<System.Windows.Input.Key>()
        {
            System.Windows.Input.Key.Escape,
            System.Windows.Input.Key.Space,
            System.Windows.Input.Key.Enter,
            System.Windows.Input.Key.Up,
            System.Windows.Input.Key.Down,
            System.Windows.Input.Key.Left,
            System.Windows.Input.Key.Right,
            System.Windows.Input.Key.A,
            System.Windows.Input.Key.B,
            System.Windows.Input.Key.C,
            System.Windows.Input.Key.D,
            System.Windows.Input.Key.E,
            System.Windows.Input.Key.F,
            System.Windows.Input.Key.G,
            System.Windows.Input.Key.H,
            System.Windows.Input.Key.I,
            System.Windows.Input.Key.J,
            System.Windows.Input.Key.K,
            System.Windows.Input.Key.L,
            System.Windows.Input.Key.M,
            System.Windows.Input.Key.N,
            System.Windows.Input.Key.O,
            System.Windows.Input.Key.P,
            System.Windows.Input.Key.Q,
            System.Windows.Input.Key.R,
            System.Windows.Input.Key.S,
            System.Windows.Input.Key.T,
            System.Windows.Input.Key.U,
            System.Windows.Input.Key.V,
            System.Windows.Input.Key.W,
            System.Windows.Input.Key.X,
            System.Windows.Input.Key.Y,
            System.Windows.Input.Key.Z,
            System.Windows.Input.Key.D1,
            System.Windows.Input.Key.D2,
            System.Windows.Input.Key.D3,
            System.Windows.Input.Key.D4,
            System.Windows.Input.Key.D5,
            System.Windows.Input.Key.D6,
            System.Windows.Input.Key.D7,
            System.Windows.Input.Key.D8,
            System.Windows.Input.Key.D9,
            System.Windows.Input.Key.D0,
            System.Windows.Input.Key.Oem4,
            System.Windows.Input.Key.OemPlus,
            System.Windows.Input.Key.Add,
            System.Windows.Input.Key.Subtract,
            System.Windows.Input.Key.NumPad1,
            System.Windows.Input.Key.NumPad2,
            System.Windows.Input.Key.NumPad3,
            System.Windows.Input.Key.NumPad4,
            System.Windows.Input.Key.NumPad5,
            System.Windows.Input.Key.NumPad6,
            System.Windows.Input.Key.NumPad7,
            System.Windows.Input.Key.NumPad8,
            System.Windows.Input.Key.NumPad9,
            System.Windows.Input.Key.NumPad0,
            System.Windows.Input.Key.LeftAlt,
            System.Windows.Input.Key.RightAlt,
            System.Windows.Input.Key.LeftShift,
            System.Windows.Input.Key.RightShift,
            System.Windows.Input.Key.LeftCtrl,
            System.Windows.Input.Key.RightCtrl,
            System.Windows.Input.Key.Tab,
            System.Windows.Input.Key.Delete,
            System.Windows.Input.Key.Back,
            System.Windows.Input.Key.Insert,
        };

        public static void Init() => Update();
        //public static void Update() => AvailableKeys.ForEach(k => Released[k] = !Keyboard.IsKeyDown(k));
        public static void Update()
        {
            bool a = false, b = false;
            for(int i=0;i<AvailableKeys.Count;i++)
            {
                a = !Released[AvailableKeys[i]];
                Released[AvailableKeys[i]] = !Keyboard.IsKeyDown(AvailableKeys[i]);
                b = Released[AvailableKeys[i]];
                if (a)
                {
                    if (b) OnKeyPressed?.Invoke(AvailableKeys[i]);
                    else OnKeyDown?.Invoke(AvailableKeys[i]);
                }
                else
                {
                    if (b) OnKeyReleased?.Invoke(AvailableKeys[i]);
                }
            }
        }
        public static bool IsKeyDown(Key k) => Keyboard.IsKeyDown(AvailableKeys[(int)k]);
        public static bool IsKeyPressed(Key k) => Keyboard.IsKeyDown(AvailableKeys[(int)k]) && Released[AvailableKeys[(int)k]];
        public static bool IsKeyUp(Key k) => !IsKeyDown(k);
        public static bool LeftShift => Keyboard.IsKeyDown(System.Windows.Input.Key.LeftShift);
        public static bool LeftCtrl => Keyboard.IsKeyDown(System.Windows.Input.Key.LeftCtrl);
        public static bool LeftAlt => Keyboard.IsKeyDown(System.Windows.Input.Key.LeftAlt);
        public static (bool z, bool q, bool s, bool d) ZQSD(bool pressed = false)
        {
            return pressed ? (IsKeyPressed(Key.Z), IsKeyPressed(Key.Q), IsKeyPressed(Key.S), IsKeyPressed(Key.D)) : (IsKeyDown(Key.Z), IsKeyDown(Key.Q), IsKeyDown(Key.S), IsKeyDown(Key.D));
        }
        public static bool AnyZQSD()
        {
            return IsKeyDown(Key.Z) || IsKeyDown(Key.Q) || IsKeyDown(Key.S) || IsKeyDown(Key.D);
        }
    }
}
