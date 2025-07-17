using Microsoft.Xna.Framework.Graphics;

namespace Tools.Animations
{
    public class AnimationController
    {
        private string _currentAnimation = "";

        public Dictionary<string, Animation> Animations;
        public string CurrentAnimation
        {
            get => _currentAnimation;
            set
            {
                if (_currentAnimation != value && Animations.ContainsKey(value))
                    Animations[value].FrameIndex = 0F;
                _currentAnimation = value;
            }
        }

        public AnimationController(GraphicsDevice graphics)
        {
            Animations = new Dictionary<string, Animation>();
            CurrentAnimation = string.Empty;
        }
        public void Update()
        {
            if (Animations.ContainsKey(CurrentAnimation))
                Animations[CurrentAnimation].Update();
        }
        public Texture2D? GetCurrentFrame()
        {
            if (Animations.ContainsKey(CurrentAnimation))
                return Animations[CurrentAnimation].GetCurrentFrame();
            return null;
        }
        public Animation? GetCurrentAnimation()
        {
            if (Animations.ContainsKey(CurrentAnimation))
                return Animations[CurrentAnimation];
            return null;
        }
        /// <returns>true if loading is successful.</returns>
        public bool AddAnimation(GraphicsDevice graphics, string AnimationName, string filename, int frames_count = 4)
        {
            if (Animations.ContainsKey(AnimationName))
                return false;
            Animations[AnimationName] = new Animation(graphics, filename, frames_count);
            return true;
        }
        public void AddAnimation(GraphicsDevice graphics, string AnimationName, List<Texture2D> frames)
        {
            if (!Animations.ContainsKey(AnimationName))
                Animations[AnimationName] = new Animation(graphics, frames);
        }
    }
}
