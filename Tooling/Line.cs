using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tooling
{
    public class Line
    {
        public Point A = Point.Empty, B = Point.Empty;
        public Line() { A = Point.Empty; B = Point.Empty; }
        public Line(Point A, Point B) { this.A = A; this.B = B; }
        public Line(Line line) { A = line.A; B = line.B; }
        public Line(LineF line) { A = line.A.ToPoint(); B = line.B.ToPoint(); }
        public static readonly Line Empty = new Line();
        public LineF ToLineF() => new LineF(this);
        public Point Middle => B.Minus(A).Div(2);
        public override string ToString()
        {
            return $"A:{A} B:{B}";
        }
    }
    public class LineF
    {
        public PointF A, B;
        public LineF() { A = PointF.Empty; B = PointF.Empty; }
        public LineF(PointF A, PointF B) { this.A = A; this.B = B; }
        public LineF(Line line) { A = line.A; B = line.B; }
        public LineF(LineF line) { A = line.A; B = line.B; }
        public static readonly LineF Empty = new LineF();
        public Line ToLine() => new Line(this);
        public PointF Middle => B.MinusF(A).DivF(2f);
        public override string ToString()
        {
            return $"A:{A} B:{B}";
        }
    }
}
