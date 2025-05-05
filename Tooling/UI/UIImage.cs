using System;
using System.Drawing;

namespace Tooling.UI
{
    public class UIImage : UI
    {
        public Bitmap Image;
        public int? argb_outline = null;
        public Action<UI> OnClick;

        public UIImage(string name, Bitmap image, int x, int y)
        {
            Name = name;
            Image = image;
            Position = (x, y).Vf();
            Size = image.Size.Vf();
        }
        public UIImage(string name, Bitmap image, int argb_outline, int x, int y)
        {
            Name = name;
            Image = image;
            this.argb_outline = argb_outline;
            Position = (x, y).Vf();
            Size = image.Size.Vf();
        }
        public UIImage(string name, int argb, int x, int y, int w, int h)
        {
            Name = name;
            Image = new Bitmap(w, h);
            NewImageFromArgb(argb);
            Position = (x, y).Vf();
            Size = new vecf(w, h);
        }
        public UIImage(string name, int argb, int argb_outline, int x, int y, int w, int h)
        {
            Name = name;
            Image = new Bitmap(w, h);
            NewImageFromArgb(argb);
            this.argb_outline = argb_outline;
            Position = (x, y).Vf();
            Size = new vecf(w, h);
        }
        public UIImage(string name, int argb, int argb_outline, int x, int y, int w, int h, Action<UI> onClick)
        {
            Name = name;
            Image = new Bitmap(w, h);
            NewImageFromArgb(argb);
            this.argb_outline = argb_outline;
            Position = (x, y).Vf();
            Size = new vecf(w, h);
            OnClick += onClick;
        }
        public void NewImageFromArgb(int argb)
        {
            using (Graphics g = Graphics.FromImage(Image))
                g.Clear(Color.FromArgb(argb));
        }

        public override void Draw(Graphics g)
        {
            g.DrawImage(Image, Position.pt);
            if (argb_outline != null)
                g.DrawRectangle(new Pen(Color.FromArgb(argb_outline.Value)), new Rectangle(Position.ipt, Image.Size));
        }
        public override void Click()
        {
            base.Click();
            OnClick?.Invoke(this);
        }
    }
}
