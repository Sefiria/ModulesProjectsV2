using Microsoft.Xna.Framework.Graphics;

namespace Project7.Source.Events
{
    public class Event
    {
        public bool Exists = true;
        public Event()
        {
        }
        public virtual void Update()
        {
        }
        public virtual void Draw(GraphicsDevice graphics)
        {
        }
        public virtual void Dispose()
        {
            Exists = false;
        }
    }
}
