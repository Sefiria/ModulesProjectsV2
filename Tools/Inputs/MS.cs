using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Tools.Inputs
{
    public class MS
    {
        public enum MouseButtons
        {
            Left, Middle, Right, XL, XR, None
        }
        public MouseState State, PreviousState;
        public delegate void ButtonPressedArgs(MouseButtons button);
        public delegate void WheelArgs(int delta);
        public delegate void WheelSignArgs(float sign);
        public delegate void WheelSignIntArgs(int sign);
        public event ButtonPressedArgs OnButtonPressed, OnButtonDown;
        public event WheelArgs OnWheelScroll;
        public event WheelSignArgs OnWheelScrollSign;
        public event WheelSignIntArgs OnWheelScrollSignInt;

        public bool IsLeftDown, IsLeftPressed, IsMiddleDown, IsMiddlePressed, IsRightDown, IsRightPressed, IsXLDown, IsXLPressed, IsXRDown, IsXRPressed, IsWheelScrolling;
        public int PreviousScrollWheelValue = 0, ScrollWheelValue = 0, ScrollWheelSignInt = 0;
        public float ScrollWheelSign = 0;

        public void Update()
        {
            PreviousState = State;
            State = Mouse.GetState();

            IsLeftDown = State.LeftButton == ButtonState.Pressed;
            IsLeftPressed = IsLeftDown && PreviousState.LeftButton == ButtonState.Released;
            IsMiddleDown = State.MiddleButton == ButtonState.Pressed;
            IsMiddlePressed = IsMiddleDown && PreviousState.MiddleButton == ButtonState.Released;
            IsRightDown = State.RightButton == ButtonState.Pressed;
            IsRightPressed = IsRightDown && PreviousState.RightButton == ButtonState.Released;
            IsXLDown = State.RightButton == ButtonState.Pressed;
            IsXLPressed = IsXLDown && PreviousState.XButton1 == ButtonState.Released;
            IsXRDown = State.RightButton == ButtonState.Pressed;
            IsXRPressed = IsXRDown && PreviousState.XButton2 == ButtonState.Released;

            ScrollWheelValue = State.ScrollWheelValue - PreviousScrollWheelValue;
            IsWheelScrolling = ScrollWheelValue != 0;
            PreviousScrollWheelValue = State.ScrollWheelValue;

            if (IsLeftDown)
            {
                OnButtonDown?.Invoke(MouseButtons.Left);
                if(IsLeftPressed)
                    OnButtonPressed?.Invoke(MouseButtons.Left);
            }

            if (IsMiddleDown)
            {
                OnButtonDown?.Invoke(MouseButtons.Right);
                if (IsMiddlePressed)
                    OnButtonPressed?.Invoke(MouseButtons.Middle);
            }

            if (IsRightDown)
            {
                OnButtonDown?.Invoke(MouseButtons.Right);
                if (IsRightPressed)
                    OnButtonPressed?.Invoke(MouseButtons.Right);
            }

            if (IsXLDown)
            {
                OnButtonDown?.Invoke(MouseButtons.Right);
                if (IsXLPressed)
                    OnButtonPressed?.Invoke(MouseButtons.XL);
            }

            if (IsXRDown)
            {
                OnButtonDown?.Invoke(MouseButtons.Right);
                if (IsXRPressed)
                    OnButtonPressed?.Invoke(MouseButtons.XR);
            }

            if (IsWheelScrolling)
            {
                ScrollWheelSign = ScrollWheelValue > 0 ? 1F : -1F;
                ScrollWheelSignInt = (int)ScrollWheelSign;
                OnWheelScroll?.Invoke(ScrollWheelValue);
                OnWheelScrollSign?.Invoke(ScrollWheelSign);
                OnWheelScrollSignInt?.Invoke(ScrollWheelSignInt);
            }
            else
            {
                ScrollWheelValue = 0;
                ScrollWheelSign = 0F;
                ScrollWheelSignInt = 0;
            }
        }

        public Point Position => State.Position;
        public int X => State.X;
        public int Y => State.Y;
        public Rectangle Rect => new Rectangle(X, Y, 1, 1);
    }
}
