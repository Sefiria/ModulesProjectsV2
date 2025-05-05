using Microsoft.Xna.Framework.Input;

namespace Tools.Inputs
{
    public class KB
    {
        public KeyboardState State;
        public Dictionary<Keys, bool> KeysReleased = new Dictionary<Keys, bool>();
        public Dictionary<Keys, char> keyMap = new Dictionary<Keys, char>
        {
            { Keys.D0, '0' },{ Keys.D1, '1' },{ Keys.D2, '2' },{ Keys.D3, '3' },{ Keys.D4, '4' },{ Keys.D5, '5' },{ Keys.D6, '6' },{ Keys.D7, '7' },{ Keys.D8, '8' },{ Keys.D9, '9' },
            { Keys.NumPad0, '0' },{ Keys.NumPad1, '1' },{ Keys.NumPad2, '2' },{ Keys.NumPad3, '3' },{ Keys.NumPad4, '4' },{ Keys.NumPad5, '5' },{ Keys.NumPad6, '6' },{ Keys.NumPad7, '7' },{ Keys.NumPad8, '8' },{ Keys.NumPad9, '9' },
            { Keys.A, 'A' },{ Keys.B, 'B' },{ Keys.C, 'C' },{ Keys.D, 'D' },{ Keys.E, 'E' },{ Keys.F, 'F' },{ Keys.G, 'G' },{ Keys.H, 'H' },{ Keys.I, 'I' },{ Keys.J, 'J' },{ Keys.K, 'K' },{ Keys.L, 'L' },{ Keys.M, 'M' },{ Keys.N, 'N' },{ Keys.O, 'O' },{ Keys.P, 'P' },{ Keys.Q, 'Q' },{ Keys.R, 'R' },{ Keys.S, 'S' },{ Keys.T, 'T' },{ Keys.U, 'U' },{ Keys.V, 'V' },{ Keys.W, 'W' },{ Keys.X, 'X' },{ Keys.Y, 'Y' },{ Keys.Z, 'Z' },
            { Keys.Back, (char)999 },{ Keys.Space, (char)1000 }
        };
        public delegate void NullArgs();
        public delegate void KeyPressedArgs(char key);
        public delegate void RawKeyPressedArgs(Keys key);
        public event KeyPressedArgs OnKeyPressed, OnKeyDown;
        public event RawKeyPressedArgs OnRawKeyDown, OnRawKeyPressed;
        public event NullArgs OnBackspace, OnSpace;

        public bool IsKeyDown(Keys key) => State.IsKeyDown(key);
        public bool IsKeyPressed(Keys key) => State.IsKeyDown(key) && (!KeysReleased.ContainsKey(key) || KeysReleased[key]);

        public void Update()
        {
            State = Keyboard.GetState();

            State.GetPressedKeys().ToList().ForEach(key => {
                OnRawKeyDown?.Invoke(key);
                char k = ConvertKeyToChar(key);
                OnKeyDown?.Invoke(k);
                if (!KeysReleased.ContainsKey(key))
                    KeysReleased[key] = true;
                if (KeysReleased[key])
                {
                    if (k == 999)// back
                    {
                        OnBackspace?.Invoke();
                    }
                    else if (k == 1000)// space
                    {
                        OnSpace?.Invoke();
                    }
                    else
                    {
                        if (k != '\0')
                        {
                            OnKeyPressed?.Invoke(k);
                        }
                        else
                        {
                            OnRawKeyPressed?.Invoke(key);
                        }
                    }
                }
            });

            KeysReleased.Keys.ToList().ForEach(key => KeysReleased[key] = !State.IsKeyDown(key));
        }
        public char ConvertKeyToChar(Keys key) => keyMap.ContainsKey(key) ? keyMap[key] : '\0';
        public string Backspace(string text) => string.Concat(text.Take(text.Length - 1));
    }
}
