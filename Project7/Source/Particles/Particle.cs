using Microsoft.Xna.Framework.Graphics;
using Project7.Source.Entities.Behaviors;
using System.Collections.Generic;
using System.Drawing;
using System.Net.NetworkInformation;
using Tools.Animations;

namespace Project7.Source.Entities
{
    public class Particle
    {
        Game1 Context => Game1.Instance;

        public bool Exists;
        public AnimationController AnimationController;
        public float X, Y, Scale;

        public Particle(string anim_tex, float x, float y, float scale = 1F)
        {
            Exists = true;
            AnimationController = new AnimationController(Context.GraphicsDevice);
            AnimationController.AddAnimation(Context.GraphicsDevice, "anim", anim_tex, 10);
            AnimationController.CurrentAnimation = "anim";
            X = x;
            Y = y;
            Scale = scale;
            Context.ParticleManager.Particles.Add(this);
        }
        public Particle(List<Texture2D> frames, float x, float y, float scale = 1F)
        {
            Exists = true;
            AnimationController = new AnimationController(Context.GraphicsDevice);
            AnimationController.AddAnimation(Context.GraphicsDevice, "anim", frames);
            AnimationController.CurrentAnimation = "anim";
            X = x;
            Y = y;
            Scale = scale;
            Context.ParticleManager.Particles.Add(this);
        }
        public void Update()
        {
            AnimationController?.Update();

            var anim = AnimationController.GetCurrentAnimation();
            if (anim.FrameIndex >= anim.Frames.Count-anim.Speed/100F)
                Exists = false;
        }
        public void Draw(GraphicsDevice graphics)
        {
            Texture2D tex = AnimationController?.GetCurrentFrame();
            if (tex != null)
            {
                Graphics.Graphics.Instance.DrawTexture(tex, X, Y, 0F, Scale, false, 0F);
            }
        }
    }
}
