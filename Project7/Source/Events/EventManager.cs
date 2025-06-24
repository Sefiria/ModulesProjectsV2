using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Project7.Source.Events
{
    public class EventManager
    {
        public Queue<Event> EventsQueue;
        public EventManager()
        {
            EventsQueue = new Queue<Event>();
        }
        public void Update()
        {
            if (EventsQueue.Count > 0)
                return;
            var e = EventsQueue.Peek();
            e.Update();
            if (!e.Exists)
                EventsQueue.Dequeue();
        }
        public void Draw(GraphicsDevice graphics)
        {
            if (EventsQueue.Count > 0)
                EventsQueue.Peek().Draw(graphics);
        }
    }
}
