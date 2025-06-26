using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Animations
{
    public class Animation
    {
        public List<Texture2D> Frames;
        public float FrameIndex;
        public float Speed = 10F;

        public Animation(GraphicsDevice graphics, string filename, int frames_count)
        {
            Frames = graphics.SplitTexturePerCount(filename, frames_count, 1).ToList();
            FrameIndex = 0F;
        }
        public void Update()
        {
            FrameIndex += Speed / 100F;
            while (FrameIndex < 0F)
                FrameIndex += Frames.Count;
            while (FrameIndex >= Frames.Count)
                FrameIndex -= Frames.Count;
        }
        public Texture2D GetCurrentFrame() => Frames[(int)FrameIndex];
    }
}
