using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Tooling.UI
{
    public class UIPanelSelection : UI, IUIContainer
    {
        public int SelectedId = 0;
        public List<UI> Content { get; set; } = new List<UI>();

        public override void Click()
        {
            UI clicked;
            if ((clicked = Content.FirstOrDefault(ui => ui.Bounds.Contains(UIMgt.GetMousePosition()))) != null)
            {
                SelectedId = Content.IndexOf(clicked);
                clicked.Click();
            }
        }

        public override void Draw(Graphics g)
        {
            Content.ForEach(ui => ui.Draw(g));
            if (SelectedId > -1)
                g.DrawRectangle(PenSelection, Content[SelectedId].Bounds.ToIntRect());
        }

        public override void Update()
        {
            if (SelectedId < -1) SelectedId = -1;
            if (SelectedId > Content.Count - 1) SelectedId = Content.Count - 1;
            Content.ForEach(ui => ui.Update());
        }
    }
}
