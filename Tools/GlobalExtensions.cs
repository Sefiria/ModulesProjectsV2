using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Tools
{
    public static class GlobalExtensions
    {
        public static T[] ToSingleArray<T>(this T[,] multiArray) where T: struct
        {
            int rows = multiArray.GetLength(1);
            int cols = multiArray.GetLength(0);
            T[] singleArray = new T[rows * cols];

            int index = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    singleArray[index++] = multiArray[i, j];
                }
            }

            return singleArray;
        }
        public static Texture2D Clone(this Texture2D originalTexture, GraphicsDevice graphicsDevice)
        {
            Texture2D clonedTexture = new Texture2D(graphicsDevice, originalTexture.Width, originalTexture.Height, false, originalTexture.Format);
            Color[] data = new Color[originalTexture.Width * originalTexture.Height];
            originalTexture.GetData(data);
            clonedTexture.SetData(data);
            return clonedTexture;
        }
        public static Texture2D[] SplitTexturePerCount(this GraphicsDevice graphicsDevice, string filename, int column_count, int row_count)
        {
            if (column_count <= 0 || row_count <= 0)
                return Array.Empty<Texture2D>();
            Texture2D tex = Texture2D.FromFile(graphicsDevice, filename);
            int column_size = tex.Width / (column_count);
            int row_size = tex.Height / (row_count);
            return graphicsDevice.SplitTexture(filename, column_size, row_size);
        }
        public static Texture2D[] SplitTexture(this GraphicsDevice graphicsDevice, string filename, int column_size, int row_size)
        {
            if (column_size < 0 || row_size < 0)
                return Array.Empty<Texture2D>();
            Texture2D tex = Texture2D.FromFile(graphicsDevice, filename);
            var result = new List<Texture2D>();
            if (column_size == 0)
            {
                if (row_size == 0)
                    return new Texture2D[1] { tex };
                for(int i=0; i < tex.Height / row_size; i++)
                {
                    Rectangle sourceRectangle = new Rectangle(0, i * row_size, tex.Width, row_size);
                    Texture2D subTexture = new Texture2D(graphicsDevice, tex.Width, row_size);
                    Color[] data = new Color[tex.Width * row_size];
                    tex.GetData(0, sourceRectangle, data, 0, data.Length);
                    subTexture.SetData(data);
                    result.Add(subTexture);
                }
            }
            else if (row_size == 0)
            {
                for (int i = 0; i < tex.Width / column_size; i++)
                {
                    Rectangle sourceRectangle = new Rectangle(i * column_size, 0, column_size, tex.Height);
                    Texture2D subTexture = new Texture2D(graphicsDevice, column_size, tex.Height);
                    Color[] data = new Color[column_size * tex.Height];
                    tex.GetData(0, sourceRectangle, data, 0, data.Length);
                    subTexture.SetData(data);
                    result.Add(subTexture);
                }
            }
            else
            {
                int columns = tex.Width / column_size;
                int rows = tex.Height / row_size;

                for (int y = 0; y < rows; y++)
                {
                    for (int x = 0; x < columns; x++)
                    {
                        Rectangle sourceRectangle = new Rectangle(x * column_size, y * row_size, column_size, row_size);
                        Texture2D subTexture = new Texture2D(graphicsDevice, column_size, row_size);
                        Color[] data = new Color[column_size * row_size];
                        tex.GetData(0, sourceRectangle, data, 0, data.Length);
                        subTexture.SetData(data);
                        result.Add(subTexture);
                    }
                }
            }
            return result.ToArray();
        }
        public static Texture2D CropTexture2D(this GraphicsDevice graphicsDevice, string filename, int x, int y, int w, int h)
        {
            Texture2D tex = Texture2D.FromFile(graphicsDevice, filename);
            Rectangle sourceRectangle = new Rectangle(x * w, y * h, w, h);
            Texture2D subTexture = new Texture2D(graphicsDevice, w, h);
            Color[] data = new Color[w * h];
            tex.GetData(0, sourceRectangle, data, 0, data.Length);
            subTexture.SetData(data);
            return subTexture;
        }
        public static Texture2D CloneAsWhite(this GraphicsDevice graphicsDevice, Texture2D original)
        {
            // Récupérer les données de pixels
            Color[] data = new Color[original.Width * original.Height];
            original.GetData(data);

            // Modifier chaque pixel : blanc avec alpha d'origine
            for (int i = 0; i < data.Length; i++)
            {
                byte alpha = data[i].A;
                data[i] = new Color((byte)255, (byte)255, (byte)255, alpha);
            }

            // Créer une nouvelle texture et y appliquer les données modifiées
            Texture2D whiteTexture = new Texture2D(graphicsDevice, original.Width, original.Height);
            whiteTexture.SetData(data);

            return whiteTexture;
        }
        public static Texture2D ResizeTexture(this GraphicsDevice graphicsDevice, Texture2D source, int newWidth, int newHeight)
        {
            RenderTarget2D rt = new RenderTarget2D(graphicsDevice, newWidth, newHeight);
            graphicsDevice.SetRenderTarget(rt);
            graphicsDevice.Clear(Color.Transparent);

            SpriteBatch sb = new SpriteBatch(graphicsDevice);
            sb.Begin();
            sb.Draw(source, new Rectangle(0, 0, newWidth, newHeight), Color.White);
            sb.End();

            graphicsDevice.SetRenderTarget(null);
            return rt;
        }

    }

}
