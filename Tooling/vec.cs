using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Tooling
{
    public class vec
    {
        public int x { get; set; }
        public int y { get; set; }
        public byte UserData { get; set; } = 0;
        public vec() { x = y = 0; }
        public vec(vec v) { x = v.x; y = v.y; }
        public vec(int x, int y) { this.x = x; this.y = y; }
        public vec(float x, float y) { this.x = (int)x; this.y = (int)y; }
        public vec(float x, float y, byte usrdt = 0) { this.x = (int)x; this.y = (int)y; UserData = usrdt; }
        [JsonIgnore] public vecf f => new vecf(x, y);
        [JsonIgnore] public Point ipt => new Point(x, y);
        [JsonIgnore] public PointF pt => new PointF(x, y);

        [JsonIgnore] public static vec Null => new vec(int.MaxValue, int.MaxValue);
        [JsonIgnore] public static vec Zero => new vec(0, 0);

        public vec tile(int sz) => new vec(x / sz, y / sz);
        public vec snap(int sz) => new vec((x / sz) * sz, (y / sz) * sz);
        public vec snap() => new vec((x / Core.TSZ) * Core.TSZ, (y / Core.TSZ) * Core.TSZ);

        public override bool Equals(object obj)
        {
            return obj is vec vec &&
                        x == vec.x &&
                        y == vec.y;
        }

        public override int GetHashCode()
        {
            int hashCode = 1954509452;
            hashCode = hashCode * -1521134295 + x.GetHashCode();
            hashCode = hashCode * -1521134295 + y.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(vec a, vec b) => a?.x == b?.x && a?.y == b?.y;
        public static bool operator !=(vec a, vec b) => a?.x != b?.x || a?.y != b?.y;
        public static vec operator +(vec a, vec b) => new vec((a?.x + b?.x).Value, (a?.y + b?.y).Value);
        public static vec operator +(vec a, int v) => new vec((a?.x + v).Value, (a?.y + v).Value);
        public static vec operator +(vec a, float v) => new vec((a?.x + v).Value, (a?.y + v).Value);
        public static vec operator -(vec a, vec b) => new vec((a?.x - b?.x).Value, (a?.y - b?.y).Value);
        public static vec operator -(vec a, int v) => new vec((a?.x - v).Value, (a?.y - v).Value);
        public static vec operator -(vec a, float v) => new vec((a?.x - v).Value, (a?.y - v).Value);
        public static vec operator *(vec a, vec b) => new vec((a?.x * b?.x).Value, (a?.y * b?.y).Value);
        public static vec operator *(vec a, int b) => new vec((a?.x * b).Value, (a?.y * b).Value);
        public static vec operator *(vec a, float b) => new vec((a?.x * b).Value, (a?.y * b).Value);
        public static vec operator /(vec a, vec b) => new vec((a?.x / Math.Max(1, b?.x ?? 1)).Value, (a?.y / Math.Max(1, b?.y ?? 1)).Value);
        public static vec operator /(vec a, vecf b) => new vec((a?.x / b?.x).Value, (a?.y / b?.y).Value);
        public static vec operator /(vec a, int b) => b == 0 ? a : new vec((a?.x / b).Value, (a?.y / b).Value);
        public static vec operator /(vec a, float b) => Math.Round(b, 4) == 0F ? a : new vec((a?.x / b).Value, (a?.y / b).Value);
        public static vec operator %(vec a, float b) => new vec((a?.x % b).Value, (a?.y % b).Value);
        public static vec operator %(vec a, vec b) => new vec((a?.x % b?.x).Value, (a?.y % b?.y).Value);
        public static vecf operator %(vec a, vecf b) => new vecf((a?.x % b?.x).Value, (a?.y % b?.y).Value);

        public override string ToString()
        {
            return $"({x}, {y})";
        }
    }
}
