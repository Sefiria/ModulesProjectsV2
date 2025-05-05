using ILGPU.IR.Values;
using System;
using System.Drawing;
using static Tooling.Enumerations;

namespace Tooling.UI
{
    public class UIBar : UI
    {
        public Color OutlineColor = Color.Gray;
        public Color EmptyColor = Color.Transparent;
        public Color FillColor = Color.Gray;
        public Action OnClick;
        public RangeValueF RangeValue = new RangeValueF(0F, 0.5F, 1F);
        public UIBarStyle Style = UIBarStyle.Horizontal;

        public override void Click()
        {
            base.Click();
            OnClick?.Invoke();
        }

        public override void Draw(Graphics g)
        {
            var bounds = Bounds;
            var x = bounds.X;
            var y = bounds.Y;
            var w = bounds.Width;
            var h = bounds.Height;
            var val = RangeValue.Value;

            if (EmptyColor != Color.Transparent)
                g.FillRectangle(new SolidBrush(EmptyColor), x, y, w, h);
            if (Style == UIBarStyle.Horizontal) g.FillRectangle(new SolidBrush(FillColor), x, y, w * val, h);
            else g.FillRectangle(new SolidBrush(FillColor), x, y + h * (1F - val), w, h * val);
            g.DrawRectangle(new Pen(OutlineColor, 1F), x, y, w, h);
        }

        public override void Update()
        {
            base.Update();
        }
    }
}
