using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Tooling
{
    public static class BitmapExtensions
    {
        static float[][] fadeMatrix = {
        new float[] {1, 0, 0, 0, 0},
        new float[] {0, 1, 0, 0, 0},
        new float[] {0, 0, 1, 0, 0},
        new float[] {0, 0, 0, 1, 0},
        new float[] {0, 0, 0, 0, 1}
    };

        public static Bitmap SetOpacity(this Bitmap bitmap, float Opacity, float Gamma = 1.0f)
        {
            var mx = new ColorMatrix(fadeMatrix);
            mx.Matrix33 = Opacity;
            var bmp = new Bitmap(bitmap.Width, bitmap.Height);

            using (var g = Graphics.FromImage(bmp))
            using (var attributes = new ImageAttributes())
            {
                attributes.SetGamma(Gamma, ColorAdjustType.Bitmap);
                attributes.SetColorMatrix(mx, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                g.Clear(Color.Transparent);
                g.DrawImage(bitmap, new Rectangle(0, 0, bmp.Width, bmp.Height),
                    0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel, attributes);
                return bmp;
            }
        }

        public static Bitmap Crop(this Bitmap b, int sq_sz, int sq_resz = 0, int offset_index_x = 0, int offset_index_y = 0) => b.Crop(sq_sz, sq_sz, sq_resz, sq_resz, offset_index_x, offset_index_y);
        public static Bitmap Crop(this Bitmap b, int sz_x, int sz_y, int resz_x, int resz_y, int offset_index_x = 0, int offset_index_y = 0)
        {
            var nb = new Bitmap(resz_x == 0 ? sz_x : resz_x, resz_y== 0 ? sz_y : resz_y);
            using (Graphics g = Graphics.FromImage(nb))
            {
                g.DrawImage(b, new Rectangle(0, 0, resz_x == 0 ? sz_x : resz_x, resz_y == 0 ? sz_y : resz_y), new Rectangle(offset_index_x * sz_x, offset_index_y * sz_y, sz_x, sz_y), GraphicsUnit.Pixel);
            }
            return nb;
        }
    }
}
