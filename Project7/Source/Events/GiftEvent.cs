using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Project7.Source.Events
{
    public class GiftEvent : Event
    {
        public string Message;
        public Texture2D GiftTexture;
        public EventCallback Action;

        Panel panel;

        public GiftEvent(string message, Texture2D giftTexture, Action action, Vector2 panel_size, Vector2 dragdrop_panel_size, Vector2 dragdrop_panel_offset, Vector2 img_size, Vector2 img_offset) : base()
        {
            Message = message;
            GiftTexture = giftTexture;
            Action = (e) => { action(); Game1.Instance.PlaySoundAsync(Game1.Instance.SE_GIFT); Dispose(); };

            Initialize(panel_size, dragdrop_panel_size, dragdrop_panel_offset, img_size, img_offset);

            Game1.Instance.EventManager.EventsQueue.Enqueue(this);
        }
        void Initialize(Vector2 panel_size, Vector2 dragdrop_panel_size, Vector2 dragdrop_panel_offset, Vector2 img_size, Vector2 img_offset)
        {
            panel = new Panel(panel_size);
            panel.Draggable = true;
            UserInterface.Active.AddEntity(panel);

            var msg = new RichParagraph(Message);
            panel.AddChild(msg);

            var img = new Image(GiftTexture, img_size, offset: img_offset);
            img.OnMouseDown += Action;
            var dragdrop_panel = new Panel(dragdrop_panel_size, offset: dragdrop_panel_offset);
            dragdrop_panel.AddChild(img);
            panel.AddChild(dragdrop_panel);
        }
        public override void Update()
        {
        }
        public override void Draw(GraphicsDevice graphics)
        {
        }
        public override void Dispose()
        {
            UserInterface.Active.ShowCursor = false;
            panel.Draggable = false;
            panel.RemoveFromParent();
            panel.Dispose();
            base.Dispose();
        }
    }
}
