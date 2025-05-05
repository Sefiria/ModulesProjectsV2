using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Tooling.UI
{
    public class UIPanel : UI, IUIContainer
    {
        public List<UI> Content { get; set; } = new List<UI>();

        public override void Click()
        {
            Content.Where(ui => ui.Bounds.Contains(UIMgt.GetMousePosition())).ToList().ForEach(ui => ui.Click());
        }

        public override void Draw(Graphics g)
        {
            Content.ForEach(ui => ui.Draw(g));
        }

        public override void Update()
        {
            Content.ForEach(ui => ui.Update());
        }
    }
}
