using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;

namespace Tooling
{
    public static class Maths
    {
        public static float g = 9.807F; // m/s

        public static float Smallest(ISized c) => Math.Min((int)c.W, (int)c.H);
        public static float SmallestSum(List<ISized> list)
        {
            float sum = 0;
            foreach (var c in list)
                sum += Smallest(c);
            return sum;
        }
        public static float Largest(ISized c) => Math.Max((int)c.W, (int)c.H);
        public static float LargestSum(List<ISized> list)
        {
            float sum = 0;
            foreach (var c in list)
                sum += Largest(c);
            return sum;
        }
        public static float Diagonal(int w, int h) => Sqrt(Sq(w) + Sq(h));
        public static float Diagonal(float w, float h) => Sqrt(Sq(w) + Sq(h));
        public static float Diagonal(ISized c) => Diagonal((int)c.W, (int)c.H);
        public static float DiagonalSum(List<ISized> list)
        {
            float sum = 0;
            foreach (var c in list)
                sum +=Diagonal(c);
            return sum;
        }
        public static float Distance(float x1, float y1, float x2, float y2) => Sq(x2 - x1) + Sq(y2 - y1) == 0 ? 0F : Sqrt(Sq(x2 - x1) + Sq(y2 - y1));
        public static float Distance(float x1, float y1, float z1, float x2, float y2, float z2) => Sq(x2 - x1) + Sq(y2 - y1) + Sq(z2 - z1) == 0 ? 0F : Sqrt(Sq(x2 - x1) + Sq(y2 - y1) + Sq(z2 - z1));
        public static float Distance(float x1, float y1, int x2, int y2) => Distance(x1, y1, (float)x2, (float)y2);
        public static float Distance(int x1, int y1, float x2, float y2) => Distance((float)x1, (float)y1, x2, y2);
        public static float Distance(int x1, int y1, int x2, int y2) => Distance((float)x1, (float)y1, (float)x2, (float)y2);
        public static float Distance(PointF a, PointF b) => Distance(a.X, a.Y, b.X, b.Y);
        //public static float Distance(this Point3D a, Point3D b) => Distance((float)a.X, (float)a.Y, (float)a.Z, (float)b.X, (float)b.Y, (float)b.Z);
        public static float Distance(PointF pt_to_center) => Distance(0, 0, pt_to_center.X, pt_to_center.Y);
        public static float Distance(vec a, vec b) => Distance(a.x, a.y, b.x, b.y);
        public static float Distance(vecf a, vecf b) => Distance(a.x, a.y, b.x, b.y);
        public static float Distance(ICoords a, ICoords b) => Distance(a.X, a.Y, b.X, b.Y);
        public static float DistanceFromLine(PointF pt, float ax, float by, float c) => Abs(ax * pt.X + by * pt.Y + c) / Sqrt(Sq(ax) + Sq(by));
        public static float ScalarProduct(PointF AB, PointF AP) => AB.X * AP.X + AB.Y * AP.Y;
        public static float ScalarProduct(PointF A, PointF B, PointF P) => ScalarProduct(B.MinusF(A), P.MinusF(A));
        //public static float Abs(float K) => Sqrt(Sq(K));
        public static float Abs(float K) => K == 0 ? 1 : (K < 0F ? -K : K);
        public static float Sign(float K)
        {
            var v = (float)Math.Round(K / Abs(K));
            return v is float.NaN ? 0F : v;
        }
        public static float Normalize(float K) => K / Abs(K);
        public static PointF Normale(PointF a, PointF b)
        {
            float dx = b.X - a.X;
            float dy = b.Y - a.Y;
            float magnitude = Distance(a, b);
            return new PointF(dx / magnitude, dy / magnitude);
        }
        public static PointF Perpendiculaire(PointF a, PointF b, PointF normale = default)
        {
            return (normale == default ? Normale(a, b) : normale).Rotate(-90F);
        }
        public static float Sq(float i) => i * i;
        public static unsafe float Sqrt(float number)
        {
            float y = number;
            long i = 0x5f3759df - ((*(long*)&y) >> 1);
            y = *(float*)&i;
            y = y * (1.5F - (number * 0.5F * y * y));
            y = y * (1.5F - (number * 0.5F * y * y));
            return 1F / y * (1.5F - (number * 0.5F * y * y));
        }
        public static float Length(PointF v) => Sqrt(Sq(v.X) + Sq(v.Y));
        public static float Length(vecf v) => Sqrt(Sq(v.x) + Sq(v.y));
        public static PointF Normalized(PointF v)
        {
            float distance = Length(v);
            if (Math.Round(distance, 4) == 0F) return new PointF(0F, 0F);
            return new PointF(v.X / distance, v.Y / distance);
        }
        public static vecf Normalized(vecf v)
        {
            float distance = Length(v);
            if (Math.Round(distance, 4) == 0F) return new vecf(0F, 0F);
            return new vecf(v.x / distance, v.y / distance);
        }
        public static bool CollisionDroite(PointF A, PointF B, float CX, float CY, float CR)
        {
            PointF u = PointF.Empty;
            u.X = B.X - A.X;
            u.Y = B.Y - A.Y;
            PointF AC = PointF.Empty;
            AC.X = CX - A.X;
            AC.Y = CY - A.Y;
            float numerateur = u.X * AC.Y - u.Y * AC.X;   // norme du vecteur v
            if (numerateur < 0F)
                numerateur = -numerateur;   // valeur absolue ; si c'est négatif, on prend l'opposé.
            float denominateur = Sqrt(u.X * u.X + u.Y * u.Y);  // norme de u
            float CI = numerateur / denominateur;
            return CI < CR;
        }
        public static bool CollisionSegment(float Ax, float Ay, float Bx, float By, float CX, float CY, float CR)
        {
            var A = new PointF(Ax, Ay);
            var B = new PointF(Bx, By);
            if (CollisionDroite(A, B, CX, CY, CR) == false)
                return false;  // si on ne touche pas la droite, on ne touchera jamais le segment
            PointF AB = PointF.Empty, AC = PointF.Empty, BC = PointF.Empty;
            AB.X = B.X - A.X;
            AB.Y = B.Y - A.Y;
            AC.X = CX - A.X;
            AC.Y = CY - A.Y;
            BC.X = CX - B.X;
            BC.Y = CY - B.Y;
            float pscal1 = AB.X * AC.X + AB.Y * AC.Y;  // pRoduit scalaiRe
            float pscal2 = (-AB.X) * BC.X + (-AB.Y) * BC.Y;  // pRoduit scalaiRe
            if (pscal1 >= 0F && pscal2 >= 0F)
                return true;   // I entRe A et B, ok.
                               // deRnièRe possibilité, A ou B dans le ceRcle
            if (CollisionPointCercle(A.X, A.Y, CX, CY, CR))
                return true;
            if (CollisionPointCercle(B.X, B.Y, CX, CY, CR))
                return true;
            return false;
        }
        public static bool ProjectionSurSegment(float Cx, float Cy, float Ax, float Ay, float Bx, float By)
        {
            float ACx = Cx - Ax;
            float ACy = Cy - Ay;
            float ABx = Bx - Ax;
            float ABy = By - Ay;
            float BCx = Cx - Bx;
            float BCy = Cy - By;
            float s1 = (ACx * ABx) + (ACy * ABy);
            float s2 = (BCx * ABx) + (BCy * ABy);
            if (s1 * s2 < 0F)
                return false;
            return true;
        }
        public static PointF ProjectionSurSegment(PointF segmentPointA, PointF segmentPointB, PointF pointAProjeter)
        {
            var A = segmentPointA;
            var B = segmentPointB;
            var P = pointAProjeter;
            var AB = B.MinusF(A);
            var AP = P.MinusF(A);
            // t = produit_scalaire( AP, AB ) / norme( AB )^2
            float scal = ScalarProduct(AB, AP) / Sq(AB.Length());
            // t = min( max( 0, t ), 1 )
            scal = Math.Min(Math.Max(0F, scal), 1F);
            // P' = A + AB*t
            return  A.PlusF(AB.x(scal));
        }

        public static PointF Rotate(this PointF v, float degrees)
        {
            float rad = degrees.ToRadians();
            PointF result = PointF.Empty;
            result.X = v.X * (float)Math.Cos(rad) - v.Y * (float)Math.Sin(rad);
            result.Y = v.X * (float)Math.Sin(rad) + v.Y * (float)Math.Cos(rad);
            return result;
        }

        public static float ToRadians(this float degrees) => (float)Math.PI * degrees / 180F;
        public static float ToDegrees(this float radians) => radians / (float)Math.PI * 180F;
        public static PointF AngleToPointF(this float degrees) => new PointF((float)Math.Cos(degrees.ToRadians()), (float)Math.Sin(degrees.ToRadians()));
        public static float GetAngle(this PointF pt, bool in_degrees = true) => in_degrees ? ((float)Math.Atan2(pt.Y, pt.X)).ToDegrees() : ((float)Math.Atan2(pt.Y, pt.X));
        public static float GetAngleWith(this ICoords self, ICoords other) => new PointF(other.X - self.X, other.Y - self.Y).GetAngle();

        public static float Lerp(float v0, float v1, double t)
        {
            return (float)((1D - t) * (double)v0 + t * (double)v1);
        }
        public static int Lerp(int v0, int v1, double t)
        {
            return (int)((1D - t) * v0 + t * v1);
        }
        public static PointF Lerp(PointF v0, PointF v1, double t)
        {
            return new PointF(Lerp(v0.X, v1.X, t), Lerp(v0.Y, v1.Y, t));
        }
        public static vecf Lerp(vecf v0, vecf v1, double t)
        {
            return new vecf(Lerp(v0.x, v1.x, t), Lerp(v0.y, v1.y, t));
        }
        public static vec Lerp(vec v0, vec v1, double t)
        {
            return new vec(Lerp(v0.x, v1.x, t), Lerp(v0.y, v1.y, t));
        }

        public static PointF GetRayToolingLine(PointF v, PointF look, float bx, float by, float bw, float bh) => GetRayToolingLine(v, look, new RectangleF(bx, by, bw, bh));
        public static PointF GetRayToolingLine(PointF r, PointF look, RectangleF bounds)
        {
            if (look == PointF.Empty)
                return r;
            while (r.X > bounds.X && r.X < bounds.Width && r.Y > bounds.Y && r.Y < bounds.Height)
                r.Add(look.X, look.Y);
            if (r.X < bounds.X) r.X = bounds.X;
            if (r.Y < bounds.Y) r.Y = bounds.Y;
            if (r.X >= bounds.Width) r.X = bounds.Width - 1;
            if (r.Y >= bounds.Height) r.Y = bounds.Height - 1;
            return r;
        }
        public static PointF RayTooling(PointF s, PointF look, Bitmap map, Color color)
        {
            bool check(PointF _v) => _v.X >= 0F && _v.Y >= 0F && _v.X < map.Width && _v.Y < map.Height;

            PointF e = GetRayToolingLine(s, look, 0, 0, map.Width, map.Height);
            double length = new Vector(e.X - s.X, e.Y - s.Y).Length;
            PointF v, prevv = new PointF(float.NaN, float.NaN);
            for (double t = 0D; t <= 1D; t += 1D / length)
            {
                v = Lerp(s, e, t);
                if (prevv != new PointF(float.NaN, float.NaN) && prevv == v)
                    continue;
                prevv = v;
                if (check(v) && map.GetPixel((int)v.X, (int)v.Y).ToArgb() != color.ToArgb())
                    return v;
            }
            return PointF.Empty;
        }
        public static bool PointOnLine2D(this PointF _p, PointF _a, PointF _b, float t = 1E-03f)
        {
            PointF p = _p.ToPoint();
            PointF a = _a.ToPoint();
            PointF b = _b.ToPoint();

            // ensure points are collinear
            var zero = (b.X - a.X) * (p.Y - a.Y) - (p.X - a.X) * (b.Y - a.Y);
            if (zero > t || zero < -t) return false;

            // check if X-coordinates are not equal
            if (a.X - b.X > t || b.X - a.X > t)
                // ensure X is between a.X & b.X (use tolerance)
                return a.X > b.X
                    ? p.X + t > b.X && p.X - t < a.X
                    : p.X + t > a.X && p.X - t < b.X;

            // ensure Y is between a.Y & b.Y (use tolerance)
            return a.Y > b.Y
                ? p.Y + t > b.Y && p.Y - t < a.Y
                : p.Y + t > a.Y && p.Y - t < b.Y;
        }
        public static bool IsPointLeftToSegment(vecf a, vecf b, vecf o)
        {
            float crossProduct = (b.x - a.x) * (o.y - a.y) - (b.y - a.y) * (o.x - a.x);
            bool withinBounds = o.x >= Math.Min(a.x, b.x) && o.x <= Math.Max(a.x, b.x) && o.y >= Math.Min(a.y, b.y) && o.y <= Math.Max(a.y, b.y);
            return crossProduct < 0 && withinBounds;
        }
        public static bool IsLeftOfSegment(PointF A, PointF B, PointF o)
        {
            // Calcul du produit vectoriel
            double crossProduct = (B.X - A.X) * (o.Y - A.Y) - (B.Y - A.Y) * (o.X - A.X);
            // Si le produit vectoriel est négatif, le point est à gauche du segment
            if (crossProduct < 0)
            {
                return true;
            }

            // Vérification si le point est dans les limites du segment AB
            double dotProduct = (o.X - A.X) * (B.X - A.X) + (o.Y - A.Y) * (B.Y - A.Y);
            if (dotProduct < 0 || dotProduct > Math.Pow(B.X - A.X, 2) + Math.Pow(B.Y - A.Y, 2))
            {
                return true;
            }

            // Si le produit vectoriel est positif ou nul, le point n'est pas à gauche du segment
            return false;
        }
        public static bool IsLeftOfSegment(Segment s, PointF o, float distance)
        {
            if (IsLeftOfSegment(s.A, s.B, o))
                return true;
            if (IsLeftOfSegment(s.A.MinusF(s.P.x(distance)), s.B.MinusF(s.P.x(distance)), o) == false)// if at right but after the distance, then it is ok
                return true;
            return false;
        }
        public static bool IsPointLeftToLine(vecf a, vecf b, vecf o)
        {
            float crossProduct = (b.x - a.x) * (o.y - a.y) - (b.y - a.y) * (o.x - a.x);
            return crossProduct > 0;
        }
        public static vecf Round(this vecf v, int digits) => ((float)Math.Round(v.x, digits), (float)Math.Round(v.y, digits)).Vf();
        public static PointF Round(this PointF pt) => new PointF((float)Math.Round(pt.X), (float)Math.Round(pt.Y));
        public static Point Round(this Point pt) => new Point((int)Math.Round((double)pt.X), (int)Math.Round((double)pt.Y));
        public static float Round(this float f, int digits) => (float)Math.Round(f, digits);
        public static int Snap<T, U>(this T v, U tiler) where T : struct where U : struct => (int)((int)(v / (dynamic)tiler) * (dynamic)tiler);

        public static RaycastHitInfo SimpleRaycastHit(PointF src, PointF look, float speed, float max_distance, List<RectangleF> rectangles, float angle = 10F, float angle_step = 1F)
        {
            RaycastHitInfo result = new RaycastHitInfo(-1, src);
            float d = 0F;
            RectangleF hit_seg;

            int HitThat(float _angle)
            {
                while (d < max_distance)
                {
                    result.LastPoint = result.LastPoint.PlusF(look.Rotate(_angle).x(speed));
                    d += speed;
                    var _rects = rectangles
                        .Where(r => Maths.Distance(r.Location.MinusF(result.LastPoint)) < max_distance )
                        .OrderBy(r => Maths.Distance(r.Location.MinusF(result.LastPoint))); // Tri des rectangles par distance
                    hit_seg = _rects.FirstOrDefault(r => r.Contains(result.LastPoint));
                    if (hit_seg == null)
                        continue;
                    return rectangles.IndexOf(hit_seg);
                }
                return -1;
            }

            if (angle_step == 0F)
                angle_step = 1F;
            for (float a = 0F; a <= angle; a += angle_step)
            {
                result.Index = HitThat(a);
                if (result.Index > -1) return result;
                if (a != 0F)
                {
                    result.Index = HitThat(-a);
                    if (result.Index > -1) return result;
                }
            }
            return result;
        }
        public static RaycastHitInfo SimpleRaycastHit(PointF src, PointF look, float speed, float max_distance, List<Segment> segments, float segment_right_distance = 1F, float angle = 0F, float angle_step = 1F)
        {
            RaycastHitInfo result = new RaycastHitInfo(-1, src);
            float d = 0F;
            Segment hit_seg;

            int HitThat(float _angle)
            {
                while (d < max_distance)
                {
                    result.LastPoint = result.LastPoint.PlusF(look.Rotate(_angle).x(speed));
                    d += speed;
                    var _segments = segments
                        .Where(s => result.LastPoint.DistanceFromSegment(s.A, s.B) < max_distance)
                        .OrderBy(s => result.LastPoint.DistanceFromSegment(s.A, s.B)); // Tri des segments par distance
                    hit_seg = _segments.FirstOrDefault(s => !IsLeftOfSegment(s, result.LastPoint, segment_right_distance));
                    if (hit_seg == null)
                        continue;
                    return segments.IndexOf(hit_seg);
                }
                return -1;
            }

            if (angle_step == 0F)
                angle_step = 1F;
            for (float a = 0F; a <= angle; a += angle_step)
            {
                result.Index = HitThat(a);
                if (result.Index > -1) return result;
                if (a != 0F)
                {
                    result.Index = HitThat(-a);
                    if (result.Index > -1) return result;
                }
            }
            return result;
        }
        public static bool CollisionSquareSquare(float x1, float y1, float w1, float h1, float x2, float y2, float w2, float h2)
        {
            RectangleF box1 = new RectangleF(x1, y1, w1, h1);
            RectangleF box2 = new RectangleF(x2, y2, w2, h2);
            var a = box1.TopLeft();
            var z = box1.TopRight();
            var q = box1.BottomLeft();
            var s = box1.BottomRight();
            var e = box2.TopLeft();
            var r = box2.TopRight();
            var d = box2.BottomLeft();
            var f = box2.BottomRight();
            return
                box1.Contains(e) || box1.Contains(r) || box1.Contains(d) || box1.Contains(f) ||
                box2.Contains(a) || box2.Contains(z) || box2.Contains(q) || box2.Contains(s) ||
                box1.Contains(box2) || box2.Contains(box1);
        }
        public static bool CollisionSquareCircle(float x1, float y1, float w1, float h1, float x2, float y2, float r)
        {
            return new RectangleF(x1, y1, w1, h1).IntersectsWith(new RectangleF(x2, y2, r * 2, r * 2));
        }

        public static bool CollisionBoxBox(Box box1, Box box2)
        {
            if ((box2.x >= box1.x + box1.w) // trop à droite
                || (box2.x + box2.w <= box1.x) // trop à gauche
                || (box2.y >= box1.y + box1.h)  // trop en bas
                || (box2.y + box2.h <= box1.y)) // trop en haut
                return false;
            else
                return true;
        }

        public static bool CollisionPointCercle(float x, float y, float CX, float CY, float CR)
        {
            float d2 = (x - CX) * (x - CX) + (y - CY) * (y - CY);
            return d2 <= CR * CR;
        }
        public static bool CollisionPointCercle(float x, float y, Circle C) => CollisionPointCercle(x, y, C.x, C.y, C.r);
        public static bool CollisionPointCercle(PointF point, Circle C) => CollisionPointCercle(point.X, point.Y, C);
        public static bool CollisionPointCercle(PointF point, float CX, float CY, float CR) => CollisionPointCercle(point.X, point.Y, CX, CY, CR);

        public static bool CollisionPointBox(Point point, Box box) => CollisionPointBox(point.X, point.Y, box);
        public static bool CollisionPointBox(float curseur_x, float curseur_y, Box box)
        {
            if (curseur_x >= box.x
                && curseur_x < box.x + box.w
                && curseur_y >= box.y
                && curseur_y < box.y + box.h)
                return true;
            else
                return false;
        }
        /// <returns>(0)no collision (1)corner top-left (2)corner bottom-left (3)corner top-right (4)corner bottom-right (5)one inside the other (6)segment top (7)segment right (8)segment bottom (9)segment left</returns>
        public static int CollisionCercleBox(Circle C1, Box box)
        {
            Box boxCercle = new Box(C1);  // retourner la bounding box de l'image porteuse, ou calculer la bounding box.
                                          // premier test :
            if (CollisionBoxBox(box, boxCercle) == false)
                return 0;// no collision
                         // deuxieme test :
            if (CollisionPointCercle(box.x, box.y, C1)) return 1;// corner top-left
            if (CollisionPointCercle(box.x, box.y + box.h, C1)) return 2;// corner bottom-left
            if (CollisionPointCercle(box.x + box.w, box.y, C1)) return 3;// corner top-right
            if (CollisionPointCercle(box.x + box.w, box.y + box.h, C1)) return 4;// corner bottom-right
                                                                                 // cas E :
            if (CollisionSegment(box.x + box.w, box.y, box.x, box.y, C1.x, C1.y + C1.r, C1.r)) return 6;// segment top
            if (CollisionSegment(box.x + box.w, box.y + box.h, box.x + box.w, box.y, C1.x - (int)(C1.r / 2), C1.y, C1.r)) return 7;// segment right
            if (CollisionSegment(box.x, box.y + box.h, box.x + box.w, box.y + box.h, C1.x, C1.y - (int)(C1.r / 2), C1.r)) return 8;// segment bottom
            if (CollisionSegment(box.x, box.y, box.x, box.y + box.h, C1.x + C1.r, C1.y, C1.r)) return 9;// segment left
                                                                                                        // troisieme test :
            if (CollisionPointBox(C1.x, C1.y, box)) return 5;// one inside the other
                                                             // cas B :
            return 0;// no collision
        }
        public static bool CollisionCercleCercle(Circle C1, Circle C2)
        {
            return Length(C2.vec - C1.vec) < C1.r + C2.r;
        }

        public static bool Collides(ICoords Ac, ISized Asz, ICoords Bc, ISized Bsz) => Distance(Ac, Bc) < SmallestSum(new List<ISized>() { Asz, Bsz });

        public static float Range(float min, float max, float value)
        {
            if (value < min) value = min;
            if (value > max) value = max;
            return value;
        }

        public static int Diff(int a, int b) => Math.Max(a, b) - Math.Min(a, b);
        public static float Diff(float a, float b) => Math.Max(a, b) - Math.Min(a, b);

        public static PointF ProjectionSurRectangle(Rectangle rect, PointF pt)
        {
            int rx = rect.X, ry = rect.Y, rw = rect.Width, rh = rect.Height;
            var pt_norm = pt.MinusF(rx + rw / 2F, ry + rh / 2F);

            if(Sq(pt.X) > Sq(pt.Y))
            {
                if(pt_norm.X < 0)
                    return ProjectionSurSegment((rx, ry).P(), (rx, ry + rh).P(), pt);
                else
                    return ProjectionSurSegment((rx + rw, ry).P(), (rx + rw, ry + rh).P(), pt);

            }
            else
            {
                if (pt_norm.Y < 0)
                    return ProjectionSurSegment((rx, ry).P(), (rx + rw, ry).P(), pt);
                else
                    return ProjectionSurSegment((rx, ry + rh).P(), (rx + rw, ry + rh).P(), pt);
            }
        }
        public static PointF ProjectionSurRectangleSimple(Rectangle rect, PointF pt)
        {
            float rx = rect.X, ry = rect.Y, rw = rect.Width, rh = rect.Height;
            var pt_fromcenter = pt.MinusF(rx + rw / 2F, ry + rh / 2F);

            if (Sq(pt_fromcenter.X) > Sq(pt_fromcenter.Y))
                return new PointF(rx + (pt_fromcenter.X < 0F ? 0F : rw), Range(ry, ry + rh, pt.Y));
            else
                return new PointF(Range(rx, rx + rw, pt.X), ry + (pt_fromcenter.Y < 0 ? 0F : rh));
        }
        public static PointF ProjectionSurRectangle(RectangleF rect, PointF pt) => ProjectionSurRectangle(rect.ToIntRect(), pt);
        public static PointF ProjectionSurRectangleSimple(RectangleF rect, PointF pt) => ProjectionSurRectangleSimple(rect.ToIntRect(), pt);
    }

    public class RaycastHitInfo
    {
        public int Index;
        public PointF LastPoint;
        public RaycastHitInfo(){}
        public RaycastHitInfo(int Index, PointF LastPoint)
        {
            this.Index = Index;
            this.LastPoint = LastPoint;
        }
    }
}