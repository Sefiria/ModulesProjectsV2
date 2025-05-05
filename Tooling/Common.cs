using ILGPU.IR;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using Tooling;

namespace Tooling
{
    public static class Common
    {
        public static Random Rnd = new Random((int)DateTime.UtcNow.Ticks);
        public static List<Bitmap> SplitVertically(this Bitmap img, int height)
        {
            List<Bitmap> result = new List<Bitmap>();
            int w = img.Width;
            int h = img.Height;
            for (int j=0;j<h/height;j++)
            {
                result.Add(img.Clone(new Rectangle(0, height * j, w, height), img.PixelFormat));
            }
            return result;
        }
        public static Bitmap Resize(this Bitmap img, int w, int h) => new Bitmap(img, w, h);
        public static Bitmap Resize(this Bitmap img, int sz) => new Bitmap(img, sz, sz);
        public static Bitmap Transparent(this Bitmap img)
        {
            var b = new Bitmap(img);
            b.MakeTransparent(Color.White);
            return b;
        }
        public static List<Bitmap> Resize(this List<Bitmap> imgs, int w, int h) => imgs.Select(img => img.Resize(w, h)).ToList();
        public static List<Bitmap> Resize(this List<Bitmap> imgs, int sz) => imgs.Select(img => img.Resize(sz)).ToList();
        public static vecf vecf(this Point pt) => new vecf(pt.X, pt.Y);
        public static vecf vecf(this PointF pt) => new vecf(pt.X, pt.Y);
        public static bool IsPositive(this float v) => v > 0F;
        public static bool IsNegative(this float v) => v < 0F;// 0 isn't positive or negative, no entry into the possible condition if 0
        public static vec Normalized(this vec vec) => Maths.Normalized(vec.f).i;
        public static vecf Normalized(this vecf vec) => Maths.Normalized(vec);

        public static PointF TopLeft(this RectangleF box) => (box.Left, box.Top).P();
        public static PointF TopRight(this RectangleF box) => (box.Right, box.Top).P();
        public static PointF BottomLeft(this RectangleF box) => (box.Left, box.Bottom).P();
        public static PointF BottomRight(this RectangleF box) => (box.Right, box.Bottom).P();


        /// <summary>
        /// Warning : long process but exact, w & h need to be > to src ones
        /// </summary>
        public static Bitmap ResizeExact(this Bitmap img, int w, int h)
        {
            Bitmap output = new Bitmap(w, h);
            int pw = w / img.Width;
            int ph = h / img.Height;
            Color px;

            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    px = img.GetPixel(x, y);
                    for (int j = 0; j < ph; j++)
                    {
                        for (int i = 0; i < pw; i++)
                        {
                            output.SetPixel(x * pw + i, y * ph + j, px);
                        }
                    }
                }
            }
            return output;
        }

        public static Font SmallFont = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Pixel);
        public static Font MediumFont = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
        public static Font BigFont = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Pixel);
        public static Font SmallFontItalic = new Font("Segoe UI", 10F, FontStyle.Italic, GraphicsUnit.Pixel);
        public static Font MediumFontItalic = new Font("Segoe UI", 16F, FontStyle.Regular, GraphicsUnit.Pixel);
        public static Font BigFontItalic = new Font("Segoe UI", 24F, FontStyle.Regular, GraphicsUnit.Pixel);
        public static Font SmallFontBold = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Pixel);
        public static Font MediumFontBold = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Pixel);
        public static Font BigFontBold = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Pixel);

        public static GraphicsPath CreateRoundedRect(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            // top left arc  
            path.AddArc(arc, 180, 90);

            // top right arc  
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // bottom right arc  
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // bottom left arc 
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }
        public static GraphicsPath CreateRoundedRect(float x, float y, float w, float h, int radius) => CreateRoundedRect(new Rectangle((int)x, (int)y, (int)w, (int)h), radius);
        /// <summary>  
        /// Method for changing the opacity of an image  
        /// </summary>  
        /// <param name="image">image to set opacity on</param>  
        /// <param name="opacity">percentage of opacity</param>  
        /// <returns></returns>  
        public static Image WithOpacity(this Image image, float opacity)
        {
            try
            {
                //create a Bitmap the size of the image provided  
                Bitmap bmp = new Bitmap(image.Width, image.Height);

                //create a graphics object from the image  
                using (Graphics gfx = Graphics.FromImage(bmp))
                {

                    //create a color matrix object  
                    ColorMatrix matrix = new ColorMatrix();

                    //set the opacity  
                    matrix.Matrix33 = opacity;

                    //create image attributes  
                    ImageAttributes attributes = new ImageAttributes();

                    //set the color(opacity) of the image  
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                    //now draw the image  
                    gfx.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
                }
                return bmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
        public static Bitmap CreateRectangle(this Bitmap b, Color c, byte alpha = 255) => CreateRectangle(b,c,alpha/255F);
        public static Bitmap CreateRectangle(this Bitmap b, Color c, float alpha = 1F)
        {
            using (Graphics g = Graphics.FromImage(b))
                g.Clear(c);
            return b.WithOpacity(alpha);
        }
    }
}
