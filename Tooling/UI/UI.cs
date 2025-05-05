using System;
using System.Collections.Generic;
using System.Drawing;

namespace Tooling.UI
{
    public class UI
    {
        public UI Owner;
        public string Name = null;
        public vecf Position { get; set; }
        public vecf Size { get; set; }
        public RectangleF Bounds => new RectangleF(GetGlobalPosition().pt, Size.sz);
        public Action<UI> UpdateValue;
        public bool IsHover => UIMgt.CurrentHover == this;

        public vecf GetGlobalPosition()
        {
            vecf pos = Position;
            UI owner = Owner;
            while(owner != null)
            {
                pos += owner.Position;
                owner = owner.Owner;
            }
            return pos;
        }

        public UI(UI owner = null)
        {
            Owner = owner;
        }

        public virtual void Update()
        {
            if (Bounds.Contains(UIMgt.GetMousePosition())) UIMgt.CurrentHover = this;
            UpdateValue?.Invoke(this);
        }
        public virtual void Draw(Graphics g) { }
        public virtual void Click() { UIMgt.CurrentClicked = this; }


        public static Pen PenSelection = new Pen(Color.White, 3F);

        public UI Search(string Name)
        {
            UI RecursiveSearch(UI owner)
            {
                if (owner is IUIContainer)
                {
                    foreach (UI ui in (owner as IUIContainer).Content)
                    {
                        if (ui.Name == Name)
                            return ui;
                        return RecursiveSearch(owner);
                    }
                    return null;
                }
                return null;
            }
            return RecursiveSearch(this);
        }
    }
}
