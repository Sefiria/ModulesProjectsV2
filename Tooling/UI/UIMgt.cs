using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tooling.UI
{
    public static class UIMgt
    {
        public static UI CurrentClicked, CurrentHover;
        public static List<UI> UI = new List<UI>();
        public static int MouseStatesVersion = 1;

        static UIMgt() { }

        public static void Update()
        {
            CurrentClicked = null;
            CurrentHover = null;
            UI.ForEach(ui => ui.Update());

            if (GetMousePressed(MouseButtons.Left))
            {
                MouseClickEventUpdate();
                UI.Where(ui => ui.IsHover).ToList().ForEach(ui => ui.Click());
            }
        }

        public static void Draw(Graphics g)
        {
            UI.ForEach(ui => ui.Draw(g));
        }

        // moved to Update
        //public static void MouseDown()
        //{
        //    if (MouseStates.IsDown && MouseStates.ButtonDown == MouseButtons.Left)
        //        UI.Where(ui => ui.Bounds.Contains(MouseStates.Position)).ToList().ForEach(ui => ui.Click());
        //}

        public static PointF GetMousePosition()
        {
            switch (MouseStatesVersion)
            {
                case 1: return MouseStatesV1.Position;
            }
            return MouseStates.Position;
        }
        public static bool GetMouseIsDown()
        {
            switch (MouseStatesVersion)
            {
                case 1: return MouseStatesV1.IsDown;
            }
            return MouseStates.IsDown;
        }
        public static bool GetMousePressed(MouseButtons button)
        {
            switch (MouseStatesVersion)
            {
                case 1: return MouseStatesV1.IsDown;
            }
            return MouseStates.IsButtonPressed(button);
        }
        public static void MouseClickEventUpdate()
        {
            if (MouseStatesVersion == 1)
                return;
            MouseStates.ReleasedButtons[MouseButtons.Left] = false;
        }
        public static T GetUIByName<T>(string name) where T:UI
        {
            return UI.OfType<T>().FirstOrDefault(ui => ui.Name == name);
        }
    }
}
