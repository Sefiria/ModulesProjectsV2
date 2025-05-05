using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
