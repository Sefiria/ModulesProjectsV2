using System.Windows.Forms;
using System.Drawing;
using System;
using System.Windows.Media.Media3D;

namespace Tooling
{
    public class Core
    {
        public static int TSZ = 32;

        public Timer TimerUpdate = new Timer { Enabled = true, Interval = 10 };
        public Timer TimerDraw = new Timer { Enabled = true, Interval = 10 };
        public float RW, RH;
        public Graphics g;
        public Bitmap Image;
        public Font Font = new Font("Segoe UI", 12);
        public Font SmallFont = new Font("Segoe UI", 8);

        public int iRW => (int)RW;
        public int iRH => (int) RH;
        /// <summary>
        /// Half Render Width
        /// </summary>
        public float hw => RW / 2F;
        /// <summary>
        /// Half Render Height
        /// </summary>
        public float hh => RH / 2F;
        public virtual PointF CenterPoint => new PointF((int)hw, (int)hh);

        public byte Ticks = 0;

        public virtual void Init(PictureBox render, Action update, Action draw)
        {
            TimerUpdate.Tick += (object sender, EventArgs e) => update();
            TimerDraw.Tick += (object sender, EventArgs e) => draw();

            RW = render.Width;
            RH = render.Height;
            Image = new Bitmap(iRW, iRH);
            g = Graphics.FromImage(Image);
        }
    }
}
