using System.Drawing;

namespace Project11.Helpers
{
    public static class PointExt
    {
        public static Point Add(this Point p1, Point p2) => new Point(p1.X + p2.X, p1.Y + p2.Y);
        public static Point Sub(this Point p1, Point p2) => new Point(p1.X - p2.X, p1.Y - p2.Y);

        public static Point Add(this Point p1, int x = 0, int y = 0) => new Point(p1.X + x, p1.Y + y);
        public static Point Sub(this Point p1, int x = 0, int y = 0) => new Point(p1.X - x, p1.Y - y);

        public static void Set(this Point p1, int x, int y) => p1 = new Point(x, y);
        public static void Next(this Point p1, int w, int h)
        {
            int nX = p1.X + 1;
            int nY = p1.Y;
            if(nX >= w && nY + 1 < h)
            {
                nX = 0;
                nY++;
            }
            p1 = new Point(nX, nY);
        }
    }
}
