using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static Tooling.RPDIalogMsg;

namespace Tooling
{
    public class RPDIalogConv
    {
        private List<RPDIalogMsg> Messages = new List<RPDIalogMsg>();
        private int Current = 0;

        public RPDIalogConv(List<RPDIalogMsg> messages)
        {
            Messages = messages;
        }

        public RPDIalogMsg GetCurrent()
        {
            return IsEnded ? null : Messages[Current];
        }

        public RPDIalogConv Next()
        {
            Current++;
            return this;
        }

        public bool NotEnded => !IsEnded;
        public bool IsEnded => Current >= (Messages?.Count ?? -1);
    }
    public class RPDIalogMsg
    {
        public enum Responses
        {
            OK,
            YesNo
        }

        public Bitmap Image;
        public string Text;
        public Responses Response;
        public int SelectedButton = 0;

        public RPDIalogMsg(Bitmap img, string text, Responses response)
        {
            Image = img;
            Text = text;
            Response = response;
        }

        public void Handle()
        {
            //if(Response == Responses.OK)
        }
    }

    public class RPDialog
    {
        private Rectangle m_RenderBounds;
        public Rectangle RenderBounds
        {
            get => m_RenderBounds;
            set
            {
                m_RenderBounds = new Rectangle(value.X, value.Y, value.Width - 3, value.Height - 3);
                DialogBounds = new Rectangle(0, m_RenderBounds.Height - m_RenderBounds.Height / 5, m_RenderBounds.Width, m_RenderBounds.Height / 5);
                ImageBounds = new Rectangle(5, DialogBounds.Y + 5, DialogBounds.Width / 5 - 10, DialogBounds.Height - 10);
                TextBounds = new Rectangle(0 + DialogBounds.Width / 5, DialogBounds.Y, DialogBounds.Width - DialogBounds.Width / 5, DialogBounds.Height);
                int btw = 40, bth = 30;
                ButtonBounds_OK = new Rectangle(DialogBounds.Width - btw - 5, DialogBounds.Y - bth + 5, btw, bth);
                ButtonBounds_YES = new Rectangle(DialogBounds.Width - (btw - 5) * 2 - 20, DialogBounds.Y - bth + 5, btw, bth);
                ButtonBounds_NO = ButtonBounds_OK;
            }
        }

        private Rectangle DialogBounds;
        private Rectangle ImageBounds;
        private Rectangle TextBounds;
        private Rectangle ButtonBounds_OK, ButtonBounds_YES, ButtonBounds_NO;
        private RPDIalogConv Conversation = null;

        public RPDialog(Rectangle renderBounds)
        {
            RenderBounds = renderBounds;
        }

        public void SetConversation(RPDIalogConv conversation)
        {
            Conversation = conversation;
        }

        /// <summary>
        /// Returns true if conversation ended
        /// </summary>
        /// <returns></returns>
        public bool Update()
        {
            if(Conversation?.IsEnded ?? true)
            {
                return true;
            }

            RPDIalogMsg msg = Conversation.GetCurrent();
            msg.Handle();
            return false;
        }

        public void Draw(Graphics g)
        {
            if(Conversation?.NotEnded ?? false)
            {
                RPDIalogMsg msg = Conversation.GetCurrent();
                g.FillRectangle(Brushes.Gray, DialogBounds);
                g.DrawRectangle(Pens.White, DialogBounds);
                if(msg.Image != null)
                    g.DrawImage(msg.Image, ImageBounds);
                else
                    g.FillRectangle(Brushes.Black, ImageBounds);
                g.DrawString(msg.Text, Common.MediumFont, Brushes.White, TextBounds);

                if (msg.Response == Responses.OK)
                {
                    g.FillRectangle(Brushes.Gray, ButtonBounds_OK);
                    g.DrawRectangle(Pens.White, ButtonBounds_OK);
                    g.DrawString("OK", Common.MediumFont, Brushes.White, ButtonBounds_OK);
                }
                else if (msg.Response == Responses.YesNo)
                {
                    g.FillRectangle(msg.SelectedButton == 0 ? Brushes.Gray : Brushes.DimGray, ButtonBounds_YES);
                    g.DrawRectangle(msg.SelectedButton == 0 ? Pens.White : Pens.Gray, ButtonBounds_YES);
                    g.DrawString("Yes", Common.MediumFont, Brushes.White, ButtonBounds_YES);

                    g.FillRectangle(msg.SelectedButton == 1 ? Brushes.Gray : Brushes.DimGray, ButtonBounds_NO);
                    g.DrawRectangle(msg.SelectedButton == 1 ? Pens.White : Pens.Gray, ButtonBounds_NO);
                    g.DrawString("No", Common.MediumFont, Brushes.White, ButtonBounds_NO);
                }
            }
        }

        public void KeyDown(Keys key)
        {
            if (Conversation?.NotEnded ?? false)
            {
                var msg = Conversation.GetCurrent();
                if (key == Keys.Left && KB.IsKeyUp(KB.Key.Left) && msg.SelectedButton > 0)
                    msg.SelectedButton--;
                if (key == Keys.Right && KB.IsKeyUp(KB.Key.Right))
                {
                    if (msg.Response == Responses.OK && msg.SelectedButton < 1)
                        msg.SelectedButton++;
                    else if (msg.Response == Responses.YesNo && msg.SelectedButton < 1)
                        msg.SelectedButton++;
                }
                if(key == Keys.Enter && KB.IsKeyUp(KB.Key.Enter))
                {
                    Conversation.Next();
                }
            }
        }
    }
}