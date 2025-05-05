using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tooling
{
    public class Segment
    {
        private PointF m_A, m_B;
        public PointF A
        {
            get => m_A;
            set
            {
                m_A = value;
                N = Maths.Normale(m_A, m_B);
                P = Maths.Perpendiculaire(m_A, m_B, N);
                Angle = Maths.GetAngle(m_B.MinusF(m_A));
            }
        }
        public PointF B
        {
            get => m_B;
            set
            {
                m_B = value;
                N = Maths.Normale(m_A, m_B);
                P = Maths.Perpendiculaire(m_A, m_B, N);
                Angle = Maths.GetAngle(m_B.MinusF(m_A));
            }
        }
        public PointF N, P;
        public float Angle;
        public Segment()
        {
        }
        public Segment(PointF A, PointF B)
        {
            this.A = A;
            this.B = B;
        }
        public Segment(float ax, float ay, float bx, float by)
        {
            A = new PointF(ax, ay);
            B = new PointF(bx, by);
        }

        public void Draw(Graphics g, PointF? Offset = null)
        {
            if(Offset != null)
                g.DrawLine(Pens.White, A.PlusF(Offset.Value), B.PlusF(Offset.Value));
            else
                g.DrawLine(Pens.White, A, B);
            //g.DrawLine(new Pen(Color.FromArgb(100, 0, 0), 8F), A.MinusF(P.x(5F)), B.MinusF(P.x(5F)));
        }
    }
}
