using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using ILGPU.Runtime.Cuda;
using System.Windows;

namespace Tooling
{
    public static class Tools
    {

        static ColorMatrix matrix1 = new ColorMatrix();
        static ColorMatrix matrix2 = new ColorMatrix();
        static ImageAttributes attributes1 = new ImageAttributes();
        static ImageAttributes attributes2 = new ImageAttributes();
        public static Bitmap GetFaded(Bitmap B, Bitmap A, float t)
        {
            if (A == null || B == null)
                throw new ArgumentException("One of the bitmap input isn't initialized.");

            int Width = A.Width;
            int Height = A.Height;

            Bitmap img = new Bitmap(Width, Height);
            using (Graphics g = Graphics.FromImage(img))
            {
                matrix1.Matrix33 = 1 - t;
                attributes1.SetColorMatrix(matrix1, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                matrix2.Matrix33 = t;
                attributes2.SetColorMatrix(matrix2, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                g.DrawImage(A, new Rectangle(0, 0, Width, Height), 0, 0, Width,
                                        Height, GraphicsUnit.Pixel, attributes1);
                g.DrawImage(B, new Rectangle(0, 0, Width, Height), 0, 0, Width,
                                        Height, GraphicsUnit.Pixel, attributes2);
            }
            return img;
        }

        public static Bitmap RotatedAlphaResizedFaded(Bitmap A, Bitmap B, float t, float angle, float opacity, float scale)
        {
            Bitmap bmp = GetFaded(A, B, t);
            int w = (int)(bmp.Width * scale);
            int h = (int)(bmp.Height * scale);
            if (w < 1) w = 1;
            if (h < 1) h = 1;
            Bitmap rotatedImage = new Bitmap(w, h);
            rotatedImage.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution);

            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                // Set the rotation point to the center in the matrix
                g.TranslateTransform(bmp.Width / 2, bmp.Height / 2);
                // Rotate
                g.RotateTransform(angle);
                // Restore rotation point in the matrix
                g.TranslateTransform(-bmp.Width / 2, -bmp.Height / 2);
                // Draw the image on the bitmap

                //create a color matrix object  
                ColorMatrix matrix = new ColorMatrix();
                //set the opacity  
                matrix.Matrix33 = opacity;
                //create image attributes  
                ImageAttributes attributes = new ImageAttributes();
                //set the color(opacity) of the image  
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                // draw the image  
                g.DrawImage(bmp, new Rectangle(0, 0, w, h), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attributes);
            }

            return rotatedImage;
        }
        public static Bitmap Rotated(this Bitmap bmp, float angle)
        {
            int w = bmp.Width;
            int h = bmp.Height;
            int sz = (int)Maths.Diagonal(w, h);
            Bitmap rotatedImage = new Bitmap(sz, sz);
            rotatedImage.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution);
            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                g.TranslateTransform(w / 2F, h / 2F);
                g.RotateTransform(angle);
                g.TranslateTransform(-w / 2F, -h / 2F);
                g.DrawImage(bmp, new Rectangle(0, 0, sz, sz), 0, 0, sz, sz, GraphicsUnit.Pixel, new ImageAttributes());
            }
            return rotatedImage;
        }
        public static Bitmap RotatedAlphaResized(Bitmap bmp, float angle, float opacity, float scale)
        {
            int w = (int)(bmp.Width * scale);
            int h = (int)(bmp.Height * scale);
            if (w < 1) w = 1;
            if (h < 1) h = 1;
            Bitmap rotatedImage = new Bitmap(w, h);
            rotatedImage.SetResolution(bmp.HorizontalResolution, bmp.VerticalResolution);

            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                // Set the rotation point to the center in the matrix
                g.TranslateTransform(bmp.Width / 2F, bmp.Height / 2F);
                // Rotate
                g.RotateTransform(angle);
                // Restore rotation point in the matrix
                g.TranslateTransform(-bmp.Width / 2F, -bmp.Height / 2F);
                // Draw the image on the bitmap

                //create a color matrix object  
                ColorMatrix matrix = new ColorMatrix();
                //set the opacity  
                matrix.Matrix33 = opacity;
                //create image attributes  
                ImageAttributes attributes = new ImageAttributes();
                //set the color(opacity) of the image  
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                // draw the image  
                g.DrawImage(bmp, new Rectangle(0, 0, w, h), 0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attributes);
            }

            return rotatedImage;
        }
        public static Bitmap[,] Split2D(this Bitmap tex, int square_size, bool make_transparent = true, Color? transparent_color = null)
        {
            int s = square_size;
            int w = tex.Width, h = tex.Height;
            int cw = w / s, ch = h / s;
            var result = new Bitmap[cw, ch];
            for (int i = 0; i < cw; i++)
                for (int j = 0; j < ch; j++)
                {
                    result[i, j] = tex.Clone(new Rectangle(i * s, j * s, s, s), tex.PixelFormat);
                    if (make_transparent)
                    {
                        if (transparent_color == null)
                            transparent_color = Color.White;
                        result[i, j].MakeTransparent(transparent_color.Value);
                    }
                }
            return result;
        }
        public static Bitmap[,] Split2DAndResize(this Bitmap tex, int square_size, int new_size_in_px, bool make_transparent = false, Color? transparent_color = null)
        {
            bool resize = new_size_in_px != square_size;
            int s = square_size;
            int w = tex.Width, h = tex.Height;
            int cw = w / s, ch = h / s;
            var result = new Bitmap[cw, ch];
            for (int i = 0; i < cw; i++)
                for (int j = 0; j < ch; j++)
                {
                    result[i, j] = tex.Clone(new Rectangle(i * s, j * s, s, s), tex.PixelFormat);
                    if (make_transparent && transparent_color != null)
                        result[i, j].MakeTransparent(transparent_color.Value);
                    if(resize)
                        result[i, j] = new Bitmap(result[i, j], new_size_in_px+1, new_size_in_px+1);
                }
            return result;
        }
        public static Bitmap[,] Split2D(this Bitmap tex, int tw, int th, bool make_transparent = true, Color? transparent_color = null)
        {
            int w = tex.Width, h = tex.Height;
            int cw = w / tw, ch = h / th;
            var result = new Bitmap[cw, ch];
            for (int i = 0; i < cw; i++)
                for (int j = 0; j < ch; j++)
                {
                    result[i, j] = tex.Clone(new Rectangle(i * tw, j * th, tw, th), tex.PixelFormat);
                    if (make_transparent)
                    {
                        if (transparent_color == null)
                            transparent_color = Color.White;
                        result[i, j].MakeTransparent(transparent_color.Value);
                    }
                }
            return result;
        }
        public static Bitmap[,] Split2DAndResize(this Bitmap tex, int tw, int th, int new_tw, int new_th, bool make_transparent = false, Color? transparent_color = null)
        {
            bool resize = new_tw != tw || new_th != th;
            int w = tex.Width, h = tex.Height;
            int cw = w / tw, ch = h / th;
            var result = new Bitmap[cw, ch];
            for (int i = 0; i < cw; i++)
                for (int j = 0; j < ch; j++)
                {
                    result[i, j] = tex.Clone(new Rectangle(i * tw, j * th, tw, th), tex.PixelFormat);
                    if (make_transparent && transparent_color != null)
                        result[i, j].MakeTransparent(transparent_color.Value);
                    if (resize)
                        result[i, j] = new Bitmap(result[i, j], new_tw + 1, new_th + 1);
                }
            return result;
        }
        public static List<Bitmap> Split(this Bitmap tex, int cx = 0, int cy = 0, int x = 0, int y = 0)
        {
            int @case = 0;
            if (cx > 0 && cy > 0) @case = 1;
            else if (x > 0 && y > 0) @case = 2;
            List<Bitmap> result = new List<Bitmap>();

            if (@case == 0) return result;

            int w = tex.Width;
            int h = tex.Height;

            if (@case == 1)
            {
                for (int j = 0; j < h; j += h / cy)
                    for (int i = 0; i < w; i += w / cx)
                        result.Add(tex.Clone(new Rectangle(i, j, w / cx, h / cy), tex.PixelFormat));
            }
            else
            {
                for (int j = 0; j < h / y; j++)
                    for (int i = 0; i < w / x; i++)
                        result.Add(tex.Clone(new Rectangle(i*x, j*y, x, y), tex.PixelFormat));
            }

            return result;
        }

        public static int Range(int min, int val, int max) => Math.Max(min, Math.Min(max, val));
        public static Bitmap CreateRectBitmap(int size, Color color)
        {
            Bitmap output = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(output))
                g.FillRectangle(new SolidBrush(color), 0, 0, size, size);
            return output;
        }
    }
}
