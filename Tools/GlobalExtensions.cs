using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }

}
