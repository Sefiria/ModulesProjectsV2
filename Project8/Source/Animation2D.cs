using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Project8.Source
{
    public class Animation2D
    {
        public static Dictionary<string, Animation2D> Animations = new Dictionary<string, Animation2D>();

        public string Name, Filename;
        public float Speed = 10F;

        public int rFramesCount { get; private set; }
        public Texture2D Texture;
        public float FrameIndex;

        public Animation2D(string name, string filename)
        {
            Name = name;
            Filename = filename;
            Texture = Texture2D.FromFile(GameMain.Instance.GraphicsDevice, filename);
            if (!Animations.ContainsKey(name))
            {
                Animations.Add(name, this);
                rFramesCount = Texture.Width / GlobalVariables.tilesize;
            }
            else
            {
                rFramesCount = Animations[name].rFramesCount;
            }
        }
        public void Update()
        {
            FrameIndex += Speed / 100F;
            while (FrameIndex < 0F)
                FrameIndex += rFramesCount;
            while (FrameIndex >= rFramesCount)
                FrameIndex -= rFramesCount;
        }

        public Rectangle Get() => new Rectangle((int)FrameIndex * GlobalVariables.tilesize, 0, GlobalVariables.tilesize, GlobalVariables.tilesize);
    }
}
